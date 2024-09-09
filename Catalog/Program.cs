using Catalog;
using Catalog.Infrastructure.InternalServices;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.LoggerConfigure();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ShortnerService>(configure =>
{
    var shortenOptions = builder.Configuration.GetSection(ShortnerOptions.SectionName).Get<ShortnerOptions>();
    if (shortenOptions is null)
    {
        throw new ArgumentNullException(nameof(shortenOptions));
    }
    configure.BaseAddress = new Uri(shortenOptions.BaseUrl);
});

builder.Services.AddScoped<ShortnerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("/api/v1/brands")
   .WithTags("Brand APIs")
   .MapCatalogBrandEndpoints();

app.MapGroup("/api/v1/categories")
   .WithTags("Category APIs")
   .MapCatalogCategoryEndpoints();

app.MapGroup("/api/v1/items")
   .WithTags("Item APIs")
   .MapCatalogItemEndpoints();

app.Run();


