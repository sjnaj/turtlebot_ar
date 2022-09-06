using UnityEngine;
using UnityEngine.UI;

    public class JoyReader:MonoBehaviour
    {


    public string Name;

  private  Button btn1, btn2, btn3, btn4;
    public void Start()
    {
        btn1 = GameObject.Find("Button1").GetComponent<Button>();
        btn2 = GameObject.Find("Button2").GetComponent<Button>();
        btn3 = GameObject.Find("Button3").GetComponent<Button>();
        btn4 = GameObject.Find("Button4").GetComponent<Button>();

    }

    public double Read()
        {
        if(Name== "Horizontal")
        {
            return (btn1.GetComponent<ButtonListener>().isPressed ? 1 : 0) + (btn2.GetComponent<ButtonListener>().isPressed ? -1 : 0);


        }
        else
        {
            return (btn4.GetComponent<ButtonListener>().isPressed ? 0.3 : 0) + (btn3.GetComponent<ButtonListener>().isPressed ? -0.3 : 0);

        }
    }
    }
