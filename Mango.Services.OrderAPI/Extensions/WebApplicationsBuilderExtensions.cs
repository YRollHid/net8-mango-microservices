// Import the necessary namespaces for JWT Bearer authentication  
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Define an extension method for the WebApplicationBuilder class  
namespace Mango.Services.OrderAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            // Get the JWT Bearer authentication settings from the application's configuration file  
            var settingsSection = builder.Configuration.GetSection("ApiSettings");

            // Retrieve the secret key, issuer, and audience values from the configuration section  
            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");

            // Encode the secret key as bytes using ASCII encoding  
            var key = Encoding.ASCII.GetBytes(secret);

            // Add JWT Bearer authentication to the application's service collection  
            builder.Services.AddAuthentication(x =>
            {
                // Set the default authentication and challenge schemes to JwtBearer  
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                // Configure JWT Bearer authentication options  
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate the issuer and audience of the token  
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateAudience = true,

                    // Verify the signature using the secret key  
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Return the WebApplicationBuilder instance to allow method chaining  
            return builder;
        }
    }
}
