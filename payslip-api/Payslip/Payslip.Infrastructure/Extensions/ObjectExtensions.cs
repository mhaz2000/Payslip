using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Payslip.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object data)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }
    }
}
