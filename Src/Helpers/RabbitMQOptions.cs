namespace UserManagementService.Src.Helpers
{
    public class RabbitMQOptions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string RoutingKeyUserCreated { get; set; } = string.Empty;
        public string RoutingKeyProgressUpdated { get; set; } = string.Empty;
    }

}