using GorillaNetworking;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ComputerInterfaceReloaded.Resources
{
    public class OptionButtonScript : MonoBehaviour
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
            releasemat.color = new Color(111f / 255f, 207f / 255f, 243f / 255f);
        }
        public void OnTriggerEnter(Collider other)
        {
            GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (!(component == null))
            {
                gameObject.GetComponent<MeshRenderer>().material = pressmat;
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                if(gameObject.name.Contains("1"))
                {
                    if (Plugin.states == Plugin.States.Room)
                        NetworkSystem.Instance.ReturnToSinglePlayer();
                    else if (Plugin.states == Plugin.States.Mic)
                    {
                        Plugin.instance.pttType = "ALL CHAT";
                        PlayerPrefs.SetString("pttType", Plugin.instance.pttType);
                        PlayerPrefs.Save();
                    }
                    else if (Plugin.states == Plugin.States.Queue)
                    {
                        Plugin.Gpc.currentQueue = "DEFAULT";
                        PlayerPrefs.SetString("currentQueue", Plugin.Gpc.currentQueue);
                        PlayerPrefs.Save();
                    }
                    else if (Plugin.states == Plugin.States.Color) { Plugin.col = 1; }
                }
                else if (gameObject.name.Contains("2"))
                {
                    if (Plugin.states == Plugin.States.Mic)
                    {
                        Plugin.instance.pttType = "PUSH TO TALK";
                        PlayerPrefs.SetString("pttType", Plugin.instance.pttType);
                        PlayerPrefs.Save();
                    }
                    else if (Plugin.states == Plugin.States.Queue)
                    {
                        Plugin.Gpc.currentQueue = "MINIGAMES";
                        PlayerPrefs.SetString("currentQueue", Plugin.Gpc.currentQueue);
                        PlayerPrefs.Save();
                    }
                    else if (Plugin.states == Plugin.States.Color)
                    {
                        Plugin.col = 3;
                    }
                }
                else if (gameObject.name.Contains("3"))
                {
                    if (Plugin.states == Plugin.States.Mic)
                    {
                        Plugin.instance.pttType = "PUSH TO MUTE";
                        PlayerPrefs.SetString("pttType", Plugin.instance.pttType);
                        PlayerPrefs.Save();
                    }
                    else if (Plugin.states == Plugin.States.Queue)
                    {
                        if (Plugin.Gpc.allowedInCompetitive)
                        {
                            Plugin.Gpc.currentQueue = "COMPETITIVE";
                            PlayerPrefs.SetString("currentQueue", Plugin.Gpc.currentQueue);
                            PlayerPrefs.Save();
                        }
                    }
                    else if (Plugin.states == Plugin.States.Color)
                    {
                        Plugin.col = 3;
                    }
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (!(component == null))
            {
                gameObject.GetComponent<MeshRenderer>().material = releasemat;
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            }
      
        }
    }
}
