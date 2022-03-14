using FundaAssignment.WebApp.Configurations;
using FundaAssignment.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// registers funda-api http client. applies rate limit + retries on error
var fundaApiConfiguration = builder.Configuration.GetSection(nameof(FundaApiConfiguration)).Get<FundaApiConfiguration>();
builder.Services.AddFundaHttpClient(fundaApiConfiguration);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapDefaultControllerRoute();
app.Run();
