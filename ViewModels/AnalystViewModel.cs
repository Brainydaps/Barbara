using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Barbara.Services;

namespace Barbara.ViewModels
{
    public partial class AnalystViewModel : BaseViewModel
    {
        private readonly BotService _bot;
        public IAsyncRelayCommand SendCommand { get; }

        public AnalystViewModel(BotService bot)
        {
            _bot = bot;
            SendCommand = new AsyncRelayCommand(OnSendAsync);
        }

        private async Task OnSendAsync()
        {
            if (string.IsNullOrWhiteSpace(InputText)) return;
            var user = InputText;
            AddUser(user);
            InputText = string.Empty;
            var reply = await _bot.SendToAgentAsync("AnalystAgent", user);
            AddBot("Analyst", reply);
        }
    }
}