using AspNetCore.Identity.LiteDB.Data;
using LiteDB;
using System.Text.Json;

namespace JonathanBout.Authentication
{
	public static class WebApplicationBuilderExtensions
	{
		#region Username & Password
		public static WebApplicationBuilder AddCredentialAuthentication(
			this WebApplicationBuilder builder,
			Action<CredentialsAuthenticationOptions>? configure)
		{
			if (configure is not null)
			{
				configure(CredentialsAuthenticationOptions.Instance);
			}
			return builder;
		}
		public static WebApplicationBuilder AddCredentialAuthentication(this WebApplicationBuilder builder)
		{
			return builder.AddCredentialAuthentication(null);
		}

		public static WebApplication UseCredentialAuthentication(this WebApplication application)
		{
			application.MapPost(CredentialsAuthenticationOptions.Instance.BasePath + "/Login",
			(HttpContext context) =>
			{

			});
			return application;
		}

		#endregion
		#region Key
		public static WebApplicationBuilder AddKeyAuthentication(this WebApplicationBuilder builder, Action<KeyAuthenticationOptions>? configure)
		{
			if (configure is not null)
				configure(KeyAuthenticationOptions.Instance);
			builder.Services.AddSingleton<IAuthenticator, KeyAuthenticator>();
			builder.Services.AddSingleton<ILiteDbContext, LiteDbContext>((services) =>
			{
				var fileInfo = new FileInfo(KeyAuthenticationOptions.Instance.DatabasePath);
				if (!(fileInfo.Directory?.Exists ?? false))
				{
					fileInfo.Directory?.Create();
				}
				return new LiteDbContext(new LiteDatabase(KeyAuthenticationOptions.Instance.DatabasePath));
			});
			builder.Services.AddScoped<IAuthenticationSession, AuthenticationSession>();
			return builder;
		}

		public static WebApplicationBuilder AddKeyAuthentication(this WebApplicationBuilder builder)
		{
			return builder.AddKeyAuthentication(null);
		}

		public static WebApplication UseKeyAuthentication(this WebApplication application)
		{
			var service = application.Services.GetService<IAuthenticator>();
			service?.Initialize();

			string basePath = KeyAuthenticationOptions.Instance.BasePath.TrimEnd('/');
			application.MapPost(basePath + "/Login", async (HttpContext context, IAuthenticator authenticator) =>
			{
				string key = "";
				int userId = 0;
				// keep track of current activity for easier debugging on the user end.
				string currentActivity = "parsing the body";
				try
				{
					JsonDocument bodyJson = (await ReadBody(context))!;
					if (bodyJson.RootElement.TryGetProperty("key", out var keyElement))
					{
						key = keyElement.GetString() ?? "";
					}
					currentActivity = "reading the 'userId' element";
					if (bodyJson.RootElement.TryGetProperty("userId", out var idElement))
					{
						if (idElement.TryGetInt32(out int id))
						{
							userId = id;
						}
					}
				}
				catch (Exception ex)
				{
					return Results.BadRequest(new { ExceptionType = ex.GetType().Name, ex.Message, While = currentActivity });
				}

				if (string.IsNullOrWhiteSpace(key))
					return Results.BadRequest("The key could not be retrieved from the body.");
				if (userId < 1)
					return Results.BadRequest("The userId was not specified or less than one.");

				if (authenticator.Login(key, userId) is KeyUserIdentifier identifier)
				{
					context.Response.Cookies.Append(KeyAuthenticationOptions.Instance.CookieName, Hasher.Hash(identifier.KeyHash),
						new CookieOptions
						{
							HttpOnly = true,
							Secure = true,
							IsEssential = true,
						});
					return Results.Ok(new
					{
						userId = identifier.UserId
					});
				}
				return Results.Unauthorized();
			});

			application.MapGet(basePath + "/Key", (IAuthenticator authenticator) =>
			{
				var (key, id) = authenticator.AddUser();
				return new
				{
					Secret = key
				};
			}).AddEndpointFilter<AuthenticatedFilter>();
			return application;
		}
		#endregion

		internal static async Task<JsonDocument?> ReadBody(HttpContext context)
		{
			string currentActivity = "beginning the body read";
			using (var streamreader = new StreamReader(context.Request.Body))
			{
				try
				{
					currentActivity = "reading the body";
					string body = await streamreader.ReadToEndAsync();
					currentActivity = "parsing the JSON provided in the body";
					using var bodyJson = JsonDocument.Parse(body, new JsonDocumentOptions()
					{
						AllowTrailingCommas = true,
						CommentHandling = JsonCommentHandling.Skip
					});
					currentActivity = "reading the 'key' element";
					return bodyJson;
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message + " - Caught while " + currentActivity + ".", ex);
				}
			};
		}
	}
}