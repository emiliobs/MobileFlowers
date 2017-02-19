using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileFlowers.Service
{
    public class DialogService
    {

        public async Task showMessage(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "Accept.");

        }

        public async Task<bool> ShowConfirm(string title, string mesage)
        {
            return await App.Current.MainPage.DisplayAlert(title, mesage, "Yes", "NO");
        }

    }
}
