using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rdvperso.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rdvperso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ProvidersPage), typeof(ProvidersPage));
            //Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            //Routing.RegisterRoute(nameof(PrestaPage), typeof(PrestaPage));
            //Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            //Routing.RegisterRoute(nameof(AvailabilitiesPage), typeof(AvailabilitiesPage));
            //Routing.RegisterRoute(nameof(AppointmentsPage), typeof(AppointmentsPage));
        }
    }
}