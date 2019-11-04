using Common.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

class ApiClient
{
    private readonly HttpClient httpClient = new HttpClient();

    public ApiClient() { }

    public async Task SendAsync(IMessage message, string url)
    {
        var json = JsonConvert.SerializeObject(message);
        using (var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"))
        {
            await httpClient.PutAsync(url, content);
            Debug.Log($"Sent to {url}");
        }
    }

    public async Task GetAsync(string url)
    {
        try
        {
            await httpClient.GetAsync(url);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}