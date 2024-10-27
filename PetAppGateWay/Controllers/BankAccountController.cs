using Mediator_BankAccount.Commands;
using Mediator_BankAccount.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAppGateWay.Enams;

namespace PetAppGateWay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : Controller
    {
        private readonly IMediator mediator;
        public BankAccountController(IMediator mediator) => this.mediator = mediator;

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> GetBankAccount()
        {
            var response = await mediator.Send(new BankAccountQuery(User.FindFirst(nameof(Claims.Id)).Value));
            if (response is not null)
                return Json(response);
            else
                return Json(await mediator.Send(new BankAccountCommand(User.FindFirst(nameof(Claims.Id)).Value)));
        }

        [HttpGet]
        [Route("[action]/{sum}")]
        [Authorize]
        public async Task<IActionResult> ToUpAccount(int sum)
        {
            await mediator.Send(new TopUaAccountCommand(User.FindFirst(nameof(Claims.Id)).Value, sum));
            return Json(true);
        }
    }
}
