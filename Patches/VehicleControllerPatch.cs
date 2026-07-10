using HarmonyLib;

namespace CompanyCruiserConfig.Patches;

[HarmonyPatch(typeof(VehicleController))]
public static class VehicleControllerPatch
{
    [HarmonyPatch(nameof(VehicleController.Start))]
    [HarmonyPrefix]
    private static void Start_Prefix(VehicleController __instance)
    {
        if (__instance.vehicleID != 0)
        {
            return;
        }
        __instance.baseCarHP = CompanyCruiserConfig.baseCarHP.Value;
    }

    [HarmonyPatch(nameof(VehicleController.Start))]
    [HarmonyPostfix]
    private static void Start_Postfix(VehicleController __instance) 
    {
        if (__instance.vehicleID != 0)
        {
            return;
        }        
        //__instance.brakeSpeed = CompanyCruiserConfig.brakeSpeed.Value; //unused
        __instance.carAcceleration = CompanyCruiserConfig.carAcceleration.Value;
        //__instance.carFragility = CompanyCruiserConfig.carFragility.Value; //unused
        //__instance.carHitPlayerForceFraction = CompanyCruiserConfig.carHitPlayerForceFraction.Value; //unused
        //__instance.carMaxSpeed = CompanyCruiserConfig.carMaxSpeed.Value; //unused
        __instance.carReactToPlayerHitMultiplier = CompanyCruiserConfig.carReactToPlayerHitMultiplier.Value;
        //__instance.engineIntensityPercentage = CompanyCruiserConfig.engineIntensityPercentage.Value; //no point changing this, this is for audio effects
        __instance.EngineTorque = CompanyCruiserConfig.engineTorque.Value;
        __instance.idleSpeed = CompanyCruiserConfig.idleSpeed.Value;
        __instance.jumpForce = CompanyCruiserConfig.jumpForce.Value;
        //__instance.MaxEngineRPM = CompanyCruiserConfig.maxEngineRPM.Value; //unused
        __instance.maximumBumpForce = CompanyCruiserConfig.maximumBumpForce.Value;
        __instance.mediumBumpForce = CompanyCruiserConfig.mediumBumpForce.Value;
        //__instance.MinEngineRPM = CompanyCruiserConfig.minEngineRPM.Value; //unused
        __instance.minimalBumpForce = CompanyCruiserConfig.minimalBumpForce.Value;
        __instance.movingAverageLength = CompanyCruiserConfig.movingAverageLength.Value;
        __instance.pushForceMultiplier = CompanyCruiserConfig.pushForceMultiplier.Value;
        __instance.pushVerticalOffsetAmount = CompanyCruiserConfig.pushVerticalOffsetAmount.Value;
        //__instance.radioSignalDecreaseThreshold = CompanyCruiserConfig.radioSignalDecreaseThreshold.Value; //gets changed everytime you tune the radio
        //__instance.radioSignalQuality = CompanyCruiserConfig.radioSignalQuality.Value; //gets changed everytime you tune the radio
        __instance.radioSignalTurbulence = CompanyCruiserConfig.radioSignalTurbulence.Value;
        __instance.speed = CompanyCruiserConfig.speed.Value;
        __instance.springForce = CompanyCruiserConfig.springForce.Value;
        __instance.stability = CompanyCruiserConfig.stability.Value;
        __instance.steeringWheelTurnSpeed = CompanyCruiserConfig.steeringWheelTurnSpeed.Value;
        __instance.syncRotationSpeed = CompanyCruiserConfig.syncRotationSpeed.Value;
        __instance.syncSpeedMultiplier = CompanyCruiserConfig.syncSpeedMultiplier.Value;
        __instance.torqueForce = CompanyCruiserConfig.torqueForce.Value;
        __instance.turboBoostForce = CompanyCruiserConfig.turboBoostForce.Value;
        __instance.turboBoostUpwardForce = CompanyCruiserConfig.turboBoostUpwardForce.Value;

        if (CompanyCruiserConfig.editRigidbody.Value)
        {
            __instance.mainRigidbody.angularDrag = CompanyCruiserConfig.angularDrag.Value;
            __instance.mainRigidbody.drag = CompanyCruiserConfig.drag.Value;
            __instance.mainRigidbody.mass = CompanyCruiserConfig.mass.Value;
            __instance.mainRigidbody.maxAngularVelocity = CompanyCruiserConfig.maxAngularVelocity.Value;
            __instance.mainRigidbody.maxDepenetrationVelocity = CompanyCruiserConfig.maxDepenetrationVelocity.Value;
            __instance.mainRigidbody.maxLinearVelocity = CompanyCruiserConfig.maxLinearVelocity.Value;
            __instance.mainRigidbody.sleepThreshold = CompanyCruiserConfig.sleepThreshold.Value;
            __instance.mainRigidbody.solverIterations = CompanyCruiserConfig.solverIterations.Value;
            __instance.mainRigidbody.solverVelocityIterations = CompanyCruiserConfig.solverVelocityIterations.Value;
        }
    }

    [HarmonyPatch(nameof(VehicleController.PushTruckWithArms))]
    [HarmonyPrefix]
    private static void PushTruckWithForcePatch(VehicleController __instance)
    {
        __instance.pushForceMultiplier = CompanyCruiserConfig.pushForceMultiplier.Value;
    }

    [HarmonyPatch(nameof(VehicleController.PushTruckClientRpc))]
    [HarmonyPrefix]
    private static void PushTruckClientRpcPatch(VehicleController __instance)
    {
        __instance.pushForceMultiplier = CompanyCruiserConfig.pushForceMultiplier.Value;
    }
}
