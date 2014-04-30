using _30JoursDeBD.Common;
using _30JoursDeBD.Common.testmodel;
using _30JoursDeBD.testmodel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace _30JoursDeBD
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class listeAuteur : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private List<Auteur> _listeAuteur = new List<Auteur>();
        private List<Auteur> _listeFiltre = new List<Auteur>();
        public List<Auteur> ListeAuteur { get { return _listeAuteur; } }
        public List<Auteur> ListeFiltre { get { return _listeFiltre; } }
        private string[] Alphabet = new string[28] {"Tout","0-9","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

        #region navigationhelper
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


        public listeAuteur()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += navigationHelper_LoadState;
            //this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            this.POR_ComboBoxFiltre.ItemsSource = Alphabet;
            this.POR_ComboBoxFiltre.SelectedIndex = 0;
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_listeAuteur.Count == 0)
            {
                //Storyboard de chargement
                POR_Engrenage_Load.Begin();
                POR_Engrenage_Load.RepeatBehavior = RepeatBehavior.Forever;
                DEF_Engrenage_Load.Begin();
                DEF_Engrenage_Load.RepeatBehavior = RepeatBehavior.Forever;

                HttpClient client = new HttpClient();
                var jsonString = await client.GetStringAsync(new Uri("http://30joursdebd.com/?json=get_author_index"));
                var httpresponse = JsonConvert.DeserializeObject<AuthorIndex>(jsonString.ToString());
                Auteur  auteur;
                List<Auteur> lstAut = new List<Auteur>();
                foreach (Author a in httpresponse.authors)
                {
                    auteur = new Auteur();
                        auteur.Id = a.id;
                    auteur.Nom = a.name;
                    auteur.URL = a.url;
                        auteur.Image = "http://30joursdebd.com/30jdbdv3/wp-content/themes/30jdbd/scripts/timthumb.php?src=/30jdbdv3/wp-content/themes/30jdbd/images/auteurs/" + a.name + ".jpg&w=130&h=130&zc=1&q=90";
                    auteur.Description = a.description;
                    lstAut.Add(auteur);
                }

                //Tri des auteurs par ordre alphabétique
                IEnumerable<Auteur> sortedAuteurs =
                    from aut in lstAut
                    orderby aut.Nom ascending
                    select aut;
                _listeAuteur = sortedAuteurs.ToList();
                lstAut.Clear();


                //Storyboard de chargement ( fin )
                POR_Grid_Load.Visibility = Visibility.Collapsed;
                POR_Engrenage_Load.Stop();
                DEF_Grid_Load.Visibility = Visibility.Collapsed;
                DEF_Engrenage_Load.Stop();

                this.DataContext = this;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarTop.IsOpen = false;
            AppBarTop.Height = this.ActualHeight / 5;
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

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Auteur AuteurSelectionne = ((Image)sender).DataContext as Auteur;
            Frame.Navigate(typeof(pageAuteur), AuteurSelectionne);
        }


        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }

        #endregion
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
                    
                    break;
                case 5:
                   
                    break;
            }
        }

        private void TouchMenu(object sender, TappedRoutedEventArgs e)
        {
            //Frame.GoBack();
        }


        /// <summary>
        /// Méthodes de tri des auteurs
        /// </summary>
        /// 

        private void POR_BoutonFiltre_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (POR_ComboBoxFiltre.SelectedItem != null)
            {
                FiltreAuteurs((string)POR_ComboBoxFiltre.SelectedValue);
            }
        }

        private void FiltreAuteurs(string carac)
        {
            _listeFiltre.Clear();
            if (carac == "Tout")
            {
                GestionVisibiliyNoAuteur(ListeAuteur);
                SourceGridView.Source = null;
                SourceGridView.Source = ListeAuteur;
            }
            else
            {
                if (carac == "0-9")
                {
                    foreach (Auteur a in _listeAuteur)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (a.Nom.ToUpper().StartsWith(i.ToString()))
                            {
                                _listeFiltre.Add(a);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (Auteur a in _listeAuteur)
                    {
                        if (a.Nom.ToUpper().StartsWith(carac))
                        {
                            _listeFiltre.Add(a);
                        }
                    }
                    
                }

                GestionVisibiliyNoAuteur(ListeFiltre);
                SourceGridView.Source = null;
                SourceGridView.Source = ListeFiltre;
                
            }
            
        }

        private void GestionVisibiliyNoAuteur(List<Auteur> lst)
        {
            if (lst.Count == 0)
            {
                POR_NoAuteurs.Visibility = Visibility.Visible;
            }
            else
            {
                POR_NoAuteurs.Visibility = Visibility.Collapsed;
            }
        }
    }
}
