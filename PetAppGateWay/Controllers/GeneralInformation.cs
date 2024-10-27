using MediatoR_CurrencyRate.Models;
using Mediator_Email.Commands;
using MediatR;
using Micro_Account.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PetAppGateWay.Enams;

namespace PetAppGateWay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralInformationController : Controller
    {
        private readonly IDistributedCache distributedCache;
        public GeneralInformationController(IDistributedCache distributedCache) => this.distributedCache = distributedCache;

        [HttpGet]
        [Route("[action]")]  
        public async Task<IActionResult> GetCurrencyRate() => Ok(distributedCache.GetString(nameof(RadisKeys.Currency)));       
    }
}
