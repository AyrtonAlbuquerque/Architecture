using Architecture.Application.Abstractions;
using Architecture.Application.Common;
using Architecture.Domain.Entities;
using Mapster;

namespace Architecture.Application.Authentication.Register
{
    public class Mapping : IMapping
    {
        public void AddMapping()
        {
            TypeAdapterConfig<Command, User>
                .NewConfig()
                .Map(dest => dest.Email, source => source.Email)
                .Map(dest => dest.Password, source => Hasher.Hash(source.Password));
        }
    }
}