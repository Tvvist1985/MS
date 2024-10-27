using Mediator_Email.Commands;
using MediatR;
using Micro_Account.Commands;
using Micro_Account.Queries;
using Micro_Person.Queries;
using Microsoft.AspNetCore.Mvc;
using PetAppGateWay.Enams;
using PetAppGateWay.Services.Gwt;
using Microsoft.AspNetCore.OutputCaching;
namespace PetAppGateWay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator mediator;
        private readonly IOutputCacheStore cacheStore;

        public AccountController(IMediator mediator, IOutputCacheStore cacheStore)
        {
            this.mediator = mediator;
            this.cacheStore = cacheStore;
        }  
        
        [HttpGet]
        [Route("[action]/{email}/{number}")]
        public async Task<IActionResult> GetCodeFromEmail(string email, string number)
        {
            var res = await mediator.Send(new CheckingOnUniqueQuery(email, number));

            if (res.EmailError || res.NumberError)
                return Json(res);
            else
            {
                string code = await mediator.Send(new SMPTCommand(email));
                res.CodeFroeEmail = code;

                await Console.Out.WriteLineAsync(code);

                return Json(res);            
            }
        }

        [HttpPost]
        [Route("[action]")]       
        public async Task<IActionResult> AddOrUpdateAccount([FromBody] AddOrUpdateCommand account)
        {
            var response = await mediator.Send(account);

            if (response.Id is not null)
                response.JWT = AddJWT.AddToken(response.Id, nameof(Roles.Person));
            else if(response.Success)
                await cacheStore.EvictByTagAsync(nameof(CashTag.GetUser) , default);

            return Json(response);
        }

        [HttpGet]
        [Route("[action]")]
        [OutputCache(PolicyName = nameof(CashPolicy.GetUser))]
        public async Task<IActionResult> GetAccount()
        {
            await cacheStore.EvictByTagAsync(nameof(CashTag.GetUser), default);

            var response = await mediator.Send(new AccountQuery(User.FindFirst(nameof(Claims.Id)).Value));
            if (response is not null)
                return Json(response);
            else
               return NotFound();
        }

        [HttpGet]
        [Route("[action]/{login}/{password}")]      
        public async Task<IActionResult> Login(string login, string password)
        {
            var response = await mediator.Send(new AccountQuery(login, password));
            if(response is not null)
                 response.JWT = AddJWT.AddToken(response.Id.ToString(), nameof(Roles.Person));

            return Json(response);
        }
    }
}
