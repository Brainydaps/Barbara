# Barbara

**A Shell-TabBar .NET MAUI app using MVVM**  
**Microsoft AI Skill Fest Hackathon Submission**

---

## 🚀 Overview

We live in times where most people are disconnected from their physical world and people around them, often focusing on a virtual world (instagram, twitter, facebook, tiktok etc) where reality is grossly wapped, creating an illusion in people's mind that every other person is doing better than them. These social media platforms are powered by algorithms whose main purpose is to keep you on them for as long as possible, neglecting your physical and mental health. Now is the time to turn algorithms against algorithms to begin to heal the world. Your mental health is important, you are more than a data point to be analysed , you are important.

Imagine a well-read psychologist who is also a data analyst—with months of your health metrics and a full year of your journal entries at their fingertips. What if that “person” could support you through the ups and downs of life? **Barbara** brings that vision to your devices.

Barbara is a multi-agent, single-turn conversation app built in **.NET MAUI** (Shell + TabBar) with a clean **MVVM** architecture. Under the hood, it leverages **Azure AI Agent Services** (hosted on AI Foundry) via an API-key in the project connection string.

---

## ✨ Why .NET MAUI?

- **Multi-platform**: runs on Android, iOS, Mac, Windows  
- **Seamless Microsoft integration**: first-class support for Azure, MAUI controls, MVVM  
- **One codebase for all devices**: because **everyone deserves Barbara**, no matter what device they own

