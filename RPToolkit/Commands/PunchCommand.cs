using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPToolkit.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class PunchCommand : ICommand
    {
        public string Command => "punch";

        public string[] Aliases => new string[] { "p" };

        public string Description => "Punches nearest non-scp player";

        public static Dictionary<Player, CoroutineHandle> Cooldowned { get; } = new Dictionary<Player, CoroutineHandle>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is RemoteAdmin.PlayerCommandSender p)
            {
                if (!Round.IsStarted)
                {
                    response = "You can't use this yet!";
                    return false;
                }
                Player player = Player.Get(p.PlayerId);
                if (player.IsHuman)
                {
                    if (Plugin.Instance.Config.PunchDamage.ContainsKey(player.Role))
                    {
                        if (!Cooldowned.ContainsKey(player))
                        {
                            float damage = Plugin.Instance.Config.PunchDamage[player.Role];
                            Player target = null;
                            float dist = float.MaxValue;
                            foreach (Player nearest in Player.List.Where(pi => pi.IsAlive && pi != player && !pi.IsScp && pi.RankName != "NPC"))
                            {
                                float cdist = Vector3.Distance(nearest.Position, player.Position);
                                if (cdist <= Plugin.Instance.Config.PunchRange)
                                {
                                    if (cdist < dist)
                                    {
                                        dist = cdist;
                                        target = nearest;
                                    }
                                }
                            }
                            if (target != null)
                            {
                                target.Hurt(damage, player, DamageTypes.Falldown);
                                Cooldowned.Add(player, Timing.CallDelayed(Plugin.Instance.Config.PunchCooldown, () => Cooldowned.Remove(player)));
                                response = $"Punched {Util.GetNickname(target)}";
                                player.ReferenceHub.falldamage.RpcDoSound();
                                return true;
                            }
                            else
                            {
                                response = "No target found!";
                                return false;
                            }
                        }
                        else
                        {
                            response = "You can't use this yet!";
                            return false;
                        }
                    }
                }
            }
            response = "You can't use this!";
            return false;
        }
    }
}