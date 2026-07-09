using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using CompanyCruiserConfig.Patches;
using HarmonyLib;

namespace CompanyCruiserConfig;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class CompanyCruiserConfig : BaseUnityPlugin
{
    public static CompanyCruiserConfig Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    // general
    public static ConfigEntry<bool> editRigidbody = null!;
    public static ConfigEntry<bool> despawnInVoid = null!;
    public static ConfigEntry<bool> despawnInSky = null!;
    public static ConfigEntry<float> despawnDepthThreshold = null!;
    public static ConfigEntry<float> despawnHeightThreshold = null!;
    public static ConfigEntry<bool> editEnemyCollisionDamage = null!;

    // vehiclecontroller
    public static ConfigEntry<int> baseCarHP = null!; // 30
    public static ConfigEntry<float> brakeSpeed = null!; // 500
    public static ConfigEntry<float> carAcceleration = null!; // 250
    public static ConfigEntry<float> carFragility = null!; // 1
    public static ConfigEntry<float> carHitPlayerForceFraction = null!; // 30
    public static ConfigEntry<float> carMaxSpeed = null!; // 600
    public static ConfigEntry<float> carReactToPlayerHitMultiplier = null!; // 2850
    public static ConfigEntry<float> engineIntensityPercentage = null!; // 180
    public static ConfigEntry<float> engineTorque = null!; // 1100
    public static ConfigEntry<float> idleSpeed = null!; // 15
    public static ConfigEntry<float> jumpForce = null!; // 600
    public static ConfigEntry<float> maxEngineRPM = null!; // 3000
    public static ConfigEntry<float> maximumBumpForce = null!; // 75000
    public static ConfigEntry<float> mediumBumpForce = null!; // 30000
    public static ConfigEntry<float> minEngineRPM = null!; // 1000
    public static ConfigEntry<float> minimalBumpForce = null!; // 9000
    public static ConfigEntry<int> movingAverageLength = null!; // 20
    public static ConfigEntry<float> pushForceMultiplier = null!; // 27
    public static ConfigEntry<float> pushVerticalOffsetAmount = null!; // 1
    public static ConfigEntry<float> radioSignalDecreaseThreshold = null!; // 50
    public static ConfigEntry<float> radioSignalQuality = null!; // 3
    public static ConfigEntry<float> radioSignalTurbulence = null!; // 4
    public static ConfigEntry<float> speed = null!; // 50
    public static ConfigEntry<float> springForce = null!; // 130
    public static ConfigEntry<float> stability = null!; // 0.4
    public static ConfigEntry<float> steeringWheelTurnSpeed = null!; // 4
    public static ConfigEntry<float> syncRotationSpeed = null!; // 0.2
    public static ConfigEntry<float> syncSpeedMultiplier = null!; // 10
    public static ConfigEntry<float> torqueForce = null!; // 2.5
    public static ConfigEntry<float> turboBoostForce = null!; // 3000
    public static ConfigEntry<float> turboBoostUpwardForce = null!; // 7200

    // rigidbody
    public static ConfigEntry<float> angularDrag = null!;
    public static ConfigEntry<float> drag = null!;
    public static ConfigEntry<float> mass = null!;
    public static ConfigEntry<float> maxAngularVelocity = null!;
    public static ConfigEntry<float> maxDepenetrationVelocity = null!;
    public static ConfigEntry<float> maxLinearVelocity = null!;
    public static ConfigEntry<float> sleepThreshold = null!;
    public static ConfigEntry<int> solverIterations = null!;
    public static ConfigEntry<int> solverVelocityIterations = null!;

