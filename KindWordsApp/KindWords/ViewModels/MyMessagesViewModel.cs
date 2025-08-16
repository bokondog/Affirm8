using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KindWords.Models;
using KindWords.Services;
using System.Collections.ObjectModel;

namespace KindWords.ViewModels
{
    /// <summary>
    /// ViewModel for My Messages screen - shows user's messages with all replies
    /// </summary>
    public partial class MyMessagesViewModel : ObservableObject
    {
        private readonly MessageService _messageService;

        [ObservableProperty]
        private ObservableCollection<Message> myMessages = new();

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool hasMessages = false;

        public MyMessagesViewModel(MessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task InitializeAsync()
        {
            await LoadMyMessagesAsync();
        }

        [RelayCommand]
        public async Task LoadMyMessagesAsync()
        {
            IsLoading = true;
            try
            {
                var messageList = await _messageService.GetMyMessagesAsync();
                MyMessages.Clear();
                foreach (var message in messageList)
                {
                    MyMessages.Add(message);
                }
                HasMessages = MyMessages.Count > 0;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load your messages. Please try again.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task RefreshAsync()
        {
            await LoadMyMessagesAsync();
        }
    }
} 