using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileFlowers.Models;

namespace MobileFlowers.ViewModel
{
    public class EditFlowerViewModel : Flowers
    {

        public EditFlowerViewModel(Flowers flowers)
        {
            Description = flowers.Description;
            Price = flowers.Price;
            FlowerId = flowers.FlowerId;
        }

    }
}
