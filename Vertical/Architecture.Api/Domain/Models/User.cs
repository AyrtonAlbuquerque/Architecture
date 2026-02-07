namespace Architecture.Api.Domain.Models
{
    public class User
    {
        public virtual int Id { get; protected set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
    }
}