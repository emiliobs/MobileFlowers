using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileFlowers.Models
{
    public class Flowers
    {
        public int FlowerId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string Image { get; set; }

        public string FullImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return $"imgavatar.png";
                }

                return $"http://flowershome.azurewebsites.net{Image.Substring(1)}";
            }

        }

        public override int GetHashCode()
        {
            return FlowerId;
        }
    }
}
