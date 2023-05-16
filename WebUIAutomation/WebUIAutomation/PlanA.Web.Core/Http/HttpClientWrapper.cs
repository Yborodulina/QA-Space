using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace PlanA.Web.Core.Http;

public class HttpClientWrapper : IHttpClientWrapper
{
    private const string ApplicationJson = "application/json";

    private static readonly HttpClient _client = new()
    {
        Timeout = TimeSpan.FromSeconds(100),
        DefaultRequestHeaders =
        {
            Accept =
            {
                new MediaTypeWithQualityHeaderValue(ApplicationJson)
            }
        }
    };

    private string BearerToken;

    // getting token for Auth2.0
    /// <inheritdoc />
    public async Task<IHttpClientWrapper> AddToken(string userName = "", string password = "")
    {
        if (string.IsNullOrEmpty(BearerToken))
        {
            var identityClient = new HttpClient();

            // request token
            BearerToken = (await identityClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = "",
                ClientId = "",
                ClientSecret = "",
                Scope = "",
                UserName = userName,
                Password = password
            })).AccessToken;
        }

        _client.SetBearerToken(BearerToken);

        return this;
    }

    public async Task<T> PostRequest<T>(string url, string body, string token = null, Dictionary<string, string> customHeaders = null)
    {
        var response = await SendRequest(HttpMethod.Post, url, new StringContent(body, Encoding.UTF8, ApplicationJson), token, customHeaders);
        var model = JsonConvert.DeserializeObject<T>(response);

        return model;
    }

    public async Task<T> PostRequest<T>(string url, object body, string token = null, Dictionary<string, string> customHeaders = null)
    {
        var response = await SendRequest(HttpMethod.Post, url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, ApplicationJson), token,
            customHeaders);
        var model = JsonConvert.DeserializeObject<T>(response);

        return model;
    }

    public async Task PostRequest(string url, string body, string token = null, Dictionary<string, string> customHeaders = null)
    {
        await SendRequest(HttpMethod.Post, url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, ApplicationJson), token, customHeaders);
    }

    public async Task PostRequest(string url, object body, string token = null, Dictionary<string, string> customHeaders = null)
    {
        await SendRequest(HttpMethod.Post, url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, ApplicationJson), token, customHeaders);
    }

    public async Task<string> GetRequest(string url, string body = null, string token = null, Dictionary<string, string> customHeaders = null)
    {
        var response = await SendRequest(HttpMethod.Get, url, null, token, customHeaders);
        return response;
    }

    public async Task<string> GetRequest(string url, Dictionary<string, string> queryString, string token = null, Dictionary<string, string> customHeaders = null)
    {
        //Convert dictionary to query string
        var queryParams = string.Empty;

        using (var content = new FormUrlEncodedContent(queryString))
        {
            queryParams = await content.ReadAsStringAsync();
        }

        //Append query string to base api url.
        url = $"{url}?{queryParams}";

        var response = await SendRequest(HttpMethod.Get, url, null, token, customHeaders);

        return response;
    }

    public async Task<string> DeleteRequest(string url, int id, string token = null, Dictionary<string, string> customHeaders = null)
    {
        var response = await SendRequest(HttpMethod.Delete, $"{url}/{id}", null, token, customHeaders);

        return response;
    }

    private async Task<string> SendRequest(HttpMethod method, string url, HttpContent body, string token = null, Dictionary<string, string> headers = null)
    {
        HttpResponseMessage response = null;

        using (var requestMessage = new HttpRequestMessage(method, url))
        {
            //Set content and content type
            requestMessage.Content = body;

            //Set not default customHeaders
            if (headers != null)
                foreach (var header in headers)
                    requestMessage.Headers.Add(header.Key, header.Value);

            response = await _client.SendAsync(requestMessage);
        }

        if (response is { IsSuccessStatusCode: true }) return await response.Content.ReadAsStringAsync();

        throw new HttpRequestException("There was an error during the call." +
                                       $" Response is not null:{response != null}." +
                                       $" Response code is: {response?.StatusCode}." +
                                       $" Response message: {await response.Content.ReadAsStringAsync()}");
    }
}