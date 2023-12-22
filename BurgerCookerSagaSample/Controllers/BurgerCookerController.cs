using BurgerCookerSagaSample.Contracts;
using BurgerCookerSagaSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace BurgerCookerSagaSample.Controllers
{
	public class BurgerCookerController : ControllerBase
	{
		private readonly ICookBurgerService _cookBurgerService;

        public BurgerCookerController(ICookBurgerService cookBurgerService)
        {
            _cookBurgerService = cookBurgerService;
        }

        [HttpPost]
		[Route("api/burgercooker/cook")]
		public async Task<IActionResult> CookBurger([FromBody] CookBurger cookBurger)
		{
			await _cookBurgerService.SendCookBurgerMessage(cookBurger);
			return Ok();
		}
	}
}
