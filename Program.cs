using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StreamerApi.AdditionalServices;
using StreamerApi.Auhtorization;
using StreamerApi.Entities;
using StreamerApi.Middleware;
using StreamerApi.Services;
using Hangfire;
using Hangfire.PostgreSql;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

ConfigurationHelper.Initialize(builder.Configuration);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddAuthorization(options => {
    options.AddPolicy("IpVerify", builder => builder.AddRequirements(new IPVerifyRequirement()));
});

builder.Services.AddHangfire(options =>
    options.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration["ConnectionStrings:Hangfire"])
);

builder.Services.AddHangfireServer();

builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StreamerDbContext>();
builder.Services.AddScoped<ExceptionsMiddleware>();
builder.Services.AddScoped<IStreamerService, StreamerService>();
builder.Services.AddScoped<IFunctions, Functions>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddTransient<StreamerSeeder>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IAuthorizationHandler, IPVerifyRequirementHandler>();
builder.Services.AddControllers().AddNewtonsoftJson(options => 
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;

});
builder.Services.AddCors(options => {
    options.AddPolicy(name: "AllowOrigin", policy =>
    {
        policy.WithOrigins("http://localhost")
            .AllowAnyHeader()
            .WithMethods("GET", "POST")
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseIpRateLimiting();

app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionsMiddleware>();

app.UseAuthorization();

app.MapControllers();


var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = scopedFactory.CreateScope())
{
    var service = scope.ServiceProvider.GetService<StreamerSeeder>();
    service.Seed();
}
app.UseCors("AllowOrigin");

app.Run();







