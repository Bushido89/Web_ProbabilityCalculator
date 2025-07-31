using Web_ProbabilityCalculator.Models.ViewModels;

namespace Web_ProbabilityCalculator.Models.CoreModels
{
	public class CalculationParameter(double minValue = 0, double maxValue = 1)
	{
		public string Name { get; set; }

		//Consideration: Should decimal be used here to avoid floating point errors, or since the calculation is fuzzy would double be better for performance
		public double MinValue { get; } = minValue;

		public double MaxValue { get; } = maxValue;

		public CalculationResult CheckValidation(double input)
		{;
			return input switch
			{
				double v when v < MinValue => AssignFalure($@"{Name} parameter value too low"),
				double v when v > MaxValue => AssignFalure($@"{Name} parameter value too high"),
				_ => new CalculationResult() { Success = true }
			};
		}

		public static CalculationResult AssignFalure(string message, int code = 401)
		{
			return new CalculationResult()
			{
				Success = false,
				Message = message,
				Code = code
			};
		}

		public static CalculationResult AssignSuccess(double value, string message = "")
		{
			return new CalculationResult()
			{
				Success = true,
				Code = 200,
				Result = value
			};
		}
	}
}
