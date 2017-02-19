using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileFlowers.Models;
using MobileFlowers.Service;

namespace MobileFlowers.ViewModel
{
    public class MainViewModel
    {

        #region Attributes

        private ApiService apiService;

        #endregion

        #region Properties

        public ObservableCollection<FlowerItemViewModel> Flowers { get; set; }

        #endregion

        #region Constructors

        public MainViewModel()
        {
           apiService = new ApiService();

            Flowers = new ObservableCollection<FlowerItemViewModel>();

            LoadFlowers();

        }



        #endregion


        #region Commands

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
