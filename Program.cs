using BlazorApp2.Data;
using BlazorApp2.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddHttpClient<IPostcodeSearch, PostcodeSearchService>(hc =>
{
	hc.BaseAddress = new Uri("https://api.addressy.com/Capture/Interactive/Find/v1.10/json3.ws?Key=GX11-FZ37-MG29-DW69");
});

builder.Services.AddAntDesign();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
