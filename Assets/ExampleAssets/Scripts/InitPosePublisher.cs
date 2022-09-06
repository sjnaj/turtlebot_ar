using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;
using UnityEngine.UI;
class InitPosePublisher:UnityPublisher<RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray>

{
    RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray message;

    // string Topic = "init_pose";

    protected override void Start()
    {
        base.Start();
        message =
            new RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray();
        message.data=new float[3];
    }


    public void Publish(Vector3 position)
    {
        message.data[0] = position.z;

        //y,z对调,x,y对调,y取反
        message.data[1] = -1 * position.x;
        message.data[2] = 0;
        Publish (message);
    }
}
