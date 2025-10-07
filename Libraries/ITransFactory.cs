namespace TransService
{
    public interface ITransFactory
    {
        bool TryGetService(string ServiceName, out ITransProcessor? service);
    }
}