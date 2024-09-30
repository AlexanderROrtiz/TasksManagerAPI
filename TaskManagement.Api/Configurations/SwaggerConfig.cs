using Microsoft.OpenApi.Models;

namespace TaskManagement.Api.Configurations
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT con el prefijo 'Bearer tokenek5NGCI6ctoken'.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",  
                    BearerFormat = "JWT" 
                });

                // Define el requisito de seguridad para los endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header,
                            Name = "Authorization",
                            BearerFormat = "JWT",
                            Type = SecuritySchemeType.ApiKey
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
