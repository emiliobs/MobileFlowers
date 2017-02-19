using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MobileFlowers.Models;
using MobileFlowers.Service;

namespace MobileFlowers.ViewModel
{
    public class MainViewModel
    {

        #region Attributes

        private ApiService apiService;
        private NavigationService navigationService;

        #endregion

        #region Properties

        public ObservableCollection<FlowerItemViewModel> Flowers { get; set; }



        #endregion

        #region Constructors

        public MainViewModel()
        {
            //Services
           apiService = new ApiService();
           navigationService = new NavigationService();

            //View models
            Flowers = new ObservableCollection<FlowerItemViewModel>();

            //Load Data
            LoadFlowers();

        }



        #endregion


        #region Commands

        public ICommand AddFlowerCommand
        {
            get { return new RelayCommand(AddFlower); }
        }

        private async void AddFlower()
        {
           await navigationService.Navigate("AddFlowerView");
        }

        #endregion

        #region Methods

        private async void LoadFlowers()
        {
            var listFlowers = await apiService.Get<Flowers>("http://flowerback.azurewebsites.net", "/api", "/Flowers");

            ReloadFlowers(listFlowers);

        }

        private void ReloadFlowers(List<Flowers> listFlowers)
        {
           Flowers.Clear();

            foreach (var flower in listFlowers)
            {
                Flowers.Add(new FlowerItemViewModel()
                {
                    Description = flower.Description,
                    FlowerId = flower.FlowerId,
                    Price = flower.Price,
                });
            }
        }

        #endregion


    }
}
