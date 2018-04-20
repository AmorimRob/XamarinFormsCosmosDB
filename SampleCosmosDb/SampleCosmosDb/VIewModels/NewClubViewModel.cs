using Microsoft.Azure.Documents.Client;
using MvvmHelpers;
using SampleCosmosDb.Constants;
using SampleCosmosDb.Models;
using SampleCosmosDb.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SampleCosmosDb.VIewModels
{
    public class NewClubViewModel : BaseViewModel
    {
        private readonly DocumentDbService _documentDbService;

        private Clubs _newClub;
        public Clubs NewClub
        {
            get { return _newClub; }
            set { _newClub = value; }
        }

        public ICommand SaveCmd{ get; set; }

        public NewClubViewModel()
        {
            NewClub = new Clubs();
            _documentDbService = new DocumentDbService("Clubs");

            SaveCmd = new Command(async () => await InsertItemAsync(NewClub));
        }

        public async Task InsertItemAsync(Clubs club)
        {
            try
            {
                club.Id = Guid.NewGuid().ToString();
                
                await _documentDbService.Create(club);

                await App.Current.MainPage.Navigation.PushAsync(new Views.ClubList());
                
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }

    }
}
