using Microsoft.AspNet.Mvc.Facebook;
using Newtonsoft.Json;

// Ajouter les champs à enregistrer pour chaque utilisateur et spécifier le nom de champ dans le code JSON renvoyé par Facebook
// http://go.microsoft.com/fwlink/?LinkId=273889

namespace MvcApplication7.Models
{
    public class MyAppUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [JsonProperty("picture")] // Cette option renomme la propriété en image.
        [FacebookFieldModifier("type(large)")] // Cette option définit une grande taille d'image.
        public FacebookConnection<FacebookPicture> ProfilePicture { get; set; }

        [FacebookFieldModifier("limit(8)")] // Cette option limite la taille de la liste d'amis à 8 utilisateurs. Supprimez-la pour inclure tous les amis.
        public FacebookGroupConnection<MyAppUserFriend> Friends { get; set; }

        [FacebookFieldModifier("limit(16)")] // Cette option limite la taille de la liste de photos à 16 utilisateurs. Supprimez-la pour inclure toutes les photos.
        public FacebookGroupConnection<FacebookPhoto> Photos { get; set; }
    }
}
