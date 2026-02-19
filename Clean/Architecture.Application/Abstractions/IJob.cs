namespace Architecture.Application.Abstractions
{
    public interface IJob
    {
        Task RunAsync();
    }
}