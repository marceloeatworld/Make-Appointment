using rdvperso.Models;
using rdvperso.Resx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MvvmHelpers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using rdvperso.Services;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProvidersPage : ContentPage
    {
        public ObservableCollection<ClassProv> ProvidersCollection;

        public ProvidersPage()
        {
            InitializeComponent();
            VerifCo();
            ProvidersCollection = new ObservableCollection<ClassProv>();
            GetProviders().SafeFireAndForget(ex => Debug.WriteLine(ex));
            textNomP.Text = Convert.ToString(AppResources.Hello);
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


        private async Task GetProviders()
        {
            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var providers = await ApiService.GetProviders();
            UserDialogs.Instance.HideLoading();
            if (providers.Any())
            {
                foreach (var provider in providers)
                {
                    ProvidersCollection.Add(provider);
                }
                CvProviders.ItemsSource = ProvidersCollection;
            }
            else
            {
                await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);
            }
        }
        private async void CvProviders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as ClassProv;
            if (currentSelection == null) return;
            Preferences.Set("providerid", currentSelection.id);
            await this.Navigation.PushAsync(new HomePage(currentSelection.id));
            ((CollectionView)sender).SelectedItem = null;
        }

    }
}