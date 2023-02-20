using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JonathanBout.Authentication
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthenticatedAttribute : ActionFilterAttribute
	{
		public AuthenticatedAttribute() { }

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (ExecuteAsync(context.HttpContext, context.ActionDescriptor))
			{
				await next();
			}
		}
	
		static internal bool ExecuteAsync(HttpContext context, ActionDescriptor? descriptor)
		{
			if (descriptor?.FilterDescriptors.Any(x => x.Filter is SkipAuthenticationAttribute) ?? false)
			{
				return true;
			}
			var authenticator = context.RequestServices.GetService<IAuthenticator>();
			var userSession = context.RequestServices.GetService<IAuthenticationSession>();
			var cookieName = KeyAuthenticationOptions.Instance.CookieName;

			if (authenticator is not null && userSession is not null)
			{
				if (context.Request.Cookies.ContainsKey(cookieName)
					&& context.Request.Cookies[cookieName] is string key)
				{
					if (authenticator.Authenticate(key) is KeyUserIdentifier identifier)
					{
						userSession.Identifier = identifier;
						return true;
					}
				}
			}
			context.Response.StatusCode = 401;
			return false;
		}
	}
}
