using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Barbara.Services;

namespace Barbara.ViewModels
{
    public partial class PsychologistViewModel : BaseViewModel
    {
        private readonly BotService _bot;
        public IAsyncRelayCommand SendCommand { get; }

        public PsychologistViewModel(BotService bot)
        {
            _bot = bot;
            SendCommand = new AsyncRelayCommand(OnSendAsync);
            
        }

        private async Task OnSendAsync()
        {
            if (string.IsNullOrWhiteSpace(InputText)) return;
            var user = InputText;
            AddUser(user);
            InputText = "";
            var reply = await _bot.SendToAgentAsync("PsychologistAgent", user);
            AddBot("Psychologist", reply);
        }
    }
}
