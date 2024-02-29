using System.ComponentModel.DataAnnotations;

namespace ContactFormProject.Data
{
    public class Mail
    {
        [Key]
        public int Id { get; set; }
        public string FromAddress { get; set; } = string.Empty;
        public string ToAddress { get; set; } = string.Empty;
        public string? GuestFirstName { get; set; }
        public string? GuestLastName { get; set; }
        public string? GuestMailAddress { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsIncomingMail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
    }
}
