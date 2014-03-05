using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using _30JoursDeBD.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

        private List<Item> _myCollection = new List<Item>();
        public List<Item> MyCollection { get { return _myCollection; } }

        public Item Test = new Item();
        public Item Test2 = new Item();
        public Item Test3 = new Item();
        public Item Test4 = new Item();
        public Item Test5 = new Item();
        public Item Test6 = new Item();
        public Item Test7 = new Item();
        public Item Test8 = new Item();
        public Item Test9 = new Item();
        public Item Test10 = new Item();
        public Item Test11 = new Item();
        public Item Test12 = new Item();
        public Item Test13 = new Item();

        public class Item
        {
            public string Titre { get; set; }
            public string Auteur { get; set; }
            public string Rubrique { get; set; }
            public string Note { get; set; }
            public string Image { get; set; }
            public Uri Link { get; set; }
        }


        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += navigationHelper_LoadState;
            //this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.DataContext = this;

            Test.Titre = "La vie privée des extraterrestres #2La vie privée des extraterrestre";
            Test.Auteur = "Kix";
            Test.Rubrique = "Planches";
            Test.Note = "Assets/Star.png";
            Test.Image = "Assets/Img_Strip.png";
            Test2 = Test;
            Test3 = Test;
            Test4 = Test;
            Test5 = Test;
            Test6 = Test;
            Test7 = Test;
            Test8 = Test;
            Test9 = Test;
            Test10 = Test;
            Test11 = Test;
            Test12 = Test;
            Test13 = Test;
            _myCollection.Add(Test);
            _myCollection.Add(Test2);
            _myCollection.Add(Test3);
            _myCollection.Add(Test4);
            _myCollection.Add(Test5);
            _myCollection.Add(Test6);
            _myCollection.Add(Test7);
            _myCollection.Add(Test8);
            _myCollection.Add(Test9);
            _myCollection.Add(Test10);
            _myCollection.Add(Test11);
            _myCollection.Add(Test12);
            _myCollection.Add(Test13);

            this.SizeChanged += Page_SizeChanged;
        }


        //Gestion des Visuals States en fonction de la taille de l'écran, lors de l'appel de l'event SizeChanged
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 600)
            {
                VisualStateManager.GoToState(this, "NarrowLayout", true);
                CacheMenuPOR();
            }
            else if (e.NewSize.Height > e.NewSize.Width)
            {
                VisualStateManager.GoToState(this, "PortraitLayout", true);
            }
            else if (e.NewSize.Width > 2000)
            {
                VisualStateManager.GoToState(this, "BigDefaultLayout", true);
                CacheMenuPOR();
            }
            else
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
                CacheMenuPOR();
            }
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MenuPOR.Height.Value > 0)
            {
                MenuPOR.Height = new GridLength(0, GridUnitType.Star);
                CorpsPOR.Height = new GridLength(115, GridUnitType.Star);
            }
            else
            {
                MenuPOR.Height = new GridLength(20, GridUnitType.Star);
                CorpsPOR.Height = new GridLength(95, GridUnitType.Star);
            }
            Frame.Navigate(typeof(Liste_Auteur_Page));
        }

        private void CacheMenuPOR()
        {
            MenuPOR.Height = new GridLength(0, GridUnitType.Star);
            CorpsPOR.Height = new GridLength(115, GridUnitType.Star);
        }

    }
}