![Screenshot 2025-04-30 183935](https://github.com/user-attachments/assets/a1d4ae31-1aa6-49a9-bd4b-a759de128751)

---

## 🌟 Why “Barbara”?

![Screenshot 2025-04-30 182410](https://github.com/user-attachments/assets/dc08cf69-5cd5-4d29-bb28-36cdca13e8f4)


Barbara is named after the Deaconess of the Church of Favonius (Genshin Impact). In Mondstadt citizens found her songs to be strange but later she became beloved for her songs and miraculous healing, ultimately life-changing. Our Barbara channels that same spirit:

> **“The sight of Barbara makes all my problems disappear.”**  
> — Citizens of Mondstadt

She embodies a healer radiating positive energy—exactly what this app aims to be for your mental health.

---

## 🏗 Architecture

- **.NET MAUI Shell + TabBar**  
  Three tabs, one per agent:  
  1. **Analyst**  
  2. **JournalHistorian**  
  3. **Psychologist**

- **MVVM Pattern**  
  - **Views**: XAML pages (one per agent)  
  - **ViewModels**: bind user input → BotService → messages list  
  - **Models/Controls**: `MessageItem`, `MessageView`, `AuthorToColorConverter`

- **Azure AI Agents**  
  - **AnalystAgent**: crunches your health & environment metrics  
  - **JournalHistorianAgent**: semantic search over one year of journal entries  
  - **PsychologistAgent**: reference-backed psychological insights

---

## 🤖 Agents & Their Roles

### 1. AnalystAgent  
- **Input data**:  
  - Sleep quality & duration, BPM, steps, kcal  
  - Movement & standing time  
  - Temperature, AQI  
  - Journal “positive/negative events” count  
  - Mood score (–1 to +1)
  - This dataset was gotten from Xiaomi smart band 8 and iPhone Journal app.
- **Output**: Data-driven insights, RFE feature-importance, time-series forecasts based on user query

  ![IMG_3283 PNG](https://github.com/user-attachments/assets/914e2873-43cb-4100-8da4-2f4de21b9ca8)


### 2. JournalHistorianAgent  
- **Data**: Vectorized journal entries (1 year) of user, hybrid keyword + semantic search. The journal.pdf was exported from iPhone Journal app.
- **Output**:  
  - Summaries by date/time-range/theme based on user specifications
  - Emerging patterns & temporal trends  
  - Reflective journal insights
 
    ![IMG_3284 PNG](https://github.com/user-attachments/assets/5ec1880a-d02c-4170-a40f-2244edbfa50e)


### 3. PsychologistAgent  
- **Data**: Vectorized corpus of psychological texts on mood, emotion, depression. Well referenced psychological books on human emotions/mood; royalty free books only. 
- **Output**:  
  - Empathetic validation  
  - Evidence-based coping strategies  
  - Explanations of psychological phenomena

---

## 📁 Project Structure

```text
Barbara/
├─ Barbara.sln
└─ Barbara/                          ← MAUI app project
   ├─ Barbara.csproj
   ├─ MauiProgram.cs
   ├─ App.xaml
   ├─ App.xaml.cs
   ├─ AppShell.xaml
   ├─ AppShell.xaml.cs
   ├─ Services/
   │  ├─ KernelSetup.cs            ← connects to AI Foundry & retrieves agents
   │  └─ BotService.cs             ← routes user input → correct agent
   ├─ Models/
   │  └─ MessageItem.cs            ← chat message data model
   ├─ Converters/
   │  └─ AuthorToColorConverter.cs ← user vs. agent bubble color
   ├─ Controls/
   │  ├─ MessageView.xaml
   │  └─ MessageView.xaml.cs       ← message bubble UI
   ├─ ViewModels/
   │  ├─ BaseViewModel.cs          ← common logic & ObservableCollection<MessageItem>
   │  ├─ AnalystViewModel.cs       ← sends InputText → BotService.SendToAgentAsync
   │  ├─ JournalHistorianViewModel.cs
   │  └─ PsychologistViewModel.cs
   ├─ Views/
   │  ├─ AnalystPage.xaml          ← binds to AnalystViewModel
   │  ├─ AnalystPage.xaml.cs
   │  ├─ JournalHistorianPage.xaml
   │  ├─ JournalHistorianPage.xaml.cs
   │  ├─ PsychologistPage.xaml
   │  └─ PsychologistPage.xaml.cs
   └─ Resources/
      ├─ Fonts/
      └─ Images/
         ├─ analyst_icon.png
         ├─ journal_icon.png
         └─ psychologist_icon.png
```

---

## 🔄 Class Communication

1. **`AppShell`**  
   - Defines the TabBar (Analyst, JournalHistorian, Psychologist)  
   - Each `ShellContent` injects its respective Page & ViewModel via DI  

2. **`MauiProgram`**  
   - Registers services & view-models in the DI container  
   - Calls `UseMauiApp<App>()`, which sets `MainPage = AppShell`

3. **`KernelSetup.SetupAgentsAsync()`**  
   - Reads `AZURE_AI_PROJECT_CONNECTION_STRING`  
   - Builds `AIProjectClient` → `AgentsClient`  
   - Retrieves (or skips) pre-created agents by ID  
   - Returns `(AgentsClient, Dictionary<string,Agent>)`

4. **`BotService`**  
   - On construction: calls `SetupAgentsAsync()` to get clients & agents map  
   - `SendToAgentAsync(userInput)` inspects the **active Shell route** to pick the right `Agent`  
   - Creates/uses a thread, posts the user message, runs the agent, polls for completion, aggregates reply  

5. **ViewModels**  
   - **`BaseViewModel`**: exposes `ObservableCollection<MessageItem> Messages` & `string InputText`  
   - **`AnalystViewModel`**, **`JournalHistorianViewModel`**, **`PsychologistViewModel`**:  
     ```csharp
     AddUser(InputText);
     var reply = await _bot.SendToAgentAsync(InputText);
     AddBot("AgentName", reply);
     ```
   - `Messages` collection binds to a `CollectionView` in each Page, rendered with `MessageView`

---

## 📊 Execution Flow

```text
[User taps Analyst tab]
        ↓
[AnalystPage displays]
        ↓
User types question → presses Send
        ↓
AnalystViewModel.OnSendAsync()
    • AddUser()
    • _bot.SendToAgentAsync(InputText)
        ↓
BotService.SendToAgentAsync():
    • Determine active tab → “AnalystAgent”
    • Post message → start run → poll → collect reply
        ↓
Returns reply string
        ↓
AnalystViewModel receives reply → AddBot("Analyst", reply)
        ↓
CollectionView updates → MessageView bubble appears
```

---

## ⚠️ Problems Faced

1. **Authentication Disabled**  
   - My Microsoft MVP subscription doesn’t support either  
     - **Managed Identity** (throws “user-assigned managed identity not supported for the managed identity environment in this subscription”)  
     - **API Key** (connection hangs indefinitely most likely due to problem 2 below)

2. **Agent Retrieval Format**  
   - `agentsClient.GetAgentAsync(...)` returns types with explicit interfaces (e.g., `IPersistableModel`)  
   - Storing these objects in a `Dictionary<string,Agent>` fails at runtime  
   - May require serialization/deserialization to persist properly, documentation doesnt detail how the retrieved agent is stored in memory for those not using semantic kernel. 

---

Thank you for checking out **Barbara**—your virtual healer powered by data, diaries, and deep psychological insight!  
```
