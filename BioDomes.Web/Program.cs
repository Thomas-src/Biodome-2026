using BioDomes.Infrastructures;
using BioDomes.Web.Middlewares;
using BioDomes.Web.Transformers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<LogOriginMiddleware>();
builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
builder.Services.AddSingleton<IRepository, SqlRepository>();

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
app.UseAuthorization();

app.MapRazorPages();

app.Run();