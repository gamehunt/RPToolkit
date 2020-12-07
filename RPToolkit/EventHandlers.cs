using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace RPToolkit
{
    public class EventHandlers
    {
        public void OnRoundStart()
        {
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
        }

        public void OnRoleChange(ChangingRoleEventArgs ev)
        {
            Timing.CallDelayed(0.5f, () =>
              {
                  if (Plugin.Instance.Config.WalkSpeedMultipliers.ContainsKey(ev.NewRole))
                  {
                      ev.Player.SendCustomSyncVar(ServerConfigSynchronizer.Singleton.netIdentity, typeof(ServerConfigSynchronizer), nameof(ServerConfigSynchronizer.Singleton.NetworkHumanWalkSpeedMultiplier), Plugin.Instance.Config.WalkSpeedMultipliers[ev.NewRole]);
                  }
                  if (Plugin.Instance.Config.SprintSpeedMultipliers.ContainsKey(ev.NewRole))
                  {
                      ev.Player.SendCustomSyncVar(ServerConfigSynchronizer.Singleton.netIdentity, typeof(ServerConfigSynchronizer), nameof(ServerConfigSynchronizer.Singleton.NetworkHumanSprintSpeedMultiplier), Plugin.Instance.Config.SprintSpeedMultipliers[ev.NewRole]);
                  }
                  ev.Player.PlayerInfoArea = PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.Nickname | PlayerInfoArea.PowerStatus | PlayerInfoArea.Role | PlayerInfoArea.UnitName;
                  if (Plugin.Instance.Config.HiddenPlayerInfoElements.ContainsKey(ev.NewRole))
                  {
                      foreach (PlayerInfoArea el in Plugin.Instance.Config.HiddenPlayerInfoElements[ev.NewRole])
                      {
                          ev.Player.PlayerInfoArea &= ~el;
                      }
                  }
                  if (Plugin.Instance.Config.CustomNameFormat.ContainsKey(ev.NewRole))
                  {
                      string name = Util.CreateCustomName(Plugin.Instance.Config.CustomNameFormat[ev.NewRole]);
                      if (Plugin.Instance.Config.CustomNameType == Util.CustomNameType.DisplayNickname)
                      {
                          ev.Player.DisplayNickname = name;
                      }
                      else
                      {
                          ev.Player.CustomPlayerInfo = name;
                      }

                      if (!string.IsNullOrEmpty(Plugin.Instance.Config.CustomNameBroadcast))
                      {
                          ev.Player.Broadcast(5, Plugin.Instance.Config.CustomNameBroadcast.Replace("%name%", name));
                      }
                  }
                  else
                  {
                      if (Plugin.Instance.Config.CustomNameType == Util.CustomNameType.DisplayNickname)
                      {
                          ev.Player.DisplayNickname = "";
                      }
                      else
                      {
                          ev.Player.CustomPlayerInfo = "";
                      }
                  }
                  if (Plugin.Instance.Config.RandomizePlayerSizes)
                  {
                      if (ev.NewRole == RoleType.Spectator || Plugin.Instance.Config.RandomSizeIgnoreRoles.Contains(ev.NewRole))
                      {
                          if (ev.Player.Scale != Vector3.one)
                          {
                              ev.Player.Scale = Vector3.one;
                          }
                      }
                      else
                      {
                          ev.Player.Scale = new Vector3(UnityEngine.Random.Range(Plugin.Instance.Config.RandomSizePlayerSizeMinimum[0], Plugin.Instance.Config.RandomSizePlayerSizeMaximum[0]), UnityEngine.Random.Range(Plugin.Instance.Config.RandomSizePlayerSizeMinimum[1], Plugin.Instance.Config.RandomSizePlayerSizeMaximum[1]), UnityEngine.Random.Range(Plugin.Instance.Config.RandomSizePlayerSizeMinimum[2], Plugin.Instance.Config.RandomSizePlayerSizeMaximum[2]));
                      }
                  }
              });
        }

        public void OnItemPickup(PickingUpItemEventArgs ev)
        {
            if (Plugin.Instance.Config.InventoryRestrictions.ContainsKey(ev.Player.Role))
            {
                if (ev.Player.Inventory.items.Count >= Plugin.Instance.Config.InventoryRestrictions[ev.Player.Role])
                {
                    ev.IsAllowed = false;
                    ev.Pickup.Locked = false;
                    ev.Pickup.InUse = false;
                    ev.Player.ShowHint(Plugin.Instance.Config.CustomInventoryLimitReachedHint);
                    return;
                }
            }
            if (Plugin.Instance.Config.ItemGroupsRestrictions.ContainsKey(ev.Player.Role))
            {
                foreach (string gr in Plugin.Instance.Config.ItemGroupsRestrictions[ev.Player.Role].Keys)
                {
                    if (Plugin.Instance.Config.ItemGroups[gr].Contains(ev.Pickup.ItemId))
                    {
                        int amount = 0;
                        foreach (Inventory.SyncItemInfo itm in ev.Player.Inventory.items)
                        {
                            if (Plugin.Instance.Config.ItemGroups[gr].Contains(itm.id))
                            {
                                amount++;
                                if (amount >= Plugin.Instance.Config.ItemGroupsRestrictions[ev.Player.Role][gr])
                                {
                                    ev.IsAllowed = false;
                                    ev.Pickup.Locked = false;
                                    ev.Pickup.InUse = false;
                                    ev.Player.ShowHint(Plugin.Instance.Config.CustomGroupLimitReachedHint);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}