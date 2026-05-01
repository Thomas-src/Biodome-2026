using System.Threading.RateLimiting;
using BioDomes.Domains;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Domains.Services.mail;
using BioDomes.Infrastructures;
using BioDomes.Infrastructures.context;
using BioDomes.Infrastructures.entities;
using BioDomes.Infrastructures.repository;
using BioDomes.Infrastructures.Repository;
using BioDomes.Web.Middlewares;
using BioDomes.Web.Transformers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(AppRoles.Admin));
});
// Add services to the container.
builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
});
builder.Services.AddScoped<LogOriginMiddleware>();
builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
 
builder.Services.AddCors(options => 
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("https://ue19.cg.helmo.be/q240075", "https://localhost:7108");
        });
});
var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("DevConnection")
    : builder.Configuration.GetConnectionString("ProdConnection");
// context
builder.Services.AddDbContext<BioDomesDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<BioDomesDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.AllowedForNewUsers = false;
});

// Repositories
builder.Services.AddTransient<IBiodomeRepository, BiodomeRepository>();
builder.Services.AddTransient<ISpeciesRepository, SpeciesRepository>();
builder.Services.AddTransient<ISpeciesClassificationRepository, SpeciesClassificationRepository>();
builder.Services.AddTransient<IDietTypeRepository, DietTypeRepository>();
builder.Services.AddTransient<IResearchOrganismRepository, ResearchOrganismRepository>();
builder.Services.AddTransient<IPictureRepository, PictureRepository>();
builder.Services.AddTransient<IEquipementRepository, EquipementRepository>();
builder.Services.AddTransient<IProductionRepository, ProductionRepository>();
builder.Services.AddTransient<IConsumptionRepository, ConsumptionRepository>();
// Services
builder.Services.AddTransient<IBiodomeService, BiodomeService>();
builder.Services.AddTransient<ISpeciesService, SpeciesService>();
builder.Services.AddTransient<IResearchService, ResearchOrganismService>();
builder.Services.AddTransient<IEquipementService, EquipementService>();


// Mail service
builder.Services.AddSingleton(sp => builder.Configuration.GetSection("MailConfig").Get<MailConfig>());
builder.Services.AddHttpClient<IMyEmailSender, ResendMailSender>();



// Code reprit de la lecon8
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString() ?? "anonymous",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100, // Avant c'était sur 10. -> Refus de réponse apres plusieurs navigation
                QueueLimit = 10, // Avant c'était sur 1
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
    options.ConstraintMap["slug"] = typeof(SlugTransformer);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseExceptionHandler("/Error"); // Quand je suis en production
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions());

app.UseHttpsRedirection();

app.UseWebOptimizer();

app.UseStaticFiles(); // Pour utiliser le CSS...

app.UseLogOrigin();
app.UseReverseProxy();

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins); 
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapRazorPages();

app.Run();