using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

public class ServerImp 
{
    public event ResponseRecievedEventHandler ResponseRecieved;
    public delegate void ResponseRecievedEventHandler(object sender, ResponseEventArgs e);

    protected virtual void OnResponse(ResponseEventArgs e)
    {
        ResponseRecievedEventHandler handler = ResponseRecieved;
        handler?.Invoke(this, e);
    }
    private static readonly HttpClient client = new HttpClient();
    public ServerImp()
    {
        Caster.Log("Connection initiated...");
    }
    public async void Send(Dictionary<string, string> headers)
    {
        try
        {
        // Flurl will use 1 HttpClient instance per host
        string url = "http://localhost:8080";

        var values = new Dictionary<string, string>
        {
        { "Game", "1" },
        };

        var content = new FormUrlEncodedContent(values);
        foreach (string key in headers.Keys)
        {
            content.Headers.Add(key, headers[key]);
        }
        var response = await client.PostAsync(url, content);

        var responseString = await response.Content.ReadAsStringAsync();

        ResponseEventArgs Response = new ResponseEventArgs();
        Response.Message = responseString;
        OnResponse(Response);

        } catch (Exception e)
        {
            Caster.Log("<color=red><b>ERROR: " + e.Message + "</b></color>");
        }

    }

}
