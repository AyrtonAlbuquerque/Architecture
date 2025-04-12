namespace Architecture.Api.Domain.Entities
{
    public class User
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
    }
}