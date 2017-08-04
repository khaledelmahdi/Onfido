using System;
using System.Net.Http;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Onfido.Http;
using Onfido.Resources;
using Onfido.Test.Setup;

namespace Onfido.Test.Integration
{
    public abstract class ApplicantsTestBase
    {
        protected Applicants ApplicantService { get; set; }

        protected Mock<IOnfidoHttpClient> HttpClient { get; set; }

        protected Uri UriUsed { get; set; }

        protected int PageUsed { get; set; }

        protected int PerPageUsed { get; set; }

        public ApplicantsTestBase()
        {
            HttpClient = new Mock<IOnfidoHttpClient>();
            ApplicantService = new Applicants(new Requestor(HttpClient.Object));
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
    public class When_calling_Applicants_create_service : ApplicantsTestBase
    {

        public When_calling_Applicants_create_service() : base()
        {
            var stubResponse = new HttpResponseMessage();
            stubResponse.Content = new StringContent(ApplicantGenerator.Json());

            HttpClient.Setup(client => client.Post(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Returns(stubResponse)
                .Callback<Uri, object>((u, h) => { UriUsed = u; });
            HttpClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Throws(new Exception("Applicant create should use POST"));
        }

        [TestInitialize]
        public void CallService()
        {
            var applicant = ApplicantGenerator.Applicant(false);
            ApplicantService.Create(applicant);
        }

        [TestMethod]
        public override void Should_have_called_correct_endpoint()
        {
            Assert.AreEqual(UriUsed.PathAndQuery, string.Format("/{0}/{1}", Onfido.Settings.GetApiVersion(), "applicants"));
        }
    }

    [TestClass]
    public class When_calling_Applicants_find_service : ApplicantsTestBase
    {
        private const string _applicantFindId = "test123";

        public When_calling_Applicants_find_service() : base()
        {
            var stubResponse = new HttpResponseMessage();
            stubResponse.Content = new StringContent(ApplicantGenerator.Json());

            HttpClient.Setup(client => client.Post(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Throws(new Exception("Applicant find should use GET"));
            HttpClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Returns(stubResponse)
                .Callback<Uri>(c => { UriUsed = c; });
        }

        [TestInitialize]
        public void CallService()
        {
            ApplicantService.Find(_applicantFindId);
        }

        [TestMethod]
        public override void Should_have_called_correct_endpoint()
        {
            Assert.AreEqual(UriUsed.PathAndQuery, string.Format("/{0}/{1}/{2}", Onfido.Settings.GetApiVersion(), "applicants", _applicantFindId));
        }
    }

    [TestClass]
    public class When_calling_Applicants_all_service : ApplicantsTestBase
    {
        public When_calling_Applicants_all_service() : base()
        {
            var stubResponse = new HttpResponseMessage();
            stubResponse.Content = new StringContent(@"{ ""applicants"": [" + ApplicantGenerator.Json() + "]}");

            HttpClient.Setup(client => client.Post(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Throws(new Exception("Applicant all should use GET"));
            HttpClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Returns(stubResponse)
                .Callback<Uri>(c => { UriUsed = c; });
        }

        [TestInitialize]
        public void CallService()
        {
            ApplicantService.All();
        }

        [TestMethod]
        public override void Should_have_called_correct_endpoint()
        {
            var pathQuerySplit = UriUsed.PathAndQuery.Split('?');
            Assert.AreEqual(pathQuerySplit.Length, 2);

            var path = pathQuerySplit[0];
            Assert.AreEqual(path, string.Format("/{0}/{1}", Onfido.Settings.GetApiVersion(), "applicants"));

            var query = HttpUtility.ParseQueryString(pathQuerySplit[1]);
            Assert.AreEqual(Int32.Parse(query["page"]), PageUsed);
            Assert.AreEqual(Int32.Parse(query["per_page"]), PerPageUsed);
        }
    }

    [TestClass]
    public class When_calling_Applicants_all_service_with_pagination : ApplicantsTestBase
    {
        public When_calling_Applicants_all_service_with_pagination() : base()
        {
            var stubResponse = new HttpResponseMessage();
            stubResponse.Content = new StringContent(ApplicantGenerator.Json());

            HttpClient.Setup(client => client.Post(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Throws(new Exception("Applicant all should use GET"));
            HttpClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Returns(stubResponse)
                .Callback<Uri>(c => { UriUsed = c; });

            PageUsed = 2;
            PerPageUsed = 20;
        }

        [TestInitialize]
        public void CallService()
        {
            ApplicantService.All(PageUsed, PerPageUsed);
        }

        [TestMethod]
        public override void Should_have_called_correct_endpoint()
        {
            var pathQuerySplit = UriUsed.PathAndQuery.Split('?');
            Assert.AreEqual(pathQuerySplit.Length, 2);

            var path = pathQuerySplit[0];
            Assert.AreEqual(path, string.Format("/{0}/{1}", Onfido.Settings.GetApiVersion(), "applicants"));

            var query = HttpUtility.ParseQueryString(pathQuerySplit[1]);
            Assert.AreEqual(Int32.Parse(query["page"]), PageUsed);
            Assert.AreEqual(Int32.Parse(query["per_page"]), PerPageUsed);
        }
    }
}
