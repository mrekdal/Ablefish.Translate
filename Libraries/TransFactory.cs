using Microsoft.Extensions.Configuration;
using System.Transactions;

namespace TransService
{
    public class TransFactory : ITransFactory
    {
        private readonly IConfiguration _configuration;

        public TransFactory(IConfiguration config)
        {
            _configuration = config;
        }

        Dictionary<string, ITransProcessor> _services = new();
        public bool TryGetService(string serviceName, out ITransProcessor? service)
        {
            service = null;
            if (_services.TryGetValue(serviceName, out service))
            {
                return true;
            }
            else
                switch (serviceName)
                {
                    case "Lara": service = new LaraService(_configuration); break;
                    case "DeepL": service = new DeepLService(_configuration); break;
                    default: return false;
                }
            _services.Add(serviceName, service);
            return true;
        }

    }
}
