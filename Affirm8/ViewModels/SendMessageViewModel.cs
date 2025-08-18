using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Affirm8.Models;
using Affirm8.Services;
using System.Collections.ObjectModel;

namespace Affirm8.ViewModels
{
    /// <summary>
    /// ViewModel for sending new affirmation messages
    /// </summary>
    public partial class SendMessageViewModel : ObservableObject
    {
        private readonly MessageService _messageService;

        [ObservableProperty]
        private string messageContent = string.Empty;

        [ObservableProperty]
        private string selectedCategory = "Support";

        [ObservableProperty]
        private bool isSending = false;

        public ObservableCollection<string> Categories { get; } = new()
        {
            "Support",
            "Hope", 
            "Celebration",
            "Gratitude"
        };

        public SendMessageViewModel(MessageService messageService)
        {
            _messageService = messageService;
        }

        [RelayCommand]
        public async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageContent))
                return;

            IsSending = true;

            try
            {
                // Send message using current authenticated user
                await _messageService.SendMessageAsync(MessageContent, SelectedCategory);
                
                // Clear the form
                MessageContent = string.Empty;
                SelectedCategory = "Support";

                // Show success feedback (could be a toast notification)
                await Application.Current.MainPage.DisplayAlert("Sent", "Your message has been shared with the community! 💙", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to send message. Please try again.", "OK");
            }
            finally
            {
                IsSending = false;
            }
        }

        [RelayCommand]
        public void ClearMessage()
        {
            MessageContent = string.Empty;
        }

        partial void OnMessageContentChanged(string value)
        {
            // Enable/disable send button based on content
            SendMessageCommand.NotifyCanExecuteChanged();
        }

        private bool CanSendMessage() => !string.IsNullOrWhiteSpace(MessageContent) && !IsSending;
    }
} 