using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MobileFlowers.Annotations;
using MobileFlowers.Models;
using MobileFlowers.Service;

namespace MobileFlowers.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region Attributes

        private ApiService apiService;
        private NavigationService navigationService;
        private DialogService dialogService;

        private bool isBusy;
        private bool isBusy2;
        #endregion

        #region Properties

        public ObservableCollection<FlowerItemViewModel> Flowers { get; set; }

        public NewFlowerViewModel NewFlowerViewModel { get; set; }

        public EditFlowerViewModel EditFlower { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;

                OnPropertyChanged();
            }
        }

        public bool IsBusy2
        {
            get { return isBusy2; }
            set
            {
                isBusy2 = value;

                OnPropertyChanged();
            }
        }

        public ICommand AddFlowerCommand
        {
            get { return new RelayCommand(AddFlower); }
        }

        public ICommand RefreshCommand
        {
            get
            {

               return new RelayCommand(RefreshFlowers);

            }
        }

        private void RefreshFlowers()
        {
            IsBusy2 = true;

           LoadFlowers();

            IsBusy2 = false;
        }

        #endregion

        #region Constructors

        public MainViewModel()
        {


            //Singleton:
            instance = this;

            //Services
           apiService = new ApiService();
           navigationService = new NavigationService();
            dialogService = new DialogService();

            //View models
            Flowers = new ObservableCollection<FlowerItemViewModel>();

            //Load Data
            //LoadFlowers();

        }



        #endregion

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }

        #endregion


        #region Commands
                          
        private async void AddFlower()
        {
           await navigationService.Navigate("AddFlowerView");
        }

        #endregion

        #region Methods

        private async void LoadFlowers()
        {
            IsBusy = true;

            //var response = await apiService.Get<Flowers>("http://flowersback2.azurewebsites.net", "/api", "/Flowers");
            var response = await apiService.Get<Flowers>("http://flowershome.azurewebsites.net", "/api", "/Flowers");

            if (!response.IsSuccess)
            {
                await dialogService.ShowConfirm("Error", response.Message);
                return;
            }

            ReloadFlowers((List<Flowers>)response.Result);

            IsBusy = false;


        }

        private void ReloadFlowers(List<Flowers> listFlowers)
        {
           Flowers.Clear();



            foreach (var flower in listFlowers.OrderBy(f=>f.Description))
            {
                Flowers.Add(new FlowerItemViewModel()
                {
                    Description = flower.Description,
                    FlowerId = flower.FlowerId,
                    Price = flower.Price,
                    Image = flower.Image,
                });
            }


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
