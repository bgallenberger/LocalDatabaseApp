using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocalDatabaseApp
{
    public partial class App : Application
    {
        static FactDatabase database;
        public App()
        {
            InitializeComponent();

            var nav = new NavigationPage(new MainPage());
            nav.BarBackgroundColor = (Color)App.Current.Resources["primaryGreen"];
            nav.BarTextColor = Color.White;

            MainPage = nav;
        }

        public static FactDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new FactDatabase();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
