namespace Web_ProbabilityCalculator.Models.ViewModels
{
	public class CalculationResult
	{
		public bool Success { get; set; }

		public int Code { get; set; }

		public double Result { get; set; } = -1;

		public string Message { get; set; }


	}
}
