using System;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using PostmanTests.DTO;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace PostmanTests
{
    [TestFixture]
    public class IntegrationTests
    {

        protected const string BaseUrl = "https://jsonblob.com";
        protected string FullUrlWithAlias
        {
            get { return BaseUrl + "/api/jsonBlob"; }
        }

        protected IDisposable Server { get; set; }

        protected HttpClient HttpClient { get; set; }

        //This method will be called before each test
        [SetUp]
        public void SetUp()
        {
            HttpClient = new HttpClient();
            Console.WriteLine("I'm doing something to setup the system ready for the test");
        }

        private HttpRequestMessage CreateRequest(string url, HttpMethod method)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;

            return request;
        }

        private HttpRequestMessage CreateRequest(string url, HttpMethod method, string jsonString)
        {
            HttpRequestMessage request = CreateRequest(url, method);
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return request;
        }

        [Test]
        public void PostShouldReturn201andContent()
        {
            BlogPost blogpost = new BlogPost { Content = "my new post :)" };
            string body = String.Empty;

            var obj = JsonConvert.SerializeObject(blogpost);
            HttpRequestMessage request = CreateRequest(FullUrlWithAlias, HttpMethod.Post, obj);
            HttpResponseMessage response = HttpClient.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStreamAsync().Result;
                    StreamReader reader = new StreamReader(stream);
                    body = reader.ReadToEnd();
                 }

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(obj, body);
        }

        //This method will be called after each test
        [TearDown]
        public void Teardown()
        {
            HttpClient = null;
           // Console.WriteLine("I'm doing something to tidy up after the test");
        }

    }
}

       