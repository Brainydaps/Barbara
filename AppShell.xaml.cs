using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace Barbara
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Debug.WriteLine("[AppShell] Constructor start");
            InitializeComponent();
            Debug.WriteLine("[AppShell] Initialized components");
        }
    }
}