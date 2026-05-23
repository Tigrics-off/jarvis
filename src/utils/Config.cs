//! Displaying a token is punishable under Article 153 of the Criminal Code of the Russian Federation.
namespace jarvis.config;

using System.Text.Json;
using jarvis.debug;

public class Config
{
    public string apiKey { get; set; } = string.Empty; //? Api token (let's not forget about the prison term)
    public string modelName { get; set; } = "google/gemma-4-31B-it:novita"; //? Model for thinking (llama don`t take, she`s stormy on high temperature)
    public float temperature { get; set; } = 1.5f; //? Creativity level (You don't need much, the cuckoo will go)
    public float topP { get; set; } = 0.9f; //? ceiling on adequacy (好吧，你他妈的为什么要翻译这个？)
    public float presencePenalty { get; set; } = 0.8f; //? Penalty for once themes (To avoid discussing bananas for 2 hours)
    public string language { get; set; } = "ru"; //? Language for response (take whatever you want)
    public string actor { get; set; } = "br"; //* br - Viacheslav Baranov (russhian voice. I swear he's really cool), bt - Paul Bettani (original)
    public float irritation { get; set; } = 2.1f; //? Maximum until not speak himself
}

public class ConfManager
{
    private static readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true, //? Cool JSON look
        PropertyNameCaseInsensitive = true //? Don`t care on case
    };

    public static readonly string dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "jarvis"); //? %Appdata%/jarvis or ~/.config/jarvis
    public static readonly string configPath = Path.Combine(dataDir, "config.json");

    public static Config Load()
    {
        if (!File.Exists(configPath))
            return new Config();
        
        try
        {
            string jsonStr = File.ReadAllText(configPath);

            Config? config = JsonSerializer.Deserialize<Config>(jsonStr, options);
            return config ?? new Config();
        }
        catch (Exception e)
        {
            Debug.Warn($"Failed read config ({e})");
            return new Config();
        }
    }

    public static void Save(Config config)
    {
        try
        {
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            string json = JsonSerializer.Serialize(config, options);

            File.WriteAllText(configPath, json);
        }
        catch (Exception e)
        {
            Debug.Warn($"Failed save config ({e})");
        }
    }
}