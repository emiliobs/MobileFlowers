using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MobileFlowers.Annotations;
using MobileFlowers.Classes;
using MobileFlowers.Models;
using MobileFlowers.Service;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace MobileFlowers.ViewModel
{
    public class EditFlowerViewModel : Flowers, INotifyPropertyChanged
    {
        #region Attributes

        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;    

        private ImageSource imageSource;
        private MediaFile file;

        private bool isRunning;
        private bool isEnabled;



        #endregion


        #region Properties

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
            LastPurchase = flowers.LastPurchase;
            IsActive = flowers.IsActive;
            Observation = flowers.Observation;
            Image = flowers.Image;

            IsEnabled = true;



            //services:
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();


            //los boton se inician: en verdaderos
            IsEnabled = true;
        }
        #endregion


        #region Commands

       public ICommand TakePictureCommand
        {
            get { return new RelayCommand(TakePicture); }
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

            IsRunning = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    return stream;
                });
            }

            IsRunning = false;

        }

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

            //aqui invoco el metodo de la clase fileHelper para con vertir los bits de la foto a array de string:
            var imageArray = FilesHelper.ReadFully(file.GetStream());

            //limpio memoria:
            file.Dispose();


            var flawer = new Flowers()
            {
                Image = Image,
                LastPurchase = LastPurchase,
                IsActive = IsActive,
                Observation = Observation,
                ImageArray = imageArray,
                Description = Description,
                FlowerId = FlowerId,
                Price = Price,
                
            };

            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Put("http://flowershome.azurewebsites.net", "/api","/Flowers", flawer);

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

            var response = await apiService.Delete("http://flowershome.azurewebsites.net", "/api", "/Flowers", this);

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
