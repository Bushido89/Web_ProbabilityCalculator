using Web_ProbabilityCalculator.Dal;
using Web_ProbabilityCalculator.Models.CoreModels;
using Web_ProbabilityCalculator.Models.ViewModels;

namespace Web_ProbabilityCalculator.Services
{
	public interface ICalculationService
	{
		Task<List<Calculation>> GetCalculations();
		CalculationResult GetCalculationResult(string calculationName, Dictionary<string, double> queryParameters);
	}
	public class CalculationService : ICalculationService
	{
		private CalculationDao calculationDao;
		public CalculationService(IAppSettings appSettings)
		{
			calculationDao = new CalculationDao(TimeSpan.FromMilliseconds(appSettings.CacheConfiguration.CalculationCacheMs));
		}

		//Unit test not added as is simple data return with no parameters and no logic
		public async Task<List<Calculation>> GetCalculations() => await calculationDao.GetCalculations();


		public CalculationResult GetCalculationResult(string calculationName, Dictionary<string, double> queryParameters)
		{
			var calc = calculationDao.Calculations.FirstOrDefault(x => x.Name == calculationName);
			if (calc == null)
			{
				return CalculationParameter.AssignFalure("Calculation name does not exist");
			}
			return calc.CalculationFunc(queryParameters, calc);
		}
	}
}
