using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))] //确保添加该脚本的对象上有ARRaycastManager组件
public class ARPositionController : MonoBehaviour
{
    // public GameObject spawnPrefab;//预制体，拖入设置
    public Plane ground;

    private List<ARRaycastHit> Hits; //点击位置信息

    private ARRaycastManager mRaycastManager; //射线碰撞位置信息

    // private GameObject spawnObject = null;
    // Start is called before the first frame update
    public static Vector3 position = new Vector3(0, 1, 0);

    private static Ray ray;

    private GameObject arCamera;

    private void Start()
    {
        Hits = new List<ARRaycastHit>();

        mRaycastManager = GetComponent<ARRaycastManager>();

        arCamera = GameObject.Find("AR Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0) return;
        var touch = Input.GetTouch(0); //获取第一个触碰的手指信息
        if (touch.position.x >= 1600 && touch.position.y >= 700) return; //避开控制区域
        if (
            mRaycastManager
                .Raycast(touch.position,
                Hits,
                TrackableType.PlaneWithinPolygon |
                TrackableType.PlaneWithinBounds) //射线落在平面的（多边形）边界或内部
        )
        {
            var hitPose = Hits[0].pose;

            foreach (var hit in Hits)
            {
                if (hit.pose.position.y < hitPose.position.y)//选出最靠下的点，防止多余平面干扰
                {
                    hitPose = hit.pose;
                }
            }
            // MyLogger.Log(hitPose.position.ToString());
            position = hitPose.position;

        }
    }
}
