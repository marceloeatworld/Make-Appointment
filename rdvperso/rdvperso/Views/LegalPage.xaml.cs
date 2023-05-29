using System;
using System.Collections.Generic;
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
    public partial class LegalPage : ContentPage
    {
        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public LegalPage()
        {
            InitializeComponent();
            VersionTracking.Track();
            // Current app version (1.0.0)
        }
       
    }
}