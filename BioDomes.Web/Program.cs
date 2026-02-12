using BioDomes.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<LogOriginMiddleware>();
builder.Services.AddScoped<ReverseProxyLinksMiddleware>();

builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/bundle.css",
        "site.css",
        "paragraph-custom-style.css",
        "title-custom-style.css");
});

var app = builder.Build();

app.UseLogOrigin();
app.UseReverseProxy();
app.UseExceptionHandler("/Error");
app.UseWebOptimizer();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseExceptionHandler("/Error"); // Quand je suis en production
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles(); // Pour utiliser le CSS...

app.UseForwardedHeaders(new ForwardedHeadersOptions());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();