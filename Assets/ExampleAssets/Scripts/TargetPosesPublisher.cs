using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;
using UnityEngine.UI;

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
            message.data[i] = target.transform.position.z;
            message.data[i + 1] = -1 * target.transform.position.x;
            message.data[i + 2] = 0;

            i += 3;
        }
        Publish (message);
    }
}
