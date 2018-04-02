using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleCosmosDb.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClubList : ContentPage
	{
        private VIewModels.ClubListViewModel _viewModel;
        public ClubList ()
		{
            _viewModel = new VIewModels.ClubListViewModel();

            InitializeComponent ();

            BindingContext = _viewModel;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.GetAll(); 
        }
    }
}