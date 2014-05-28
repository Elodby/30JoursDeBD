using _30JoursDeBD.testmodel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;

namespace _30JoursDeBD.Common.testmodel
{
    public class BDRecuperees
    {
        public static List<BD> ListeBD = new List<BD>();

        public async static Task initialiserLaListe(HttpClient client)
        {
            var jsonString = await client.GetStringAsync(new Uri("http://30joursdebd.com/?json=get_recent_post&count=30"));
            var httpresponse = JsonConvert.DeserializeObject<RootObject>(jsonString.ToString());

            BD uneBD;

            foreach (Post post in httpresponse.posts)
            {
                List<Commentaire> lesCommentaires = new List<Commentaire>();
                uneBD = new BD();
                try
                {
                    foreach (Comment comment in post.comments)
                    {
                        lesCommentaires.Add(new Commentaire()
                        {
                            Content = HtmlUtilities.ConvertToText(comment.content),
                            Date = comment.date,
                            Nom = comment.name
                        });
                    }
                    if (post.categories.Where(c => c.slug == "strips" || c.slug == "planches").Count() != 0)
                    {
                        uneBD.Titre = HtmlUtilities.ConvertToText(post.title);
                        uneBD.Auteur = HtmlUtilities.ConvertToText(post.author.name);
                        uneBD.Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title;
                        uneBD.Image = post.attachments.Single(c => c.slug.ToUpper().Contains("PREVIEW")
                            || c.slug.ToUpper().Contains("BANNIERE")
                            || c.slug.ToUpper().Contains("BANDEAU")).url;
                        uneBD.ImagesAttachees = post.attachments.Select(a => a.url).ToList();
                        uneBD.ImagesAttachees.Sort();
                        uneBD.NombreVues = post.custom_fields.views.First();
                        uneBD.Excerpt = HtmlUtilities.ConvertToText(post.excerpt);
                        uneBD.Commentaires = lesCommentaires;
                        ListeBD.Add(uneBD);
                    }

                }
                catch
                {
                    uneBD.Titre = HtmlUtilities.ConvertToText(post.title);
                    uneBD.Auteur = HtmlUtilities.ConvertToText(post.author.name);
                    uneBD.Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title;
                    uneBD.Image = post.attachments.Last().url;
                    uneBD.ImagesAttachees = post.attachments.Select(a => a.url).ToList();
                    uneBD.ImagesAttachees.Sort();
                    uneBD.Excerpt = HtmlUtilities.ConvertToText(post.excerpt);
                    uneBD.Commentaires = lesCommentaires;
                    uneBD.NombreVues = post.custom_fields.views.First();
                    ListeBD.Add(uneBD);
                }
            }
        }
        
    }
}
