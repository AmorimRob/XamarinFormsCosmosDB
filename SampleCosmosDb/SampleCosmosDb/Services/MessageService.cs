using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleCosmosDb.Services
{
    public class MessageService 
    {
        public async Task DisplayAlert(string message)
        {
            await App.Current.MainPage.DisplayAlert("ClubsApp", message, "Ok");
        }
    }
}
