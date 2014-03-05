using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Facebook.Models;
using Microsoft.AspNet.Mvc.Facebook.Realtime;

// Pour en savoir plus sur les mises à jour en  temps réel de Facebook, consultez la page http://go.microsoft.com/fwlink/?LinkId=273887

namespace MvcApplication7.Controllers
{
    public class UserRealtimeUpdateController : FacebookRealtimeUpdateController
    {
        private readonly static string UserVerifyToken = ConfigurationManager.AppSettings["Facebook:VerifyToken:User"];

        public override string VerifyToken
        {
            get
            {
                return UserVerifyToken;
            }
        }

        public override Task HandleUpdateAsync(ChangeNotification notification)
        {
            if (notification.Object == "user")
            {
                foreach (var entry in notification.Entry)
                {
                    // Emplacement de votre logique de gestion de la mise à jour
                }
            }

            throw new NotImplementedException();
        }
    }
}
