using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace DesertCamel.BaseMicroservices.SuperBootstrap.Base
{
    public class CorrelationIdLogMiddleware : IMiddleware
    {
        private const string _CorrelationIdLogName = "CorrelationId";
        private readonly ICorrelationIdUtility _correlationIdUtility;

        public CorrelationIdLogMiddleware(
            ICorrelationIdUtility correlationIdUtility)
        {
            _correlationIdUtility = correlationIdUtility;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var correlationId = _correlationIdUtility.Get();
            using (LogContext.PushProperty(_CorrelationIdLogName, correlationId))
            {
                await next(context);
            }
        }
    }
}
