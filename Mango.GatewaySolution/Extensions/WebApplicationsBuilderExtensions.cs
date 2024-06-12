// Import the necessary namespaces  
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Define a namespace for the extension method  
namespace Mango.GatewaySolution.Extensions
{
    // Define a static class for the extension method  
    public static class WebApplicationBuilderExtensions
    {
        // Define an extension method for the WebApplicationBuilder class  
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            // Get the "ApiSettings" configuration section  
            var settingsSection = builder.Configuration.GetSection("ApiSettings");

            // Read the JWT secret, issuer, and audience from the configuration  
            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");

            // Convert the secret to a byte array  
            var key = Encoding.ASCII.GetBytes(secret);

            // Configure the authentication middleware to use JWT bearer authentication  
            builder.Services.AddAuthentication(x =>
            {
                // Set the default authentication and challenge schemes to JWT bearer  
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Configure the JWT bearer authentication options  
            .AddJwtBearer(x =>
            {
                // Set the token validation parameters  
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate the issuer signing key  
                    ValidateIssuerSigningKey = true,
                    // Set the signing key to the byte array created from the secret  
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // Validate the issuer  
                    ValidateIssuer = true,
                    // Set the valid issuer to the value read from the configuration  
                    ValidIssuer = issuer,
                    // Set the valid audience to the value read from the configuration  
                    ValidAudience = audience,
                    // Validate the audience  
                    ValidateAudience = true
                };
            });

            // Return the WebApplicationBuilder instance for method chaining  
            return builder;
        }
    }
}
