using JhaRepository;

namespace JhaServer.Config
{
	public static class ConfigExtensions
	{
		/// <summary>
		/// central place to configure DI for the app
		/// </summary>
		/// <param name="webApp"></param>
		public static void ConfigureDependentServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddLogging();
			builder.Services.AddSingleton<ITweetStatisticsRepository, InMemoryTweetStatisticsRepository>();
		}//end ConfigureDependentServices
	}
}
