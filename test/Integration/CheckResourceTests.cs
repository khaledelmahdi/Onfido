using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Onfido.Http;
using Onfido.Resources;
using Onfido.Test.Setup;

namespace Onfido.Test.Integration
{
    public abstract class ChecksTestBase
    {
        protected Checks CheckService { get; set; }

        protected Mock<IOnfidoHttpClient> HttpClient { get; set; }

        protected Uri UriUsed { get; set; }

        protected int PageUsed { get; set; }

        protected int PerPageUsed { get; set; }

        public ChecksTestBase()
        {
            HttpClient = new Mock<IOnfidoHttpClient>();
            CheckService = new Checks(new Requestor(HttpClient.Object));
            PageUsed = 1;
            PerPageUsed = 10;
        }

        [TestMethod]
        public void Should_have_used_correct_url()
        {
            Assert.AreEqual(UriUsed.Host, Onfido.Settings.Hostname);
        }

        [TestMethod]
        public virtual void Should_have_called_correct_endpoint()
        {
            throw new NotImplementedException("test not implemented! needs overridden in derived class");
        }
    }

    [TestClass]
    public class When_calling_Checks_create_service : ChecksTestBase
    {
        public When_calling_Checks_create_service()
        {
            var stubResponse = new HttpResponseMessage();
            stubResponse.Content = new StringContent(CheckGenerator.Json());

            HttpClient.Setup(client => client.Post(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Returns(stubResponse)
                .Callback<Uri, object>((u, h) => { UriUsed = u; });

            HttpClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Throws(new Exception("Check create should use POST"));
            
        }

        [TestInitialize]
        public void CallService()
        {
            CheckService.Create(CheckGenerator.ApplicantId, CheckGenerator.Check());
        }

        [TestMethod]
        public override void Should_have_called_correct_endpoint()
        {
            Assert.AreEqual(UriUsed.PathAndQuery, string.Format("/{0}/applicants/{1}/checks", Settings.GetApiVersion(), CheckGenerator.ApplicantId));
        }
    }

    [TestClass]
    public class When_calling_Checks_find_service : ChecksTestBase
    {
        public When_calling_Checks_find_service()
        {
            var stubResponse = new HttpResponseMessage();
            stubResponse.Content = new StringContent(CheckGenerator.Json());

            HttpClient.Setup(client => client.Post(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Throws(new Exception("Check find should use GET"));
            HttpClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Returns(stubResponse)
                .Callback<Uri>(c => { UriUsed = c; });
        }

        [TestInitialize]
        public void CallService()
        {
            CheckService.Find(CheckGenerator.ApplicantId, CheckGenerator.CheckId);
        }

        [TestMethod]
        public override void Should_have_called_correct_endpoint()
        {
            Assert.AreEqual(UriUsed.PathAndQuery, string.Format("/{0}/applicants/{1}/checks/{2}", Settings.GetApiVersion(), CheckGenerator.ApplicantId, CheckGenerator.CheckId));
        }
    }


    [TestClass]
    public class When_calling_Checks_all_service : ChecksTestBase
    {
        public When_calling_Checks_all_service()
        {
            var stubResponse = new HttpResponseMessage();
            stubResponse.Content = new StringContent(CheckGenerator.JsonList());

            HttpClient.Setup(client => client.Post(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Throws(new Exception("Check all should use GET"));
            HttpClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Returns(stubResponse)
                .Callback<Uri>(c => { UriUsed = c; });
        }

        [TestInitialize]
        public void CallService()
        {
            CheckService.All(CheckGenerator.ApplicantId);
        }

        [TestMethod]
        public override void Should_have_called_correct_endpoint()
        {
            Assert.AreEqual(UriUsed.PathAndQuery, string.Format("/{0}/applicants/{1}/checks", Settings.GetApiVersion(), CheckGenerator.ApplicantId));
        }
    }
}
