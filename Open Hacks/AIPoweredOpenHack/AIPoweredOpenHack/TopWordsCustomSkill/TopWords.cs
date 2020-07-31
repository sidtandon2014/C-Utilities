using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using TopWordsCustomSkill.HelperClasses;
using System.Linq;

namespace TopWordsCustomSkill
{
    public static class TopWords
    {
        [FunctionName("TopWords")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            int totalWordsToReturn = 10;
            log.LogInformation("C# HTTP trigger function processed a request.");

            //-- Create a response object
            var response = new WebApiResponse
            {
                Values = new List<OutputRecord>()
            };


            string requestBody = new StreamReader(req.Body).ReadToEnd();
            log.LogInformation("Request Body: {0}", requestBody);
            var data = JsonConvert.DeserializeObject<WebApiRequest>(requestBody);

            // Do some schema validation
            if (data == null)
            {
                return new BadRequestObjectResult("The request schema does not match expected schema.");
            }
            if (data.Values == null)
            {
                return new BadRequestObjectResult("The request schema does not match expected schema. Could not find values array.");
            }

            foreach (var record in data.Values)
            {
                if (record == null || record.RecordId == null) continue;

                OutputRecord responseRecord = new OutputRecord
                {
                    RecordId = record.RecordId
                };

                try
                {
                    responseRecord.Data = GetTopWordsInMergedText(record.Data.MergedText, totalWordsToReturn);
                }
                catch (Exception e)
                {
                    // Something bad happened, log the issue.
                    var error = new OutputRecord.OutputRecordMessage
                    {
                        Message = e.Message
                    };

                    responseRecord.Errors = new List<OutputRecord.OutputRecordMessage>
                    {
                        error
                    };
                }
                finally
                {
                    response.Values.Add(responseRecord);
                }
            }
            return (ActionResult)new OkObjectResult(response);
        }

        private static OutputRecord.OutputRecordData GetTopWordsInMergedText(string mergedText, int totalWordsToReturn)
        {
            var result = new OutputRecord.OutputRecordData()
            {
                TopWords = countWordFrequency(mergedText, totalWordsToReturn)
            };
            return result;
        }

        private static string[] countWordFrequency(string input, int totalWordsToReturn)
        {
            var words = input.Split(Delimiters.delimiters,
            StringSplitOptions.RemoveEmptyEntries);

            // Allocate new dictionary to store found words
            var wordDict = new Dictionary<string, int>();

            // Loop through all words
            foreach (string currentWord in words)
            {
                string lowerWord = currentWord.ToLower();

                if (!StopWords._stops.ContainsKey(lowerWord))
                {
                    if (wordDict.ContainsKey(lowerWord))
                        wordDict[lowerWord] = wordDict[lowerWord] + 1;
                    else
                        wordDict[lowerWord] = 1;
                }
            }
            return wordDict
                .OrderByDescending(Key => Key.Value)
                .Take(totalWordsToReturn)
                .Select(row => row.Key)
                .ToArray();
        }
    }
}
