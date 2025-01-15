using Photon.Pun;
using UnityEngine;

namespace ComputerInterfaceReloaded.Resources
{
    public class ButtonScript : MonoBehaviour
    {
        public Material pressmat;
        public Material releasemat;
        public AudioSource audioSource;
        public ScreenHandler screenHandler = GameObject.Find("CI(Clone)/help").GetComponent<ScreenHandler>();

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
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 1f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 1f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 1f; }
                                break;
                            case 2:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 2f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 2f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 2f; }
                                break;
                            case 3:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 3f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 3f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 3f; }
                                break;
                            case 4:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 4f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 4f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 4f; }
                                break;
                            case 5:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 5f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 5f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 5f; }
                                break;
                            case 6:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 6f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 6f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 6f; }
                                break;
                            case 7:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 7f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 7f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 7f; }
                                break;
                            case 8:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 8f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 8f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 8f; }
                                break;
                            case 9:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 9f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 9f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 9f; }
                                break;
                            case 0:
                                if (ScreenHandler.col == 1) { ScreenHandler.r = 0f; }
                                if (ScreenHandler.col == 2) { ScreenHandler.g = 0f; }
                                if (ScreenHandler.col == 3) { ScreenHandler.b = 0f; }
                                break;
                        }
                    }
                    float redValue = ScreenHandler.r;
                    PlayerPrefs.SetFloat("redValue", redValue);

                    float greenValue = ScreenHandler.g;
                    PlayerPrefs.SetFloat("greenValue", greenValue);

                    float blueValue = ScreenHandler.b;
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
                    if (Plugin.states == Plugin.States.Room)
                    {
                        if (ScreenHandler.Type.Length != 12)
                        {
                            if (gameObject.name.Length == 1)
                            {
                                char characterToAppend = char.ToUpper(gameObject.name[0]);
                                ScreenHandler.Type += characterToAppend;
                            }
                            else if (gameObject.name == "space")
                            {
                                ScreenHandler.Type += ' ';
                            }
                        }
                    }
                    else if(Plugin.states == Plugin.States.Name)
                    {
                        if (ScreenHandler.Type.Length != 12)
                        {
                            if (gameObject.name.Length == 1)
                            {
                                char characterToAppend = char.ToUpper(gameObject.name[0]);
                                ScreenHandler.Type += characterToAppend;
                            }
                            else if (gameObject.name == "space")
                            {
                                ScreenHandler.Type += ' ';
                            }
                        }
                    }
                    else
                    {
                        if (gameObject.name.Length == 1)
                        {
                            char characterToAppend = char.ToUpper(gameObject.name[0]);
                            ScreenHandler.Type += characterToAppend;
                        }
                        else if (gameObject.name == "space")
                        {
                            ScreenHandler.Type += ' ';
                        }
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
