using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;
using ChitChatApp.Share.ViewModels.Items;

namespace ChitChatApp.Share.ViewModels.Main;

public partial class ContactListViewModel : BaseViewModel
{
    [ObservableProperty]
    private ContactListItemViewModel? selectedContact;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isRefreshing;

    public ObservableCollection<ContactListItemViewModel> Contacts { get; } = new();
    public ObservableCollection<ContactListItemViewModel> FilteredContacts { get; } = new();

    public event EventHandler<ContactListItemViewModel>? ContactSelected;
    public event EventHandler<ContactListItemViewModel>? ContactBlocked;
    public event EventHandler<ContactListItemViewModel>? ContactRemoved;
    public event EventHandler? AddContactRequested;

    public ContactListViewModel()
    {
        Title = "Contacts";
    }

    [RelayCommand]
    private void SelectContact(ContactListItemViewModel contact)
    {
        SelectedContact = contact;
        ContactSelected?.Invoke(this, contact);
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await ExecuteAsync(async () =>
        {
            IsRefreshing = true;
            await LoadContactsAsync();
            IsRefreshing = false;
        }, showBusy: false);
    }

    [RelayCommand]
    private void AddContact()
    {
        AddContactRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private async Task BlockContactAsync(ContactListItemViewModel contact)
    {
        await ExecuteAsync(async () =>
        {
            // This would typically call a service to block the contact
            await Task.Delay(500);
            
            ContactBlocked?.Invoke(this, contact);
            Contacts.Remove(contact);
            FilterContacts();
        });
    }

    [RelayCommand]
    private async Task RemoveContactAsync(ContactListItemViewModel contact)
    {
        await ExecuteAsync(async () =>
        {
            // This would typically call a service to remove the contact
            await Task.Delay(500);
            
            ContactRemoved?.Invoke(this, contact);
            Contacts.Remove(contact);
            FilterContacts();
        });
    }

    public async Task LoadContactsAsync()
    {
        await ExecuteAsync(async () =>
        {
            // This would typically load contacts from a service
            await Task.Delay(1000);
            
            // Clear existing contacts
            Contacts.Clear();
            
            // Add sample contacts (replace with actual service call)
            // var contacts = await contactService.GetContactsAsync();
            // foreach (var contact in contacts)
            // {
            //     Contacts.Add(new ContactListItemViewModel(contact));
            // }
            
            FilterContacts();
        });
    }

    private void FilterContacts()
    {
        FilteredContacts.Clear();
        
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? Contacts
            : Contacts.Where(c => c.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                 c.UserName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var contact in filtered)
        {
            FilteredContacts.Add(contact);
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterContacts();
    }

    public void AddContact(ContactListItemViewModel contact)
    {
        Contacts.Add(contact);
        FilterContacts();
    }

    public void UpdateContactStatus(Guid userId, bool isOnline)
    {
        var contact = Contacts.FirstOrDefault(c => c.UserId == userId);
        if (contact != null)
        {
            contact.IsOnline = isOnline;
        }
    }
}

