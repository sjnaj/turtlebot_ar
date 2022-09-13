/*
© CentraleSupelec, 2017
Author: Dr. Jeremy Fix (jeremy.fix@centralesupelec.fr)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

// Adjustments to new Publication Timing and Execution Framework 
// © Siemens AG, 2018, Dr. Martin Bischoff (martin.bischoff@siemens.com)

using UnityEngine;
using RosSharp.RosBridgeClient;
public class InitPoseImagePublisher : UnityPublisher<RosSharp.RosBridgeClient.MessageTypes.Sensor.CompressedImage>
{

    private RosSharp.RosBridgeClient.MessageTypes.Sensor.CompressedImage message;

    protected override void Start()
    {
        base.Start();
        InitializeMessage();
    }


    private void InitializeMessage()
    {
        message = new RosSharp.RosBridgeClient.MessageTypes.Sensor.CompressedImage();
    }

    public void SendImage(byte[] bytes)
    {
        message.data = bytes;
        message.format = GameObject.Find("AR Camera").transform.position.ToString();//初始摄像机位置
        Publish(message);
    }
}

