using _30JoursDeBD.Common;
using _30JoursDeBD.Common.testmodel;
using _30JoursDeBD.testmodel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace _30JoursDeBD
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class pageArticle : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary(); 
        private bool DEF_zoom = false;

        // BD récupérée
        private BD maBD;
        public BD MaBD { get { return maBD; } }

        //Listes
        private List<string> lesImages;
        public List<string> LesImages { get { return lesImages; } }
        private List<Commentaire> lesCommentaires = new List<Commentaire>();
        public List<Commentaire> LesCommentaires { get { return lesCommentaires; } }

        

        #region MyRegion
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
        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session. 
        #endregion


        public pageArticle()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += navigationHelper_LoadState;
            //this.navigationHelper.SaveState += navigationHelper_SaveState;

            this.SizeChanged += Page_SizeChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AppBarTop.IsOpen = false;
            maBD = e.Parameter as BD;
            lesImages = maBD.ImagesAttachees;
            lesCommentaires = maBD.Commentaires;
            if (lesCommentaires.Count == 0)
            {
                lesCommentaires.Add(new Commentaire());
                lesCommentaires[0].Nom = "Aucun commentaire";
            }
            foreach(string nom in lesImages)
            {
                if(nom.ToUpper().Contains("PREVIEW")
                    || nom.ToUpper().Contains("BANNIERE")
                    || nom.ToUpper().Contains("BANDEAU"))
                {
                    lesImages.Remove(nom);
                    break;
                }
            }
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarTop.IsOpen = false;
            AppBarTop.Height = this.ActualHeight / 5;
            POR_Auteur.Text = maBD.Auteur;          DEF_Auteur.Text = maBD.Auteur;
            POR_Excerpt.Text = maBD.Excerpt;        DEF_Excerpt.Text = maBD.Excerpt;
            POR_Titre.Text = maBD.Titre;            DEF_Titre.Text = maBD.Titre;
            if (POR_Titre.Text.Length > 50)
            { POR_Titre.FontSize = 20; }

            this.DataContext = this;
        }
      



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
                    while (Frame.CanGoBack)
                        Frame.GoBack();
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

        
        // Menu
        private void TouchMenu(object sender, TappedRoutedEventArgs e)
        {
            AppBarTop.IsOpen = true;
        }

        //Appuie sur le bouton Retour
        private void RetourText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }

        //Appuie sur la loupe
        private void DEF_Zoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (DEF_zoom)
            {
                DEF_Corps_LesPlanches.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                DEF_Corps_LesPlanches.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                DEF_zoom = false;
            }
            else
            {
                DEF_Corps_LesPlanches.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Star);
                DEF_Corps_LesPlanches.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Star);              
                DEF_zoom = true;
            }
        }

    }
}
