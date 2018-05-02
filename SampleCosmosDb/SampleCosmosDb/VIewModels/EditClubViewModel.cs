using MvvmHelpers;
using SampleCosmosDb.ApplicationServices;
using SampleCosmosDb.Models;
using SampleCosmosDb.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SampleCosmosDb.VIewModels
{
    public class EditClubViewModel : BaseViewModel
    {
        private readonly DocumentDbService _documentDbService;
        private readonly MessageService _messageService;
        public Clubs Club { get; set; }
        public ICommand DeleteCmd { get; set; }
        public ICommand UpdateCmd { get; set; }

        public EditClubViewModel(Clubs club)
        {
            Club = club;

            _documentDbService = new DocumentDbService("Clubs");
            _messageService = new MessageService();

            DeleteCmd = new Command(Delete);
            UpdateCmd = new Command(Update);
        }

        public async void Delete()
        {
            var result = await _documentDbService.Delete(Club.Id);
            if (result == System.Net.HttpStatusCode.NoContent)
            {
                await App.Current.MainPage.Navigation.PushAsync(new Views.ClubList());
                await _messageService.DisplayAlert("Success! Club was deleted.");
            }
            else
                await _messageService.DisplayAlert("Sorry! Can't delete the club.");
        }
        public async void Update()
        {
            var result = await _documentDbService.Update<Clubs>(Club);
            if(result != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new Views.ClubList());
                await _messageService.DisplayAlert("Success! Club was updated.");
            }
            else
                await _messageService.DisplayAlert("Sorry! Can't update the club.");
        }
    }
}
