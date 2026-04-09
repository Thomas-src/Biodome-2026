using System.Threading.RateLimiting;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.repository;
using BioDomes.Web.Middlewares;
using BioDomes.Web.Transformers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<LogOriginMiddleware>();
builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
//

 
builder.Services.AddCors(options => 
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("https://ue19.cg.helmo.be/q240075", "https://localhost:7108/");
        });
});

builder.Services.AddDbContext<BioDomesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IBiodomeRepository, BiodomeRepository>();

// Code reprit de la lecon8
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});

builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/bundle.css",
        "site.css",
        "paragraph-custom-style.css",
        "title-custom-style.css");
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;

    options.ConstraintMap["slug"] = typeof(SlugTransformer);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseExceptionHandler("/Error"); // Quand je suis en production
    builder.Services.AddDbContext<BioDomesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProdConnection")));
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    builder.Services.AddDbContext<BioDomesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
}

app.UseForwardedHeaders(new ForwardedHeadersOptions());

app.UseHttpsRedirection();

app.UseWebOptimizer();

app.UseStaticFiles(); // Pour utiliser le CSS...

app.UseLogOrigin();
app.UseReverseProxy();

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins); 
app.UseAuthorization();
app.UseRateLimiter();

app.MapRazorPages();

app.Run();