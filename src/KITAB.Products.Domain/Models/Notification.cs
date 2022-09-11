namespace KITAB.Products.Domain.Models
{
    public class Notification
    {
        public string Message { get; }

        public Notification(string p_message)
        {
            Message = p_message;
        }
    }
}
