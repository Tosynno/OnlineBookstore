using Azure.Storage.Blobs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineBookstore.Application.Helpers;
using OnlineBookstore.Application.Interfaces;
using OnlineBookstore.Application.IServices;
using OnlineBookstore.Application.Models;
using OnlineBookstore.Application.Repositories;
using OnlineBookstore.Application.Services;
using OnlineBookstore.Application.Utilies;
using OnlineBookstore.Application.Validations;
using OnlineBookstore.Infrastructure.Context;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace OnlineBookstore.API.Extension
{
    public static class ApplicationServiceExtensions
    {
        public static readonly ILoggerFactory dbLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<BookdbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddSingleton(x => new BlobServiceClient(config.GetConnectionString("StorageAccount")));
            // Other service configurations...

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 1073741824; // 1 GB in bytes
            });
            services.AddSingleton<HtmlEncoder>(
    HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                               UnicodeRanges.CjkUnifiedIdeographs }));
            // Other service configurations...

            services.AddScoped<HttpClient>();


            services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IBookRepo, BookRepo>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorRepo, AuthorRepo>();
            services.AddScoped<IAuthorService, AuthorService>();

            //services.AddScoped<EncryptionActionFilter>();
            services.AddScoped<AzureBlogService>();
            services.AddHttpContextAccessor();

            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
            services.AddScoped<IValidator<BookRequest>, BookRequestValidator>();
            services.AddScoped<IValidator<AuthorRequest>, AuthorRequestValidator>();
            return services;
        }
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(Convert.ToDouble(configuration["HttpSecurity:expiryTime"]));
                options.ExcludedHosts.Add(configuration["HttpSecurity:Url"]);
                options.ExcludedHosts.Add(configuration["HttpSecurity:Url"]);
            });
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<JwtHandler>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                //o.TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidIssuer = configuration["Jwt:Issuer"],
                //    ValidAudience = configuration["Jwt:Audience"],
                //    IssuerSigningKey = new SymmetricSecurityKey
                //    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),

                //    ValidateAudience = true,
                //    ValidateLifetime = false,
                //    ValidateIssuerSigningKey = true
                //};
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Book Store API Service", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
        }
    }
}
