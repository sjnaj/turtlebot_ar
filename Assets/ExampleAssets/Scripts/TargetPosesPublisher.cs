using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;
using UnityEngine.UI;
using RosSharp;

class
TargetPosesPublisher
: UnityPublisher<RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray>
{
    RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray message;


    protected override void Start()
    {
        base.Start();
        message =
            new RosSharp.RosBridgeClient.MessageTypes.Std.Float32MultiArray();
    }

    public void Publish(List<GameObject> objects)
    {
        message.data = new float[objects.Count * 3];
        int i = 0;
        foreach (GameObject target in objects)
        {
            Vector3 rosPosition = target.transform.position.Unity2Ros();
            message.data[i] = rosPosition.x;
            message.data[i + 1] = rosPosition.y;
            message.data[i + 2] = rosPosition.z;
            i += 3;
        }
        Publish(message);
    }
}
