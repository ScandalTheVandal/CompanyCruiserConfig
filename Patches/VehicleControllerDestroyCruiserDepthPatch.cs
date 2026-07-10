using CompanyCruiserConfig.Utils;
using HarmonyLib;

namespace CompanyCruiserConfig.Patches;

[HarmonyPatch(typeof(VehicleController))]
public static class VehicleControllerDestroyCruiserDepthPatch
{
    [HarmonyPatch(nameof(VehicleController.FixedUpdate))]
    [HarmonyPostfix]
    public static void FixedUpdate_Postfix(VehicleController __instance)
    {
        if (__instance.transform.position.y < CompanyCruiserConfig.despawnDepthThreshold.Value)
        {
            __instance.OnDisable();
            __instance.gameObject.AddComponent<DestroyObject>();
        }
    }
}
