namespace JhaServer.Config
{
	/// <summary>
	/// Place to get keys for the app config
	/// </summary>
	public static class ConfigKeys
	{
		public static class Twitter
		{
			const string BaseKey = "Twitter";

			public const string AppName = $"{BaseKey}:AppName";
			public const string BearerToken = $"{BaseKey}:BearerToken";
		}
	}
}
