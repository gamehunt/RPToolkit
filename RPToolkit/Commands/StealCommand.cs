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
    class StealCommand: ICommand
    {
        public string Command => "steal";

        public string[] Aliases => new string[] { "s" };

        public string Description => "Trying to steal something from nearest non-scp player";

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
                    if (Plugin.Instance.Config.StealChance.ContainsKey(player.Role))
                    {
                        if (Plugin.Instance.Config.InventoryRestrictions.ContainsKey(player.Role))
                        {
                            if (player.Inventory.items.Count >= Plugin.Instance.Config.InventoryRestrictions[player.Role])
                            {
                                response = "Your inventory is full!";
                                return false;
                            }
                        }
                        else
                        {
                            if (player.Inventory.items.Count >= 8)
                            {
                                response = "Your inventory is full!";
                                return false;
                            }
                        }
                        if (!Cooldowned.ContainsKey(player))
                        {
                            float chance = Plugin.Instance.Config.StealChance[player.Role];
                            Player target = null;
                            float dist = float.MaxValue;
                            foreach (Player nearest in Player.List.Where(pi => pi.IsAlive && pi != player && !pi.IsScp && pi.RankName != "NPC" && pi.Inventory.items.Count > 0))
                            {
                                float cdist = Vector3.Distance(nearest.Position, player.Position);
                                if (cdist <= Plugin.Instance.Config.StealRange)
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
                                Cooldowned.Add(player, Timing.CallDelayed(Plugin.Instance.Config.StealCooldown, () => Cooldowned.Remove(player)));
                                if (Exiled.Loader.Loader.Random.NextDouble() <= Plugin.Instance.Config.StealChance[player.Role])
                                {
                                    Inventory.SyncItemInfo item = target.Inventory.items[Exiled.Loader.Loader.Random.Next(0, target.Inventory.items.Count)];
                                    target.RemoveItem(item);
                                    player.Inventory.AddNewItem(item.id, item.durability, item.modSight, item.modBarrel, item.modOther);
                                    player.Broadcast(5, Plugin.Instance.Config.StealerBroadcastSuccess.Replace("%name%", Util.GetNickname(target)));
                                    target.Broadcast(5, Plugin.Instance.Config.StealVictimBroadcastSuccess.Replace("%name%", Util.GetNickname(player)));
                                    response = $"Succesfully stolen things of {target}!";
                                    return true;
                                }
                                else
                                {
                                    player.Broadcast(5, Plugin.Instance.Config.StealerBroadcastFail.Replace("%name%", Util.GetNickname(target)));
                                    target.Broadcast(5, Plugin.Instance.Config.StealVictimBroadcastFail.Replace("%name%", Util.GetNickname(player)));
                                    response = "Steal failed!";
                                    return false;
                                }
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
