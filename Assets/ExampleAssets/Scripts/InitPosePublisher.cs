using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;
using UnityEngine.UI;
using RosSharp;
class InitPosePublisher : UnityPublisher<RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray>

{
    RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray message;

    // string Topic = "init_pose";

    protected override void Start()
    {
        base.Start();
        message =
            new RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray();
    }


    public void Publish(Vector3 position)
    {
        Vector3 rosPosition = position.Unity2Ros();
        message.data = new float[] { rosPosition.x, rosPosition.y, rosPosition.z };
        Publish(message);
    }
}
