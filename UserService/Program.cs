using Cassandra.Mapping;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text.Json.Serialization;
using UserService.Configuration;
using UserService.Contracts.Providers;
using UserService.Contracts.Repositories;
using UserService.Contracts.Services;
using UserService.Infrastructure;
using UserService.Infrastructure.Extensions;
using UserService.Infrastructure.Provider;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;
using UserServiceImplementation = UserService.Infrastructure.Services.UserService;

var AllowAllOrigins = "_AllowAllOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        AllowAllOrigins,
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var firebaseProjectId = builder.Configuration["FirebaseAuthClientConfig:ProjectId"];
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = "https://securetoken.google.com/" + firebaseProjectId,
    ValidateAudience = true,
    ValidAudience = firebaseProjectId,
    ValidateLifetime = true,
};

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/" + firebaseProjectId;
        options.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<FirebaseAuthClientConfig>(builder.Configuration.GetSection("FirebaseAuthClientConfig"));
builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection("DatabaseConfig"));

// Firebase admin app is only needed for user service
builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(builder.Configuration.GetValue<string>("FirebaseAdminSdkCredentialsPath"))
}));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddSingleton<IDatabaseSessionProvider, DatabaseSessionProvider>();
builder.Services.AddScoped<IUserProvider, UserProvider>();

builder.Services.AddScoped<IUserService, UserServiceImplementation>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthenticationProviderService, FirebaseAuthenticationProviderService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseSwagger((opt) =>
{
    opt.RouteTemplate = "swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(AllowAllOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapDefaultControllerRoute();

MappingConfiguration.Global.Define<DbMappingProfile>();

app.Migrate();

app.Run();