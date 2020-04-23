using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AspNetCoreApiJwt.Models
{
    public class MicrosoftVSConstants
    {
        public const string Issuer = "MVC";
        public const string Audience = "ApiUser";
        public const string Key = "0987612345098712";
        public const string AuthenticationSceme = "Identity.Application" + "," + JwtBearerDefaults.AuthenticationScheme;
    }
}
