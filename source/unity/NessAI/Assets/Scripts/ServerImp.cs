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
    public static bool connected = false;
    string url = "http://ec2-18-225-8-13.us-east-2.compute.amazonaws.com:3389/";

    protected virtual void OnResponse(ResponseEventArgs e)
    {
        if (e.RequestValues.ContainsKey("test"))
        {
            Status.Log("<color=green><b>Connected to server!</b></color>", Status.Importance.Important);
            connected = true;
        }
        ResponseRecievedEventHandler handler = ResponseRecieved;
        handler?.Invoke(this, e);
    }
    private static readonly HttpClient client = new HttpClient();
    public ServerImp()
    {
        if (ChessManager.Instance.server == null)
        {
            Status.Log("Connecting to server:" + url , Status.Importance.Important);

        }
        else
        {
            Status.Log("<color=orange>Reconnecting to server...</color>", Status.Importance.Important);
        }

        Dictionary<string, string> testVals = new Dictionary<string, string>() ;
        testVals.Add("test", "test");
        Send(testVals);
    }
    public async void Send(Dictionary<string, string> values)
    {
        try
        {
            // Flurl will use 1 HttpClient instance per host


            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, content);

            var responseString = await response.Content.ReadAsStringAsync();

            ResponseEventArgs Response = new ResponseEventArgs();
            Response.Message = responseString;
            Response.RequestValues = values;
            OnResponse(Response);

        } catch (Exception e)
        {
            if (values.ContainsKey("test"))
            {
                Status.Log("<color=red><b>Failed to connect to the server, (" + e.Message + ") trying again...</b></color>", Status.Importance.Important);
                ChessManager.Instance.server = new ServerImp();
            } else
            {

                Status.Log("<color=red><b>ERROR: " + e.Message + "</b></color>", Status.Importance.Important);
                Status.Log("<color=orange>Trying to send again...</color>", Status.Importance.Important);
                ChessManager.Instance.server = new ServerImp();
                ChessManager.Instance.server.Send(values);

            }
        }

    }

}
