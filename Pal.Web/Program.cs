using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Pal.Services.Configurations;
using Pal.Services.Hubs;
using Pal.Services.Logger;
using Pal.Services.Notifications;
using Pal.Web.Extensions;
using FastReport.Data;
using NLog;
using Pal.Data.Contexts;
using Syncfusion.JavaScript.DataVisualization.Models;


var builder = WebApplication.CreateBuilder(args);

FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
builder.Services.AddMyContexts();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddMyIdentity();
builder.Services.AddMyDbServices();
builder.Services.AddMyServices();
builder.Services.AddMyCRMServices();

builder.Services.AddMyCacheService();
builder.Services.AddMyLocalization();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.Configure<RazorViewEngineOptions>(opt =>
{
    opt.AreaViewLocationFormats.Add("/Areas/Admin/Views/Shared/Components/{0}.cshtml");
    opt.ViewLocationFormats.Add("/Views/Shared/Partials/{0}.cshtml");
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});
builder.Services.AddControllersWithViews();
builder.Services.AddMyActionFilters();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pal.API", Version = "v1" });
//});

builder.Services.Configure<RazorViewEngineOptions>(opt =>
{
    opt.AreaViewLocationFormats.Add("/Areas/Admin/Views/Shared/Components/{0}.cshtml");
    opt.ViewLocationFormats.Add("/Views/Shared/Partials/{0}.cshtml");
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});


builder.Services.AddControllersWithViews();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //TODO Enable CORS
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pal.API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.Use((context, next) =>
{
    // Default limit was changed some time ago. Should work by setting MaxRequestBodySize to null using ConfigureKestrel but this does not seem to work for IISExpress.
    // Source: https://github.com/aspnet/Announcements/issues/267
    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = null;
    return next.Invoke();
});



app.UseAuthorization();
//app.UseAuthentication();

app.UseRequestLocalization();

app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
   );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");

    endpoints.MapHub<NotificationHub>("/notificationHub");
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.UseEndpoints(endpoint =>
{
    endpoint.MapDefaultControllerRoute();
});
app.UseFastReport();
app.SeedData();

app.Run();