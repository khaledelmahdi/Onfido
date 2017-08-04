using Newtonsoft.Json;

namespace Onfido.Resources.InternalEntities
{
    public class OnfidoExceptionResponse
    {
        [JsonProperty("error")]
        public OnfidoApiError Error;
    }
}