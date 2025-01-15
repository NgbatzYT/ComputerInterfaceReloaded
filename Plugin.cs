using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using ComputerInterfaceReloaded.Resources;
using GorillaNetworking;
using ComputerInterfaceReloaded.Addons;
using System;
using System.Linq;
using BepInEx.Logging;

namespace ComputerInterfaceReloaded
{
    [BepInIncompatibility("org.iidk.gorillatag.iimenu")]
    [BepInIncompatibility("dev.gorillacomputer")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static AssetBundle bundle;
        public static GameObject assetBundleParent;
        public static string assetBundleName = "ci";
        public static string parentName = "CI";
        public TMP_Text help;
        public string pttType;
        public static GorillaComputer Gpc = GorillaComputer.instance;
        public NetworkSystem netsys = NetworkSystem.Instance;
        public bool addonpag = false;
        public static List<string> AddonNames = new List<string>();
        public static string allAddons;
        //public string turnType;

        public static bool initfail = false;
        public static string FailReason;
        public static bool inited = false;

        public enum States
        {
            Room,
            Name,
            Color,
            Mic,
            //Turn,
            Support,
            Queue,
            //Theme,
            Addons
        }

        public static States states = States.Room;
        internal static ManualLogSource Log;

        void Start()
        {
            pttType = PlayerPrefs.GetString("pttType", "ALL CHAT");
            //turnType = PlayerPrefs.GetString("StickTurning", "NONE");
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
            GorillaTagger.OnPlayerSpawned(Init);
            GorillaTagger.OnPlayerSpawned(LoadAddons);
            Log = base.Logger;
        }

        // rainwave turned evil help pls

        public static IEnumerable<Type> loadedAddons;

        public static void LoadAddons()
        {
            var addonType = typeof(CIReloadedAttribute);

            loadedAddons = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetCustomAttributes(addonType, false).Any());

            foreach (var addon in loadedAddons)
            {
                var attribute = (CIReloadedAttribute)addon
                    .GetCustomAttributes(addonType, false)
                    .First();

                Console.WriteLine($"Loading addon: {attribute.Name} V{attribute.Version}");

                AddonNames.Add(attribute.Name);
                allAddons = string.Join(", ", AddonNames);

                var instance = Activator.CreateInstance(addon);
                addon.GetMethod("Init")?.Invoke(instance, null);
            }
        }

