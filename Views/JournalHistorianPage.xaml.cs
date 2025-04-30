using Microsoft.Maui.Controls;
using Barbara.ViewModels;

namespace Barbara.Views
{
    public partial class JournalHistorianPage : ContentPage
    {
        public JournalHistorianPage(JournalHistorianViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
