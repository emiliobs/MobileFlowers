using System.Threading.Tasks;
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
