namespace ProductsApi.Adapters;

public interface ISystemClock
{
    DateTimeOffset GetCurrent();
}

public class SystemClock : ISystemClock
{
    public DateTimeOffset GetCurrent()
    {
        return DateTimeOffset.Now;
    }
}
