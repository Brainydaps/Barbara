using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Barbara.Models;

namespace Barbara.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        string _inputText;
        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }

        public ObservableCollection<MessageItem> Messages { get; } = new();

        protected void AddUser(string text) =>
            Messages.Add(new MessageItem { Author = "You", Text = text });

        protected void AddBot(string who, string text) =>
            Messages.Add(new MessageItem { Author = who, Text = text });
    }
}
