using System.Text;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Services.Deepseek;

public class DeepSeekService(
    IOptions<DeepSeekConfig> deepSeekConfig, 
    HttpClient httpClient
    ): IDeepSeekService
{
    private readonly DeepSeekConfig _config = deepSeekConfig.Value;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ChatbotResponse> SendMessage(ChatbotRequest message)
    {
        var apiUrl = _config.ApiUrl;
        var apiKey = _config.ApiKey;

        var requestBody = new
        {
            model = _config.Model,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = message.Request
                }
            }
        };
        
        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        
        var response = await _httpClient.PostAsync(apiUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("DeepSeek API request failed: " + responseString);
        }

        var result = string.IsNullOrEmpty(JsonConvert.DeserializeObject<ChatbotResponse>(responseString).Response)? 
            throw new HttpRequestException("DeepSeek API request failed: " + responseString) : JsonConvert.DeserializeObject<ChatbotResponse>(responseString);
        
        return result;
    }
}