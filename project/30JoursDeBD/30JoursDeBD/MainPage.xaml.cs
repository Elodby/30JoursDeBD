using _30JoursDeBD.Common;
using _30JoursDeBD.testmodel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Data.Html;
using _30JoursDeBD.Common.testmodel;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace _30JoursDeBD
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //Tout le bazar dont j'ignore l'utilité
        #region Navigation+Observable
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        #endregion

        private Auteur auteurAleatoire;

        public List<BD> ListeBD { get { return BDRecuperees.ListeBD; } }

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += navigationHelper_LoadState;
            //this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.SizeChanged += Page_SizeChanged;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            AppBarTop.IsOpen = false;
            AppBarTop.IsEnabled = false;
            AppBarTop.Visibility = Visibility.Collapsed;
        }


        //Gestion des Visuals States en fonction de la taille de l'écran, lors de l'appel de l'event SizeChanged
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ListeBD.Count == 0)
            {
                //Storyboard de chargement
                POR_Engrenage_Load.Begin();
                POR_Engrenage_Load.RepeatBehavior = RepeatBehavior.Forever;
                DEF_Engrenage_Load.Begin();
                DEF_Engrenage_Load.RepeatBehavior = RepeatBehavior.Forever;
                NAR_Engrenage_Load.Begin();
                NAR_Engrenage_Load.RepeatBehavior = RepeatBehavior.Forever;

                HttpClient client = new HttpClient();
                await BDRecuperees.initialiserLaListe(client);
                
                //Mettre au photo un auteur aléatoire
                var jsonStringListeAutheur = await client.GetStringAsync(new Uri("http://30joursdebd.com/?json=get_author_index"));
                var httpresponseListeAuteur = JsonConvert.DeserializeObject<AuthorIndex>(jsonStringListeAutheur.ToString());
                Random rand = new Random();
                int indexRandom = rand.Next(httpresponseListeAuteur.authors.Count);
                string nomAuteurAleatoire = httpresponseListeAuteur.authors[indexRandom].name;
            
                recupererDetailsAuteurAleatoire(httpresponseListeAuteur.authors[indexRandom]);
                TrouvePremierStrip();
                TrouvePremierePlanche();

                //Storyboard de chargement ( fin )
                POR_Grid_Load.Visibility = Visibility.Collapsed;
                POR_Engrenage_Load.Stop();
                DEF_Grid_Load.Visibility = Visibility.Collapsed;
                DEF_Engrenage_Load.Stop();
                NAR_Grid_Load.Visibility = Visibility.Collapsed;
                NAR_Engrenage_Load.Stop();

                itemNarrowListView.ItemsSource = ListeBD;
                this.DataContext = this;
                AppBarTop.IsEnabled = true;
                AppBarTop.IsOpen = false;
                AppBarTop.Visibility = Visibility.Visible;
            }
        }

        private async void recupererDetailsAuteurAleatoire(Author randomAuthor)
        {

            auteurAleatoire = new Auteur();
            auteurAleatoire.Id = randomAuthor.id;
            auteurAleatoire.Nom = randomAuthor.name;
            auteurAleatoire.Description = randomAuthor.description;
            auteurAleatoire.URL = randomAuthor.url;
            try
            {
            HttpClient client = new HttpClient();
                string lien = 
                    "http://30joursdebd.com/30jdbdv3/wp-content/themes/30jdbd/scripts/timthumb.php?src=/30jdbdv3/wp-content/themes/30jdbd/images/auteurs/"
                    + auteurAleatoire.Nom + ".jpg&w=130&h=130&zc=1&q=90";
                HttpResponseMessage response = await
                    client.GetAsync(lien);
                response.EnsureSuccessStatusCode();

                auteurAleatoire.Image = lien;
            }
            catch (HttpRequestException)
            {
                auteurAleatoire.Image =
                    "http://30joursdebd.com/30jdbdv3/wp-content/themes/30jdbd/scripts/timthumb.php?src=/30jdbdv3/wp-content/themes/30jdbd/images/auteurs/30JBDlogo.jpg&w=130&h=130&zc=1&q=90";
            }
            IMG_POR_Corps_Auteur.Source = new BitmapImage(new Uri(
                        auteurAleatoire.Image,
                    UriKind.Absolute));
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarTop.IsOpen = false;
            AppBarTop.Height = this.ActualHeight / 5;
        }

        private void TrouvePremierStrip(){
            foreach (var bd in ListeBD)
	        {
		        if ( bd.Rubrique == "Strips")
                {
                    IMG_POR_Corps_Strip.Source = new BitmapImage(new Uri(bd.Image, UriKind.RelativeOrAbsolute));
                    break;
                }       
	        }
            return;
        }

        private void TrouvePremierePlanche()
        {
            foreach (var bd in ListeBD)
            {
                if (bd.Rubrique == "Planches")
                {
                    IMG_POR_Corps_Planche.Source = new BitmapImage(new Uri(bd.ImagesAttachees.First(), UriKind.RelativeOrAbsolute));
                    break;
                }
            }
            return;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BD laBDSelectionnee = ((Image)sender).DataContext as BD;
            Frame.Navigate(typeof(pageArticle), laBDSelectionnee);
        }

        private void itemNarrowListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var x = ((ListView)sender).ItemsSource;
            ((ListView)sender).ItemsSource = null;
            ((ListView)sender).ItemsSource = x;
        }

        private void itemNarrowListView_Loaded(object sender, RoutedEventArgs e)
        {
            itemNarrowListView.SizeChanged += itemNarrowListView_SizeChanged;
        }


        #region appbar
        //Gestion AppBar
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
            string[] tabNom = { "Accueil", "BestOf", "Planches", "Strip", "Auteurs", "Participer" };
            int i;
            for (i = 0; i < tabNom.Length; i++)
            {
                if (leNom.Contains(tabNom[i]))
                    break;
            }
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    Frame.Navigate(typeof(BestOf));
                    break;
                case 2:
                    Frame.Navigate(typeof(PagePlanches));
                    break;
                case 3:
                    Frame.Navigate(typeof(PageStrips));
                    break;
                case 4:
                    Frame.Navigate(typeof(listeAuteur));
                    break;
                case 5:
                    Frame.Navigate(typeof(Participer_Page));
                    break;
            }
        }

        private void TouchMenu(object sender, TappedRoutedEventArgs e)
        {
            AppBarTop.IsOpen = true;
        }
        #endregion

        private void IMG_POR_Corps_Auteur_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            Frame.Navigate(typeof(pageAuteur), auteurAleatoire);
        }



    }
}

