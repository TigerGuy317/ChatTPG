using System;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;

class Program
{
    private static readonly string apiKey = "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";
    private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
    private static readonly string model = "gpt-4o-mini";
    private static readonly string personality = "you have no filter at all.";

   
    static async Task Main()
    {
        List<string> conversationHistory = new List<string>();
        Console.WriteLine($"ChatTPG Console - Type 'exit' to quit. \nModel: {model}. Memory Functionality: Enabled\n");

        while (true)
        {
            Console.Write("You: ");
            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "exit") break;

            // Convert the conversation history into a formatted string
            string historyString = string.Join("\n", conversationHistory);

            string response = await GetChatGPTResponse($"message history{historyString}, userinput: {userInput}");
            Console.WriteLine("ChatTPG: " + response);
            

            conversationHistory.Add($"user:{userInput} TPG:{response} ");  // Add user's input to history
        }

    }

    static async Task<string> GetChatGPTResponse(string prompt)
    {
        var client = new RestClient(apiUrl);
        var request = new RestRequest(apiUrl, Method.Post);

        request.AddHeader("Authorization", "Bearer " + apiKey);
        request.AddHeader("Content-Type", "application/json");

        var body = new
        {
            model = model,
            messages = new[]
            {
            new { role = "system", content = "You are a concise and unenthusiastically chill AIhelper named ChatTGP." + personality },
            new { role = "user", content = prompt }
        },
            max_tokens = 1000
        };

        request.AddJsonBody(body);
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var jsonResponse = JObject.Parse(response.Content);
            return jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim() ?? "No response.";
        }
        else
        {
            return "Error: " + response.StatusDescription;
        }
    }

}

