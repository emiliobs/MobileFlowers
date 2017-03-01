using System.Threading.Tasks;
using MobileFlowers.ViewModel;
using MobileFlowers.Views;

namespace MobileFlowers.Service
{
     public class NavigationService
    {

         public async Task Navigate(string pageName)
         {

            var mainViewModel = MainViewModel.GetInstance();

            switch (pageName)
             {
                case "AddFlowerView":

                      mainViewModel.NewFlowerViewModel = new NewFlowerViewModel();

                     await App.Current.MainPage.Navigation.PushAsync(new AddFlowerView());
                    break;

                case "EditFlowerView":

                    mainViewModel.EditFlower = new EditFlowerViewModel();

                    await App.Current.MainPage.Navigation.PushAsync(new EditFlowerView());
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
