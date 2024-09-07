namespace Catalog.Infrastructure.InternalServices
{
    public class ShortnerService(HttpClient httpClient,IOptions<CatalogOptions> options)
    {
        private readonly ShortnerOptions _options = options.Value.InternalsServices.Shortner;
        public async Task<string> GetShortnerUrl(string longUrl)
        {
            var body = new
            {
                Url = longUrl,
            };
            var response = await httpClient.PostAsJsonAsync(_options.Shorten,body);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
