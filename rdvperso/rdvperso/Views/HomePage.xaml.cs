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
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<Categories> CategoriesCollection;
        public HomePage(int id)
        {
            InitializeComponent();
            this.BindingContext = this;
            VerifCo();

            CategoriesCollection = new ObservableCollection<Categories>();

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

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            BtnSearchClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
        }

        private async Task BtnSearchClickedAsync()
        {
            String name = EntSearch.Text;
            UserDialogs.Instance.ShowLoading(AppResources.Chargement, MaskType.Black);
            var categories = await ApiService.GetCategoriesByName(name);
            UserDialogs.Instance.HideLoading();
            CategoriesCollection.Clear();
            if (categories.Any())
            {
                foreach (var category in categories)
                {
                    CategoriesCollection.Add(category);
                }
                CvCategories.ItemsSource = CategoriesCollection;
            }
            else
            {
                await DisplayAlert(AppResources.Desoler, AppResources.PasModele, AppResources.OK);
            }
        }
        private async void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as Categories;
            if (currentSelection == null) return;
            Preferences.Set("nomcategory", currentSelection.name);
            Preferences.Set("idcategory", currentSelection.id);
            await this.Navigation.PushAsync(new PrestaPage(currentSelection.id, currentSelection.name));
            ((CollectionView)sender).SelectedItem = null;
        }

    }
}