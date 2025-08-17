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
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private ObservableCollection<Message> myMessages = new();

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool hasMessages = false;

        public MyMessagesViewModel(MessageService messageService, AuthenticationService authService)
        {
            _messageService = messageService;
            _authService = authService;
            
            // Listen for authentication changes
            _authService.CurrentUserChanged += OnCurrentUserChanged;
        }

        private void OnCurrentUserChanged(User? user)
        {
            // Clear messages when user logs out
            if (user == null)
            {
                MyMessages.Clear();
                HasMessages = false;
            }
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

        [RelayCommand]
        public async Task ToggleLikeReplyAsync(Reply reply)
        {
            try
            {
                bool success;
                if (reply.IsLikedByMessageOwner)
                {
                    // Unlike the reply
                    success = await _messageService.UnlikeReplyAsync(reply.Id);
                    if (success)
                    {
                        reply.IsLikedByMessageOwner = false;
                        reply.LikeCount = Math.Max(0, reply.LikeCount - 1);
                    }
                }
                else
                {
                    // Like the reply
                    success = await _messageService.LikeReplyAsync(reply.Id);
                    if (success)
                    {
                        reply.IsLikedByMessageOwner = true;
                        reply.LikeCount++;
                    }
                }

                if (!success)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update like. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling like for reply {reply.Id}: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong. Please try again.", "OK");
            }
        }
    }
} 