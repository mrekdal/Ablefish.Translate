using Ablefish.Blazor.Status;
using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using TranslateWebApp.Components;
using TranslateWebApp.Data;
using TranslateWebApp.Models;
using TranslateWebApp.Models.TranslateWebApp.Models;
using TransService;

namespace TranslateWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Bind Logto settings from configuration
            var logtoSettings = builder.Configuration.GetSection("Logto").Get<LogtoSettings>();

            // Register it for DI if needed
            builder.Services.Configure<LogtoSettings>(builder.Configuration.GetSection("Logto"));

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped<IWebUser, WebUser>();
            builder.Services.AddScoped<IDataContext, DataContext>();
            builder.Services.AddSingleton<ITransFactory, TransFactory>();
            builder.Services.AddTransient<IStatusMessage, StatusMessage>();

            #region Set up LogTo Authentication

            builder.Services.AddLogtoAuthentication(options =>
            {
                options.Endpoint = logtoSettings?.Endpoint ?? "https://xvlray.logto.app/";
                options.AppId = logtoSettings?.AppId ?? "";
                options.AppSecret = logtoSettings?.AppSecret ?? "";
            });
            builder.Services.AddCascadingAuthenticationState();

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region LogTo Signing in and signing out

            app.UseAuthentication();
            app.MapGet("/SignIn", async context =>
            {
                if (!(context.User?.Identity?.IsAuthenticated ?? false))
                {
                    await context.ChallengeAsync(new AuthenticationProperties { RedirectUri = "/" });
                }
                else
                {
                    context.Response.Redirect("/");
                }
            });

            app.MapGet("/SignOut", async context =>
            {
                if (context.User?.Identity?.IsAuthenticated ?? false)
                {
                    await context.SignOutAsync(new AuthenticationProperties { RedirectUri = "/" });
                }
                else
                {
                    context.Response.Redirect("/");
                }
            });

            #endregion

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
