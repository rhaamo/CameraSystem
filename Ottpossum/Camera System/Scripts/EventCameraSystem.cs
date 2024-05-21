
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace CameraSystem {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class EventCameraSystem : UdonSharpBehaviour {
        [SerializeField] Material JackZone;
        [SerializeField] Color activeColor;
        [SerializeField] Color disactiveColor;
        bool cameraActive = false;

        void Start() {
        }

        private void Update() {
            if(Utilities.IsValid(Networking.LocalPlayer)) {
                if(!Networking.LocalPlayer.IsUserInVR()) {
                    if(Input.GetKeyDown(KeyCode.F10)) {
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
                } else {
                    JackZone.SetFloat("_ForceJack", 0.0f);
                }
            }
        }

        public void ToggleCameraButton() {
            ToggleCamera();
        }
    }
}
