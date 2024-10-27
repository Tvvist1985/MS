using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OutputCaching;

namespace PetAppGateWay.Filters
{
    public class DropCacheByTagFilter : Attribute, IAsyncResourceFilter
    {
        private readonly string tagName;
        public DropCacheByTagFilter(string tagName) => this.tagName = tagName;   
        
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            await next();
            IOutputCacheStore distributedCache = context.HttpContext.RequestServices.GetService(typeof(IOutputCacheStore)) as IOutputCacheStore;

            if (context.HttpContext.Response.StatusCode == 200)
                await distributedCache.EvictByTagAsync(tagName, default);
        }
    } 
}
