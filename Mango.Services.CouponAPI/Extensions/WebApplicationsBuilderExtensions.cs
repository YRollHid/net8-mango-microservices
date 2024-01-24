// Import necessary libraries for JWT Bearer authentication  
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Create a static class to add authentication extension method  
namespace Mango.Services.CouponAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        // Create an extension method to add authentication  
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            // Get the configuration section for API settings  
            var settingsSection = builder.Configuration.GetSection("ApiSettings");

            // Read the necessary configuration values for JWT authentication  
            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");

            // Create a byte array from the secret key using ASCII encoding  
            var key = Encoding.ASCII.GetBytes(secret);

            // Configure the authentication middleware by adding JwtBearer authentication scheme to the services collection  
            builder.Services.AddAuthentication(x =>
            {
                // Set the default authentication and challenge schemes to JwtBearer  
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                // Configure the JwtBearer authentication handler with token validation parameters  
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate issuer signing key  
                    ValidateIssuerSigningKey = true,
                    // Set the issuer signing key to the symmetric security key created from the secret key  
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // Validate issuer  
                    ValidateIssuer = true,
                    // Set the valid issuer to the value read from the configuration  
                    ValidIssuer = issuer,
                    // Set the valid audience to the value read from the configuration  
                    ValidAudience = audience,
                    // Validate audience  
                    ValidateAudience = true
                };
            });

            // Return the builder with authentication added  
            return builder;
        }
    }
}
