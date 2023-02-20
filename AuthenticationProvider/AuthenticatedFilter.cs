namespace JonathanBout.Authentication
{
	internal class AuthenticatedFilter : IAuthenticatedFilter
	{
		public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
		{
			if (AuthenticatedAttribute.ExecuteAsync(context.HttpContext, null))
			{
				return await next(context);
			}
			return Results.Unauthorized();
		}
	}
}
