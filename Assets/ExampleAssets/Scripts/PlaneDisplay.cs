using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneDisplay : MonoBehaviour
{
    public Dropdown menu;

    private ARPlaneManager m_ARPlaneManager;
    private List<ARPlane> m_Planes;

    void Awake()//放在start可能不被初始化
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_Planes = new List<ARPlane>();
    }
    void OnEnable()
    {
        m_ARPlaneManager.planesChanged += OnPlaneChanged;

    }

    void Start()
    {


    }
    void OnDisable()
    {
        m_ARPlaneManager.planesChanged -= OnPlaneChanged;
    }
   
    private void OnPlaneChanged(ARPlanesChangedEventArgs args)//保持更新
    {
        if(menu.value==1)
        for (int i = 0; i < args.added.Count; i++)
        {
            // MyLogger.Log(args.added.Count + "");
            m_Planes.Add(args.added[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = m_Planes.Count - 1; i >= 0; i--)
        {
            if (m_Planes[i] == null || m_Planes[i].gameObject == null)//删去旧平面
            {
                m_Planes.Remove(m_Planes[i]);
            }
            else
            {
                m_Planes[i].gameObject.SetActive(menu.value==1);
            }

        }
    }
}
