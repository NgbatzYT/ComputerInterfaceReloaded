using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using ComputerInterfaceReloaded.Resources;
using Photon.Pun;
using GorillaNetworking;
using ComputerInterfaceReloaded.Addons;
using System;
using System.Linq;
using BepInEx.Logging;
using static BuilderMaterialOptions;

namespace ComputerInterfaceReloaded
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static AssetBundle bundle;
        public static GameObject assetBundleParent;
        public static string assetBundleName = "ci";
        public static string parentName = "CI";
        public TextMeshPro menuFadup;
        public TextMeshPro menuFaddown;
        public TextMeshPro menu;
        public TextMeshPro Input;
        public string pttType;
        public static GorillaComputer Gpc = GorillaComputer.instance;
        public NetworkSystem netsys = NetworkSystem.Instance;
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

        private static IEnumerable<Type> loadedAddons;

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
            GameObject foundObject = GameObject.Find("CI(Clone)/Monitor/TextInput");
            Input = foundObject.GetComponent<TextMeshPro>();
        }
        public void Init()
        {
            GameObject but = GameObject.Find("CI(Clone)/KeyboardStuff");
            List<GameObject> buttons = new List<GameObject>();

            foreach (Transform child in but.transform)
                buttons.Add(child.gameObject);

            foreach (GameObject button in buttons)
                if (button.gameObject.name.Length == 1)
                    button.AddComponent<ButtonScript>();

            GameObject up = GameObject.Find("CI(Clone)/KeyboardStuff/up");
            GameObject down = GameObject.Find("CI(Clone)/KeyboardStuff/down");
            GameObject left = GameObject.Find("CI(Clone)/KeyboardStuff/left");
            GameObject right = GameObject.Find("CI(Clone)/KeyboardStuff/right");
            GameObject spac = GameObject.Find("CI(Clone)/KeyboardStuff/space");
            GameObject backsp = GameObject.Find("CI(Clone)/KeyboardStuff/Backspace");
            GameObject entt = GameObject.Find("CI(Clone)/KeyboardStuff/enter");
            GameObject option1 = GameObject.Find("CI(Clone)/KeyboardStuff/option1");
            GameObject option2 = GameObject.Find("CI(Clone)/KeyboardStuff/option2");
            GameObject option3 = GameObject.Find("CI(Clone)/KeyboardStuff/option3");
            if (up == null && !initfail)
                InitCheck("KEYBOARD FAILED TO LOAD CORRECTLY");
            if (left == null && !initfail)
                InitCheck("KEYBOARD FAILED TO LOAD CORRECTLY");
            if (down == null && !initfail)
                InitCheck("KEYBOARD FAILED TO LOAD CORRECTLY");
            if (right == null && !initfail)
                InitCheck("KEYBOARD FAILED TO LOAD CORRECTLY");
            if (spac == null && !initfail)
                InitCheck("KEYBOARD FAILED TO LOAD CORRECTLY");
            up.AddComponent<ArrowButtonScript>();
            down.AddComponent<ArrowButtonScript>();
            left.AddComponent<ArrowButtonScript>();
            right.AddComponent<ArrowButtonScript>();
            spac.AddComponent<ButtonScript>();
            backsp.AddComponent<FunctionButtonScript>();
            entt.AddComponent<FunctionButtonScript>();
            option1.AddComponent<OptionButtonScript>();
            option2.AddComponent<OptionButtonScript>();
            option3.AddComponent<OptionButtonScript>();
            inited = true;

            Gpc.currentQueue = PlayerPrefs.GetString("currentQueue", "DEFAULT");
            Gpc.allowedInCompetitive = (PlayerPrefs.GetInt("allowedInCompetitive", 0) == 1);
            if (!Gpc.allowedInCompetitive && Gpc.currentQueue == "COMPETITIVE")
            {
                PlayerPrefs.SetString("currentQueue", "DEFAULT");
                PlayerPrefs.Save();
                Gpc.currentQueue = "DEFAULT";
            }
        }

        public void InitCheck(string Fail)
        {
            FailReason = Fail;
            initfail = true;
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        public static float r = 7f;
        public static float g = 5f;
        public static float b = 5f;
        public string Cname = PlayerPrefs.GetString("playerName", "gorilla");
        public string RoomState = "";
        public string PlayerLobb = "NOT IN ROOM";
        public string Type = "";
        public string adlod = "";
        public bool InRoom;
        public static bool showerhead = false;
        public string rs = r.ToString();
        public string gs = g.ToString();
        public string bs = b.ToString();
        public static int col = 1;



        public void Update()
        {
            GameObject GorillaPC = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/GorillaComputerObject");
            GameObject Text = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom");
            if (GorillaPC.activeSelf)
                GorillaPC.SetActive(false);


            TextMeshPro[] textMeshes = Text.GetComponentsInChildren<TextMeshPro>(true);

            var othertmpnames = new HashSet<string>
            { "delete", "Data", "option3", "option2", "enter", "option1", "downtext", "uptext", "FunctionSelect" };

            foreach (TextMeshPro textMesh in textMeshes)
            {
                string objectName = textMesh.gameObject.name;
                if ((objectName.Length == 1 || othertmpnames.Contains(objectName)) && textMesh.gameObject.activeSelf)
                    textMesh.gameObject.SetActive(false);
            }
            if (PhotonNetwork.InRoom)
            {
                RoomState = netsys.RoomName;
                PlayerLobb = netsys.RoomPlayerCount.ToString();
            }
            else if (!PhotonNetwork.InRoom)
            {
                RoomState = "NOT IN ROOM";
                PlayerLobb = "NOT IN ROOM";
            }

            if (states == States.Room && !initfail && inited)
                Input.text = $"This is where you can join a room. Make sure to select the gamemode you want before joining a room. \n \nCurrent room: {RoomState} \n \nPlayers in lobby: {PlayerLobb} \n \nRoom to join: {Type}";
            else if (states == States.Name && !initfail && inited)
                Input.text = $"This is where you can select your name! \nIf someone asks you to change your name DO NOT do it. \n \nCurrent name: {Cname} \n \nNew name: {Type}";
            else if (states == States.Color && !initfail && inited)
                Input.text = $"This is where you select your color. \n \nRed: {r} \n \nGreen: {g} \n \nBlue: {b}";
            else if (states == States.Mic && !initfail && inited)
                Input.text = $"Change your microphone settings here. \nThere is 3 mic settings: Push To Talk, Push To Mute, All Chat. Use the Option keys to select. \n \n \nOption: {pttType}";
            else if (states == States.Support && !initfail && inited && !showerhead)
                Input.text = $"Press enter to show your PlayerID \nDO NOT share this with anyone besides Another Axiom support.";
            else if (states == States.Support && !initfail && inited && showerhead)
                Input.text = $"PlayerID: {PlayFabAuthenticator.instance.GetPlayFabPlayerId()} \nDO NOT share this with anyone besides Another Axiom support.";
            else if (states == States.Queue && !initfail && inited)
                Input.text = $"This is where you select your queue. \nUse the option keys to change your queue. There are 3 queues Default, Minigames and Competitive \n \n \nQueue: {Gpc.currentQueue}";
            else if (states == States.Addons && !initfail && inited)
                Input.text = $"Remember you use addons at your own risk. If you want to be sure there isn't any harmful code in an addon you can use Dnspy to look at the code \n \n \nLoaded addons: {adlod} \n \nTo go to addon settings press enter";
            else if (!inited && !initfail)
                Input.text = "LOGGING IN...";
            else if (initfail)
                Input.text = $"Error Occurred: {FailReason}";

            if (netsys.GameModeString.Contains("MODDED") && !InRoom)
            {
                foreach (var addon in loadedAddons)
                {
                    var instance = Activator.CreateInstance(addon);
                    addon.GetMethod("OnModdedJoin")?.Invoke(instance, null);
                }
                InRoom = true;
            }

            if (!netsys.GameModeString.Contains("MODDED") && InRoom)
            {
                foreach (var addon in loadedAddons)
                {
                    var instance = Activator.CreateInstance(addon);
                    addon.GetMethod("OnModdedLeft")?.Invoke(instance, null);
                }
                InRoom = false;
            }
        }
    }
}