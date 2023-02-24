using Api.Common;
using Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

 builder.Services.AddCors(options =>
 {
    options.AddPolicy(name: MyAllowSpecificOrigins,
      policy  =>
      {
          policy.WithOrigins("http://localhost:5000",
                            "https://localhost:5001",
                            "http://localhost:4200",
                            "https://localhost:4200",
                            "http://localhost",
                            "https://localhost",
                            "http://localhost:80",
                            "https://localhost:443",
                            "https://pymage.io",
                            "http://pymage.io",
                            "https://www.pymage.io",
                            "http://www.pymage.io")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
      });
 });

//builder.Services.AddApplicationServices();
//builder.Services.AddInfrastructureServices(builder.Configuration);
//builder.Services.AddWebUIServices();

// Controllers
builder.Services
    .AddControllers(options =>
    {
        options.AllowEmptyInputInBodyModelBinding = true;
        options.Filters.Add<ValidationErrorResultFilter>();
    })
    .AddValidationSetup();

// Authn / Authrz
builder.Services.AddAuthSetup(builder.Configuration);

// Swagger
builder.Services.AddSwaggerSetup();

// Persistence
builder.Services.AddPersistenceSetup(builder.Configuration);

// Application layer setup
builder.Services.AddApplicationSetup();

// Request response compression
builder.Services.AddCompressionSetup();

// HttpContextAcessor
builder.Services.AddHttpContextAccessor();

// Mediator
builder.Services.AddMediatRSetup();

// Middleware
builder.Services.AddScoped<ExceptionHandlerMiddleware>();

builder.Logging.ClearProviders();

// Add serilog
if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Host.UseLoggingSetup(builder.Configuration);
}

// Add opentelemetry
builder.AddOpenTemeletrySetup();

var app = builder.Build();

app.UseHttpsRedirection();
//app.UseForwardedHeaders(new ForwardedHeadersOptions
//{
//      ForwardedHeaders = ForwardedHeaders.XForwardedProto
//});
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
app.UseSwaggerSetup();
app.UseResponseCompression();

app.UseAuthentication();
//app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers()
   .RequireAuthorization();

await app.RunAsync();