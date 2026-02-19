using System.ComponentModel.DataAnnotations;

namespace Architecture.Application.Common
{
    public sealed class Settings
    {
        public const string Section = "AppSettings";

        [Required]
        public string Environment { get; set; }

        [Required]
        public Jwt Jwt { get; set; }

        [Required]
        public Mail Mail { get; set; }

        [Required]
        public SMS SMS { get; set; }
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

    public sealed class Mail
    {
        public const string Section = "AppSettings:Mail";

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Smtp { get; set; }

        [Required]
        public int Port { get; set; }
    }

    public sealed class SMS
    {
        public const string Section = "AppSettings:SMS";

        [Required]
        public string Server { get; set; }

        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string Secret { get; set; }

        [Required]
        public string Carteira { get; set; }
    }
}