using GorillaNetworking;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;
using ComputerInterfaceReloaded.Resources;
using System;
using Photon.Realtime;

namespace ComputerInterfaceReloaded.Resources
{
    public class FunctionButtonScript : MonoBehaviour
    {
        public Material pressmat;
        public Material releasemat;
        public AudioSource audioSource;

        public void Start()
        {
            audioSource = GetComponent<AudioSource>();
            pressmat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            pressmat.color = Color.red;
            releasemat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            releasemat.color = Color.gray;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.layer = 18;
            gameObject.GetComponent<MeshRenderer>().material = releasemat;
            releasemat.color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }

        public void OnTriggerEnter(Collider other)
        {
            GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (component != null)
            {
                if (gameObject.name.Contains("enter"))
                {
                    gameObject.GetComponent<MeshRenderer>().material = pressmat;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                    GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);

                    if (Plugin.states == Plugin.States.Name)
                    {
                        if (!string.IsNullOrEmpty(Plugin.instance.Type))
                        {
                            if (GorillaComputer.instance.CheckAutoBanListForName(Plugin.instance.Type))
                            {
                                if (Plugin.instance.Type.Length < 10)
                                {
                                    Plugin.instance.Type = Plugin.instance.Type.Replace(" ", "");
                                    GorillaComputer.instance.currentName = Plugin.instance.Type;
                                    PhotonNetwork.LocalPlayer.NickName = Plugin.instance.Type;
                                    GorillaComputer.instance.offlineVRRigNametagText.text = Plugin.instance.Type;
                                    GorillaComputer.instance.savedName = Plugin.instance.Type;
                                    PlayerPrefs.SetString("playerName", Plugin.instance.Type);
                                    PlayerPrefs.Save();
                                }
                            }
                        }
                    }
                    else if (Plugin.states == Plugin.States.Room)
                    {
                        if (!string.IsNullOrEmpty(Plugin.instance.Type))
                        {
                            if (GorillaComputer.instance.CheckAutoBanListForName(Plugin.instance.Type))
                            {
                                if (Plugin.instance.Type.Length < 10)
                                {
                                    Plugin.instance.Type = Plugin.instance.Type.Replace(" ", "");
                                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(Plugin.instance.Type, JoinType.Solo);
                                }
                            }
                        }
                    }
                    else if (Plugin.states == Plugin.States.Support) { Plugin.showerhead = true; }
                }
                else if (gameObject.name.Contains("Backspace"))
                {
                    gameObject.GetComponent<MeshRenderer>().material = pressmat;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                    GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                    if (!string.IsNullOrEmpty(Plugin.instance.Type))
                    {
                        Plugin.instance.Type = Plugin.instance.Type.Substring(0, Plugin.instance.Type.Length - 1);
                    }
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (component != null)
            {
                gameObject.GetComponent<MeshRenderer>().material = releasemat;
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            }
        }
    }
}