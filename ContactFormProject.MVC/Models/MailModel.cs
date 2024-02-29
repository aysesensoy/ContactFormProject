namespace ContactFormProject.MVC.Models
{
    public class MailModel
    {
        public int Id { get; set; } = 0;
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public string GuestMailAddress { get; set; }
        public string FromAddress { get; set; } = "";
        public string ToAddress { get; set; } = "";
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool IsIncomingMail { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
    }
}
