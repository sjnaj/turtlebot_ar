using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEarthColor : MonoBehaviour
{
    public GameObject Earth;
    //申请GameObject类型的变量 储存地球模型

    public Texture Card_01;
    //申请Texture类型的变量  储存Card_01图片
    // Start is called before the first frame update
    void Start()
    {
        // MyLogger.Log( Earth.GetComponent<MeshRenderer>().material.mainTexture.ToString());
        Earth.GetComponent<MeshRenderer>().material.mainTexture = Card_01;
        // MyLogger.Log( Earth.GetComponent<MeshRenderer>().material.mainTexture.ToString());

        //将地球模型材质的主贴图替换为Card_01
    }

    // Update is called once per frame
    void Update()
    {

    }
}
