using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ConsumeQnAService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------------Welcome to MArgie's Travel Portal---------");

            new QnAService().getAnswers();
        }
    }

    class QnAService
    {
        private const string endpointVar = "https://hack-sid-qnamaker.azurewebsites.net/";
        private const string endpointKeyVar = "5b37bdd6-2815-4a60-baf6-d87dd3f30e59";
        private const string kbIdVar = "9c95e5f9-e1e3-4dfa-90b9-4ba5a8f918cd";

        // Your QnA Maker resource endpoint.
        // From Publish Page: HOST
        // Example: https://YOUR-RESOURCE-NAME.azurewebsites.net/
        private static readonly string endpoint = endpointVar;
        // Authorization endpoint key
        // From Publish Page
        // Note this is not the same as your QnA Maker subscription key.
        private static readonly string endpointKey = endpointKeyVar;
        private static readonly string kbId = kbIdVar;
        public void getAnswers()
        {
            var uri = endpoint + "/qnamaker/knowledgebases/" + kbId + "/generateAnswer";
            string question = string.Empty;
            Console.WriteLine("Please ask your question. Enter quit to exit the application");
            while (question.ToLower() != "quit")
            {
                question = Console.ReadLine();
                getHttpResponse(question, uri);
            }
        }

        public void getHttpResponse(string question, string uri)
        {
            // JSON format for passing question to service
            // string question = @"{'question': 'Is the QnA Maker Service free?','top': 3}";

            // Create http client
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // POST method
                request.Method = HttpMethod.Post;

                // Add host + service to get full URI
                request.RequestUri = new Uri(uri);

                Dictionary<string, object> requestData = new Dictionary<string, object>
                {
                    ["question"] = question
                };
                // set question
                request.Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                // set authorization
                request.Headers.Add("Authorization", "EndpointKey " + endpointKey);

                // Send request to Azure service, get response
                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Output JSON response
                Console.WriteLine(jsonResponse);

                Console.WriteLine("Please ask another question. Enter quit to exit the application");
                //Console.ReadKey();
            }
        }

    }
}

