namespace OMS.Model
{
    public class Posts
    {
        public Posts()
        {
        }

        public Posts(string id, string message, string createdTime)
        {
            Id = id;
            Message = message;
            CreatedTime = createdTime;
        }

        public string Id { get; set; }
        public string Message { get; set; }
        public string CreatedTime { get; set; }
    }
}