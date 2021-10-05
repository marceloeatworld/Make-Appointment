using Acr.UserDialogs;
using rdvperso.Models;
using rdvperso.Services;
using System;
using System.Diagnostics;
using MvvmHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using rdvperso.Resx;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AvailabilitiesPage : ContentPage
    {
        public ObservableCollection<string> AvailabilitiesCollection;
        public DateTime Today { get; set; }
        public AvailabilitiesPage(int id, int duration, string name)
        {
            InitializeComponent();
            this.BindingContext = this;
            Today = DateTime.Now;
            AvailabilitiesCollection = new ObservableCollection<string>();
            VerifCo();
        }

        private async void VerifCo()
        {
            var networkAccess = Connectivity.NetworkAccess;
            if (networkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert(AppResources.Desoler, AppResources.ConnectionError, AppResources.OK);
                return;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var rdv = new Rdv
            {
                date = Today.ToString("yyyy-MM-dd"),
                serviceId = Preferences.Get("serviceid", 0),
                providerId = Preferences.Get("providerid", 0), //AppSettings.prov
            };
            Preferences.Set("Daterdv", rdv.date);
            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var heures = await ApiService.GetAvailabilities(rdv);
            UserDialogs.Instance.HideLoading();
            if (heures.Any())
            {
                AvailabilitiesCollection.Clear();
                foreach (var heure in heures)
                {
                    AvailabilitiesCollection.Add(heure);
                }
                CvAvailabilities.ItemsSource = AvailabilitiesCollection;
            }
            else
            {
                AvailabilitiesCollection.Clear();
                await DisplayAlert(AppResources.Desoler, AppResources.ErrorHoraire, AppResources.OK);
            }
        }

        private async void DatePicker_OnSelectedIndexChanged(object sender, DateChangedEventArgs e)
        {
            var rdv = new Rdv
            {
                date = e.NewDate.ToString("yyyy-MM-dd"),
                serviceId = Preferences.Get("serviceid", 0),
                providerId = Preferences.Get("providerid", 0),
            };
            Preferences.Set("Daterdv", rdv.date);
            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var heures = await ApiService.GetAvailabilities(rdv);
            UserDialogs.Instance.HideLoading();
            if (heures.Any())
            {
                AvailabilitiesCollection.Clear();
                foreach (var heure in heures)
                {
                    AvailabilitiesCollection.Add(heure);
                }
                CvAvailabilities.ItemsSource = AvailabilitiesCollection;
            }
            else
            {
                AvailabilitiesCollection.Clear();
                await DisplayAlert(AppResources.Desoler, AppResources.ErrorHoraire, AppResources.OK);
            }
        }

        private async void CvAvailabilities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var register = new Register
            {
                id = Preferences.Get("idclient", 0)
            };
            UserDialogs.Instance.ShowLoading("Chargement", MaskType.Black);
            var response = await ApiService.VerifCustomer(register);
            UserDialogs.Instance.HideLoading();
            if (response)
            {
                var currentSelection = e.CurrentSelection.FirstOrDefault() as string;
                if (currentSelection == null) return;
                Preferences.Set("idRdv", currentSelection.ToString());
                await this.Navigation.PushAsync(new AppointmentsPage());
                ((CollectionView)sender).SelectedItem = null;
            }
            else
            {
                var currentSelection = e.CurrentSelection.FirstOrDefault() as string;
                if (currentSelection == null) return;
                Preferences.Set("idRdv", currentSelection.ToString());
                await this.Navigation.PushAsync(new RegisterPage(currentSelection.ToString()));
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}