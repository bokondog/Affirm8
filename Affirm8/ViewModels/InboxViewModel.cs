using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Affirm8.Models;
using Affirm8.Services;
using System.Collections.ObjectModel;

namespace Affirm8.ViewModels
{
    /// <summary>
    /// ViewModel for the inbox where users respond to others' messages
    /// </summary>
    public partial class InboxViewModel : ObservableObject
    {
        private readonly MessageService _messageService;
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private ObservableCollection<Message> messages = new();

        [ObservableProperty]
        private Message? selectedMessage;

            [ObservableProperty]
    private string replyContent = string.Empty;
    
    [ObservableProperty]
    private bool isRefreshing = false;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool isSendingReply = false;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private string selectedCategoryFilter = "All";

        public ObservableCollection<string> CategoryFilters { get; } = new()
        {
            "All",
            "Support",
            "Hope",
            "Celebration", 
            "Gratitude"
        };

        public InboxViewModel(MessageService messageService, AuthenticationService authService)
        {
            _messageService = messageService;
            _authService = authService;
            
            // Listen for authentication changes
            _authService.CurrentUserChanged += OnCurrentUserChanged;
        }

        private async void OnCurrentUserChanged(User? user)
        {
            if (user == null)
            {
                // Clear messages when user logs out
                Messages.Clear();
                SelectedMessage = null;
                ReplyContent = string.Empty;
            }
            else
            {
                // Load messages when user logs in
                await LoadMessagesAsync();
            }
        }

        public async Task InitializeAsync()
        {
            await LoadMessagesAsync();
        }

            [RelayCommand]
    public async Task RefreshAsync()
    {
        IsRefreshing = true;
        try
        {
            await LoadMessagesAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    public async Task LoadMessagesAsync()
        {
            IsLoading = true;
            try
            {
                // Get inbox messages (random 5 that user hasn't replied to)
                var messageList = await _messageService.GetInboxMessagesAsync(5);
                Messages.Clear();
                foreach (var message in messageList)
                {
                    Messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load messages. Please try again.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task SearchMessagesAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText) && SelectedCategoryFilter == "All")
            {
                await LoadMessagesAsync();
                return;
            }

            IsLoading = true;
            try
            {
                var categoryFilter = SelectedCategoryFilter == "All" ? null : SelectedCategoryFilter;
                var messageList = await _messageService.SearchInboxMessagesAsync(SearchText, categoryFilter);
                Messages.Clear();
                foreach (var message in messageList)
                {
                    Messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to search messages. Please try again.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public void SelectMessage(Message message)
        {
            SelectedMessage = message;
            ReplyContent = string.Empty;
        }

        [RelayCommand]
        public async Task SendReplyAsync()
        {
            if (SelectedMessage == null || string.IsNullOrWhiteSpace(ReplyContent))
                return;

            IsSendingReply = true;
            try
            {
                await _messageService.SendReplyAsync(SelectedMessage.Id, ReplyContent);
                
                // Update the selected message's reply count
                SelectedMessage.ReplyCount++;
                SelectedMessage.HasBeenRepliedTo = true;

                // Remove this message from inbox since user has now replied
                Messages.Remove(SelectedMessage);

                ReplyContent = string.Empty;
                SelectedMessage = null;

                await Application.Current.MainPage.DisplayAlert("Sent", "Your kind words have been delivered! 💫", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to send reply. Please try again.", "OK");
            }
            finally
            {
                IsSendingReply = false;
            }
        }

        [RelayCommand]
        public void CancelReply()
        {
            SelectedMessage = null;
            ReplyContent = string.Empty;
        }

        partial void OnSearchTextChanged(string value)
        {
            // Auto-search after a short delay (debouncing could be added here)
            if (string.IsNullOrEmpty(value))
            {
                Task.Run(async () => await LoadMessagesAsync());
            }
        }

        partial void OnSelectedCategoryFilterChanged(string value)
        {
            Task.Run(async () => await SearchMessagesAsync());
        }
    }
} 