using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure.AI.Projects;
using Azure.Identity;           // for DefaultAzureCredential

namespace Barbara.Services
{
    public static class KernelSetup
    {
        /// <summary>
        /// Initializes the AgentsClient by connecting via DefaultAzureCredential,
        /// then kicks off timed, fire-and-forget retrievals of three pre-created agents.
        /// Returns immediately with an (initially empty) dictionary; agents populate if retrieved within 6s.
        /// </summary>
        public static async Task<(AgentsClient AgentsClient, Dictionary<string, Agent> Agents)> SetupAgentsAsync()
        {
            Debug.WriteLine("[KernelSetup] Starting SetupAgentsAsync...");

            // 1) Read and validate your AI Foundry project connection string
            var connectionString = Environment
                .GetEnvironmentVariable("AZURE_AI_PROJECT_CONNECTION_STRING")
                ?? throw new InvalidOperationException("AZURE_AI_PROJECT_CONNECTION_STRING is not set.");
            Debug.WriteLine($"[KernelSetup] Connection string {(string.IsNullOrEmpty(connectionString) ? "is empty" : "loaded successfully")}");
            Debug.WriteLine(Environment.GetEnvironmentVariable("AZURE_AI_PROJECT_CONNECTION_STRING"));

            // 2) Create AIProjectClient using DefaultAzureCredential
            Debug.WriteLine("[KernelSetup] Creating AIProjectClient...");
            var credential = new DefaultAzureCredential();
            var projectClient = new AIProjectClient(connectionString, credential);
            var agentsClient = projectClient.GetAgentsClient();
            var connectionsClient = projectClient.GetConnectionsClient();
            Debug.WriteLine("[KernelSetup] AIProjectClient, AgentsClient, and ConnectionsClient created.");

            // 3) Prepare the dictionary of agents
            var agents = new Dictionary<string, Agent>(StringComparer.OrdinalIgnoreCase);
            Debug.WriteLine("[KernelSetup] Initialized agents dictionary.");

            // 4) Kick off each agent retrieval in the background with a 6-second timeout
            Debug.WriteLine("[KernelSetup] Initiating timed, fire-and-forget agent retrievals...");

            // AnalystAgent
            _ = Task.Run(async () =>
            {
                try
                {
                    Debug.WriteLine("[KernelSetup] Starting AnalystAgent retrieval task...");
                    var retrievalTask = agentsClient.GetAgentAsync("asst_MV6NyxNopqA8YBQdIaT3gdA5");
                    var winner = await Task.WhenAny(retrievalTask, Task.Delay(TimeSpan.FromSeconds(6)));
                    if (winner == retrievalTask && retrievalTask.Status == TaskStatus.RanToCompletion)
                    {
                        var agent = retrievalTask.Result.Value;
                        agents["AnalystAgent"] = agent;
                        Debug.WriteLine($"[KernelSetup] Retrieved AnalystAgent: {agent.Name} ({agent.Id})");
                    }
                    else
                    {
                        Debug.WriteLine("[KernelSetup] AnalystAgent retrieval timed out or failed.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[KernelSetup] AnalystAgent retrieval exception: {ex.GetBaseException().Message}");
                }
            });

            // JournalHistorianAgent
            _ = Task.Run(async () =>
            {
                try
                {
                    Debug.WriteLine("[KernelSetup] Starting JournalHistorianAgent retrieval task...");
                    var retrievalTask = agentsClient.GetAgentAsync("asst_DPrsHwgtsa2kK8TRwSEtWjfL");
                    var winner = await Task.WhenAny(retrievalTask, Task.Delay(TimeSpan.FromSeconds(6)));
                    if (winner == retrievalTask && retrievalTask.Status == TaskStatus.RanToCompletion)
                    {
                        var agent = retrievalTask.Result.Value;
                        agents["JournalHistorianAgent"] = agent;
                        Debug.WriteLine($"[KernelSetup] Retrieved JournalHistorianAgent: {agent.Name} ({agent.Id})");
                    }
                    else
                    {
                        Debug.WriteLine("[KernelSetup] JournalHistorianAgent retrieval timed out or failed.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[KernelSetup] JournalHistorianAgent retrieval exception: {ex.GetBaseException().Message}");
                }
            });

            // PsychologistAgent
            _ = Task.Run(async () =>
            {
                try
                {
                    Debug.WriteLine("[KernelSetup] Starting PsychologistAgent retrieval task...");
                    var retrievalTask = agentsClient.GetAgentAsync("asst_vNbWvziHbSlRpa5hh5HgGIFv");
                    var winner = await Task.WhenAny(retrievalTask, Task.Delay(TimeSpan.FromSeconds(6)));
                    if (winner == retrievalTask && retrievalTask.Status == TaskStatus.RanToCompletion)
                    {
                        var agent = retrievalTask.Result.Value;
                        agents["PsychologistAgent"] = agent;
                        Debug.WriteLine($"[KernelSetup] Retrieved PsychologistAgent: {agent.Name} ({agent.Id})");
                    }
                    else
                    {
                        Debug.WriteLine("[KernelSetup] PsychologistAgent retrieval timed out or failed.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[KernelSetup] PsychologistAgent retrieval exception: {ex.GetBaseException().Message}");
                }
            });

            Debug.WriteLine("[KernelSetup] SetupAgentsAsync returning immediately (agents load in background).");
            return (agentsClient, agents);
        }
    }
}
