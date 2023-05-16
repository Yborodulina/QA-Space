using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanA.Web.Core.Http;

public interface IHttpClientWrapper
{
    /// <summary>
    ///     Sets an authorization header with a bearer token.
    /// </summary>
    /// <example>
    ///     <code>
    ///     await _httpClient.AddToken().PostRequest("example.com", model);
    ///     or
    ///     await _httpClient.AddToken("username", "password").PostRequest("example.com", model);
    /// </code>
    /// </example>
    /// <param name="userName">UserName</param>
    /// <param name="password">Password</param>
    Task<IHttpClientWrapper> AddToken(string userName = "", string password = "!");

    #region Http Post

    Task<T> PostRequest<T>(string url, string body, string token = null, Dictionary<string, string> customHeaders = null);

    Task<T> PostRequest<T>(string url, object body, string token = null, Dictionary<string, string> customHeaders = null);

    Task PostRequest(string url, string body, string token = null, Dictionary<string, string> customHeaders = null);

    Task PostRequest(string url, object body, string token = null, Dictionary<string, string> customHeaders = null);

    Task<string> GetRequest(string url, string body = null, string token = null, Dictionary<string, string> customHeaders = null);

    Task<string> GetRequest(string url, Dictionary<string, string> queryString, string token = null, Dictionary<string, string> customHeaders = null);

    Task<string> DeleteRequest(string url, int id, string token = null, Dictionary<string, string> customHeaders = null);

    #endregion
}