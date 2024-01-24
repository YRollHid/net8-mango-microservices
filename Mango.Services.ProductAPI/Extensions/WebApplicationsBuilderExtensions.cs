// Import the necessary namespaces  
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Define a static class that contains an extension method for configuring authentication  
namespace Mango.Services.ProductAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        // Define the extension method, which takes a WebApplicationBuilder as input and returns the same object  
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            // Retrieve the ApiSettings section from the application's configuration  
            var settingsSection = builder.Configuration.GetSection("ApiSettings");

            // Read the secret, issuer, and audience values from the configuration  
            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");

            // Convert the secret key to a byte array  
            var key = Encoding.ASCII.GetBytes(secret);

            // Configure the authentication middleware using the JwtBearer authentication scheme  
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                // Set the validation parameters for the JWT token  
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateAudience = true
                };
            });

            // Return the WebApplicationBuilder object  
            return builder;
        }
    }
}
