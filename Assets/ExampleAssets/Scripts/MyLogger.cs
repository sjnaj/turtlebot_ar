using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
    public class MyLogger : MonoBehaviour
    {
        [SerializeField]
        Text m_LogText;
        public Text logText
        {
            get => m_LogText;
            set => m_LogText = value;
        }

        [SerializeField]
        int m_VisibleMessageCount = 40;
        public int visibleMessageCount
        {
            get => m_VisibleMessageCount;
            set => m_VisibleMessageCount = value;
        }

        int m_LastMessageCount;

        static List<string> s_Log = new List<string>();

        static StringBuilder m_StringBuilder = new StringBuilder();

        void Awake()
        {
            if (m_LogText == null)
            {
                m_LogText = GetComponent<Text>();
            }

            lock (s_Log)//给线程加锁，保证线程同步
            {
                s_Log?.Clear();
            }

            Log("Log console initialized.");
        }

        void Update()
        {
            lock (s_Log)//防止更新text过程中String被更新
            {
                if (m_LastMessageCount != s_Log.Count)
                {
                    m_StringBuilder.Clear();
                    var startIndex = Mathf.Max(s_Log.Count - m_VisibleMessageCount, 0);
                    for (int i = startIndex; i < s_Log.Count; ++i)//打印更新的部分
                    {
                        m_StringBuilder.Append($"{i:000}> {s_Log[i]}\n");
                    }

                    var text = m_StringBuilder.ToString();

                    if (m_LogText)
                    {
                        m_LogText.text = text;//输出到文本
                    }
                    else
                    {
                        Debug.Log(text);
                    }
                }

                m_LastMessageCount = s_Log.Count;
            }
        }

        public static void Log(string message)//用于被其它类调用
        {
            lock (s_Log)
            {
                if (s_Log == null)
                    s_Log = new List<string>(5);

                s_Log.Add(message);
            }
        }
    }

