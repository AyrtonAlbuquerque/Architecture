using System.ComponentModel.DataAnnotations;

namespace Architecture.Api.Common
{
    public sealed class Settings
    {
        public const string Section = "AppSettings";

        [Required]
        public string Environment { get; set; }

        [Required]
        public Jwt Jwt { get; set; }
    }

    public sealed class Jwt
    {
        public const string Section = "AppSettings:Jwt";

        [Required]
        public string Secret { get; set; }

        [Required]
        public string Issuer { get; set; }

        [Required]
        public string Audience { get; set; }

        [Required]
        public double Expiration { get; set; }
    }
}