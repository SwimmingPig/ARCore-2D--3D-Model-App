//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyAndroidPrefab;
        public GameObject InputPrefab = null;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;


        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        //public static Mesh InputObj = new Mesh();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        // public Button m_Photo;

        //private void TaskOnClick()
        //{
        //    //Output this to console when the Button is clicked
        //    Debug.Log("You have clicked the button!");
        //}

        // Load Obj code from github https://github.com/google-ar/arcore-unity-sdk/issues/144
        //private GameObject render3DObject()
        //{
        //    GameObject object3D = Resources.Load(Application.persistentDataPath + "Input.obj") as GameObject;
        //    GameObject objectInstance = Instantiate(object3D) as GameObject;
        //    Renderer renderer = objectInstance.GetComponent<Renderer>();
        //    renderer.enabled = true;
        //    //renderer.material.mainTexture = Resources.Load(mType + "_texture") as Texture;

        //    //Debug.Log("X -> 3D object rendered");

        //    Mesh meshToCollide = objectInstance.gameObject.GetComponent<MeshFilter>().mesh;
        //    if (!meshToCollide)
        //    {
        //        //Debug.Log("Mesh not assigned to collide...");
        //        //return;
        //    }
        //    else
        //    {
        //        objectInstance.transform.gameObject.AddComponent<MeshCollider>();
        //        objectInstance.transform.GetComponent<MeshCollider>().sharedMesh = null;
        //        objectInstance.transform.GetComponent<MeshCollider>().sharedMesh = meshToCollide;
        //        objectInstance.transform.GetComponent<MeshCollider>().name = "model";
        //    }

        //    return objectInstance;
        //}
        // ------- end github code ---------

        //public void Start()
        //{
        //    string path = Application.persistentDataPath + "Input.obj";
        //    Mesh InputObj = new Mesh();
        //    InputObj = ObjImporter.ImportFile(path);
        //}

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();

            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = false;
            // for (int i = 0; i < m_AllPlanes.Count; i++)
            // {
            //     if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
            //     {
            //         showSearchingUI = false;
            //         break;
            //     }
            // }

            SearchingForPlaneUI.SetActive(showSearchingUI);

            // Load Obj code from github https://github.com/google-ar/arcore-unity-sdk/issues/144
            //rendered3DObject = render3DObject();
            //float distance = 1;
            //rendered3DObject.transform.position = FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * distance;

            //if (mAnchor == null)
            //{
            //    Pose pose1 = Pose.identity;
            //    pose1.position = Frame.PointCloud.GetPoint(0);
            //    mAnchor = Session.CreateAnchor(pose1);
            //}
            //rendered3DObject.transform.parent = mAnchor.transform;
            // ------- end github code ---------

            // If the player has not touched the screen, we are done with this update.
             Touch touch;
             if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
             {
                 return;
             }

            // Raycast against the location the player touched to search for planes.
             TrackableHit hit;
             TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
             TrackableHitFlags.FeaturePointWithSurfaceNormal;

             if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
             {
                 // Use hit pose and camera pose to check if hittest is from the
                 // back of the plane, if it is, no need to create the anchor.
                 if ((hit.Trackable is DetectedPlane) &&
                     Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                         hit.Pose.rotation * Vector3.up) < 0)
                 {
                     Debug.Log("Hit at back of the current DetectedPlane");
                 }
                 else
                 {
                    // Instantiate Andy model at the hit pose.
                    string path = Application.persistentDataPath + "Input.obj";
                    var InputObject = Instantiate(Resources.Load("chair"), hit.Pose.position, hit.Pose.rotation) as GameObject;
                    InputObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    InputObject.transform.rotation = Quaternion.Euler(0f, 0f, 270f);

                    //Mesh InputObj = new Mesh();
                    //InputObj = ObjImporter.ImportFile(path);
                    //var InputObject = Instantiate(Application.persistentDataPath + "Input.obj"), hit.Pose.position, hit.Pose.rotation);

                     // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                     //andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                    //InputObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                     // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                     // world evolves.
                     var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                     // Make Andy model a child of the anchor.
                     //andyObject.transform.parent = anchor.transform;
                    InputObject.transform.parent = anchor.transform;
                 }
             }
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
