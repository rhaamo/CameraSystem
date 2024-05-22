
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace CameraSystem {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class EventCameraSystem : UdonSharpBehaviour {
        [SerializeField] Material JackZone;
        bool cameraActive = false;

        void Start() {
        }

        private void Update() {
            if(Utilities.IsValid(Networking.LocalPlayer)) {
                if(!Networking.LocalPlayer.IsUserInVR()) {
                    if(Input.GetKeyDown(KeyCode.F10)) {
                        Debug.Log($"[OTT_CAMERA_EVENT_SYSTEM][Update] Toggling the camera output in desktop via F10");
                        ToggleCamera();
                    }
                }
            }
        }

        private void ToggleCamera() {
            cameraActive = !cameraActive;

            if (!Networking.LocalPlayer.IsUserInVR()) {
                if (cameraActive) {
                    JackZone.SetFloat("_ForceJack", 1.0f);
                    Debug.Log($"[OTT_CAMERA_EVENT_SYSTEM][ToggleCamera] _ForceJack 1");
                } else {
                    JackZone.SetFloat("_ForceJack", 0.0f);
                    Debug.Log($"[OTT_CAMERA_EVENT_SYSTEM][ToggleCamera] _ForceJack 0");
                }
            }
        }

        public void ToggleCameraButton() {
            Debug.Log($"[OTT_CAMERA_EVENT_SYSTEM][ToggleCameraButton] Toggling the camera output in desktop via button");
            ToggleCamera();
        }
    }
}
