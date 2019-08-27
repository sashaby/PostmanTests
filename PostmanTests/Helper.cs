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
    public class Helper
    {
        public HttpRequestMessage CreateRequest(string url, HttpMethod method)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;

            return request;
        }

        public HttpRequestMessage CreateRequest(string url, HttpMethod method, string jsonString)
        {
            HttpRequestMessage request = CreateRequest(url, method);
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return request;
        }

        public ResponseData ExecutePost(HttpClient client, string url, string jsonstring)
        {
            string body = String.Empty;
            ResponseData respData = new ResponseData();
            HttpRequestMessage request = CreateRequest(url, HttpMethod.Post, jsonstring);
            HttpResponseMessage response = client.SendAsync(request).Result;
            respData.RespMessage = response;

            if (response.IsSuccessStatusCode)
            {
                var stream = response.Content.ReadAsStreamAsync().Result;
                StreamReader reader = new StreamReader(stream);
                body = reader.ReadToEnd();
                respData.ContentStream = body;
            }
            return respData;
        }

        public ResponseData ExecutePut(HttpClient client, string url, string jsonstring)
        {
            string body = String.Empty;
            ResponseData respData = new ResponseData();
            HttpRequestMessage request = CreateRequest(url, HttpMethod.Put, jsonstring);
            HttpResponseMessage response = client.SendAsync(request).Result;
            respData.RespMessage = response;

            if (response.IsSuccessStatusCode)
            {
                var stream = response.Content.ReadAsStreamAsync().Result;
                StreamReader reader = new StreamReader(stream);
                body = reader.ReadToEnd();
                respData.ContentStream = body;
            }
            return respData;
        }

        public ResponseData ExecuteGet(HttpClient client, string url)
        {
            string body = String.Empty;
            ResponseData respData = new ResponseData();
            HttpRequestMessage request = CreateRequest(url, HttpMethod.Get);
            HttpResponseMessage response = client.SendAsync(request).Result;
            respData.RespMessage = response;

            if (response.IsSuccessStatusCode)
            {
                var stream = response.Content.ReadAsStreamAsync().Result;
                StreamReader reader = new StreamReader(stream);
                body = reader.ReadToEnd();
                respData.ContentStream = body;
            }
            return respData;
        }

        public ResponseData ExecuteDelete(HttpClient client, string url)
        {
            string body = String.Empty;
            ResponseData respData = new ResponseData();
            HttpRequestMessage request = CreateRequest(url, HttpMethod.Delete);
            HttpResponseMessage response = client.SendAsync(request).Result;
            respData.RespMessage = response;            
            return respData;
        }
    }

    public class ResponseData
    {
        public HttpResponseMessage RespMessage { get; set; }
        public string ContentStream { get; set; }

    }
}



