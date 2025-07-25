using API.Extentions;
using Application.Models;
using Serilog;
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddContext(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddDomainServices(builder.Configuration);
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddValidators();
builder.Services.AddSupabaseStorage(builder.Configuration);
builder.Services.AddQuickSMSService(builder.Configuration);
builder.Services.AddAzconMatching(builder.Configuration,builder.Environment);
var masterFilePath = Path.Combine(builder.Environment.ContentRootPath, "Data", "master.xlsx");
var vocabularyFilePath = Path.Combine(builder.Environment.ContentRootPath, "Data", "vocab.json");

builder.Services.AddSingleton(new MasterFileOptions
{
    MasterPath = masterFilePath,
    VocabPath = vocabularyFilePath,
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    //serverOptions.ListenAnyIP(80);
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var error = exceptionHandlerPathFeature?.Error;

        // Optionally log the error here

        await context.Response.WriteAsJsonAsync(new
        {
            Error = "An unexpected error occurred.",
        });
    });
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Azconprice API V1");
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.SeedRolesAsync();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.Run();
