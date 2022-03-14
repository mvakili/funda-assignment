using FundaAssignment.WebApp.Configurations;
using FundaAssignment.WebApp.Extensions;
using FundaAssignment.WebApp.Repositories;
using FundaAssignment.WebApp.Repositories.Contracts;
using FundaAssignment.WebApp.Services;
using FundaAssignment.WebApp.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// registers funda-api http client. applies rate limit + retries on error
var fundaApiConfiguration = builder.Configuration.GetSection(nameof(FundaApiConfiguration)).Get<FundaApiConfiguration>();
builder.Services.AddFundaHttpClient(fundaApiConfiguration);

// registers repositories
builder.Services.AddSingleton<IObjectsRepository, ObjectsRepository>();

// registers logic services
builder.Services.AddSingleton<IObjectsService, ObjectsService>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapDefaultControllerRoute();
app.Run();
