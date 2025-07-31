using Web_ProbabilityCalculator.Models.ViewModels;

namespace Web_ProbabilityCalculator.Models.CoreModels
{
	public class Calculation
	{
		public string Name { get; set; }

		public Dictionary<string, CalculationParameter> CalculationParameters { get; set; } = new();

		public delegate CalculationResult CalculationDelegate(Dictionary<string, double> inputs, Calculation calculation);
		public CalculationDelegate CalculationFunc { get; set; }

		public CalculationResult CheckParameters(Dictionary<string, double> queryParameters)
		{
			var res = new CalculationResult() { Success = true };
			//Check all mandatory parameters have been provided
			if(queryParameters == null)
			{
				return CalculationParameter.AssignFalure("queryParameters cannot be null");
			}
			var missing = CalculationParameters.Keys.Except(queryParameters?.Keys);
			if (missing.Count() > 0)
			{
				return CalculationParameter.AssignFalure($@"Following mandatory parameters are missing: {string.Join(',', missing)}");
			}

			//Check values of parameters
			foreach (var qp in queryParameters)
			{
				if (CalculationParameters.TryGetValue(qp.Key, out var param))
				{
					res = param.CheckValidation(qp.Value);
					if(!res.Success)
					{
						return res;
					}
				}
			}
			return res;
		}
	}
}
