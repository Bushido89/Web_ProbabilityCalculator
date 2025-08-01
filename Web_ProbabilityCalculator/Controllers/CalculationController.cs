using Microsoft.AspNetCore.Mvc;
using Web_ProbabilityCalculator.Models.CoreModels;
using Web_ProbabilityCalculator.Models.ViewModels;
using Web_ProbabilityCalculator.Services;

namespace Web_ProbabilityCalculator.Controllers
{
	public class CalculationController : Controller
	{
		private ICalculationService service;
		public CalculationController(ICalculationService service)
		{
			this.service = service;
		}

		[HttpPost]
		public CalculationResult GetCalculationResult([FromBody] CalculationResultReq req) => service.GetCalculationResult(req.CalculationName, req.QueryParameters);

		//Marked as async to simulate when this would be either a cache hit or a database call
		public async Task<List<Calculation>> GetCalculations() => await service.GetCalculations();
	}
}
