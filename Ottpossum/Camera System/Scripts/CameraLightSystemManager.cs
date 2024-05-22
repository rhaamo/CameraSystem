
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace CameraSystem {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class CameraLightSystemManager : UdonSharpBehaviour {
        [Header("Cameras")]
        public GameObject theCamera;
        public Transform[] cameraTransforms;
        public string[] cameraViewNames;
        public Animator cameraAnimator;

        [Header("UI")]
        public Image[] cameraButtons;
        public TextMeshProUGUI currentCameraText;
        public TextMeshProUGUI[] cameraTextButtons;

        private int lastCamera = 0;
        [HideInInspector] [UdonSynced] public int currentCamera = 0;

        [Header("State colors")]
        public Color32 colorGreen = new Color(15/255f, 132/255f, 12/255f, 255/255f);
        public Color32 colorGrey = new Color(255f, 255f, 255f, 255/255f);
        public Color32 colorRed = new Color(255f, 0/255f, 0/255f, 255/255f);
        public Color32 colorBlack = new Color(0f, 0f, 0f, 255/255f);

        private bool isAuthorized = false;

        public void authorize() {
            Debug.Log($"[OTT_CAMERA_SYSTEM_LIGHT][authorize] User is now authorized to use the console");
            isAuthorized = true;
        }

        public void deauthorize() {
            Debug.Log($"[OTT_CAMERA_SYSTEM_LIGHT][deauthorize] User is now forbidden to have fun");
            isAuthorized = false;
        }

        void Start() {
            Debug.Log($"[OTT_CAMERA_SYSTEM_LIGHT][Start] Initializing...");
            // Set all buttons to grey and set the text
            foreach (Image img in cameraButtons) {
                if (Utilities.IsValid(img)) {
                    img.color = colorGrey;
                }
            }
            for (int i=0; i<cameraTextButtons.Length; i++) {
                if (Utilities.IsValid(cameraTextButtons[i]) && Utilities.IsValid(cameraViewNames[i])) {
                    cameraTextButtons[i].text = cameraViewNames[i];
                }
            }
            // Enable the first view
            toggleView(0, true);
        }

        public override void OnDeserialization() {
            Debug.Log($"[OTT_CAMERA_SYSTEM_LIGHT][OnDeserialization] Received an update, current camera is now {currentCamera}/{cameraViewNames[currentCamera]}, last was {lastCamera}/{cameraViewNames[lastCamera]}");
            if (currentCamera == lastCamera) {
                Debug.Log($"[OTT_CAMERA_SYSTEM_LIGHT][toggleView] current camera == last camera, ignoring");
                return;
            }
            toggleView(currentCamera, true);
        }

        // Remember that arrays starts at 0, so our index 0 is view 1, etc.
        public void toggleView(int index, bool force = false) {
            if (!isAuthorized && !force) {
                Debug.Log($"[OTT_CAMERA_SYSTEM_LIGHT][toggleView] No fun allowed");
                return;
            }
            if (index == currentCamera &&  !force) {
                Debug.Log($"[OTT_CAMERA_SYSTEM_LIGHT][toggleView] index == current camera, doing nothing");
                return;
            }
            Debug.Log($"[OTT_CAMERA_SYSTEM_LIGHT][toggleView] Switching to view {index+1}: {cameraViewNames[index]}");
            // Set the previous button
            cameraButtons[lastCamera].color = colorGrey;
            // Set the button color
            cameraButtons[index].color = colorGreen;

            // Change the camera position
            theCamera.transform.position = cameraTransforms[index].transform.position;
            theCamera.transform.rotation = cameraTransforms[index].transform.rotation;

            // Set the camera live text
            currentCameraText.text = cameraViewNames[index];

            // Special camera "View 5"  animation
            if (index == 4) {
                cameraAnimator.SetBool("Motion", true);
            } else {
                cameraAnimator.SetBool("Motion", false);
            }

            // Set our indexes
            currentCamera = index;
            lastCamera = index;

            // Then sync
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            RequestSerialization();
        }

        public void toggleView1() {
            toggleView(0);
        }
        public void toggleView2() {
            toggleView(1);
        }
        public void toggleView3() {
            toggleView(2);
        }
        public void toggleView4() {
            toggleView(3);
        }
        public void toggleView5() {
            toggleView(4);
        }
    }
}
