using Microsoft.Azure.Documents.Client;
using MvvmHelpers;
using SampleCosmosDb.Constants;
using SampleCosmosDb.Models;
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
        private DocumentClient client;
        private Uri collectionLink = UriFactory.CreateDocumentCollectionUri(@"SampleCosmos", @"Clubs");

        private Club _newClub;
        public Club NewClub
        {
            get { return _newClub; }
            set { _newClub = value; }
        }

        public ICommand SaveCmd{ get; set; }

        public NewClubViewModel()
        {
            NewClub = new Club();
            client = new DocumentClient(new System.Uri(DocumentDbConstants.Url), DocumentDbConstants.ReadWritePrimaryKey);

            SaveCmd = new Command(async () => await InsertItemAsync(NewClub));
        }

        public async Task InsertItemAsync(Club club)
        {
            try
            {
                club.Id = Guid.NewGuid().ToString();

                var result = await client.CreateDocumentAsync(collectionLink, club);

                if(result.StatusCode == System.Net.HttpStatusCode.OK)
                    await App.Current.MainPage.Navigation.PushAsync(new Views.ClubList());
                
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }
    }
}
