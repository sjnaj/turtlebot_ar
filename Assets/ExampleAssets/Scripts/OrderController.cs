using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using UnityEngine.UI;

public class OrderController : UnityPublisher<RosSharp.RosBridgeClient.MessageTypes.Std.UInt8>
{
    private RosSharp.RosBridgeClient.MessageTypes.Std.UInt8 message;

    private byte order;
    public Dropdown menu;
 
    public Button btn;

    private void InitDropdown()
    {
        //清空默认节点
        menu.options.Clear();

        //初始化
        Dropdown.OptionData op0 = new Dropdown.OptionData();
        op0.text = "建图";
        menu.options.Add(op0);

        Dropdown.OptionData op1 = new Dropdown.OptionData();
        op1.text = "打开map1";
        menu.options.Add(op1);

        Dropdown.OptionData op2 = new Dropdown.OptionData();
        op2.text = "打开map2";
        menu.options.Add(op2);

        Dropdown.OptionData op3 = new Dropdown.OptionData();
        op3.text = "打开map3";
        menu.options.Add(op3);

        Dropdown.OptionData op4 = new Dropdown.OptionData();
        op4.text = "保存";
        menu.options.Add(op4);



    }
     private void Confirm()
    {

       order=(byte)menu.value;
       UpdateMessage();

    }

    protected override void Start()
    {
        base.Start();
        InitializeMessage();
        InitDropdown();
        btn.onClick.AddListener(Confirm);
        btn.gameObject.SetActive(false);
    }
    
    private  void Update()
    {
        if (ModeController.mode == "手动控制")
            btn.gameObject.SetActive(true);
        else btn.gameObject.SetActive(false);
    }


    private void InitializeMessage()
    {
        message = new RosSharp.RosBridgeClient.MessageTypes.Std.UInt8();
    }

    private void UpdateMessage()
    {

        message.data = order;
        Publish(message);
    }
}

