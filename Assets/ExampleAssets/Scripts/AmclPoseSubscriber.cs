using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace RosSharp.RosBridgeClient
{
    public class
    AmclPoseSubscriber
    : UnitySubscriber<MessageTypes.Geometry.PoseWithCovarianceStamped>
    {
        public Transform PublishedTransform;

        public Transform PublishedTransform1;

        private Vector3 position;

        private Quaternion rotation;

        private bool isMessageReceived;

        private float ar_y = 1;

        private ARRaycastManager mRaycastManager;

        private List<ARRaycastHit> Hits;

        protected override void Start()
        {
            PublishedTransform.gameObject.SetActive(false);
            PublishedTransform1.gameObject.SetActive(false);
            base.Start();
            mRaycastManager =
                GameObject
                    .Find("AR Session Origin")
                    .GetComponent<ARRaycastManager>();
        }

        public new void Update()
        {
            base.Update();
            if (ModeController.mode == "自主导航")
            {
                PublishedTransform1.gameObject.SetActive(false);

                PublishedTransform.gameObject.SetActive(true);
                if (isMessageReceived)
                {
                    position.y = (float) 0.5; //防止两个模型碰撞
                    ProcessMessage (PublishedTransform);
                }
            }
            else if (ModeController.mode == "AR模式")
            {
                PublishedTransform.gameObject.SetActive(false);
                PublishedTransform1.gameObject.SetActive(true);
                if (ar_y > 0)
                {
                    ar_y = ARPositionController.position.y; //获取平面高度
                    position.y = ar_y;
                    ProcessMessage (PublishedTransform1);//以初始位姿显示
                }

                if (ar_y < 0 && isMessageReceived)
                {
                    position.y = ar_y;
                    ProcessMessage (PublishedTransform1);
                }
                // MyLogger.Log(position.ToString());

            }
        }

        protected override void ReceiveMessage(
            MessageTypes.Geometry.PoseWithCovarianceStamped message
        )
        {
            position = GetPosition(message).Ros2Unity();

            rotation = GetRotation(message).Ros2Unity();
            isMessageReceived = true;
        }

        private void ProcessMessage(Transform PublishedTransform)
        {
            PublishedTransform.position = position;
            PublishedTransform.rotation = rotation;
            isMessageReceived = false;
        }

        private Vector3
        GetPosition(MessageTypes.Geometry.PoseWithCovarianceStamped message)
        {
            return new Vector3((float) message.pose.pose.position.x,
                (float) message.pose.pose.position.y,
                (float) message.pose.pose.position.z);
        }

        private Quaternion
        GetRotation(MessageTypes.Geometry.PoseWithCovarianceStamped message)
        {
            return new Quaternion((float) message.pose.pose.orientation.x,
                (float) message.pose.pose.orientation.y,
                (float) message.pose.pose.orientation.z,
                (float) message.pose.pose.orientation.w);
        }
    }
}
