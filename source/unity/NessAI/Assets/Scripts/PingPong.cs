using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

public class PingPong : MonoBehaviour
{
    public Text output;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ping()
    {
        
    }
    public void DisplayResponse(object sender, ResponseEventArgs e)
    {
        output.text = e.Message;
    }
}
