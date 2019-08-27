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
        protected Helper helper;
        
        
        //This method will be called before each test
        [SetUp]
        public void SetUp()
        {
            HttpClient = new HttpClient();
            helper = new Helper();
            Console.WriteLine("I'm doing something to setup the system ready for the test");
        }
        

        [Test]
        public void PostShouldReturn201andContent()
        {
            BlogPost blogpost = new BlogPost { Content = "my new post :)" };
            
            var obj = JsonConvert.SerializeObject(blogpost);
            ResponseData resp = helper.ExecutePost(HttpClient, FullUrlWithAlias, obj);


            Assert.AreEqual(resp.RespMessage.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(obj,resp.ContentStream);
        }

        [Test]
        public void GetShouldReturn200andContent()
        {
            BlogPost blogpost = new BlogPost { Content = "my GET post :)" };          
            string location = String.Empty;

            //create a post
            var obj = JsonConvert.SerializeObject(blogpost);
            ResponseData postResp = helper.ExecutePost(HttpClient, FullUrlWithAlias, obj);
            location = postResp.RespMessage.Headers.Location.ToString();

            //create a get
            ResponseData getReq = helper.ExecuteGet(HttpClient, location);
            
            Assert.AreEqual(getReq.RespMessage.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(obj, getReq.ContentStream);
        }

        [Test]
        public void PutShouldReturn200andUpdatedContent()
        {
            BlogPost blogpost = new BlogPost { Content = "my POST post :)" };
            string location = String.Empty;

            //create a post
            var obj = JsonConvert.SerializeObject(blogpost);
            ResponseData postResp = helper.ExecutePost(HttpClient, FullUrlWithAlias, obj);
            location = postResp.RespMessage.Headers.Location.ToString();

            //create PUT
            BlogPost blogpostUpdated = new BlogPost { Content = "my Updated post :)" };
            var objUpdated = JsonConvert.SerializeObject(blogpostUpdated);
            ResponseData putReq = helper.ExecutePut(HttpClient, location,objUpdated);

            Assert.AreEqual(putReq.RespMessage.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(objUpdated, putReq.ContentStream);
        }

        [Test]
        public void DeleteShouldReturn200()
        {
            BlogPost blogpost = new BlogPost { Content = "my Delete post :)" };
            string location = String.Empty;

            //create a post
            var obj = JsonConvert.SerializeObject(blogpost);
            ResponseData postResp = helper.ExecutePost(HttpClient, FullUrlWithAlias, obj);
            location = postResp.RespMessage.Headers.Location.ToString();

            //create a get
            ResponseData deleteReq = helper.ExecuteDelete(HttpClient, location);

            Assert.AreEqual(deleteReq.RespMessage.StatusCode, HttpStatusCode.OK);
        }


        //This method will be called after each test
        [TearDown]
        public void Teardown()
        {
            HttpClient.Dispose();
            HttpClient = null;          
        }

    }
}

       