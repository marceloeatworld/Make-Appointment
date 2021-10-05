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

namespace rdvperso.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwipePopupPage : PopupPage
    {
        public SwipePopupPage()
        {
            InitializeComponent();
            PremierPop();
        }

        private  void PremierPop()
        {
            Preferences.Set("popswipe", 1);
        }


    }
}