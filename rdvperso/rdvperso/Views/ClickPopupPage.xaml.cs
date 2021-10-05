using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using rdvperso.Models;
using Xamarin.Essentials;
using System.Windows.Input;
using MvvmHelpers;
using System.Diagnostics;

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClickPopupPage : PopupPage
    {
        public ICommand AnimationClickedCommand { get; set; }
        public ClickPopupPage()
        {
            AnimationClickedCommand = new Command(() =>
            {
                ClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
            });
            InitializeComponent();
            PremierPop();
            
        }
        private void CloseClick(object sender, EventArgs e)
        {
            ClickedAsync().SafeFireAndForget(ex => Debug.WriteLine(ex));
        }

        private async Task ClickedAsync()
        {
            await PopupNavigation.Instance.PopAsync();
        }

            private void PremierPop()
        {
            Preferences.Set("popswipe", 1);
        }


    }
}