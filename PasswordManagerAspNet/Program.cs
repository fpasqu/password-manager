using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PasswordManagerAspNet.Core.Models.Context;
using PasswordManagerAspNet.Core.Models.Utils;
using PasswordManagerAspNet.Models.Repositories;

var ENVS = new EnvConfig 
    {
        Instance = Environment.GetEnvironmentVariable("PM_INSTANCE_ID"),
        TenantId = Environment.GetEnvironmentVariable("PM_TENANT_ID"),
        ClientId = Environment.GetEnvironmentVariable("PM_CLIENT_ID"),
        ApiAudience = Environment.GetEnvironmentVariable("PM_API_AUDIENCE"),
        CallbackPath = Environment.GetEnvironmentVariable("PM_CALLBACK_PATH"),
        GroupId = Environment.GetEnvironmentVariable("PM_GROUP_ID"),
        DbName = Environment.GetEnvironmentVariable("PM_DB_NAME")
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect("OpenIdConnect", options =>
{
    options.ClientId = ENVS.ClientId;
    options.Authority = $"{ENVS.Instance}{ENVS.TenantId}";
    options.CallbackPath = ENVS.CallbackPath;
    options.SignedOutCallbackPath = "/Home";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PMGroup", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("groups", ENVS.GroupId);
    });
});

builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<IFunctions, Functions>();
builder.Services.AddDbContext<BackendDbContext>(options => 
    options.UseSqlite($"Data Source={ENVS.DbName};"),
    ServiceLifetime.Scoped);

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages().AddMicrosoftIdentityUI();

//checks
builder.Services.AddHealthChecks()
    .AddCheck("DB", () =>
    {
        return HealthCheckResult.Healthy();
    });

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

void ApplyDatabaseMigrations()
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BackendDbContext>();
            dbContext.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying database migrations: {ex.Message}");
    }
}
ApplyDatabaseMigrations();

Task WriteHealthCheckResponse(HttpContext http, HealthReport result)
{
    http.Response.ContentType = "application/json";
    var json = new JObject(
        new JProperty("Status ", result.Status.ToString()),
        new JProperty("TotalCheckDuration", result.TotalDuration.TotalMilliseconds.ToString() + " ms"),
        new JProperty("DependencyHealthCheck", new JObject(result.Entries.Select(dI =>
            new JProperty(dI.Key, new JObject(
                new JProperty("Status", dI.Value.Status.ToString())
                ))
            )))
        );
    return http.Response.WriteAsync(json.ToString(Formatting.Indented));
}
app.UseEndpoints(endpoints =>
{
    //health endpoint
    endpoints.MapHealthChecks("/health", new HealthCheckOptions()
    {
        ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError
            },
        ResponseWriter = WriteHealthCheckResponse
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();