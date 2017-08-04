using System;
using System.Net.Http;

namespace Onfido.Http
{
    public interface IOnfidoHttpClient
    {
        HttpResponseMessage Get(Uri uri);

        HttpResponseMessage Post(Uri uri, HttpContent payload);
    }
}