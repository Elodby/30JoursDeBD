using Microsoft.AspNet.Mvc.Facebook;

// Ajouter les champs à enregistrer pour chaque utilisateur et spécifier le nom de champ dans le code JSON renvoyé par Facebook
// http://go.microsoft.com/fwlink/?LinkId=273889

namespace MvcApplication7.Models
{
    public class MyAppUserFriend
    {
        public string Name { get; set; }
        public string Link { get; set; }

        [FacebookFieldModifier("height(100).width(100)")] // Cette option définit la hauteur et la largeur de l'image à 100px.
        public FacebookConnection<FacebookPicture> Picture { get; set; }
    }
}
