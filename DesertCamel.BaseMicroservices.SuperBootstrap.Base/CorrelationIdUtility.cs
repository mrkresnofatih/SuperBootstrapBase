using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertCamel.BaseMicroservices.SuperBootstrap.Base
{
    public class CorrelationIdUtility : ICorrelationIdUtility
    {
        private string _correlationId = Guid.NewGuid().ToString();

        public string Get() => _correlationId;

        public void Set(string correlationId)
        {
            _correlationId = correlationId;
        }
    }

    public interface ICorrelationIdUtility
    {
        string Get();
        void Set(string correlationId);
    }
}
