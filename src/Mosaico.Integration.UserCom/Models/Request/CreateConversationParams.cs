namespace Mosaico.Integration.UserCom.Models.Request
{
    public class CreateConversationParams
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }
}