        void OnGameInitialized()
        {
            instance = this;
            bundle = LoadAssetBundle("ComputerInterfaceReloaded.Resources." + assetBundleName);
            assetBundleParent = Instantiate(bundle.LoadAsset<GameObject>(parentName));
            assetBundleParent.transform.position = new Vector3(-67.2225f, 11.53f, -82.611f);
            
        }
        public void Init()
        {
            GameObject but = GameObject.Find("CI(Clone)/keyboardstuff");
            List<GameObject> buttons = new List<GameObject>();

            foreach (Transform child in but.transform)
                buttons.Add(child.gameObject);

            foreach (GameObject button in buttons)
                if (button.gameObject.name.Length == 1)
                    if(button.GetComponent<ButtonScript>() == null)
                        button.AddComponent<ButtonScript>();

            GameObject up = GameObject.Find("CI(Clone)/keyboardstuff/up");
            GameObject down = GameObject.Find("CI(Clone)/keyboardstuff/down");
            GameObject left = GameObject.Find("CI(Clone)/keyboardstuff/left");
            GameObject right = GameObject.Find("CI(Clone)/keyboardstuff/right");
            GameObject spac = GameObject.Find("CI(Clone)/keyboardstuff/space");
            GameObject backsp = GameObject.Find("CI(Clone)/keyboardstuff/backspace");
            GameObject entt = GameObject.Find("CI(Clone)/keyboardstuff/enter");
            GameObject eba = GameObject.Find("CI(Clone)/keyboardstuff/back");
            GameObject option1 = GameObject.Find("CI(Clone)/keyboardstuff/option1");
            GameObject option2 = GameObject.Find("CI(Clone)/keyboardstuff/option2");
            GameObject option3 = GameObject.Find("CI(Clone)/keyboardstuff/option3");
            GameObject tex = GameObject.Find("CI(Clone)/help");
            tex?.AddComponent<ScreenHandler>();

            /*Logger.Log($"InputText = {help.name}");

            if (help == null)
            {
                TextMeshPro[] helpe = { assetBundleParent.GetComponentInChildren<TextMeshPro>() };

                foreach (TextMeshPro text in helpe)
                    if(text.gameObject.name.Equals("help"))
                        if(helpe != null)
                            help = text;
            }*/
            if (up == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (left == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (down == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (right == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (spac == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (backsp == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (entt == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (eba == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (option1 == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (option2 == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            if (option3 == null && !initfail)
                InitCheck("Keyboard failed to load correctly");
            up.AddComponent<ArrowButtonScript>();
            down.AddComponent<ArrowButtonScript>();
            left.AddComponent<ArrowButtonScript>();
            right.AddComponent<ArrowButtonScript>();
            spac.AddComponent<ButtonScript>();
            backsp.AddComponent<FunctionButtonScript>();
            entt.AddComponent<FunctionButtonScript>();
            eba.AddComponent<FunctionButtonScript>();
            option1.AddComponent<OptionButtonScript>();
            option2.AddComponent<OptionButtonScript>();
            option3.AddComponent<OptionButtonScript>();
            if (!initfail)
            {
                inited = true;
                initfail = false;
            }   
            
            GameObject Text = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom");

            TextMeshPro[] textMeshes = Text.GetComponentsInChildren<TextMeshPro>(true);

            var othertmpnames = new HashSet<string>
            { "delete", "Data", "option3", "option2", "enter", "option1", "downtext", "uptext", "FunctionSelect" };

            foreach (TextMeshPro textMesh in textMeshes)
            {
                string objectName = textMesh.gameObject.name;
                if ((objectName.Length == 1 || othertmpnames.Contains(objectName)) && textMesh.gameObject.activeSelf)
                    textMesh.gameObject.SetActive(false);
            }
        }

        public void InitCheck(string Fail)
        {
            FailReason = Fail;
            initfail = true;
            Init();
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        /*public static float r = 7f;
        public static float g = 5f;
        public static float b = 5f;
        public string Cname = PlayerPrefs.GetString("playerName", "gorilla");
        public string RoomState = "";
        public string PlayerLobb = "NOT IN ROOM";
        public string Type = "";
        public string EGG = "Players online";
        public bool InRoom;
        public static bool showerhead = false;
        public string rs = r.ToString();
        public string gs = g.ToString();
        public string bs = b.ToString();
        public static int col = 1;

        /*public void Update()
        {
            if(help != null)
            {
                //TextUpdateOrSmth();
            }

            if(help == null)
            {
                help = GameObject.Find("CI(Clone)/help").GetComponent<TextMeshPro>();
            }
        }

        /*private void TextUpdateOrSmth()
        {
            if (netsys.InRoom)
            {
                RoomState = netsys.RoomName;
                PlayerLobb = netsys.RoomPlayerCount.ToString();
                EGG = "Players online";
            }
            else if (!netsys.InRoom)
            {
                RoomState = "NOT IN ROOM";
                PlayerLobb = netsys.GlobalPlayerCount().ToString();
                EGG = "Players online";
            }
            if (!addonpag)
            {
                if (states == States.Room && !initfail && inited)
                    help.text = $"This is where you can join a room. Make sure to select the gamemode you want before joining a room. \n \nCurrent room: {RoomState} \n \n{EGG}: {PlayerLobb} \n \nRoom to join: {Type}";
                else if (states == States.Name && !initfail && inited)
                    help.text = $"This is where you can select your name! \nIf someone asks you to change your name DO NOT do it. \n \nCurrent name: {Cname} \n \nNew name: {Type}";
                else if (states == States.Color && !initfail && inited)
                    help.text = $"This is where you select your color. \n \nRed: {r} \n \nGreen: {g} \n \nBlue: {b}";
                else if (states == States.Mic && !initfail && inited)
                    help.text = $"Change your microphone settings here. \nThere is 3 mic settings: Push To Talk, Push To Mute, All Chat. Use the Option keys to select. \n \n \nOption: {pttType}";
                else if (states == States.Support && !initfail && inited && !showerhead)
                    help.text = $"Press enter to show your PlayerID \nDO NOT share this with anyone besides Another Axiom support.";
                else if (states == States.Support && !initfail && inited && showerhead)
                    help.text = $"PlayerID: {PlayFabAuthenticator.instance.GetPlayFabPlayerId()} \nDO NOT share this with anyone besides Another Axiom support.";
                else if (states == States.Queue && !initfail && inited)
                    help.text = $"This is where you select your queue. \nUse the option keys to change your queue. There are 3 queues Default, Minigames and Competitive \n \n \nQueue: {Gpc.currentQueue}";
                else if (states == States.Addons && !initfail && inited)
                    help.text = $"Remember you use addons at your own risk. If you want to be sure there isn't any harmful code in an addon you can use Dnspy to look at the code \n \n \nLoaded addons: {allAddons} \n \nTo go to addon settings press enter.";
                else if (!inited && !initfail)
                    help.text = "LOGGING IN...";
                else if (initfail)
                    help.text = $"Error Occurred: {FailReason}";
            }
            else if (addonpag)
            {
                addonpag = false;
                help.text = "Error: addon page isnt unlocked till version 1.1.0 sadly... Press back button to leave.";
            }
        }*/
    }
}