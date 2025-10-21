namespace Service
{
    public interface IAnalyticsService
    {
        void SendEvent(string name);
    }
}