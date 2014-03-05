using System;
using System.Web.Mvc;
using Microsoft.AspNet.Mvc.Facebook;
using Microsoft.AspNet.Mvc.Facebook.Authorization;

namespace MvcApplication7
{
    public static class FacebookConfig
    {
        public static void Register(FacebookConfiguration configuration)
        {
            // Charge les paramètres du fichier web.config à l'aide des clés de paramètres d'application suivantes :
            // Facebook:AppId, Facebook:AppSecret, Facebook:AppNamespace
            configuration.LoadFromAppSettings();

            // Ajout du filtre d'autorisation pour rechercher les demandes et autorisations signées de Facebook
            GlobalFilters.Filters.Add(new FacebookAuthorizeFilter(configuration));
        }
    }
}
