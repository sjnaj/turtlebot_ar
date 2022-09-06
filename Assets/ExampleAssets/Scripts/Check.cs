using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
// c# 为什么要用 get set 属性
// 1 可以对赋值 做验证 范围限制，额外的限制

// 2 可以设置 只读 只写

// 3 可以做线程同步

// 4 可以将属性设置在interface接口中

// 5 可以使用虚属性 或 抽象属性

// 可以填补 没有 虚字段 抽象字段的 遗憾，在设计组件的时候非常有用



// 　1.该类型的主要职责是否用于数据存储？ 　 　　2.该类型的公有接口是否都是一些存取属性？

// 　　3.是否确信该类型永远不可能有子类？

// 　　4.是否确信该类型永远不可能具有多态行为？

// 　　如果所有问题的答案都是yes，那么就应该采用值类型。
public class Check : MonoBehaviour
{
    [SerializeField]//使属性显示在inspector面板,public变量会自动显示，但不够安全
    ARSession m_Session;

    //变量对外接口
    public ARSession Session { get => m_Session; set => m_Session = value; }

    [SerializeField]
    Button m_InstallButton;
    public Button InstallButton { get => m_InstallButton; set => m_InstallButton = value; }


    [SerializeField]
    Text m_LogText;
    public Text LogText { get => m_LogText; set => m_LogText = value; }

    void Log(string message)//Dubug 信息输出在文本控件上
    {
        m_LogText.text += $"{message}\n";
    }

    IEnumerator CheckSupport()
    {
        try
        {
            SetInstallButtonActive(false);//隐藏按钮

            Log("检查设备...");
            yield return ARSession.CheckAvailability();
            if (ARSession.state == ARSessionState.NeedsInstall)
            {
                Log("设备支持AR;但需要更新");
                Log("尝试更新...");

                yield return ARSession.Install();
            }
            if (ARSession.state == ARSessionState.Ready)
            {
                Log("设备支持AR!");
                Log("启动AR...");
                yield return new WaitForSeconds(5);// 文本框延时两秒
                m_Session.enabled = true;
            }
            else
            {
                switch (ARSession.state)
                {
                    case ARSessionState.Unsupported:
                        Log("设备不支持AR.");
                        break;
                    case ARSessionState.NeedsInstall:
                        Log("更新失败。");
                        SetInstallButtonActive(true);

                        // In this case, we enable a button which allows the user
                        // to try again in the event they decline the update the first time.
                        break;
                }
                //启动非AR的替代方案
            }
        }
        finally//释放资源
        {
            LogText.gameObject.SetActive(false);//隐藏文本框

        }
        // Destroy(m_LogText,2);//2s后销毁文本框
    }
    void SetInstallButtonActive(bool active)
    {
        if (InstallButton != null)
        {
            InstallButton.gameObject.SetActive(active);
        }
    }
    IEnumerator Install()//手动安装，checksupport安装失败时启用
    {
        SetInstallButtonActive(false);//隐藏

        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            Log("尝试安装...");
            yield return ARSession.Install();

            if (ARSession.state == ARSessionState.NeedsInstall)
            {
                Log("升级失败,或者你取消了安装");
                SetInstallButtonActive(true);
            }
            else if (ARSession.state == ARSessionState.Ready)
            {
                Log("成功! 启动 AR...");
                m_Session.enabled = true;
            }
        }
        else
        {
            Log("ARSession 不需要安装.");
        }
    }

    void OnInstallButtonPressed()
    {
        StartCoroutine(Install());
    }
    void Start()//按钮监听
    {
        m_InstallButton.onClick.AddListener(() => { OnInstallButtonPressed(); });

    }
    void OnEnable()
    {
        StartCoroutine(CheckSupport());//启动一个协程
                                       //使用 yield 语句，随时暂停协程的执行。使用 yield 语句时，协程会暂停执行，并在下一帧自动恢复。分次加载，减少卡顿

    }


}