

using System.Security.Principal;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class MapSubscriber : UnitySubscriber<MessageTypes.Sensor.Image>
    {
        public MeshRenderer meshRenderer;

        private Texture2D texture2D;
        private byte[] imageData;
        private bool isMessageReceived;

        protected override void Start()
        {
            base.Start();
            texture2D = new Texture2D(1, 1);
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }
        private new void Update()
        {
            base.Update();
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Sensor.Image gridImage)
        {
            Debug.Log(isMessageReceived);

            imageData = gridImage.data;

            // for (int i = 0; i < 1; i++)//map->rgb 线程锁导致不能修改，只能修改ros端发送的数据
            // {
            //     if (tempData[i] == -1)
            //     {
            //         imageData[i] = 10;
            //     }
            //     else if (tempData[i] == 0)
            //     {
            //         imageData[i] = 255;
            //     }
            //     else { imageData[i] = 0; }
            // }

            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            // Debug.Log(imageData.Length);
            texture2D.LoadImage(imageData);
            texture2D.Apply();
            meshRenderer.material.SetTexture("_MainTex", texture2D);
            isMessageReceived = false;
        }

    }
}

