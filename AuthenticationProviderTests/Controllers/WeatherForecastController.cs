using JonathanBout.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationProviderTests.Controllers
{
	[ApiController]
	[KeyAuthenticated]
	[Route("/[controller]/")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
		"Freezing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[SkipAuthentication]
		public object Get()
		{
			return Ok(new
			{
				Forecast = GetForecasts()
			});
		}

		[HttpGet("Personalized")]
		public object Get(IAuthenticationSession session)
		{
			return Ok(new
			{
				Forecast = GetForecasts(),
				userId = session.CurrentUserId
			});
		}

		private static IEnumerable<WeatherForecast> GetForecasts()
		{
			return Enumerable.Range(1, 5).Select(index =>
			{
				const int min = -20;
				const int max = 50;
				const int range = max - min;

				int temp = Random.Shared.Next(min, max);
				int summaryToChoose = (int)double.Round(((double)(temp - min) / ((double)range / (Summaries.Length - 1))));

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