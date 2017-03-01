using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using MobileFlowers.Annotations;
using MobileFlowers.Models;
using MobileFlowers.Service;
using Xamarin.Forms.PlatformConfiguration;

namespace MobileFlowers.ViewModel
{
    public class EditFlowerViewModel : Flowers, INotifyPropertyChanged
    {
        #region Attributes

        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

        private bool isRunning;
        private bool isEnabled;



        #endregion


        #region Properties

        public bool IsRunning
        {
            get { return isRunning; }

            set
            {
                if (isRunning != value)
                {
                    isRunning = value;

                    OnPropertyChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }

            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;

                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor
        public EditFlowerViewModel(Flowers flowers)
        {
            Description = flowers.Description;
            Price = flowers.Price;
            FlowerId = flowers.FlowerId;


            //services:
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();


            //los boton se inician: en verdaderos
            IsEnabled = true;
        }
        #endregion


        #region Commands
        public object SaveFlowerCommand
        {
            get { return  new RelayCommand(SaveFlower);}
        }

        private async void SaveFlower()
        {

            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.showMessage("Error", "You must enter a description.");
                return;
            }



            if (Price <= 0)
            {
                await dialogService.showMessage("Error", "You must enter a number greater than zero in Price.");
                return;
            }






            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Put("http://flowersback2.azurewebsites.net","/api","/Flowers", this);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await dialogService.showMessage("Error", response.Message);

                return;
            }


            await navigationService.Back();

        }

        public object DeleteFlowerCommand
        {
            get { return   new RelayCommand(DeleteFlower);}
        }



        private async void DeleteFlower()
        {
            var answer = await dialogService.ShowConfirm("Confirm","Are yoou sure to delete this?");

            if (!answer)
            {
                return;
            }


            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Delete("http://flowersback2.azurewebsites.net", "/api", "/Flowers", this);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await dialogService.showMessage("Error", response.Message);

                return;
            }


            await navigationService.Back();

        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        #endregion
    }
}
