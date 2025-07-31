namespace Web_ProbabilityCalculator.Models.ViewModels
{
	public class CalculationResultReq
	{
		public string CalculationName { get; set; }

		public Dictionary<string, double> QueryParameters { get; set; }
	}
}
