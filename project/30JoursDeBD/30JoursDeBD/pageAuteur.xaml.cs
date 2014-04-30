using _30JoursDeBD.Common.testmodel;
using _30JoursDeBD.testmodel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Html;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
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
                return lesPlanches.Select(p => p.Image).ToList();
            }
        }
        public pageAuteur()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            auteurSelectionne = e.Parameter as Auteur;
            HttpClient client = new HttpClient();
            var jsonString = await client.GetStringAsync(new Uri("http://30joursdebd.com/?json=get_author_posts&author_id=" + auteurSelectionne.Id));
            var httpresponse = JsonConvert.DeserializeObject<RootObject>(jsonString.ToString());
            foreach (Post post in httpresponse.posts)
            {
                try
                {
                    if (post.categories.Where(c => c.slug == "planches").Count() != 0)
                    {
                        lesPlanches.Add(new BD()
                        {
                            Titre = HtmlUtilities.ConvertToText(post.title),
                            Auteur = HtmlUtilities.ConvertToText(post.author.name),
                            Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title,
                            Image = post.attachments.Single(c => c.slug.ToUpper().Contains("PREVIEW")
                                || c.slug.ToUpper().Contains("BANNIERE")
                                || c.slug.ToUpper().Contains("BANDEAU")).url,
                            ImagesAttachees = post.attachments.Select(a => a.url).ToList(),
                            Excerpt = HtmlUtilities.ConvertToText(post.excerpt),
                            Note = "Assets/Star.png"
                        });
                    }
                    else if (post.categories.Where(c => c.slug == "strips").Count() != 0)
                    {
                        lesStrips.Add(new BD()
                        {
                            Titre = HtmlUtilities.ConvertToText(post.title),
                            Auteur = HtmlUtilities.ConvertToText(post.author.name),
                            Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title,
                            Image = post.attachments.Single(c => c.slug.ToUpper().Contains("PREVIEW")
                                || c.slug.ToUpper().Contains("BANNIERE")
                                || c.slug.ToUpper().Contains("BANDEAU")).url,
                            ImagesAttachees = post.attachments.Select(a => a.url).ToList(),
                            Excerpt = HtmlUtilities.ConvertToText(post.excerpt),
                            Note = "Assets/Star.png"
                        });
                    }
                }
                catch
                {
                    if (post.categories.Where(c => c.slug == "planches").Count() != 0)
                    {
                        lesPlanches.Add(new BD()
                        {
                            Titre = HtmlUtilities.ConvertToText(post.title),
                            Auteur = HtmlUtilities.ConvertToText(post.author.name),
                            Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title,
                            Image = post.attachments.First().url,
                            ImagesAttachees = post.attachments.Select(a => a.url).ToList(),
                            Excerpt = HtmlUtilities.ConvertToText(post.excerpt),
                            Note = "Assets/Star.png"
                        });
                    }
                    else if (post.categories.Where(c => c.slug == "strips").Count() != 0)
                    {
                        lesStrips.Add(new BD()
                        {
                            Titre = HtmlUtilities.ConvertToText(post.title),
                            Auteur = HtmlUtilities.ConvertToText(post.author.name),
                            Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title,
                            Image = post.attachments.First().url,
                            ImagesAttachees = post.attachments.Select(a => a.url).ToList(),
                            Excerpt = HtmlUtilities.ConvertToText(post.excerpt),
                            Note = "Assets/Star.png"
                        });
                    }
                }
            }
            this.DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarTop.Height = this.ActualHeight / 5;
            POR_Auteur.Text = auteurSelectionne.Nom;
            POR_AuteurDescription.Text = auteurSelectionne.Description;
            POR_ImageAuteur.Source = new BitmapImage(new Uri(auteurSelectionne.Image, UriKind.Absolute));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 600)
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
            string[] tabNom = { "Accueil", "BD", "Albums", "BestOf", "Auteurs", "Participer" };
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
    }
}
