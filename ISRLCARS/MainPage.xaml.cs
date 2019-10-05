using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISRLCARS.Views;
using Xamarin.Forms;

namespace ISRLCARS
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void NewTicketButton(object sender, System.EventArgs e)
        {

            pass.IsVisible = true;
        }

        async void TicketCheckButton(object sender, System.EventArgs e)
        {
            var navPage = new NavigationPage(new MainPage());
            Application.Current.MainPage = navPage;
            await navPage.PushAsync(new TicketCheck());
        }

        async void TicketCheckByIDButtonAsync(object sender, System.EventArgs e)
        {
            var navPage = new NavigationPage(new MainPage());
            Application.Current.MainPage = navPage;
            await navPage.PushAsync(new TicketCheckByID());
        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            if (!password.Equals("AdminISRL"))
            {
                var navPage = new NavigationPage(new MainPage());
                Application.Current.MainPage = navPage;
                await navPage.PushAsync(new NewTicket());
            }
            else
            {
                await DisplayAlert(null, "סיסמה לא נכונה", "אישור");
            }
            
        }
    }
}
