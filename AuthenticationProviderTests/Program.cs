using JonathanBout.Authentication;

namespace AuthenticationProviderTests
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllers();
			/*
			 * Add key authentication
			 */
			builder.AddKeyAuthentication();
			
			var app = builder.Build();
			app.UseHttpsRedirection();
			app.MapControllers();
			/*
			 * Use key authentication.
			 */
			app.UseKeyAuthentication();
			/*
			 * For minimal APIs, an EndpointFilter exists. This lets you do the same
			 * as the [Authenticated] attribute on Controllers. You could inject the 
			 * IAuthenticationSession here as well, to retrieve a UserId.
			 */
			app.MapGet("/testget", () =>
			{
				return Results.Ok(Random.Shared.Next(0, 100));
			}).AddEndpointFilter<IAuthenticatedFilter>();
			app.Run();
		}
	}
}