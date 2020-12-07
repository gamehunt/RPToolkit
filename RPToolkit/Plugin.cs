using Exiled.API.Features;
using HarmonyLib;
using System;
using System.IO;
using Handlers = Exiled.Events.Handlers;

namespace RPToolkit
{
    public class Plugin : Plugin<Config>
    {
        public EventHandlers EventHandlers;

        public static Plugin Instance { get; private set; }
        public static HarmonyLib.Harmony Harmony { get; private set; }

        public override string Author { get; } = "gamehunt";
        public override string Name { get; } = "RP Toolkit";
        public override string Prefix { get; } = "RPTK";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 7);

        public override void OnEnabled()
        {
            try
            {
                Instance = this;

                Harmony = new HarmonyLib.Harmony("rptoolkit.instance");

                Harmony.PatchAll();

                EventHandlers = new EventHandlers();

                if (!Directory.Exists(Config.RPToolkitRootPath))
                {
                    Directory.CreateDirectory(Config.RPToolkitRootPath);
                }
                
                if (!File.Exists(Config.NamesPath))
                {
                    StreamWriter sw = File.CreateText(Config.NamesPath);
                    sw.Write(Config.Names);
                    sw.Close();
                }
                if (!File.Exists(Config.SurnamesPath))
                {
                    StreamWriter sw = File.CreateText(Config.SurnamesPath);
                    sw.Write(Config.Surnames);
                    sw.Close();
                }

                Util.Init();

                Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
                Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;

                Handlers.Player.ChangingRole += EventHandlers.OnRoleChange;
                Handlers.Player.PickingUpItem += EventHandlers.OnItemPickup;

                Log.Info($"RPToolkit plugin loaded. @gamehunt");
            }
            catch (Exception e)
            {
                Log.Error($"There was an error loading the plugin: {e}");
            }
        }

        public override void OnDisabled()
        {

            Harmony.UnpatchAll();

            Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;

            Handlers.Player.ChangingRole -= EventHandlers.OnRoleChange;
            Handlers.Player.PickingUpItem -= EventHandlers.OnItemPickup;

            Instance = null;
            Harmony = null;
            EventHandlers = null;
        }

        public override void OnReloaded()
        {
        }
    }
}