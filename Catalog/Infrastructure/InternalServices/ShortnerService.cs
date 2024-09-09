namespace Catalog.Infrastructure.InternalServices
{
    public class ShortnerService(ILogger<ShortnerService> logger, HttpClient httpClient,IOptions<CatalogOptions> options)
    {
        private readonly ShortnerOptions _options = options.Value.InternalsServices.Shortner;
        private readonly ILogger<ShortnerService> _logger = logger;
        public async Task<string> GetShortnerUrl(string longUrl)
        {
            _logger.LogInformation("Test log");
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
