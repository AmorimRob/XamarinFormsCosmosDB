using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using MvvmHelpers;
using SampleCosmosDb.Constants;
using SampleCosmosDb.Models;
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
        private DocumentClient client;
        private Uri collectionLink = UriFactory.CreateDocumentCollectionUri(@"SampleCosmos", @"Clubs");

        private ObservableRangeCollection<Club> _clubs;

        public ObservableRangeCollection<Club> Clubs
        {
            get { return _clubs; }
            set { _clubs = value; OnPropertyChanged(); }
        }

        public ICommand AddClubCmd { get; set; }
        public ClubListViewModel()
        {
            client = new DocumentClient(new System.Uri(DocumentDbConstants.Url), DocumentDbConstants.ReadOnlyPrimaryKey);

            Clubs = new ObservableRangeCollection<Club>();

            AddClubCmd = new Command(() => App.Current.MainPage.Navigation.PushAsync(new Views.NewClub()));
        }

        public async Task GetAll()
        {
            try
            {
                var query = client.CreateDocumentQuery<Club>(collectionLink, new FeedOptions { MaxItemCount = -1 })
                      .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    Clubs.AddRange(await query.ExecuteNextAsync<Club>());
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            
        }

    }
}
