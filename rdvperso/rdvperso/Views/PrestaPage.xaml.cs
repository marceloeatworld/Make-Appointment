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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrestaPage : ContentPage
    {
        public ObservableCollection<ClassServices> ClassServicesByCategoryCollection;

        public PrestaPage(int categoryId, string name)
        {
            InitializeComponent();
            ClassServicesByCategoryCollection = new ObservableCollection<ClassServices>();
            GetClassServices().SafeFireAndForget(ex => Debug.WriteLine(ex));
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
        private async Task GetClassServices()
        {
            var idpresta = Preferences.Get("idcategory", 0);
            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var prestations = await ApiService.GetServices();
            UserDialogs.Instance.HideLoading();
            if (prestations.Any())
            {

                foreach (var prestation in prestations)
                {
                    if (idpresta == prestation.categoryId)
                    {

                        ClassServicesByCategoryCollection.Add(prestation);
                    }


                }
                CvPrestations.ItemsSource = ClassServicesByCategoryCollection;

            }
            else
            {
                await DisplayAlert(AppResources.Desoler, AppResources.Error, AppResources.OK);

                await Shell.Current.GoToAsync("//ProvidersPage");
            }
        }

        private async void CvPrestations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as ClassServices;
            if (currentSelection == null) return;
            Preferences.Set("serviceid", currentSelection.id);
            Preferences.Set("duree", currentSelection.duration);
            Preferences.Set("nomservice", currentSelection.name);
            await this.Navigation.PushAsync(new AvailabilitiesPage(currentSelection.id, currentSelection.duration, currentSelection.name));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}