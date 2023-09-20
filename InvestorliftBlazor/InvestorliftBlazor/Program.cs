using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using InvestorliftBlazor.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<HouseService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        IConfigurationSection googleAuthNSection = config.GetSection("Authentication:Google");
        options.ClientId = googleAuthNSection["ClientId"];
        options.ClientSecret = googleAuthNSection["ClientSecret"];

        options.ClaimActions.MapJsonKey("picture", "picture", "url");
        options.ClaimActions.MapJsonKey("urn:google:profile", "link");
        options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
    })
    .AddFacebook(options =>
    {
        IConfigurationSection fbAuthNSection = config.GetSection("Authentication:Facebook");
        options.ClientId = fbAuthNSection["AppId"];
        options.ClientSecret = fbAuthNSection["AppSecret"];
        options.Fields.Add("picture");
        options.Events = new OAuthEvents
        {
            OnCreatingTicket = context =>
            {
                var pictureUrl = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").ToString();
                context.Identity.AddClaim(new Claim("picture", pictureUrl));
                return Task.CompletedTask;
            }
        };
    });
// From: https://github.com/aspnet/Blazor/issues/1554
// Adds HttpContextAccessor used to determine if a user is logged in and what their username is
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();
// Required for HttpClient support in the Blazor Client project
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();

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

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

