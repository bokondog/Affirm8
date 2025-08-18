using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Affirm8
{
    public partial class ContactsPageViewModel : ObservableObject
    {
        public ObservableCollection<Contact> ContactsList { get; set; }

        public ObservableCollection<Contact> Contacts => ContactsList;

        [ObservableProperty]
        private string searchText = "";

        [ObservableProperty]
        private Contact? selectedContact;

        public ContactsPageViewModel()
        {
            ContactsList = new ObservableCollection<Contact>();
            LoadData();
        }

        [RelayCommand]
        private async Task Add()
        {
            // Add new contact logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Call(Contact contact)
        {
            // Call contact logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Message(Contact contact)
        {
            // Message contact logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task Search(string query)
        {
            // Search logic
            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task ContactSelectionChanged(Contact contact)
        {
            SelectedContact = contact;
            // Handle contact selection
            await Task.CompletedTask;
        }

        private void LoadData()
        {
            string jsonData = @"
        {
            ""contactsPageList"": [
                { ""name"": ""Aaron Thorsson"", ""backgroundColor"": ""#837bff"" },
                { ""name"": ""Alexander Allen"", ""backgroundColor"": ""#678cc8"" },
                { ""name"": ""Alta Sims"", ""backgroundColor"": ""#29aaa0"" },
                { ""name"": ""Ann Keller"", ""backgroundColor"": ""#db7faa"" },
                { ""name"": ""Arthur Kim"", ""backgroundColor"": ""#dc9737"" },
                { ""name"": ""Bernard Daniels"", ""backgroundColor"": ""#a8d35f"" },
                { ""name"": ""Bernard Rodriquez"", ""backgroundColor"": ""#ec5b76"" },
                { ""name"": ""Bettie Conlon"", ""backgroundColor"": ""#6c88ff"" },
                { ""name"": ""Blake Moore"", ""backgroundColor"": ""#cf62e2"" },
                { ""name"": ""Brandon Todd"", ""backgroundColor"": ""#f57780"" },
                { ""name"": ""Carlos Esteban"", ""backgroundColor"": ""#a8d35f"" },
                { ""name"": ""Charlotte Nash"", ""backgroundColor"": ""#ec5b76"" },
                { ""name"": ""Chase Blair"", ""backgroundColor"": ""#6c88ff"" },
                { ""name"": ""Christina Lloyd"", ""backgroundColor"": ""#cf62e2"" },
                { ""name"": ""Christina Hanson"", ""backgroundColor"": ""#f57780"" }
            ]
        }";

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<ContactsData>(jsonData, options);

            if (data?.ContactsPageList == null)
            {
                return;
            }

            foreach (var contact in data.ContactsPageList)
            {
                ContactsList.Add(contact);
            }
        }
    }

    public class Contact
    {
        public string Name { get; set; } = "";
        public string Initials { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public bool HasPhoneNumber => !string.IsNullOrEmpty(PhoneNumber);
        public bool HasEmail => !string.IsNullOrEmpty(Email);
    }

    public class ContactsData
    {
        public List<Contact>? ContactsPageList { get; set; }
    }

}
