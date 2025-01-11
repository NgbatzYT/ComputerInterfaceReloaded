using Photon.Pun;
using System;
using UnityEngine;

namespace ComputerInterfaceReloaded.Resources
{
    public class ButtonScript : MonoBehaviour
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
        }

        public void OnTriggerEnter(Collider other)
        {
            GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (component != null)
            {
                gameObject.GetComponent<MeshRenderer>().material = pressmat;
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                if (Plugin.states == Plugin.States.Color)
                {
                    if (int.TryParse(gameObject.name, out int numberKey))
                    {
                        switch (numberKey)
                        {
                            case 1:
                                if (Plugin.col == 1) { Plugin.r = 1f; }
                                if (Plugin.col == 2) { Plugin.g = 1f; }
                                if (Plugin.col == 3) { Plugin.b = 1f; }
                                break;
                            case 2:
                                if (Plugin.col == 1) { Plugin.r = 2f; }
                                if (Plugin.col == 2) { Plugin.g = 2f; }
                                if (Plugin.col == 3) { Plugin.b = 2f; }
                                break;
                            case 3:
                                if (Plugin.col == 1) { Plugin.r = 3f; }
                                if (Plugin.col == 2) { Plugin.g = 3f; }
                                if (Plugin.col == 3) { Plugin.b = 3f; }
                                break;
                            case 4:
                                if (Plugin.col == 1) { Plugin.r = 4f; }
                                if (Plugin.col == 2) { Plugin.g = 4f; }
                                if (Plugin.col == 3) { Plugin.b = 4f; }
                                break;
                            case 5:
                                if (Plugin.col == 1) { Plugin.r = 5f; }
                                if (Plugin.col == 2) { Plugin.g = 5f; }
                                if (Plugin.col == 3) { Plugin.b = 5f; }
                                break;
                            case 6:
                                if (Plugin.col == 1) { Plugin.r = 6f; }
                                if (Plugin.col == 2) { Plugin.g = 6f; }
                                if (Plugin.col == 3) { Plugin.b = 6f; }
                                break;
                            case 7:
                                if (Plugin.col == 1) { Plugin.r = 7f; }
                                if (Plugin.col == 2) { Plugin.g = 7f; }
                                if (Plugin.col == 3) { Plugin.b = 7f; }
                                break;
                            case 8:
                                if (Plugin.col == 1) { Plugin.r = 8f; }
                                if (Plugin.col == 2) { Plugin.g = 8f; }
                                if (Plugin.col == 3) { Plugin.b = 8f; }
                                break;
                            case 9:
                                if (Plugin.col == 1) { Plugin.r = 9f; }
                                if (Plugin.col == 2) { Plugin.g = 9f; }
                                if (Plugin.col == 3) { Plugin.b = 9f; }
                                break;
                            case 0:
                                if (Plugin.col == 1) { Plugin.r = 0f; }
                                if (Plugin.col == 2) { Plugin.g = 0f; }
                                if (Plugin.col == 3) { Plugin.b = 0f; }
                                break;
                        }
                    }
                    float redValue = Plugin.r;
                    PlayerPrefs.SetFloat("redValue", redValue);

                    float greenValue = Plugin.g;
                    PlayerPrefs.SetFloat("greenValue", greenValue);

                    float blueValue = Plugin.b;
                    PlayerPrefs.SetFloat("blueValue", blueValue);

                    GorillaTagger.Instance.UpdateColor(redValue, greenValue, blueValue);
                    PlayerPrefs.Save();
                    if (NetworkSystem.Instance.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
                        {
                            redValue,
                            greenValue,
                            blueValue
                        });
                    }
                }
                else
                {
                    if (gameObject.name.Length == 1)
                    {
                        char characterToAppend = char.ToUpper(gameObject.name[0]);
                        Plugin.instance.Type += characterToAppend;
                    }
                    else if (gameObject.name == "space")
                    {
                        Plugin.instance.Type += ' ';
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
