using System.Threading.Tasks;
using MobileFlowers.ViewModel;
using MobileFlowers.Views;

namespace MobileFlowers.Service
{
     public class NavigationService
    {

         public async Task Navigate(string pageName)
         {
             switch (pageName)
             {
                case "AddFlowerView":

                     var mainViewModel = MainViewModel.GetInstance();
                      mainViewModel.NewFlowerViewModel = new NewFlowerViewModel();

                     await App.Current.MainPage.Navigation.PushAsync(new AddFlowerView());
                    break;
                default:
                    break;
             }
         }

         public async Task Back()
         {
             await App.Current.MainPage.Navigation.PopAsync();
         }

    }
}
