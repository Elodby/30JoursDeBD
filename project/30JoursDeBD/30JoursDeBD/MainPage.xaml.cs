using _30JoursDeBD.Common;
using _30JoursDeBD.testmodel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

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


        private List<BD> _listeBD = new List<BD>();

        public List<BD> ListeBD { get { return _listeBD; } }

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            // this.navigationHelper.LoadState += navigationHelper_LoadState;
            //this.navigationHelper.SaveState += navigationHelper_SaveState;
            
            this.SizeChanged += Page_SizeChanged;

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


  

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Storyboard de chargement
            Engrenage_Load.Begin();
            Engrenage_Load.RepeatBehavior = RepeatBehavior.Forever;


            HttpClient client = new HttpClient();
            var jsonString = await client.GetStringAsync(new Uri("http://30joursdebd.com/?json=get_recent_post&count=30"));
            var httpresponse = JsonConvert.DeserializeObject<RootObject>(jsonString.ToString());
            foreach(Post post in httpresponse.posts)
            {
                try
                {
                    if (post.categories.Where(c => c.slug == "news").Count() == 0)
                    {
                        _listeBD.Add(new BD()
                        {
                            Titre = post.title,
                            Auteur = post.author.name,
                            Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title,
                            Image = post.attachments.Single(c => c.slug.ToUpper().Contains("PREVIEW")).url,
                            Note = "Assets/Star.png"
                        });
                    }
                        
                }
                catch
                {
                    _listeBD.Add(new BD()
                    {
                        Titre = post.title,
                        Auteur = post.author.name,
                        Rubrique = post.categories.Single(c => c.slug == "strips" || c.slug == "planches").title,
                        Image = post.attachments.First().url,
                        Note = "Assets/Star.png"
                    });
                }
            }

            TrouvePremierStrip();
            TrouvePremierePlanche();
            POR_Grid_Load.Visibility = Visibility.Collapsed;
            Engrenage_Load.Stop();
            this.DataContext = this;

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
           /* foreach (var bd in ListeBD)
            {
                if (bd.Rubrique == "Planches")
                {
                    IMG_POR_Corps_Planche.Source = new BitmapImage(new Uri(bd.Image, UriKind.RelativeOrAbsolute));
                    break;
                }
            }*/
            IMG_POR_Corps_Planche.Source = new BitmapImage(new Uri(ListeBD[9].Image, UriKind.RelativeOrAbsolute));
            return;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            /*
            if ( (BD)sender.DataContext.Rubrique == "Planches" )
            {

            }
            else if ( base.Tag == "Strips" )
            {

            }*/
        }

    }
}

