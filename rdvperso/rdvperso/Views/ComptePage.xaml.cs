using Acr.UserDialogs;
using MvvmHelpers;
using rdvperso.Models;
using rdvperso.Resx;
using rdvperso.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComptePage : ContentPage
    {
        public ObservableCollection<Register> infosCollection;
        public ObservableCollection<Infos> PremierCollection;
        public ComptePage()
        {
            InitializeComponent();
            VerifCo();
            GetCustomers().SafeFireAndForget(ex => Debug.WriteLine(ex));
            infosCollection = new ObservableCollection<Register>();
            PremierCollection = new ObservableCollection<Infos>();
            Fenetre();
            GetinfosCollection.RefreshCommand = new Command((obj) =>
               {
                   infosCollection.Clear();
                   GetCustomers().SafeFireAndForget(ex => Debug.WriteLine(ex));
                   GetinfosCollection.IsRefreshing = false;
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
   
          
            private async Task GetCustomers()
            {
            base.OnAppearing();
            var register = new Register
                {
                    id = Preferences.Get("idclient", 0)
                };
                UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
                var informations = await ApiService.GetCustomer(register);
                UserDialogs.Instance.HideLoading();
                if (informations.id == 0)
                {
                    await DisplayAlert(AppResources.Desoler, AppResources.ErrorCompte, AppResources.OK);
                    //await this.Navigation.PushAsync(new ProvidersPage());
                }
                else
                {
                    infosCollection.Add(informations);
                    GetinfosCollection.ItemsSource = infosCollection;
                }
            }
        
        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(AppResources.SupprimerCompte, AppResources.QuestionSupCompte, AppResources.Oui, AppResources.Non);
            if (result == true)
            {
                var toucher = ((MenuItem)sender);
                var cl = toucher.CommandParameter as Register;
                var register = new Register
                {
                    id = cl.id
                };
                UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
                var response = await ApiService.DeleteAccount(register);
                UserDialogs.Instance.HideLoading();
                if (response)
                {
                    Preferences.Clear("nomclient");
                    Preferences.Clear("addressclient");
                    Preferences.Clear("cityclient");
                    Preferences.Clear("codePTclient");
                    Preferences.Clear("telclient");
                    Preferences.Clear("mailclient");
                    Preferences.Clear("prenomclient");
                    Preferences.Clear("idclient");
                    infosCollection.Remove(cl);
                    await DisplayAlert(AppResources.Merci, AppResources.DeleteCompte, AppResources.OK);
                    //await this.Navigation.PushAsync(new ProvidersPage());
                }
                else
                {
                    await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);
                }
            }
        }
        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(AppResources.AjouterCompte, AppResources.QuestionAddCompte, AppResources.Oui, AppResources.Non);
            if (result == true)
            {
                await PopupNavigation.Instance.PushAsync(new AddPopupPage());
            }
        }

        private async void BtnEdit_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(AppResources.ModifierCompte, AppResources.QuestionCompte, AppResources.Oui, AppResources.Non);
            if (result == true)
            {
                await PopupNavigation.Instance.PushAsync(new EditPopupPage());
            }
        }
    }
}






