using MediatoR_CurrencyRate.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PetAppGateWay.Enams;
using System.Text.Json;

namespace PetAppGateWay.Services.Workers
{
    public class CurrencyWorker : BackgroundService
    {
        private readonly IMediator mediator;
        private readonly IDistributedCache distributedCache;
        
        public CurrencyWorker(IMediator mediator, IDistributedCache distributedCache)
        {
            this.mediator = mediator;
            this.distributedCache = distributedCache;
        }    
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                var res = await mediator.Send(new CurrencyRateQuery());
                var json = JsonSerializer.Serialize(res);               
                distributedCache.SetString(nameof(RadisKeys.Currency), json);

                await Task.Delay(TimeSpan.FromMinutes(10));
            }            
        }       
    }
}
