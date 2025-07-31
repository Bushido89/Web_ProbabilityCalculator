using Web_ProbabilityCalculator.Models.CoreModels;
using Web_ProbabilityCalculator.Models.ViewModels;

namespace Web_ProbabilityCalculator.Dal
{
	//This would normally be used for database access, but since the spec advises not to use one, I've just used this as a kind of moq
	public class CalculationDao
	{
		private DateTime lastFetchedCalculationsUtc;
		private TimeSpan cacheRefreshThreshold;
		public CalculationDao(TimeSpan cacheRefreshTimeout)
		{
			lastFetchedCalculationsUtc = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
			cacheRefreshThreshold = cacheRefreshTimeout;
			//Force blocking call to ensure populated cache for tests
			Calculations = GetCalculations().Result;
		}

		public List<Calculation> Calculations { get; set; }
		public async Task<List<Calculation>> GetCalculations()
		{
			if(Calculations == null || DateTime.UtcNow - lastFetchedCalculationsUtc > cacheRefreshThreshold)
			{
				//Artifical delay to simulate db call
				await Task.Delay(1);
				//The idea is that this would come from a database rather than being hardcoded, which would contain C# code as a string
				//(or something else that could be transpiled to C# it could then be built on the fly with Roselyn, this way the calculations
				//can changed or added without further application deployments
				Calculations =
				[
					new()
					{
						Name = "CombinedWith",
						CalculationParameters =
						{
							{ "A", new () },
							{ "B", new () }
						},
						CalculationFunc = (queryParams, calc) =>
						{
							var res = calc.CheckParameters(queryParams);
							return !res.Success ? res : CalculationParameter.AssignSuccess(queryParams["A"] * queryParams["B"]);
						}
					},
					new()
					{
						Name = "Either",
						CalculationParameters =
						{
							{ "A", new () },
							{ "B", new () }
						},
						CalculationFunc = (param, calc) =>
						{
							var res = calc.CheckParameters(param);
							return !res.Success ? res : CalculationParameter.AssignSuccess(param["A"] + param["B"] - param["A"]*param["B"]);
						}
					}
				];
				lastFetchedCalculationsUtc = DateTime.UtcNow;
			}
			return Calculations;
		}
	}
}
