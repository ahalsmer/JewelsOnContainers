namespace WebMvc.Infrastructure
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri,
            string authorizationToken = null, string authorizationMethod = "Bearer");
    }
}
