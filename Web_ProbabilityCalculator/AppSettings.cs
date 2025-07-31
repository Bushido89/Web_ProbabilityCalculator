namespace Web_ProbabilityCalculator
{
	public interface IAppSettings
	{
		CacheConfiguration CacheConfiguration { get; set; }
	}
	public class AppSettings : IAppSettings
	{
		public CacheConfiguration CacheConfiguration { get; set; }
	}
	public class CacheConfiguration
	{
		public int DefaultTimeoutMs { get; set; }

		public int CalculationCacheMs { get; set; }
	}
}
