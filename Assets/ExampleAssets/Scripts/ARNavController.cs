using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARNavController : MonoBehaviour
{
    public Button addBtn;

    private List<ARRaycastHit> Hits; //点击位置信息

    private TargetPosesPublisher targetPosesPublisher;

    public Dropdown menu;

    public GameObject PosePrefab;

    private List<GameObject> poseObjects = new List<GameObject>();

    //预制体初始化时的默认姿态
    private Vector3 initPose = Vector3.up;

    private Quaternion initRotate = Quaternion.Euler(Vector3.zero); //(0,0,0,1)

    private void Confirm()
    {
        switch (menu.value)
        {
            case 0: 
                poseObjects.Add(Instantiate(PosePrefab, initPose, initRotate));
                break;
            case 1:
                targetPosesPublisher.Publish (poseObjects);
                break;
            case 2:
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
        op0.text = "添加目标点";
        menu.options.Add (op0);
        Dropdown.OptionData op1 = new Dropdown.OptionData();
        op1.text = "开始导航";
        menu.options.Add (op1);
        Dropdown.OptionData op2 = new Dropdown.OptionData();
        op2.text = "清除标记";
        menu.options.Add (op2);
    }

    private void InitializeGameObject()
    {
        InitDropdown();

        addBtn
            .onClick
            .AddListener(() =>
            {
                Confirm();
            });
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeGameObject();
        menu.gameObject.SetActive(false);
        addBtn.gameObject.SetActive(false);
        targetPosesPublisher = GetComponent<TargetPosesPublisher>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ModeController.mode == "AR模式")
        {
            addBtn.gameObject.SetActive(true);
            menu.gameObject.SetActive(true);

            if (menu.value == 0 && poseObjects.Count > 0)
            {
                poseObjects[poseObjects.Count - 1].transform.position =
                    ARPositionController.position;
                //高度发送时当作零
            }
        }
        else
        {
            addBtn.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);
            foreach (GameObject obj in poseObjects)
            obj.gameObject.SetActive(false);
            poseObjects.Clear();
        }
    }
}
