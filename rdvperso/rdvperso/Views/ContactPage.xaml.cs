using Plugin.Messaging;
using rdvperso.Resx;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace rdvperso.Views
{
    public partial class ContactPage : ContentPage
    {
        public ICommand AnimationClickedCommand { get; set; }

        public ContactPage()
        {
            AnimationClickedCommand = new Command(() =>
            {
                BtnCallval_Clicked();
            });
            BindingContext = this;
            InitializeComponent();
        }

        private void BtnCall_Clicked(object sender, EventArgs e)
        {
            BtnCallval_Clicked();
        }
        private void BtnCallval_Clicked()
        {



            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall("+330517811087");

        }
    }
}