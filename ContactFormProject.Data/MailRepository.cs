namespace ContactFormProject.Data
{
    public class MailRepository
    {
        private string _connectionString;
        public MailRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Mail GetMail(int id)
        {
            using var dbContext = new ContactFormDbContext(_connectionString);
            var mail = dbContext.Mails.Find(id);
            mail.IsRead = true;
            dbContext.Mails.Update(mail);
            dbContext.SaveChanges();
            return mail;
        }

        public List<Mail> GetIncomingMails()
        {
            using var dbContext = new ContactFormDbContext(_connectionString);
            return dbContext.Mails.Where(x => !x.IsDeleted && x.IsIncomingMail).OrderByDescending(x=>x.SendDate).ToList();
        }   
        
        public List<Mail> GetOutgoingMails()
        {
            using var dbContext = new ContactFormDbContext(_connectionString);
            return dbContext.Mails.Where(x => !x.IsDeleted && !x.IsIncomingMail).OrderByDescending(x => x.SendDate).ToList();
        }

        public int Delete(int id)
        {
            using var dbContext = new ContactFormDbContext(_connectionString);
            var mail = dbContext.Mails.Find(id);
            mail.IsDeleted = true;
            dbContext.Mails.Update(mail);
            return dbContext.SaveChanges();
        }

        public int Create(Mail mail)
        {
            using var dbContext = new ContactFormDbContext(_connectionString);
            dbContext.Mails.Add(mail);
            return dbContext.SaveChanges();
        }   
    }
}
