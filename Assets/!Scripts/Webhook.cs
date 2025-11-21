using System;
using UnityEngine;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Management;


public class Webhook : MonoBehaviour
{
    public static Webhook instance;
    
    string webhook = "https://discord.com/api/webhooks/1440657859833499739/2j8lWycIChJTzD86xa2RlqbkIYNektQPLLq9u_IfoJKrOVS5zrxAWIi4Wzuj6FsJ_vR5";

    void Awake()
    {
        instance = this;
        
        // Start message
        SendMs($"\nSession Started: {System.Environment.UserName}\n\n{Hardware()}");

    }
    

    // Sending user info about gameplay
    public void SendStats(string elec, string time, bool quit)
    {
        string user = System.Environment.UserName;

        string msg = $"\nUser: {user}\n⚡: {elec}\n⏳: {time}\n\nTotal time: {(int)Time.time}s\nMachines placed: {GridBuilder.instance.gridObjects.Count}\n\nQuit application: {quit}";
        
        SendMs(msg);
    }
    
    
    // Send message to discord webhook.
    public void SendMs(string message)
    {
        WebClient client = new WebClient();
        client.Headers.Add("Content-Type", "application/json");
        string payload = "{\"content\": \"" + message.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n") + "\"}";
        client.UploadData(webhook, Encoding.UTF8.GetBytes(payload));
    }

    // Get hardware info
    string Hardware()
    {
        return $"GPU: {SystemInfo.graphicsDeviceName}\nCPU: {SystemInfo.processorType}\nMemory: {SystemInfo.systemMemorySize}\n\nScreen resolution: {Screen.width}x{Screen.height}";
    }
}
