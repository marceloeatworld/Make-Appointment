using Acr.UserDialogs;
using MvvmHelpers;
using rdvperso.Models;
using rdvperso.Resx;
using rdvperso.Services;
using Rg.Plugins.Popup.Pages;
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
    public partial class EditPopupPage : PopupPage
    {

        public ICommand AnimationClickedCommand { get; set; }
        public EditPopupPage()
        {
            AnimationClickedCommand = new Command(() =>
            {
                ValiderButtonClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
            });
            InitializeComponent();
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
        private void BtnValider_Clicked(object sender, EventArgs e)
        {
            ValiderButtonClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
        }


        private async Task ValiderButtonClickedAsync()
        {


            var register = new Register
            {
                id = Preferences.Get("idclient", 0),
                firstName = EntfirstName.Text,
                lastName = EntlastName.Text,
                email = Entemail.Text,
                phone = Entphone.Text,
                address = Entaddress.Text,
                city = Entcity.Text,
                zip = Entzip.Text

            };
            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var response = await ApiService.UpdateUser(register);
            UserDialogs.Instance.HideLoading();
            if (response)
            {
                await PopupNavigation.Instance.PopAsync();


            }
            else
            {
                await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);
            }
        }


    }
}