using rdvperso.Resx;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using MvvmHelpers;
using rdvperso.Models;
using Acr.UserDialogs;
using rdvperso.Services;
using Rg.Plugins.Popup.Pages;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPopupPage : PopupPage
    {
        public ICommand AnimationClickedCommand { get; set; }
        public AddPopupPage()
        {
            AnimationClickedCommand = new Command(() =>
            {
                ValiderButtonClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
            });
            InitializeComponent();
            VerifCo();
            UpdateCustomer().SafeFireAndForget(ex => Debug.WriteLine(ex));
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






            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var response = await ApiService.RegisterUser(
                         EntfirstName.Text,
                         EntlastName.Text,
                         Entemail.Text,
                         Entphone.Text,
                         Entaddress.Text,
                         Entcity.Text,
                         Entzip.Text);
            UserDialogs.Instance.HideLoading();

            if (response)
            {

            }
            else
            {
                UpdateCustomer().SafeFireAndForget(ex => Debug.WriteLine(ex));
                //await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);
            }
        }



        private async Task UpdateCustomer()
        {
            var mail = Entemail.Text;

            var updateUser = await ApiService.GetAllCustomer();

            if (updateUser.Any())
            {
                foreach (var user in updateUser)
                {
                    if (mail == user.email)
                    {
                        Preferences.Set("idclient", user.id);
                        var register = new Register
                        {
                            id = user.id,
                            firstName = EntfirstName.Text,
                            lastName = EntlastName.Text,
                            email = Entemail.Text,
                            phone = Entphone.Text,
                            address = Entaddress.Text,
                            city = Entcity.Text,
                            zip = Entzip.Text

                        };
                        UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
                        var response1 = await ApiService.UpdateUser(register);
                        UserDialogs.Instance.HideLoading();

                        if (response1)
                        {
                            await PopupNavigation.Instance.PopAsync();
                        }

                    }

                }

            }
            else
            {
                await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);

            }




        }
    }




}