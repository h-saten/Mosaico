namespace Mosaico.Integration.Sms.Abstraction
{
    public class SmsMessage
    {
        public string RecipientsPhoneNumber { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}