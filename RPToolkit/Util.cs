using Exiled.API.Features;
using System.Collections.Generic;
using System.IO;

namespace RPToolkit
{
    public class Util
    {
        public enum CustomNameType
        {
            PlayerInfo,
            DisplayNickname,
        }

        private static readonly List<string> names = new List<string>();
        private static readonly List<string> snames = new List<string>();

        public static void Init()
        {
            foreach (string line in File.ReadAllLines(Config.NamesPath))
            {
                names.Add(line);
            }
            foreach (string line in File.ReadAllLines(Config.SurnamesPath))
            {
                snames.Add(line);
            }
        }

        public static string CreateCustomName(string format)
        {
            string old_format = "";
            while (old_format != format)
            {
                old_format = format;
                format = format.ReplaceFirst("%random_name%", names[Exiled.Loader.Loader.Random.Next(0, names.Count)]).ReplaceFirst("%random_surname%", snames[Exiled.Loader.Loader.Random.Next(0, snames.Count)]).ReplaceFirst("%d%", Exiled.Loader.Loader.Random.Next(0, 10).ToString());
                string rand = "";
                rand += (char)('a' + Exiled.Loader.Loader.Random.Next(0, 26));
                format = format.ReplaceFirst("%l%", rand);
                rand = "";
                rand += (char)('a' + Exiled.Loader.Loader.Random.Next(0, 26));
                format = format.ReplaceFirst("%lu%", rand.ToUpper());
            }
            return format;
        }

        public static float GetFinalSpeedMultiplier(ReferenceHub rhub)
        {
            Player player = Player.Get(rhub);
            float mul = 1f;
            if (!player.IsScp)
            {
                if (!player.Stamina.AllowMaxSpeed)
                {
                    if (Plugin.Instance.Config.WalkSpeedMultipliers.ContainsKey(player.Role))
                    {
                        mul *= Plugin.Instance.Config.WalkSpeedMultipliers[player.Role];
                    }
                    if (Plugin.Instance.Config.ItemsWalkSpeedMultipliers.ContainsKey(player.Role))
                    {
                        if (Plugin.Instance.Config.ApplyOnlyIfItemInHand)
                        {
                            if (Plugin.Instance.Config.ItemsWalkSpeedMultipliers[player.Role].ContainsKey(player.CurrentItem.id))
                            {
                                mul *= Plugin.Instance.Config.ItemsWalkSpeedMultipliers[player.Role][player.CurrentItem.id];
                            }
                        }
                        else
                        {
                            float addy_mul = float.PositiveInfinity;
                            foreach (ItemType item in Plugin.Instance.Config.ItemsWalkSpeedMultipliers[player.Role].Keys)
                            {
                                if (player.Inventory.items.FindIndex(i => i.id == item) != -1)
                                {
                                    if (Plugin.Instance.Config.ItemsWalkSpeedMultipliers[player.Role][item] < addy_mul)
                                    {
                                        addy_mul = Plugin.Instance.Config.ItemsWalkSpeedMultipliers[player.Role][item];
                                    }
                                }
                            }
                            if (addy_mul != float.PositiveInfinity)
                            {
                                mul *= addy_mul;
                            }
                        }
                    }
                }
                else
                {
                    if (Plugin.Instance.Config.SprintSpeedMultipliers.ContainsKey(player.Role))
                    {
                        mul *= Plugin.Instance.Config.SprintSpeedMultipliers[player.Role];
                    }
                    if (Plugin.Instance.Config.ItemsSprintSpeedMultipliers.ContainsKey(player.Role))
                    {
                        if (Plugin.Instance.Config.ApplyOnlyIfItemInHand)
                        {
                            if (Plugin.Instance.Config.ItemsSprintSpeedMultipliers[player.Role].ContainsKey(player.CurrentItem.id))
                            {
                                mul *= Plugin.Instance.Config.ItemsSprintSpeedMultipliers[player.Role][player.CurrentItem.id];
                            }
                        }
                        else
                        {
                            float addy_mul = float.PositiveInfinity;
                            foreach (ItemType item in Plugin.Instance.Config.ItemsSprintSpeedMultipliers[player.Role].Keys)
                            {
                                if (player.Inventory.items.FindIndex(i => i.id == item) != -1)
                                {
                                    if (Plugin.Instance.Config.ItemsSprintSpeedMultipliers[player.Role][item] < addy_mul)
                                    {
                                        addy_mul = Plugin.Instance.Config.ItemsSprintSpeedMultipliers[player.Role][item];
                                    }
                                }
                            }
                            if (addy_mul != float.PositiveInfinity)
                            {
                                mul *= addy_mul;
                            }
                        }
                    }
                }
            }
            return mul;
        }
    }
}