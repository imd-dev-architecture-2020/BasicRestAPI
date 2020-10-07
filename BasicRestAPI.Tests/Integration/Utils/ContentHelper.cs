using System.Net.Http;
using System.Text;
// we use Newtonsoft for JSON manipulation here; this is not build in into the .NET core ecosystem (but is widely used).
// If you start from a clean project it's better to use System.Text.Json as it's both faster and more lightweight.
using Newtonsoft.Json;

namespace BasicRestAPI.Tests.Integration.Utils
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
            => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }
}