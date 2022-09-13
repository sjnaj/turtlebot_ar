using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// using OpenCvSharp;
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

    public ARCameraBackground mARCameraBackground;
    private Texture2D mLastCameraTexture;
    private RenderTexture mRenderTexture;


    private void Confirm()
    {
        switch (menu.value)
        {
            case 0:
                ScreenShot(); break;
            case 1:
                poseObjects.Add(Instantiate(PosePrefab, initPose, initRotate));
                break;
            case 2:
                //添加标记，便于ros端区分ar模式和远程模式
                GameObject arLabel = new GameObject();
                arLabel.transform.position = Vector3.down * 100;//-100
                poseObjects.Add(arLabel);
                targetPosesPublisher.Publish(poseObjects);
                break;
            case 3:
                foreach (GameObject obj in poseObjects) GameObject.Destroy(obj);
                poseObjects.Clear();
                break;
        }
    }

    // int i = 0;
    private void ScreenShot()
    {
        if (mRenderTexture == null)
        {
            RenderTextureDescriptor renderTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.BGRA32);
            mRenderTexture = new RenderTexture(renderTextureDescriptor);
        }
        Graphics.Blit(null, mRenderTexture, mARCameraBackground.material);
        var activeRenderTexture = RenderTexture.active;//暂存
        RenderTexture.active = mRenderTexture;
        if (mLastCameraTexture == null)
            mLastCameraTexture = new Texture2D(mRenderTexture.width, mRenderTexture.height, TextureFormat.RGB24, true);
        mLastCameraTexture.ReadPixels(new Rect(0, 0, mRenderTexture.width, mRenderTexture.height), 0, 0);
        mLastCameraTexture.Apply();//显示
        RenderTexture.active = activeRenderTexture;//恢复
        var bytes = mLastCameraTexture.EncodeToPNG();
        GetComponent<InitPoseImagePublisher>().SendImage(bytes);

        //打印相机标定图片
        // var path = Application.persistentDataPath + (i++) + ".png";
        // if (i == 0)
        //     MyLogger.Log(path);
        // System.IO.File.WriteAllBytes(path, bytes);
    }
    // private Transform GetInitTransform(byte[] bytes){


    // }

    private void InitDropdown()
    {
        //清空默认节点
        menu.options.Clear();

        Dropdown.OptionData op0 = new Dropdown.OptionData();
        op0.text = "计算初始位姿";
        menu.options.Add(op0);

        //初始化
        Dropdown.OptionData op1 = new Dropdown.OptionData();
        op1.text = "添加目标点";
        menu.options.Add(op1);
        Dropdown.OptionData op2 = new Dropdown.OptionData();
        op2.text = "开始导航";
        menu.options.Add(op2);
        Dropdown.OptionData op3 = new Dropdown.OptionData();
        op3.text = "清除标记";
        menu.options.Add(op3);
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

            if (menu.value == 1 && poseObjects.Count > 0)
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
