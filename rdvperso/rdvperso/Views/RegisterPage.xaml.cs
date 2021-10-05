using Acr.UserDialogs;
using MvvmHelpers;
using rdvperso.Models;
using rdvperso.Resx;
using rdvperso.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        //public ObservableCollection<Register> RegisterCollection;
        public ICommand AnimationClickedCommand { get; set; }
        public RegisterPage(string id)
        {
            AnimationClickedCommand = new Command(() =>
            {
                BtnSignUpClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
            });
            BindingContext = this;
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
        private void BtnSignUp_Clicked(object sender, EventArgs e)
        {
            BtnSignUpClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
        }

        private async Task BtnSignUpClickedAsync()
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
                await this.Navigation.PushAsync(new AppointmentsPage());

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
                            await this.Navigation.PushAsync(new AppointmentsPage());
                        }
                    }
                    //break;
                }
            }
            else
            {
                await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);
            }




        }


    }
}