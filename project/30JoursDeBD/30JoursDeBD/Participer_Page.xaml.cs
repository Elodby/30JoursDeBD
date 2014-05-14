using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using _30JoursDeBD.Common;
using Windows.UI;


// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace _30JoursDeBD
{
    public sealed partial class Participer_Page : Page
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


        public Participer_Page()
        {
             this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            // this.navigationHelper.LoadState += navigationHelper_LoadState;
            //this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.SizeChanged += Page_SizeChanged;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
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


  

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarTop.IsOpen = false;
            AppBarTop.Height = this.ActualHeight / 5;
            

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
            string[] tabNom = {"Accueil","BD","Albums","BestOf","Auteurs","Participer"};
            int i;
            for ( i=0; i < tabNom.Length; i++)
            {
                if ( leNom.Contains(tabNom[i]))
                    break;
            }
            switch(i)
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
                    break;
            }
        }

        private void RetourText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}