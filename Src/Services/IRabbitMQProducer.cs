namespace UserManagementService.Src.Services
{
    public interface IRabbitMQProducer
    {
        void PublishUserCreated(string message);
        void PublishProgressUpdated(string message);
    }
}