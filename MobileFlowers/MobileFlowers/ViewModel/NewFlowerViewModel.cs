using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MobileFlowers.Annotations;
using MobileFlowers.Service;
using MobileFlowers.Models;

namespace MobileFlowers.ViewModel
{
    public class NewFlowerViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private string description;
        private decimal price;
        private bool isBusy;
        private bool isEnabled;

        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

        #endregion

        #region Properties

        public string Description
        {

            set
            {
                if (description != value)
                {
                    description = value;

                    OnPropertyChanged();

                }
            }
            get { return description; }
        }

        public decimal Price
        {

            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged();
                }
            }

            get { return price; }
        }

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



        #endregion

        #region Constructor

        public NewFlowerViewModel()
        {


            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();

            IsEnabled = true;
        }
        #endregion

        #region Commands

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

            };



            var response = await apiService.Post("http://flowersback2.azurewebsites.net", "/api", "/Flowers", flower);

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
