namespace TaskManagement.Api.Configurations
{
    public static class CorsConfig
    {
        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200", "https://tu-front-end-url.com") // URLs del front-end
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials(); // Cookies o autenticación basada en credenciales
                });
            });
        }
    }
}