    // enemy collision damage
    public static ConfigEntry<int> smallHit = null!;
    public static ConfigEntry<int> largeHit = null!;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        InitializeConfigs();
        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal void InitializeConfigs()
    {
        // general
        editRigidbody = Config.Bind("General", "Edit Rigidbody", false, "Should configuration for the main rigidbody in VehicleController be enabled?");
        despawnInVoid = Config.Bind("General", "Despawn In Void", true, "Should any VehicleController object that goes below Despawn Depth Threshold be despawned? This check runs in FixedUpdate()");
        despawnInSky = Config.Bind("General", "Despawn In Sky", true, "Should any VehicleController object that goes above Despawn Height Threshold be despawned? This check runs in FixedUpdate()");
        despawnDepthThreshold = Config.Bind("General", "Despawn Depth Threshold", -500f, "Threshold where any VehicleController object below this height will be despawned.");
        despawnHeightThreshold = Config.Bind("Generael", "Despawn Height Threshold", 9999f, "Threshold where any VehicleController object above this height will be despawned.");
        editEnemyCollisionDamage = Config.Bind("General", "Edit Enemy Collision Damage", false, "Should configuration of enemy collision damage values be enabled?");

        // vehiclecontroller
        baseCarHP = Config.Bind("VehicleController", "Base Car HP", 30, "Initial car hp.");
        brakeSpeed = Config.Bind("VehicleController", "Brake Speed", 500f, "Vehicle braking speed.");
        carAcceleration = Config.Bind("VehicleController", "Car Acceleration", 250f, "");
        carFragility = Config.Bind("VehicleController", "Car Fragility", 1f, "");
        carHitPlayerForceFraction = Config.Bind("VehicleController", "Car Hit Player Force Fraction", 30f, "");
        carMaxSpeed = Config.Bind("VehicleController", "Car Max Speed", 600f, "");
        carReactToPlayerHitMultiplier = Config.Bind("VehicleController", "Car React To Player Hit Multiplier", 2850f, "");
        engineIntensityPercentage = Config.Bind("VehicleController", "Engine Intensity Percentage", 180f, "");
        engineTorque = Config.Bind("VehicleController", "Engine Torque", 1100f, "");
        idleSpeed = Config.Bind("VehicleController", "Idle Speed", 15f, "");
        jumpForce = Config.Bind("VehicleController", "Jump Force", 600f, "");
        maxEngineRPM = Config.Bind("VehicleController", "Max Engine RPM", 3000f, "");
        maximumBumpForce = Config.Bind("VehicleController", "Maximum Bump Force", 75000f, "");
        mediumBumpForce = Config.Bind("VehicleController", "Medium Bump Force", 30000f, "");
        minEngineRPM = Config.Bind("VehicleController", "Min Engine RPM", 1000f, "");
        minimalBumpForce = Config.Bind("VehicleController", "Minimal Bump Force", 9000f, "");
        movingAverageLength = Config.Bind("VehicleController", "Moving Average Length", 20, "");
        pushForceMultiplier = Config.Bind("VehicleController", "Push Force Multiplier", 27.0f, "Multiplier of the force to push the cruiser.");
        pushVerticalOffsetAmount = Config.Bind("VehicleController", "Push Vertical Offset Amount", 1f, "");
        radioSignalDecreaseThreshold = Config.Bind("VehicleController", "Radio Signal Decrease Threshold", 50f, "");
        radioSignalQuality = Config.Bind("VehicleController", "Radio Signal Quality", 3f, "");
        radioSignalTurbulence = Config.Bind("VehicleController", "Radio Signal Turbulence", 4f, "");
        speed = Config.Bind("VehicleController", "Speed", 50f, "");
        springForce = Config.Bind("VehicleController", "Spring Force", 130f, "");
        stability = Config.Bind("VehicleController", "Stability", 0.4f, "");
        steeringWheelTurnSpeed = Config.Bind("VehicleController", "Steering Wheel Turn Speed", 4f, "");
        syncRotationSpeed = Config.Bind("VehicleController", "Sync Rotation Speed", 0.2f, "");
        syncSpeedMultiplier = Config.Bind("VehicleController", "Sync Speed Multiplier", 10f, "");
        torqueForce = Config.Bind("VehicleController", "Torque Force", 2.5f, "");
        turboBoostForce = Config.Bind("VehicleController", "Turbo Boost Force", 3000f, "");
        turboBoostUpwardForce = Config.Bind("VehicleController", "Turbo Boost Upward Force", 7200f, "");

        // rigidbody
        angularDrag = Config.Bind("Rigidbody", "Angular Drag", 0.5f, "");
        drag = Config.Bind("Rigidbody", "Drag", 0.01f, "");
        mass = Config.Bind("Rigidbody", "Mass", 200f, "");
        maxAngularVelocity = Config.Bind("Rigidbody", "Max Angular Velocity", 4f, "");
        maxDepenetrationVelocity = Config.Bind("Rigidbody", "Max Depenetration Velocity", 1f, "");
        maxLinearVelocity = Config.Bind("Rigidbody", "Max Linear Velocity", 50f, "");
        sleepThreshold = Config.Bind("Rigidbody", "Sleep Threshold", 0.005f, "");
        solverIterations = Config.Bind("Rigidbody", "Solver Iterations", 16, "");
        solverVelocityIterations = Config.Bind("Rigidbody", "Solver Velocity Iterations", 2, "");

        // enemy collision damage
        smallHit = Config.Bind("Collision", "Small Hit", 2, "");
        largeHit = Config.Bind("Collision", "Large Hit", 12, "");
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll(typeof(VehicleControllerPatch));

        Harmony.PatchAll(typeof(RegisterNetworkPrefabPatch));

        if (despawnInVoid.Value)
        {
            Harmony.PatchAll(typeof(VehicleControllerDestroyCruiserDepthPatch));
        }
        if (despawnInSky.Value)
        {
            Harmony.PatchAll(typeof(VehicleControllerDestroyCruiserDepthPatch));
        }

        if (editEnemyCollisionDamage.Value)
        {
            Harmony.PatchAll(typeof(CarReactToObstaclePatch));
        }

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}
