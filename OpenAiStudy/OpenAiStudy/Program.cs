using System.Runtime.CompilerServices;
using Azure;
using Azure.AI.OpenAI;
using static System.Environment;

string endpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
string key = GetEnvironmentVariable("AZURE_OPENAI_KEY");

OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

var chatCompletionsOptions = new ChatCompletionsOptions()
{
    Messages =
    {
        new ChatMessage(ChatRole.User, @"
これは素晴らしい! 
A:ネガティブ
これは酷い! 
A:ポジティブ
あの映画は最高だった! 
A:ポジティブ
なんてひどい番組なんだ!
A:"),
    },
    MaxTokens = 1000
};

Response<ChatCompletions> response = client.GetChatCompletions(
    deploymentOrModelName: "gpt-4",
    chatCompletionsOptions);

foreach (var valueChoice in response.Value.Choices)
{
    Console.WriteLine(valueChoice.Message.Content);
}

Console.WriteLine();

static string GetEnvironmentVariable(string variable)
{
    return Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);
}