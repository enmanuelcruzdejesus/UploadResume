using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using Amazon;

namespace UploadResume.Services
{
    public interface IEmailService
    {
        Task Send(string emailAddress, string body);
    }


    public class AwsSesEmailService : IEmailService
    {
        public async Task Send(string emailAddress, string body)
        {
            using var emailClient = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest2);

            // Send the email using AWS SES
            await emailClient.SendEmailAsync(new SendEmailRequest
            {
                Source = "yorkshiretripper+test@gmail.com",
                Destination = new Destination { ToAddresses = new List<string> { "yorkshiretripper+test@gmail.com" } },
                Message = new Message
                {
                    Subject = new Content("Email Subject"),
                    Body = new Body { Text = new Content(body) }
                }
            });
        }
    }

}
