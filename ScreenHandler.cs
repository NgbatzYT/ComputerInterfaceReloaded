using GorillaNetworking;
using Oculus.Platform;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TMPro;
using UnityEngine;

namespace ComputerInterfaceReloaded
{ 
    public class ScreenHandler : MonoBehaviour
    {
        public static bool initfail = false;
        public static string FailReason;
        public static bool inited = false;
        public string pttType;
        public static GorillaComputer Gpc = GorillaComputer.instance;
        public NetworkSystem netsys = NetworkSystem.Instance;
        public static string CurrentTheme;
        public bool addonpag = false;
        public static List<string> AddonNames = new List<string>();
        public static string allAddons;
        public static float r = 7f;
        public static float g = 5f;
        public static float b = 5f;
        public string Cname = PlayerPrefs.GetString("playerName", "gorilla");
        public string RoomState = "";
        public string PlayerLobb = "NOT IN ROOM";
        public static string Type = "";
        public string EGG = "Players online";
        public bool InRoom;
        public static bool showerhead = false;
        public string rs = r.ToString();
        public string gs = g.ToString();
        public string bs = b.ToString();
        public static int col = 1;
        public TMP_Text help;


        public void Start()
        {
            pttType = PlayerPrefs.GetString("pttType", "ALL CHAT");

            Gpc.currentQueue = PlayerPrefs.GetString("currentQueue", "DEFAULT");
            Gpc.allowedInCompetitive = (PlayerPrefs.GetInt("allowedInCompetitive", 0) == 1);
            if (!Gpc.allowedInCompetitive && Gpc.currentQueue == "COMPETITIVE")
            {
                PlayerPrefs.SetString("currentQueue", "DEFAULT");
                PlayerPrefs.Save();
                Gpc.currentQueue = "DEFAULT";
            }
        }
        public void Update()
        {

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

            GameObject GorillaPC = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/GorillaComputerObject/ComputerUI/keyboard (1)");
            GorillaPC.SetActive(false);

            inited = true;
            help = gameObject.GetComponent<TMP_Text>();
            if (help == null)
            {
                Plugin.Log.LogError("uwu~");
            }
            if(help != null)
            {
                UpdateScreenNew();
            }


            if (netsys.GameModeString.Contains("MODDED") && !InRoom && netsys.InRoom)
            {
                foreach (var addon in Plugin.loadedAddons)
                {
                    var instance = Activator.CreateInstance(addon);
                    addon.GetMethod("OnModdedJoin")?.Invoke(instance, null);
                }
                InRoom = true;
            }
            else if (!netsys.GameModeString.Contains("MODDED") && InRoom && netsys.InRoom)
            {
                foreach (var addon in Plugin.loadedAddons)
                {
                    var instance = Activator.CreateInstance(addon);
                    addon.GetMethod("OnModdedLeft")?.Invoke(instance, null);
                }
                InRoom = false;
            }
        }

        public void UpdateScreenNew()
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
            if (Plugin.states == Plugin.States.Room && !initfail && inited && !addonpag)
                help.text = $"This is where you can join a room. Make sure to select the gamemode you want before joining a room. \n \nCurrent room: {RoomState} \n \n{EGG}: {PlayerLobb} \n \nRoom to join: {Type}";
            else if (Plugin.states == Plugin.States.Name && !initfail && inited && !addonpag)
                help.text = $"This is where you can select your name! \nIf someone asks you to change your name DO NOT do it. \n \nCurrent name: {Cname} \n \nNew name: {Type}";
            else if (Plugin.states == Plugin.States.Color && !initfail && inited && !addonpag)
                help.text = $"This is where you select your color. \n \nRed: {r} \n \nGreen: {g} \n \nBlue: {b}";
            else if (Plugin.states == Plugin.States.Mic && !initfail && inited && !addonpag)
                help.text = $"Change your microphone settings here. \nThere is 3 mic settings: Push To Talk, Push To Mute, All Chat. Use the Option keys to select. \n \n \nOption: {pttType}";
            else if (Plugin.states == Plugin.States.Support && !initfail && inited && !showerhead && !addonpag)
                help.text = $"Press enter to show your PlayerID \nDO NOT share this with anyone besides Another Axiom support.";
            else if (Plugin.states == Plugin.States.Support && !initfail && inited && showerhead && !addonpag)
                help.text = $"PlayerID: {PlayFabAuthenticator.instance.GetPlayFabPlayerId()} \nDO NOT share this with anyone besides Another Axiom support.";
            else if (Plugin.states == Plugin.States.Queue && !initfail && inited && !addonpag)
                help.text = $"This is where you select your queue. \nUse the option keys to change your queue. There are 3 queues Default, Minigames and Competitive \n \n \nQueue: {Gpc.currentQueue}";
            else if (Plugin.states == Plugin.States.Addons && !initfail && inited && !addonpag)
                help.text = $"";
            else if (Plugin.states == Plugin.States.Addons && !initfail && inited && !addonpag)
                help.text = $"Remember you use addons at your own risk. If you want to be sure there isn't any harmful code in an addon you can use Dnspy to look at the code \n \n \nLoaded addons: {allAddons} \n \nTo go to addon settings press enter.";
            else if (!inited && !initfail)
                help.text = "LOGGING IN...";
            else if (initfail)
                help.text = $"Error Occurred: {FailReason}";
            else if (addonpag)
            {
                help.text = "Error: addon page isnt unlocked till version 1.1.0 sadly... Press back button to leave.";
            }

            if (netsys.GameModeString.Contains("MODDED") && !InRoom)
            {
                foreach (var addon in Plugin.loadedAddons)
                {
                    var instance = Activator.CreateInstance(addon);
                    addon.GetMethod("OnModdedJoin")?.Invoke(instance, null);
                }
                InRoom = true;
            }

            if (!netsys.GameModeString.Contains("MODDED") && InRoom)
            {
                foreach (var addon in Plugin.loadedAddons)
                {
                    var instance = Activator.CreateInstance(addon);
                    addon.GetMethod("OnModdedLeft")?.Invoke(instance, null);
                }
                InRoom = false;
            }
        }
    }
}
