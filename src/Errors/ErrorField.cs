using System.Collections.Generic;
using Newtonsoft.Json;

namespace Onfido.Errors
{
    public class ErrorField
    {
        [JsonProperty("messages")]
        public IEnumerable<string> Messages;
    }
}