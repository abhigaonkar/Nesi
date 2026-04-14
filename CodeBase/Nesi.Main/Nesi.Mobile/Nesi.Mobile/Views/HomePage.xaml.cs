using Nesi.Mobile.ViewModels;

using Xamarin.Forms;

namespace Nesi.Mobile.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage(string text)
        {

            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = new HomePageViewModel();
            welcome.Text = text;
  
            
            
        }


           
        }
    }
