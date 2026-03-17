using Azure.AI.OpenAI;
using Azure;
using CommunityToolkit.HighPerformance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.KernelMemory.DataFormats;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SharpCompress.Common;
using System;
using System.Transactions;
using UseSemanticKernelFromNET.Plugins;
using OpenAI.VectorStores;
using OpenAI.Files;
using OpenAI.Assistants;

namespace UseSemanticKernelFromNET
{
    public class MultiModal(IConfiguration configuration)
    {
        IConfiguration configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        public async Task IntepretAnImageAndProvideSuggestions(string deploymentName, string endpoint, string apiKey)
        {
            string prompt =
                """
                You are an AI assistant that can give tips about a highlighted area on a map. 
                The Map is provided as an image url image as part of the user prompt. On the map there is a red rectangle that highlights the area of interest.
                You need to analyze the image and give information about the highlighted area and you provide at least two tips where 
                you can find good restaurants.
                """;
            string imgUrl = "https://raw.githubusercontent.com/vriesmarcel/vslive-2026-vegas-sk/refs/heads/main/NL-Map-Highlight.png";



            Kernel kernel = Kernel.CreateBuilder().
                AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey)
                .Build();
            var history = new ChatHistory();
            history.AddSystemMessage(prompt);
            var message = new ChatMessageContentItemCollection
                {
                    new TextContent("Here is the image"),
                    new ImageContent(new Uri(imgUrl))
                };
            history.AddUserMessage(message);

            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var result = await chat.GetChatMessageContentAsync(history);
            Console.WriteLine(result);
        }

        public async Task GetDatafromHandWrittenForm(string deploymentName, string endpoint, string apiKey)
        {
            string prompt =
                """
                You are an AI assistant that will help me to extract information from a hand written form. 
                The form is provided as an image url image as part of the user prompt.
                You return the information in a JSON format with the following fields: number, name, service-name and job-title.
                """;
            string imgUrl = "https://raw.githubusercontent.com/vriesmarcel/vslive-2026-vegas-sk/refs/heads/main/handwritten-form.jpeg";



            Kernel kernel = Kernel.CreateBuilder().
                AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey)
                .Build();
            var history = new ChatHistory();
            history.AddSystemMessage(prompt);
            var message = new ChatMessageContentItemCollection
                {
                    new TextContent("Here is the image"),
                    new ImageContent(new Uri(imgUrl))
                };
            history.AddUserMessage(message);

            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var result = await chat.GetChatMessageContentAsync(history);
            Console.WriteLine(result);
        }

    }
}
