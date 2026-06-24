using Ofeck.Bartify.Core;
using Ofeck.Bartify.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Ofeck.Bartify.APIEndpoints.Auth;
using Ofeck.Bartify.Core.Fotos.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddScoped<ITokenService, TokenService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddRepositories(
    builder.Configuration.GetConnectionString("mysql")! 
);

builder.Services
       .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    }
                };
            }
        );

builder.Services.AddCors(options => {
    options.AddPolicy("BartifyPolicy", policy => {
        policy.SetIsOriginAllowed(origin => {
            var uri = new Uri(origin);
            
            if (uri.Host == "localhost" || uri.Host == "127.0.0.1") 
                return true;
            
            if (uri.Host.EndsWith(".up.railway.app")) return true;
            if (uri.Host.EndsWith(".vercel.app")) return true;
            
            return false;
        });
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

builder.Services.Configure<CloudinaryRequest>(
    builder.Configuration.GetSection("Cloudinary")
);

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// Configure the HTTP request pipeline.


    app.MapOpenApi();
    
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "Ofeck API v1"); });
}

app.UseCors("BartifyPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();