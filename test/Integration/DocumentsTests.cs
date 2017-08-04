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
    public abstract class DocumentsTestBase
    {
        protected Documents DocumentService { get; set; }

        protected Mock<IOnfidoHttpClient> HttpClient { get; set; }

        protected Uri UriUsed { get; set; }

        public DocumentsTestBase()
        {
            HttpClient = new Mock<IOnfidoHttpClient>();
            DocumentService = new Documents(new Requestor(HttpClient.Object));
        }

        [TestMethod]
        public void Should_have_used_correct_url()
        {
            Assert.AreEqual(UriUsed.Host, Onfido.Settings.Hostname);
        }

        [TestMethod]
        public virtual void Should_have_called_correct_endpoint()
        {
            throw new NotImplementedException("Test not implemented! needs overridden in derived class");
        }
    }

    [TestClass]
    public class When_calling_Documents_create_service : DocumentsTestBase
    {
        public When_calling_Documents_create_service()
        {
            var stubResponse = new HttpResponseMessage();
            stubResponse.Content = new StringContent(DocumentGenerator.Json());

            HttpClient.Setup(client => client.Post(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Returns(stubResponse)
                .Callback<Uri, object>((u, h) => { UriUsed = u; });
            HttpClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Throws(new Exception("Document create should use POST"));
        }

        [TestInitialize]
        public void CallService()
        {
            DocumentService.Create(DocumentGenerator.ApplicantId, DocumentGenerator.FileStream(), DocumentGenerator.Filename, DocumentGenerator.DocumentType);
        }

        [TestMethod]
        public override void Should_have_called_correct_endpoint()
        {
            Assert.AreEqual(UriUsed.PathAndQuery, string.Format("/{0}/applicants/{1}/documents", Settings.GetApiVersion(), DocumentGenerator.ApplicantId));
        }
    }
}
