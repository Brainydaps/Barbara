using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Azure.AI.Projects;


namespace Barbara.Services
{
    public class BotService
    {
        private readonly AgentsClient _agentsClient;
        private readonly Dictionary<string, Agent> _agents;
        private readonly Dictionary<string, string> _threadIds = new();

        public BotService()
        {
            // Initialize Azure AI agents
            var (client, agents) = KernelSetup.SetupAgentsAsync().GetAwaiter().GetResult();
            _agentsClient = client;
            _agents = agents;
        }

        public async Task<string> SendToAgentAsync(string agentName, string userInput)
        {
            // Determine active tab route instead of using passed agentName
            var currentRoute = Shell.Current.CurrentState.Location.OriginalString.Trim('/');
            string effectiveAgentName = currentRoute switch
            {
                "Analyst" => "AnalystAgent",
                "JournalHistorian" => "JournalHistorianAgent",
                "Psychologist" => "PsychologistAgent",
                _ => throw new InvalidOperationException($"Unknown active tab route '{currentRoute}'")
            };

            if (!_agents.TryGetValue(effectiveAgentName, out var agent))
                throw new ArgumentException($"Agent '{effectiveAgentName}' is not registered.");

            // Ensure a conversation thread exists for this agent
            if (!_threadIds.TryGetValue(effectiveAgentName, out var threadId))
            {
                threadId = (await _agentsClient.CreateThreadAsync()).Value.Id;
                _threadIds[effectiveAgentName] = threadId;
            }

            // Post the user's message
            await _agentsClient.CreateMessageAsync(
                threadId,
                MessageRole.User,
                userInput
            );

            // Start the agent run
            var runResponse = await _agentsClient.CreateRunAsync(threadId, agent.Id);
            ThreadRun run = runResponse.Value;

            // Poll until the run completes
            do
            {
                await Task.Delay(500);
                run = (await _agentsClient.GetRunAsync(threadId, run.Id)).Value;
            }
            while (run.Status == RunStatus.Queued || run.Status == RunStatus.InProgress);

            // Retrieve messages for this thread in ascending order
            PageableList<ThreadMessage> messages = await _agentsClient.GetMessagesAsync(
                threadId: threadId,
                order: ListSortOrder.Ascending
            );

            // Aggregate only assistant (agent) responses
            var result = new StringBuilder();
            foreach (var msg in messages)
            {
                if (msg.Role == MessageRole.Agent)
                {
                    foreach (var content in msg.ContentItems)
                    {
                        if (content is MessageTextContent text)
                        {
                            result.Append(text.Text);
                        }
                    }
                }
            }

            return result.ToString().Trim();
        }
    }
}