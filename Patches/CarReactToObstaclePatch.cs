using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace CompanyCruiserConfig.Patches;

[HarmonyPatch(typeof(VehicleController))]
internal class CarReactToObstaclePatch
{
    [HarmonyPatch(nameof(VehicleController.CarReactToObstacle))]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> CarReactToObstacleTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        CompanyCruiserConfig.Logger.LogDebug("Running CarReactToObstacleTranspiler to patch damage values.");
        int index = -1;
        int startvalue = 150;

        var codes = new List<CodeInstruction>(instructions);

        for (int i = startvalue; i < codes.Count; i++)
        {
            if (codes[i-2].opcode == OpCodes.Ldarg_S && codes[i-1].opcode == OpCodes.Ldc_I4_2 && codes[i].opcode == OpCodes.Ldarg_0)
            {
                index = i - 1;
                startvalue = index;
                break;
            }
        }
        if (index > -1)
        {
            codes[index].opcode = OpCodes.Ldc_I4_S;
            codes[index].operand = (sbyte)CompanyCruiserConfig.smallHit.Value;
            CompanyCruiserConfig.Logger.LogDebug($"opcode at index {index} replaced");
        }
        else
        {
            CompanyCruiserConfig.Logger.LogError("CarReactToObstacleTranspiler failed on first inject.");
        }
        index = -1;
        for (int i = startvalue; i < codes.Count; i++)
        {
            if (codes[i-2].opcode == OpCodes.Ldarg_S && codes[i-1].opcode == OpCodes.Ldc_I4_S && codes[i].opcode == OpCodes.Ldloc_3)
            {
                index = i - 1;
                break;
            }
        }
        if (index > -1)
        {
            codes[index].operand = (sbyte)CompanyCruiserConfig.largeHit.Value;
            CompanyCruiserConfig.Logger.LogDebug($"opcode at index {index} replaced");
        }
        else
        {
            CompanyCruiserConfig.Logger.LogError("CarReactToObstacleTranspiler failed on second inject.");
        }
        return codes.AsEnumerable();
    }
}
