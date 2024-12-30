using Business.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;

namespace ChatAppBackend.Configuration
{
    public static class DependencyInjection
    {
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


        public static IServiceCollection AddDatabaseAndDatabaseHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDpContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            services.AddHealthChecks().AddSqlServer(configuration.GetConnectionString("DefaultConnection")).AddDbContextCheck<ChatDpContext>();

            return services;
        }

    }
}
