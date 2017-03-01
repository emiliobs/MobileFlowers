using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MobileFlowers.Models;
using MobileFlowers.Service;

namespace MobileFlowers.ViewModel
{
    public class FlowerItemViewModel : Flowers
    {

        #region attributes

        private NavigationService navigationService;

        #endregion


        #region Command

        public ICommand EditFlowerCommand
        {
            get
            {
                return new RelayCommand(EditFlower);

            }
        }

        private async void EditFlower()
        {
            await navigationService.EditFlower(this);
        }

        #endregion

        #region Contructor

        public FlowerItemViewModel()
        {
            navigationService = new NavigationService();
        }

        #endregion

    }
}
