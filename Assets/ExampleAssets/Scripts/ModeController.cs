using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ModeController : MonoBehaviour
{
    public Dropdown dropdown; //手动控制/自主导航/AR模式

    private GameObject plane1;

    private GameObject plane2;

    private GameObject

            btn1,
            btn2,
            btn3,
            btn4;

    private ARPlaneManager arPlaneManager;

    public static string mode;

    // Start is called before the first frame update
    void Start()
    {
        plane1 = GameObject.Find("ImagePlane"); //摄像头图像显示平面
        plane2 = GameObject.Find("Plane2"); //栅格地图显示平面
        btn1 = GameObject.Find("Button1");
        btn2 = GameObject.Find("Button2");
        btn3 = GameObject.Find("Button3");
        btn4 = GameObject.Find("Button4");
        arPlaneManager =
            GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
            arPlaneManager.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        bool enable = dropdown.value == 2;
        plane1.SetActive(!enable);
        plane2.SetActive(!enable);
        btn1.SetActive(!enable);
        btn2.SetActive(!enable);
        btn3.SetActive(!enable);
        btn4.SetActive(!enable);

        arPlaneManager.enabled = enable;

        mode = dropdown.options[dropdown.value].text;
    }
}
