using System;

namespace NESI.DTO.ViewModels.CurrentUser.Layout
{
    public class Message
    {
        public string Type { get; set; }
        public string ToName { get; set; }
        public int ToId { get; set; }
        public string FromName { get; set; }
        public int FromId { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Body { get; set; }
    }
}