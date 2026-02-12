using BioDomes.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<LogOriginMiddleware>();
builder.Services.AddScoped<ReverseProxyLinksMiddleware>();
var app = builder.Build();

app.UseLogOrigin();
app.UseReverseProxy();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles(); // Pour utiliser le CSS...

app.UseForwardedHeaders(new ForwardedHeadersOptions());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();