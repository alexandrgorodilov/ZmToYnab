using System;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using ZmToYnab.Models;
using ZmToYnab.Models.ZM;

namespace ZmToYnab.Services
{
    public class ZenMoneyService: IZenMoneyService
    {
        private readonly IConfiguration _config;
        public ZenMoneyService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public ZmDiff GetDiff()
        {
            var authResponse = GenerateAutorizationResponse();
            var code = GetAuthorizeCode(authResponse);
            var content = GetAccessToken(code);
            var tokenResult = JsonConvert.DeserializeObject<TokenResult>(content);
            var accessToken = tokenResult.access_token;
            var diff = GetDiff(accessToken);
            return JsonConvert.DeserializeObject<ZmDiff>(diff);
        }

        IRestResponse GenerateAutorizationResponse()
        {
            var restclient = new RestClient(_config["ZenMoney:Uri:BaseUrl"]);
            RestRequest request = new RestRequest(_config["ZenMoney:Uri:AuthorizationEndpoint"]) { Method = Method.GET };
            request.AddParameter("client_id", _config["ZenMoney:ClientId"]);
            request.AddParameter("response_type", "code");
            request.AddParameter("redirect_uri", _config["ZenMoney:Uri:RedirectUri"]);
            return restclient.Execute(request);
        }

        IRestResponse GetPostResponse(IRestResponse response)
        {
            var cookie = response.Cookies.First();
            var restclient = new RestClient(_config["ZenMoney:Uri:BaseUrl"]);
            var request = new RestRequest(_config["ZenMoney:Uri:AuthorizationEndpoint"]) { Method = Method.POST };
            request.AddParameter(cookie.Name, cookie.Value, ParameterType.Cookie);
            request.AddParameter("username", _config["ZenMoney:Username"]);
            request.AddParameter("password", _config["ZenMoney:Password"]);
            request.AddParameter("auth_type_password", "Sign In");
            return restclient.Execute(request);
        }

        string GetAuthorizeCode(IRestResponse response)
        {
            var tResponse = GetPostResponse(response);
            var responseUrl = tResponse.Headers.First(x => x.Name.Contains("Location")).Value.ToString();
            var nameValueCollection = HttpUtility.ParseQueryString(new Uri(responseUrl).Query);
            var keyValuePairs = nameValueCollection.AllKeys.ToDictionary(t => t, t => nameValueCollection[t]);
            return keyValuePairs["code"];
        }

        string GetAccessToken(string code)
        {
            var restclient = new RestClient(_config["ZenMoney:Uri:BaseUrl"]);
            RestRequest request = new RestRequest(_config["ZenMoney:Uri:TokenEndpoint"]) { Method = Method.POST };
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("client_id", _config["ZenMoney:ClientId"]);
            request.AddParameter("client_secret", _config["ZenMoney:ClientSecret"]);
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", _config["ZenMoney:Uri:RedirectUri"]);

            var restResponse = restclient.Execute(request);
            return restResponse.Content;
        }

        string GetDiff(string accessToken)
        {
            var restclient = new RestClient(_config["ZenMoney:Uri:BaseUrl"]);
            var request = new RestRequest(_config["ZenMoney:Uri:Diff"]) { Method = Method.POST };
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            var body = new
            {
                currentClientTimestamp = Helper.ClientUnixTimeStamp,
                serverTimestamp = 0
            };
            request.AddJsonBody(body);

            return restclient.Execute(request).Content;
        }
    }
}