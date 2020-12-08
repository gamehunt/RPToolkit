using HarmonyLib;

namespace RPToolkit.Harmony
{
    [HarmonyPatch(typeof(FirstPersonController), nameof(FirstPersonController.GetSpeed))]
    internal class CustomSpeedFix
    {
        private static void Postfix(FirstPersonController __instance, ref float speed, bool isServerSide)
        {
            if (isServerSide)
            {
                speed *= Util.GetFinalSpeedMultiplier(__instance.hub, __instance.staminaController.AllowMaxSpeed) / (__instance.staminaController.AllowMaxSpeed ? ServerConfigSynchronizer.Singleton.HumanSprintSpeedMultiplier : ServerConfigSynchronizer.Singleton.HumanWalkSpeedMultiplier);
            }
        }
    }
}