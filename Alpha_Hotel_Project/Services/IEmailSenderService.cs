namespace Alpha_Hotel_Project.Services
{
    public interface IEmailSenderService
    {
        void Send(string to, string subject, string html);
        void Send(string[] allTo, string subject, string html);
    }
}
