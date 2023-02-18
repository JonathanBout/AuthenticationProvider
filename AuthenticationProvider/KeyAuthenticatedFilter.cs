namespace JonathanBout.Authentication
{
	public class KeyAuthenticatedFilter : IEndpointFilter
	{
		public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
		{
			if (KeyAuthenticatedAttribute.ExecuteAsync(context.HttpContext))
			{
				return await next(context);
			}
			return new { };
		}
	}
}
