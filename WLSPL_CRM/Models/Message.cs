namespace WLSPL_CRM_2.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; }
    }
}
