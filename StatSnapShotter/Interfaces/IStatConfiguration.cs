namespace StatSnapShotter.Interfaces
{
    public interface IStatConfiguration
    {
        int Interval { get; set; }
        object GetConfiguration<T>();
    }
}
