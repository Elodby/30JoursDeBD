using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Mvc.Facebook;
using Microsoft.AspNet.Mvc.Facebook.Client;
using MvcApplication7.Models;

namespace MvcApplication7.Controllers
{
    public class HomeController : Controller
    {
        [FacebookAuthorize("email", "user_photos")]
        public async Task<ActionResult> Index(FacebookContext context)
        {
            if (ModelState.IsValid)
            {
                var user = await context.Client.GetCurrentUserAsync<MyAppUser>();
                return View(user);
            }

            return View("Error");
        }

        // Cette action gérera les redirections à partir de FacebookAuthorizeFilter lorsque 
        // l’application ne dispose pas des autorisations requises spécifiées dans le FacebookAuthorizeAttribute.
        // Le chemin d’accès à cette action est définie sous appSettings (dans Web.config) avec la clé « Facebook:AuthorizationRedirectPath ».
        public ActionResult Permissions(FacebookRedirectContext context)
        {
            if (ModelState.IsValid)
            {
                return View(context);
            }

            return View("Error");
        }
    }
}
