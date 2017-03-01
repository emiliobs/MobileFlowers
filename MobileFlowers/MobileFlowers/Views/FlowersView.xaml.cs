using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileFlowers.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileFlowers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlowersView : ContentPage
    {
        public FlowersView()
        {
            InitializeComponent();

            var mainViewModel = MainViewModel.GetInstance();

            Appearing += (object sender, EventArgs e) =>
            {
                mainViewModel.RefreshCommand.Execute(this);
            };

        }
    }
}
