using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using MvvmHelpers;
using SampleCosmosDb.ApplicationServices;
using SampleCosmosDb.Constants;
using SampleCosmosDb.Models;
using SampleCosmosDb.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SampleCosmosDb.VIewModels
{
    public class ClubListViewModel : BaseViewModel
    {
        private readonly DocumentDbService _documentDbService;

        private ObservableRangeCollection<Clubs> _clubs;

        public ObservableRangeCollection<Clubs> Clubs
        {
            get { return _clubs; }
            set { _clubs = value; OnPropertyChanged(); }
        }

        public ICommand AddClubCmd { get; set; }
        public ClubListViewModel()
        {
            _documentDbService = new DocumentDbService("Clubs");

            AddClubCmd = new Command(() => App.Current.MainPage.Navigation.PushAsync(new Views.NewClub()));
        }

        public async Task GetAll()
        {
            try
            {
                Clubs = new ObservableRangeCollection<Clubs>();
                IsBusy = true;
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = true);

                var listClub = await _documentDbService.GetAll<Clubs>();
               
                if (listClub.Count > 0)
                {
                    Clubs.AddRange(listClub);
                }
                IsBusy = false;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            
        }
    }
}
