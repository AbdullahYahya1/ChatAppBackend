using Business.Context;
using Business.EmailSender;
using Business.FileService;
using Business.IServices;
using Business.Services;
using DataAccess.IRepositories;
using DataAccess.Mapping;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;
using System.Text;

namespace ChatAppBackend.Configuration
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddRepositoriesInjections(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IConversationUserRepository, ConversationUserRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            return services;
        }
        public static IServiceCollection AddServicesInjections(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServicesDependency<>), typeof(ServicesDependency<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IConversationServices, ConversationServices>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<FileService>();
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection Services)
        {
            Services.AddControllers();

            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIs", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.  
                          Enter 'Bearer' [space] example : 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                    {new OpenApiSecurityScheme{
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()}
                });
            });
            return Services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDpContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                };
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policyBuilder => policyBuilder.WithOrigins("http://localhost:4200")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod()
                                                  .AllowCredentials());
            });
            return services;
        }
        public static IServiceCollection AddJson(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            return services;
        }
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
             .AddSqlServer(configuration.GetConnectionString("DefaultConnection"))
             .AddDbContextCheck<ChatDpContext>();
            return services;
        }

        public static IServiceCollection AddMappingProfile(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            return services;
        }
    }
}
