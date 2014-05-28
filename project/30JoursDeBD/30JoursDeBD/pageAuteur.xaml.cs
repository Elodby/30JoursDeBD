using _30JoursDeBD.Common.testmodel;
using _30JoursDeBD.testmodel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Windows.Data.Html;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace _30JoursDeBD
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class pageAuteur : Page
    {
        private Auteur auteurSelectionne;
        private List<BD> lesPlanches = new List<BD>();
        private List<BD> lesStrips =new List<BD>();
        public List<string> LesPlanches
        {
            get
            {
                return lesPlanches.Select(p => p.Image).ToList();
            }
        }

        public List<string> LesStrips
        {
            get
            {
                return lesStrips.Select(p => p.Image).ToList();
            }
        }
        public pageAuteur()
        {
            this.InitializeComponent();
            this.SizeChanged += Page_SizeChanged;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            auteurSelectionne = e.Parameter as Auteur;
            HttpClient client = new HttpClient();
            var jsonString = await client.GetStringAsync(new Uri("http://30joursdebd.com/?json=get_author_posts&author_id=" + auteurSelectionne.Id));
            var httpresponse = JsonConvert.DeserializeObject<RootObject>(jsonString.ToString());
            BD article;
            List<Commentaire> lesCommentaires;
            foreach (Post post in httpresponse.posts)
            {
                article = new BD();
                lesCommentaires = new List<Commentaire>();
                try
                {
                    foreach(Comment comment in post.comments)
                    {
                        lesCommentaires.Add(new Commentaire()
                        {
                            Nom = comment.name,
                            Content = HtmlUtilities.ConvertToText(comment.content),
                            Date = comment.date
                        });
                    }
                    if (post.categories.Where(c => c.slug == "planches" || c.slug == "strips").Count() != 0)
                    {
                        article.Titre = HtmlUtilities.ConvertToText(post.title);
                        article.Auteur = HtmlUtilities.ConvertToText(post.author.name);
                        article.Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title;
                        article.Excerpt = HtmlUtilities.ConvertToText(post.excerpt);
                        article.Commentaires = lesCommentaires;
                        if (post.attachments.Count != 0)
                        {
                            article.Image = post.attachments.Single(c => c.slug.ToUpper().Contains("PREVIEW")
                                            || c.slug.ToUpper().Contains("BANNIERE")
                                            || c.slug.ToUpper().Contains("BANDEAU")).url;
                            article.ImagesAttachees = post.attachments.Select(a => a.url).ToList();
                        }

                        else
                        {
                            Regex regexA = new Regex(@"<a.*?href=(""|')(?<href>.*?)(""|').*?>(?<value>.*?)</a>");
                            Regex regexImg = new Regex(@"<img.*?src=(""|')(?<src>.*?)(""|').*?>(?<value>.*?)");
                            //@"<img.+?src=[""|'](.+?)[""|'].*?>");
                            if (regexA.Matches(post.content).Count != 0)
                            {
                                foreach (Match match in regexA.Matches(post.content))
                            {
                                if (article.ImagesAttachees == null)
                                    article.ImagesAttachees = new List<string>();
                                    if(match.Groups["href"].Value.Contains("30joursdebd.com"))
                                        article.ImagesAttachees.Add(match.Groups["href"].Value);
                                    else
                                        article.ImagesAttachees.Add("http://30joursdebd.com" + match.Groups["href"].Value);
                            }
                            article.Image = article.ImagesAttachees.First();
                        }
                            else if (regexImg.Matches(post.content).Count != 0)
                            {
                                foreach (Match match in regexImg.Matches(post.content))
                                {
                                    if (article.ImagesAttachees == null)
                                        article.ImagesAttachees = new List<string>();
                                    if(match.Groups["src"].Value.Contains("30joursdebd.com"))
                                        article.ImagesAttachees.Add(match.Groups["src"].Value);
                                    else
                                        article.ImagesAttachees.Add("http://30joursdebd.com" + match.Groups["src"].Value);
                                }
                                article.Image = article.ImagesAttachees.First();
                            }
                    }
                }
                }
                catch
                {
                    article.Image = post.attachments.Last().url;
                    article.ImagesAttachees = post.attachments.Select(a => a.url).ToList();


                }
                if (article.Rubrique == "Planches")
                    lesPlanches.Add(article);
                else if(article.Rubrique == "Strips")
                    lesStrips.Add(article);
            }
            this.DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarTop.Height = this.ActualHeight / 5;
            POR_Auteur.Text = auteurSelectionne.Nom;
            POR_AuteurDescription.Text = auteurSelectionne.Description;
            POR_ImageAuteur.Source = new BitmapImage(new Uri(auteurSelectionne.Image, UriKind.Absolute));

            GR_Auteur.Text = auteurSelectionne.Nom;
            GR_AuteurDescription.Text = auteurSelectionne.Description;
            GR_ImageAuteur.Source = new BitmapImage(new Uri(auteurSelectionne.Image, UriKind.Absolute));

            NAR_Auteur.Text = auteurSelectionne.Nom;
            NAR_AuteurDescription.Text = auteurSelectionne.Description;
            NAR_ImageAuteur.Source = new BitmapImage(new Uri(auteurSelectionne.Image, UriKind.Absolute));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 700)
            {
                VisualStateManager.GoToState(this, "NarrowLayout", true);
            }
            else if (e.NewSize.Height > e.NewSize.Width)
            {
                VisualStateManager.GoToState(this, "PortraitLayout", true);
            }
            else if (e.NewSize.Width > 2000)
            {
                VisualStateManager.GoToState(this, "BigDefaultLayout", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
            }
        }

        //Gestion AppBar
        #region appbar
        private void AppBar_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void AppBar_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 139, 4, 87));
        }

        private void AppBar_Tapped(object sender, TappedRoutedEventArgs e) // Navigation
        {
            string leNom = (sender as Border).Name;
            string[] tabNom = { "Accueil", "BestOf", "Planches", "Strips", "Auteurs", "Participer" };
            int i;
            for (i = 0; i < tabNom.Length; i++)
            {
                if (leNom.Contains(tabNom[i]))
                    break;
            }
            switch (i)
            {
                case 0:
                    while (Frame.CanGoBack)
                        Frame.GoBack();
                    break;
                case 1:
                    Frame.Navigate(typeof(BestOf));
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:
                    Frame.Navigate(typeof(listeAuteur));
                    break;
                case 5:
                    Frame.Navigate(typeof(Participer_Page));
                    break;
            }
        }
        #endregion

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {            
            string lienBDSelectionnee = ((Image)sender).DataContext.ToString();
            BD laBDSelectionnee;
            try 
            { 
                laBDSelectionnee = lesPlanches.Single(p => p.ImagesAttachees.Contains(lienBDSelectionnee)); 
            }
            catch(InvalidOperationException)
            {
                laBDSelectionnee = lesStrips.Single(s => s.ImagesAttachees.Contains(lienBDSelectionnee));
            }
            Frame.Navigate(typeof(pageArticle), laBDSelectionnee);
        }
        private void TouchMenu(object sender, TappedRoutedEventArgs e)
        {
            AppBarTop.IsOpen = true;
        }

    }
}
