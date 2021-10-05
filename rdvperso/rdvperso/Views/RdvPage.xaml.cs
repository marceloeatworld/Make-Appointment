using Acr.UserDialogs;
using MvvmHelpers;
using rdvperso.Models;
using rdvperso.Resx;
using rdvperso.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RdvPage : ContentPage
    {
        public ObservableCollection<Infos> PremierCollection;
        public ObservableCollection<Aggregates.MyArray> RdvCollection;
        public RdvPage()
        {
            BindingContext = this;
            InitializeComponent();
            VerifCo();
            PremierCollection = new ObservableCollection<Infos>();
            RdvCollection = new ObservableCollection<Aggregates.MyArray>();
            GetRdv().SafeFireAndForget(ex => Debug.WriteLine(ex));
            Fenetre();
            GetRdvCollection.RefreshCommand = new Command((obj) =>
            {
                RdvCollection.Clear();
                GetRdv().SafeFireAndForget(ex => Debug.WriteLine(ex));
                GetRdvCollection.IsRefreshing = false;
            });
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
        private async void Fenetre()
        {
            int pop = Preferences.Get("popswipe", 0);
            if (pop == 1)
            {
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new ClickPopupPage());
            }
        }

        private async Task GetRdv()
        {
            base.OnAppearing();
            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var rdvs = await ApiService.GetAppointments();
            var idd = Preferences.Get("idclient", 0);
            UserDialogs.Instance.HideLoading();
            if (rdvs.Any())
            {
                foreach (var rdv in rdvs)
                {
                    if (idd == rdv.customerId && (DateTime.Parse(rdv.start, System.Globalization.CultureInfo.CurrentCulture).AddHours(2) >= DateTime.Now))
                    {
                        rdv.start = DateTime.Parse(rdv.start, System.Globalization.CultureInfo.CurrentCulture).ToString("dddd dd MMMM HH:mm");
                        RdvCollection.Add(rdv);
                    }
                }
                GetRdvCollection.ItemsSource = RdvCollection;
            }
            else
            {
                await DisplayAlert(AppResources.Desoler, AppResources.ErrorRdv, AppResources.OK);
            }
        }


        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(AppResources.AnnulerRdv, AppResources.QuestionRdv, AppResources.Oui, AppResources.Non);
            if (result == true)
            {
                var toucher = ((MenuItem)sender);
                var cl = toucher.CommandParameter as Aggregates.MyArray;
                var appointments = new Appointments
                {
                    id = cl.id
                };
                UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
                var response = await ApiService.DeleteAppointments(appointments);
                UserDialogs.Instance.HideLoading();
                if (response)
                {
                    RdvCollection.Clear();
                    UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
                    await GetRdv();
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);
                }
            }
        }

    }
}