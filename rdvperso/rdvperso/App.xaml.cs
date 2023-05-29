using rdvperso.Database;
using rdvperso.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rdvperso
{
    public partial class App : Application
    {
        static InfosDatabase database;
        public static InfosDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new InfosDatabase();
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

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
