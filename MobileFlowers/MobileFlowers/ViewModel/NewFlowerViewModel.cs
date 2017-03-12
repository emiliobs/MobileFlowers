using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MobileFlowers.Annotations;
using MobileFlowers.Service;
using MobileFlowers.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace MobileFlowers.ViewModel
{
    public class NewFlowerViewModel : Flowers, INotifyPropertyChanged
    {
        #region Attributes
       
        private bool isBusy;
        private bool isEnabled;

        private ImageSource imageSource;
        private MediaFile file;

        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

        #endregion

        #region Properties

        

        public bool IsBusy
        {
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged();
                }
            }
            get { return isBusy; }

        }

        public bool IsEnabled
        {

            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged();
                }
            }

            get { return isEnabled; }

        }

        public ImageSource ImageSource
        {
            get { return imageSource; }

            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    OnPropertyChanged();
                }
            }
        }
       
        #endregion

        #region Constructor

        public NewFlowerViewModel()
        {


            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();

            //Inicializo el campo fecha para evitar error por estar nulo:
            LastPurchase = DateTime.Now;

            IsEnabled = true;
        }
        #endregion

        #region Commands

        public ICommand TakePictureCamera
        {
            get{ return   new RelayCommand(TakePicture);}
        }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await dialogService.showMessage("No Camera", ":( No camera available.)}");
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = PhotoSize.Small,
            });

            IsBusy = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    return stream;
                });
            }

            isBusy = false;

        }


        public ICommand NewFlowerCommand
        {
            get { return new RelayCommand(NewFlower); }
        }

       


        private async void NewFlower()
        {

            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowConfirm("Error", "YOu must enter a description.");
                return;
            }

            if (Price <= 0)
            {
                await dialogService.ShowConfirm("Error", "You must enter a number greater than zero in Price.");
                return;
            }


            IsBusy = true;
            IsEnabled = false;


            var flower = new  Flowers()
            {
                Description = Description,
                Price = Price,
                IsActive = IsActive,
                LastPurchase = LastPurchase,
                Observation = Observation,

            };



            var response = await apiService.Post("http://flowershome.azurewebsites.net", "/api", "/Flowers", flower);

            IsBusy = false;
            IsEnabled = true;

            //aqui pregunto si no fue ingresado el objeto nuevo

            if (!response.IsSuccess)
            {
                await dialogService.showMessage("Error", response.Message);
                return;
            }

            //aqui pregunto si fue ingresado lo envio a la página princupal:
            await navigationService.Back();




        }

        #endregion

        #region Event
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
