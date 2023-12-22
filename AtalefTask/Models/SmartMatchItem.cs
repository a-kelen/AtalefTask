namespace AtalefTask.Models
{
    public class SmartMatchItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UniqueValue { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
