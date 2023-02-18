using JonathanBout.Authentication;

namespace AuthenticationProviderTests
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllers();
			builder.AddKeyAuthentication();
			
			var app = builder.Build();
			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();
			app.UseKeyAuthentication();
			app.Run();
		}
	}
}