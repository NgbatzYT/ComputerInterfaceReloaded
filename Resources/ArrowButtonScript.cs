﻿using System;
using UnityEngine;
using TMPro;

namespace ComputerInterfaceReloaded.Resources
{
    public class ArrowButtonScript : MonoBehaviour
    {
        public Material pressmat;
        public Material releasemat;
        public AudioSource audioSource;
        public TextMeshProUGUI iforget = GameObject.Find("CI(Clone)/NT/CurrentTab").GetComponent<TextMeshProUGUI>();
        public TextMeshProUGUI fadeddown = GameObject.Find("CI(Clone)/NT/FadedDown").GetComponent<TextMeshProUGUI>();
        public TextMeshProUGUI dedeup = GameObject.Find("CI(Clone)/NT/FadedUp").GetComponent<TextMeshProUGUI>();
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
            releasemat.color = new Color(171f / 255f, 219f / 255f, 171f / 255f);
            UpdateText();
        }

        public void OnTriggerEnter(Collider other)
        {
            GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (component != null)
            {
                if (gameObject.name == "up")
                {
                    gameObject.GetComponent<MeshRenderer>().material = pressmat;
                    Plugin.states = GetNextState();
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                    GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                    ScreenHandler.Type = "";
                    ScreenHandler.showerhead = false;
                    UpdateText();
                }
                else if (gameObject.name == "down")
                {
                    gameObject.GetComponent<MeshRenderer>().material = pressmat;
                    Plugin.states = GetPreviousState();
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                    GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                    ScreenHandler.Type = "";
                    ScreenHandler.showerhead = false;
                    UpdateText();
                }
                
            }
        }

        public void OnTriggerExit(Collider other)
        {
            GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (component != null)
            {
                gameObject.GetComponent<MeshRenderer>().material = releasemat;
            }
        }

        private void UpdateText()
        {
            var allStates = Enum.GetValues(typeof(Plugin.States));
            int totalStates = allStates.Length;
            int currentIndex = Array.IndexOf(allStates, Plugin.states);
            int nextIndex = (currentIndex + 1) % totalStates;
            int previousIndex = (currentIndex - 1 + totalStates) % totalStates;
            iforget.text = $"{Plugin.states}";
            dedeup.text = $"{allStates.GetValue(nextIndex)}";
            fadeddown.text = $"{allStates.GetValue(previousIndex)}";
        }

        private Plugin.States GetNextState()
        {
            var allStates = Enum.GetValues(typeof(Plugin.States));
            int totalStates = allStates.Length;
            int currentIndex = Array.IndexOf(allStates, Plugin.states);
            int nextIndex = (currentIndex + 1) % totalStates;
            return (Plugin.States)allStates.GetValue(nextIndex);
        }

        private Plugin.States GetPreviousState()
        {
            var allStates = Enum.GetValues(typeof(Plugin.States));
            int totalStates = allStates.Length;
            int currentIndex = Array.IndexOf(allStates, Plugin.states);
            int previousIndex = (currentIndex - 1 + totalStates) % totalStates;
            return (Plugin.States)allStates.GetValue(previousIndex);
        }
    }
}
