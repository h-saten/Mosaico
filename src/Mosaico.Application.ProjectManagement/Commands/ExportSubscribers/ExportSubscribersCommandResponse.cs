namespace Mosaico.Application.ProjectManagement.Commands.ExportSubscribers
{
    public class ExportSubscribersCommandResponse
    {
        public int Count { get; set; }
        public byte[] File { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
    }
}