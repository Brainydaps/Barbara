using Microsoft.Maui.Controls;
using Barbara.ViewModels;

namespace Barbara.Views
{
    public partial class PsychologistPage : ContentPage
    {
        public PsychologistPage(JournalHistorianViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
