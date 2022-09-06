using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Net.Http.Headers;
// using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using RosSharp.RosBridgeClient;
using UnityEngine;
using UnityEngine.UI;

public class NavController : MonoBehaviour
{
    public Button addBtn;

    public static RosConnector rosConnector = null; //非公开类不能获取到

    public Dropdown menu;

    private JoyReader[] JoyAxisReaders;

    public GameObject PosePrefab;

    private GameObject initPoseObject = null;

    private List<GameObject> poseObjects = new List<GameObject>();

    //预制体初始化时的默认姿态
    private Vector3 initPose = Vector3.up; //设置一定高度防止被机器人遮盖

    private Quaternion initRotate = Quaternion.Euler(Vector3.zero); //(0,0,0,1)

    private InitPosePublisher initPosePublisher = null;

    private   TargetPosesPublisher targetPosesPublisher;

    

    private void Confirm()
    {
        switch (menu.value)
        {
            case 0:
                initPosePublisher.Publish(initPoseObject.transform.position);
                break;
            case 1:
                poseObjects.Add(Instantiate(PosePrefab, initPose, initRotate));
                break;
            case 2:
                targetPosesPublisher.Publish (poseObjects);
                break;
            case 3:
                foreach (GameObject obj in poseObjects) GameObject.Destroy(obj);
                poseObjects.Clear();

                break;
        }
    }

    private void InitDropdown()
    {
        //清空默认节点
        menu.options.Clear();

        //初始化
        Dropdown.OptionData op0 = new Dropdown.OptionData();
        op0.text = "估计初始点";
        menu.options.Add (op0);

        Dropdown.OptionData op1 = new Dropdown.OptionData();
        op1.text = "添加目标点";
        menu.options.Add (op1);

        Dropdown.OptionData op2 = new Dropdown.OptionData();
        op2.text = "开始导航";
        menu.options.Add (op2);
        Dropdown.OptionData op3 = new Dropdown.OptionData();
        op3.text = "清除标记";
        menu.options.Add (op3);
    }

    private void InitializeGameObject()
    {
        InitDropdown();
        JoyAxisReaders = GetComponents<JoyReader>();
        rosConnector = GetComponent<RosConnector>();
        addBtn
            .onClick
            .AddListener(() =>
            {
                Confirm();
            });
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        InitializeGameObject();
    }

    void Start()
    {
        initPoseObject = Instantiate(PosePrefab, initPose, initRotate);

        menu.gameObject.SetActive(false);
        addBtn.gameObject.SetActive(false);

        initPosePublisher = GetComponent<InitPosePublisher>();
        targetPosesPublisher = GetComponent<TargetPosesPublisher>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ModeController.mode == "自主导航")
        {
            menu.gameObject.SetActive(true);
            addBtn.gameObject.SetActive(true);

            Vector3 updateVec =
                (
                new Vector3((float) JoyAxisReaders[1].Read() * 0.05f,
                    0,
                    (float) JoyAxisReaders[0].Read() * 0.02f)
                ); //缩放速度
            switch (menu.value)
            {
                case 0:
                    initPoseObject.SetActive(true);
                    initPoseObject.transform.position += updateVec;
                    break;
                case 1:
                    initPoseObject.SetActive(false);
                    if (poseObjects.Count != 0)
                        poseObjects[poseObjects.Count - 1].transform.position +=
                            updateVec;
                    break;
                default:
                    break;
            }
        }
        else
        {
            menu.gameObject.SetActive(false);
            addBtn.gameObject.SetActive(false);
            foreach (GameObject obj in poseObjects) GameObject.Destroy(obj);

            poseObjects.Clear();
        }
    }
}
