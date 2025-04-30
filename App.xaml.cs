using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace Barbara
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            Debug.WriteLine("[App] Constructor start");
            InitializeComponent();
            Debug.WriteLine("[App] Initialized components");

            MainPage = shell;
            Debug.WriteLine("[App] MainPage set to AppShell");
        }
    }
}