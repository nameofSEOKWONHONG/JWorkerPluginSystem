using System.Collections;
using System.Collections.Generic;
using eXtensionSharp;
using RestSharp;

namespace KakaoSenderPlugin
{
    public class KakaoMessageRestApiExecutor
    {
        private readonly string _accessToken = string.Empty;
        public KakaoMessageRestApiExecutor(string accessToken)
        {
            _accessToken = accessToken;
        }

        public bool Execute()
        {
            var client = new RestClient($"");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "");
            request.AddParameter("template_object", "");
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                response.Content.xToEntity<RequestBody>();
            }

            return true;
        }
    }
}