using GorillaNetworking;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

namespace ComputerInterfaceReloaded.Resources
{
    public class FunctionButtonScript : MonoBehaviour
    {
        public Material pressmat;
        public Material releasemat;
        public AudioSource audioSource;
        public ScreenHandler screenHandler = GameObject.Find("CI(clone)/help").GetComponent<ScreenHandler>();

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
            Material special = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            special.color = new Color(111f / 255f, 207f / 255f, 243f / 255f);
        }

        public void OnTriggerEnter(Collider other)
        {
            GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (component != null)
            {
                gameObject.GetComponent<MeshRenderer>().material = pressmat;
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                
                if (gameObject.name.Contains("enter"))
                {
                    

                    if (Plugin.states == Plugin.States.Name)
                    {
                        if (!string.IsNullOrEmpty(screenHandler.Type))
                        {
                            if (GorillaComputer.instance.CheckAutoBanListForName(screenHandler.Type))
                            {
                                if (screenHandler.Type.Length < 10)
                                {
                                    screenHandler.Type = screenHandler.Type.Replace(" ", "");
                                    GorillaComputer.instance.currentName = screenHandler.Type;
                                    PhotonNetwork.LocalPlayer.NickName = screenHandler.Type;
                                    GorillaComputer.instance.offlineVRRigNametagText.text = screenHandler.Type;
                                    GorillaComputer.instance.savedName = screenHandler.Type;
                                    PlayerPrefs.SetString("playerName", screenHandler.Type);
                                    PlayerPrefs.Save();
                                }
                            }
                        }
                    }
                    else if (Plugin.states == Plugin.States.Room)
                    {
                        if (!string.IsNullOrEmpty(screenHandler.Type))
                        {
                            if (GorillaComputer.instance.CheckAutoBanListForName(screenHandler.Type))
                            {
                                if (screenHandler.Type.Length < 10)
                                {
                                    screenHandler.Type = screenHandler.Type.Replace(" ", "");
                                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(screenHandler.Type, JoinType.Solo);
                                }
                            }
                        }
                    }
                    else if (Plugin.states == Plugin.States.Support) { ScreenHandler.showerhead = true; }
                    else if (Plugin.states == Plugin.States.Addons) { screenHandler.addonpag = true; }
                }
                else if (gameObject.name.Contains("backspace"))
                {
                    
                    if (!string.IsNullOrEmpty(screenHandler.Type))
                    {
                        screenHandler.Type = screenHandler.Type.Substring(0, screenHandler.Type.Length - 1);
                    }
                }
                else if(gameObject.name.Equals("back"))
                {
                    if(screenHandler.addonpag)
                    {
                        screenHandler.addonpag = false;
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