using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RosSharp.RosBridgeClient;
using RosSharp;
using WebSocketSharp;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.IO;

[RequireComponent(typeof(RosConnector))]
public class VideoProcess : UnitySubscriber<RosSharp.RosBridgeClient.MessageTypes.Sensor.CompressedImage>
{
    public MeshRenderer meshRenderer;

    private Texture2D texture2D;
    public Dropdown dropdown;

    private bool isMessageReceived;

    private byte[] imageData;



    public string webPath = "ws://192.168.246.15:8765";

    private WebSocket webSocket;

    protected new void Start()

    {

        base.Start();
        texture2D = new Texture2D(1, 1);
        meshRenderer.material = new Material(Shader.Find("Standard"));
        webSocket = new WebSocket(webPath);
        webSocket.Connect();
        webSocket.OnMessage += Receive;
    }
    private void Receive(object sender, MessageEventArgs e)
    {
        byte[] buffer = ((MessageEventArgs)e).RawData;
        if (dropdown.gameObject.activeSelf && dropdown.value == 1)
        {
            webSocket.Send(imageData);
        }
        ProcessMessage(buffer);

    }



    private new void Update()
    {
        base.Update();
        if (isMessageReceived)
        {
            if (dropdown.value == 1)
            {
                webSocket.Send(imageData);
            }
            else
            {
                ProcessMessage(imageData);
            }
        }
        dropdown.gameObject.SetActive(ModeController.mode != "AR模式");
    }

    // IEnumerator send()
    // {
    //     yield return new WaitForSeconds(0.1f);
    //     webSocket.SendAsync(imageData, null);

    // }

    protected override void ReceiveMessage(RosSharp.RosBridgeClient.MessageTypes.Sensor.CompressedImage compressedImage)
    {
        // Debug.Log(compressedImage.data.Length);
        imageData = compressedImage.data;

        isMessageReceived = true;
    }

    private void ProcessMessage(byte[] imageData)
    {
        texture2D.LoadImage(imageData);
        texture2D.Apply();
        meshRenderer.material.SetTexture("_MainTex", texture2D);
        isMessageReceived = false;
    }

}
