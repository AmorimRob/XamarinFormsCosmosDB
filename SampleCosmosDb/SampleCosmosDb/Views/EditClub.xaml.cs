using SampleCosmosDb.Models;
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
	public partial class EditClub : ContentPage
	{
		public EditClub (Clubs club)
		{
			InitializeComponent ();
            BindingContext = new VIewModels.EditClubViewModel(club);
        }
	}
}