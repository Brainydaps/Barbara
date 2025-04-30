using Microsoft.Maui.Controls;
using Barbara.ViewModels;

namespace Barbara.Views
{
    public partial class AnalystPage : ContentPage
    {
        public AnalystPage(AnalystViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
