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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentsPage : ContentPage
    {
        public ICommand AnimationClickedCommand { get; set; }
        public ObservableCollection<Categories> CategoriesCollection;
        public AppointmentsPage()
        {
            AnimationClickedCommand = new Command(() =>
            {
                ValiderButtonClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
            });
            BindingContext = this;
            InitializeComponent();
            CategoriesCollection = new ObservableCollection<Categories>();
            VerifCo();
            GetImage().SafeFireAndForget(ex => Debug.WriteLine(ex));
            var validerDate = DateTime.Parse(Preferences.Get("Daterdv", string.Empty), System.Globalization.CultureInfo.CurrentCulture).ToString("dddd dd MMMM") + " " + Preferences.Get("idRdv", string.Empty);
            textNomP.Text = Convert.ToString(Preferences.Get("prenomclient", string.Empty) + " " + Preferences.Get("nomclient", string.Empty));
            textStart.Text = Convert.ToString(validerDate);
            textAddr.Text = Convert.ToString(Preferences.Get("addressclient", string.Empty));
            textCity.Text = Convert.ToString(Preferences.Get("cityclient", string.Empty));
            textCP.Text = Convert.ToString(Preferences.Get("codePTclient", string.Empty));
            textTel.Text = Convert.ToString(Preferences.Get("telclient", string.Empty));
            textMail.Text = Convert.ToString(Preferences.Get("mailclient", string.Empty));
            textcategory.Text = Convert.ToString(Preferences.Get("nomcategory", string.Empty));
            textservice.Text = Convert.ToString(Preferences.Get("nomservice", string.Empty));
        }
        private async Task GetImage()
        {
          
            var categories = new Categories
            {
                id = Preferences.Get("idcategory", 0)
            };
       
            var image = await ApiService.GetCategoriesById(categories);
        
            if (image.id == 0)
            {
               
            }
            else
            {
                CategoriesCollection.Add(image);
                Cvimage.ItemsSource = CategoriesCollection;
            }

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
            int duree = Preferences.Get("duree", 0);
            DateTime localDate = DateTime.Now;
            var appointments = new Appointments
            {
                id = 0,
                book = localDate.ToString("yyyy-MM-dd HH:mm:ss"),
                start = Preferences.Get("Daterdv", string.Empty) + " " + Preferences.Get("idRdv", string.Empty) + ":00",
                end = Preferences.Get("Daterdv", string.Empty) + " " + (DateTime.Parse(Preferences.Get("idRdv", string.Empty), System.Globalization.CultureInfo.CurrentCulture).AddMinutes(duree).ToString("HH:mm")) + ":00",
                hash = " ",
                notes = EntNote.Text + " " + Preferences.Get("nomcategory", string.Empty),
                customerId = Preferences.Get("idclient", 0),
                providerId = Preferences.Get("providerid", 0),
                serviceId = Preferences.Get("serviceid", 0),
                googleCalendarId = null
            };
            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var response = await ApiService.PostAppointments(appointments);
            UserDialogs.Instance.HideLoading();
            if (response)
            {
                await Shell.Current.GoToAsync($"//ProvidersPage");

            }
            else
            {
                await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);
            }
        }
    }
}