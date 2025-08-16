using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Affirm8
{
    public partial class RecentChatViewModel : ObservableObject
    {
        public ObservableCollection<ChatMessage> Messages { get; set; }

        public ObservableCollection<ChatMessage> RecentChatMessages => Messages;

        [ObservableProperty]
        private string searchText = "";

        [ObservableProperty]
        private ChatMessage? selectedMessage;

        public RecentChatViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            LoadMessages();
        }

        [RelayCommand]
        private async Task Search(string query)
        {
            // Search logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Archive(ChatMessage message)
        {
            // Archive logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Delete(ChatMessage message)
        {
            // Delete logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task More(ChatMessage message)
        {
            // More options logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task MessageSelectionChanged(ChatMessage message)
        {
            SelectedMessage = message;
            // Handle message selection
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task NewMessage()
        {
            // New message logic
            await Task.CompletedTask;
        }

        private void LoadMessages()
        {
            // Sample chat messages
            Messages.Add(new ChatMessage
            {
                SenderName = "John Doe",
                LastMessage = "Hey, how are you?",
                Timestamp = DateTime.Now.AddMinutes(-30),
                UnreadCount = 2
            });

            Messages.Add(new ChatMessage
            {
                SenderName = "Jane Smith",
                LastMessage = "Thanks for the help!",
                Timestamp = DateTime.Now.AddHours(-2),
                UnreadCount = 0
            });
        }
    }

    public class ChatMessage
    {
        public string SenderName { get; set; } = "";
        public string SenderInitials { get; set; } = "";
        public string Message { get; set; } = "";
        public string LastMessage { get; set; } = "";
        public string NotificationType { get; set; } = "";
        public string MessageTypeIcon { get; set; } = "💬";
        public bool HasUnreadMessages { get; set; } = false;
        public bool IsOnline { get; set; } = true;
        public int UnreadCount { get; set; } = 0;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Time => Timestamp.ToString("HH:mm");
    }
}
