using _30JoursDeBD.Common;
using _30JoursDeBD.Common.testmodel;
using _30JoursDeBD.testmodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page de base, consultez la page http://go.microsoft.com/fwlink/?LinkId=234237

namespace _30JoursDeBD
{
    /// <summary>
    /// Page de base qui inclut des caractéristiques communes à la plupart des applications.
    /// </summary>
    public sealed partial class BestOf : Page
    {

        #region navigationHelper / DefaultViewModel
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// Cela peut être remplacé par un modèle d'affichage fortement typé.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper est utilisé sur chaque page pour faciliter la navigation et 
        /// gestion de la durée de vie des processus
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        } 
        #endregion
        public List<BD> ListeBD
        {
            get
            {
                return BDRecuperees.ListeBD;
            }
        }

        public BestOf()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.SizeChanged += Page_SizeChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AppBarTop.IsOpen = false;
            this.DataContext = this;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarTop.IsOpen = false;
            AppBarTop.Height = this.ActualHeight / 5;
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

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e){}
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e){}



        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BD laBDSelectionnee = ((Image)sender).DataContext as BD;
            Frame.Navigate(typeof(pageArticle), laBDSelectionnee);
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
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:
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
    }
}
