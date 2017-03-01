using System.Threading.Tasks;
using MobileFlowers.ViewModel;
using MobileFlowers.Views;
using  MobileFlowers.Models;
using Xamarin.Forms;

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


                default:
                    break;
             }
         }


        //Mérodo que envia datos por parametro a la vista edutflowerView:
         public async Task EditFlower(Flowers flower)
         {
             var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditFlower = new EditFlowerViewModel(flower);
             await App.Current.MainPage.Navigation.PushAsync(new EditFlowerView());
         }

         public async Task Back()
         {
             await App.Current.MainPage.Navigation.PopAsync();
         }

    }
}
