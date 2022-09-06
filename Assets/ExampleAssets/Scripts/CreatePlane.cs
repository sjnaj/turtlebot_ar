using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

//这个功能一般由SubsystemManager完成
public class CreatePlane : MonoBehaviour
{
    // Start is called before the first frame update
    XRPlaneSubsystem CreatePlaneSubsystem()
    {
        var descriptors = new List<XRPlaneSubsystemDescriptor>();
        SubsystemManager.GetSubsystemDescriptors(descriptors);//获取所有planeSubsystems
        foreach (var descriptor in descriptors)
        {
            if (descriptor.supportsBoundaryVertices)//找出一个支持BoundaryVertices的system
            {
                return descriptor.Create();//单例构造
            }
        }
        return null;
    }
    void Start()
    {
        var planeSubsystems = CreatePlaneSubsystem();
        if (planeSubsystems != null)
        {
            planeSubsystems.Start();
        }
        if (planeSubsystems != null)
        {
            planeSubsystems.Stop();
        }
        if (planeSubsystems != null)
        {
            planeSubsystems.Destroy();
            planeSubsystems = null;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
