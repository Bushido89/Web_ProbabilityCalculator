using Web_ProbabilityCalculator;
using Web_ProbabilityCalculator.Services;

namespace Tests
{
	public class CalculationTests
	{
		private CalculationService service;
		public CalculationTests()
		{
			service = new CalculationService(new AppSettings()
			{
				CacheConfiguration = new CacheConfiguration()
				{
					CalculationCacheMs = 900000,
					DefaultTimeoutMs = 450000
				}
			});
		}

		[Fact]
		public void TestWrongCalculationName() => Assert.False(service.GetCalculationResult("aaaaa", null).Success);

		[Fact]
		public void TestMissingParamsCombinedWith() => Assert.False(service.GetCalculationResult("CombinedWith", null).Success);

		[Fact]
		public void TestMissingParamsEither() => Assert.False(service.GetCalculationResult("Either", null).Success);

		[Fact]
		public void TestMissingParamEitherB() => Assert.False(service.GetCalculationResult("Either", new Dictionary<string, double>()
		{
			{ "A", 0.5d }
		}).Success);

		[Fact]
		public void TestMissingParamEitherA() => Assert.False(service.GetCalculationResult("Either", new Dictionary<string, double>()
		{
			{ "B", 0.5d }
		}).Success);

		[Fact]
		public void TestMissingCombinedWithParamB() => Assert.False(service.GetCalculationResult("CombinedWith", new Dictionary<string, double>()
		{
			{ "A", 0.5d }
		}).Success);

		[Fact]
		public void TestMissingCombinedWithParamA() => Assert.False(service.GetCalculationResult("CombinedWith", new Dictionary<string, double>()
		{
			{ "B", 0.5d }
		}).Success);

		[Fact]
		public void TestOutOfEitherRangeA() => Assert.False(service.GetCalculationResult("Either", new Dictionary<string, double>()
		{
			{ "A", -1d },
			{ "B", 0.5d }
		}).Success);

		[Fact]
		public void TestOutOfEitherRangeB() => Assert.False(service.GetCalculationResult("Either", new Dictionary<string, double>()
		{
			{ "A", 0.5d },
			{ "B", 2d }
		}).Success);

		//Success tests
		[Fact]
		public void TestSuccessValueEither() => Assert.Equal(0.75d, service.GetCalculationResult("Either", new Dictionary<string, double>()
		{
			{ "A", 0.5d },
			{ "B", 0.5d }
		}).Result, 5);


		//Success tests
		[Fact]
		public void TestSuccessValueCombinedWith() => Assert.Equal(0.25d, service.GetCalculationResult("CombinedWith", new Dictionary<string, double>()
		{
			{ "A", 0.5d },
			{ "B", 0.5d }
		}).Result, 5);
	}
}
