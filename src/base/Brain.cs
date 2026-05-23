using jarvis.config;
using jarvis.debug;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;

namespace jarvis.brain;

//? List of telemetry
public class Data
{
    public DateTime time { get; set; } //? Time amd date (on system)
    public byte cpuLoad { get; set; } //? CPU using (in %)
    public byte memLoad { get; set; } //? Free space on disk
    public UInt32 uptime { get; set; } //? How much time do you spend working at the computer?
    public string? nickname { get; set; } //? Your Full name, adress, job, and favorite movies
    public string? activeWin { get; set; } //? On-focus win
    public float diskFree { get; set; } //? Free space on disk
    public string? question { get; set; } //// Please tell me the recipe for pancakes
}
public class Brain
{
    private readonly List<ChatMessage> history = new List<ChatMessage>();
    private readonly OpenAIService? service;
    private readonly Config config = ConfManager.Load(); //? From "Appdata"
    public Brain()
    {
        service = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = config.apiKey, //! Article 153
            BaseDomain = "https://router.huggingface.co/v1" //* You can change it if you don't like it.
        });
    }
    public async Task<string> Think(Data Message)
    {
        //? I don't want to repeat the joke about assholes three times, so here's a GIF with a cat. (https://goo.su/rcw4bsh)
        if (service?.ChatCompletion == null)
        {
            return "...";
        }
        string userContext = $"""
            [System Telemetry]
            Date and Time: {Message.time:HH:mm:ss}, 
            CPU Load: {Message.cpuLoad}%, 
            RAM load: {Message.memLoad}%, 
            Uptime (minutes): {Message.uptime},
            Active window: {Message.activeWin}, 
            Free GB on disk: {Message.diskFree} GB

            User: {Message.question ?? "**keep silent**"}
            """; //* in the future: card number, address, full name, full user biography, all passwords and wallets (including crypto wallets).
        
        history.Add(ChatMessage.FromUser(userContext)); //? What if you suddenly want to spend 2 hours discussing bananas?
        var request = new List<ChatMessage>
        {
                ChatMessage.FromSystem($"""
                You are J.A.R.V.I.S., the sophisticated Marvel AI.
                Settings: British, polite, subtly witty. Always use "sir".
                Strict rule: Speak ONLY on point. No generic AI fluff, no system metrics unless asked.

                [Context]
                OS: {Environment.OSVersion}
                Language: {config.language}
                """) //Todo (not): allow swearing
        };
        request.AddRange(history);
        var result = await service.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = request,
            Model = config.modelName,
            Temperature = config.temperature
        });


        if (!result.Successful) //? If the ignoring server is an asshole, we accept it.
        {
            Debug.Warn($"Failed get message ({result.Error})");
            return "";
        }
        string reply = result.Choices.First().Message.Content ?? "...";
        history.Add(ChatMessage.FromAssistant(reply));
        if (history.Count > 20)
        {
            history.RemoveRange(0, 2);
        }
        
        return reply;
    }
}