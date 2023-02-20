using JonathanBout.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationProviderTests.Controllers
{
	/*
	 * The [Authenticated] attribute marks this controller
	 * to be authenticated by the chosen authenticator during
	 * initialization (in Program.Main()).
	 */
	[ApiController]
	[Authenticated]
	[Route("/[controller]/")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		/*
		 * Authentication will be skipped for this function.
		 * the [SkipAuthentication] attribute is useable on functions,
		 * so that it is reachable by an unauthenticated user while in 
		 * a controller with the [Authenticated] attribute.
		 */
		[HttpGet]
		[SkipAuthentication]
		public object Get()
		{
			return Ok(new
			{
				Forecast = GetForecasts()
			});
		}

		/*
		 * This function does not have the [Authenticated] attribute or the
		 * [SkipAuthentication] attribute. Because the controller has the
		 * [Authenticated] attribute it will need the user to be authenticated. 
		 * 
		 * In an authenticated endpoint, it is possible to inject the
		 * IAuthenticationSession interface. This will provide you with a 
		 * UserId as integer. This will be created when a new user gets added and
		 * is incremented by one for every user. With this ID you can load your
		 * own user from your own database and do stuff with it.
		 */
		[HttpGet("Personalized")]
		public object Get(IAuthenticationSession session)
		{
			return Ok(new
			{
				Forecast = GetForecasts(),
				userId = session.CurrentUserId
			});
		}

		/*
		 * This function is only to give some interesting looking result for the
		 * previous endpoints. It is adapted from the default WeatherForecast
		 * Controller, although this one selects the summary depending on the temperature
		 * and not completely random, which the default implementation did.
		 */
		private static IEnumerable<WeatherForecast> GetForecasts()
		{
			return Enumerable.Range(1, 5).Select(index =>
			{
				const int min = -20;
				const int max = 50;
				const int range = max - min;

				int temp = Random.Shared.Next(min, max);
				int summaryToChoose = (int)double.Round(
					(temp - min) / ((double)range / (Summaries.Length - 1)));

				return new WeatherForecast
				{
					Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					TemperatureC = temp,
					Summary = Summaries[summaryToChoose]
				};
			});
		}
	}
}