using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SampleWebAPICodeCamp.Data;
using SampleWebAPICodeCamp.Repository.Provider;
using Polly;
using Polly.Timeout;
using System.Threading;

namespace SampleWebAPICodeCamp.Repository
{
    public class IPRepository: IIPRepository
    {
        private readonly HttpClient _httpClient;
        public IPRepository()
        {
            _httpClient = new HttpClient();

        }

        public async Task<IPtoCountry> GetIpInfo(string ip)
        {
            var response = await GetValues("https://api123.ip2country.info/ip?" + ip);
            var result = await response.Content.ReadAsStringAsync();
            var output = JsonConvert.DeserializeObject<IPtoCountry>(result);
            return output;
        }

        private async Task<HttpResponseMessage> GetValues(string url, Dictionary<string, string> headervalues = null)
        {
            var request = CreateRequest(HttpMethod.Get, url, headervalues);
            //return await _httpClient.SendAsync(request);

            //var timeout = 5;

            //var PollyPolicy =
            //        Policy.TimeoutAsync(
            //            TimeSpan.FromSeconds(timeout),
            //            TimeoutStrategy.Optimistic);

            //var policyResult = await PollyPolicy.ExecuteAndCaptureAsync(async (ct) => {
            //    return await _httpClient.SendAsync(request, ct);
            //}, CancellationToken.None);

            //if (policyResult.Outcome == OutcomeType.Failure)
            //{
            //    string message = $"{request.RequestUri} - {request.Method} : execution timed out after {timeout} seconds";
            //    throw new Exception(message);
            //}

            //return policyResult.Result;






            //var retryCount = 2;
            //var PollyPolicyRety = Policy
            //.Handle<Exception>()
            //.WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            //var policyResultRetry = await PollyPolicyRety.ExecuteAndCaptureAsync(async (ct) => {
            //    Console.WriteLine("Call Retry");
            //    return await _httpClient.SendAsync(request, ct);
            //}, CancellationToken.None);

            //if (policyResultRetry.Outcome == OutcomeType.Failure)
            //{
            //    string message = $"{request.RequestUri} - {request.Method} : execution failed after {retryCount} retry";
            //    throw new Exception(message);
            //}

            //return policyResultRetry.Result;



            var policyFallBack = Policy<HttpResponseMessage>
            .Handle<Exception>().FallbackAsync<HttpResponseMessage>(async (ct) => {
                Console.WriteLine("In FallBack");
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK };
            });


            var policyResultFallBack = await policyFallBack.ExecuteAndCaptureAsync(async (ct) => {
                return await _httpClient.SendAsync(request, ct);
            }, CancellationToken.None);

            if (policyResultFallBack.Outcome == OutcomeType.Failure)
            {
                string message = $"{request.RequestUri} - {request.Method} : execution failed after fallback retry";
                throw new Exception(message);
            }

            return policyResultFallBack.Result;

        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string url, Dictionary<string, string> headervalues = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (headervalues != null)
            {
                foreach (var headers in headervalues)
                {
                    request.Headers.Add(headers.Key, headers.Value);
                }
            }
            return request;
        }
    }
}
