using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;

namespace CursoIdentity.Servicios
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        public OpcionesMailJet OpcionesMailJet;

        public MailJetEmailSender(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            OpcionesMailJet = configuration.GetSection("MailJet").Get<OpcionesMailJet>();
            MailjetClient client = new MailjetClient(OpcionesMailJet.ApiKey, OpcionesMailJet.SecretKey)
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "djtique78@soy.sena.edu.co"},
        {"Name", "NeighborFood"}
       }
      }, {
             "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "David"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
        htmlMessage
      }, {
       "CustomID",
       "AppGettingStartedTest"
      }
     }
             });
             await client.PostAsync(request);
            //if (response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
            //    Console.WriteLine(response.GetData());
            //}
            //else
            //{
            //    Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
            //    Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
            //    Console.WriteLine(response.GetData());
            //    Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            //}
        }
    }
}
