using CompanyCruiserConfig;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace CompanyCruiserConfig.Patches;

[HarmonyPatch(typeof(NetworkManager))]
public static class RegisterNetworkPrefabPatch
{
    [HarmonyPatch(nameof(NetworkManager.SetSingleton))]
    [HarmonyPostfix]
    public static void RegisterPrefab()
    {
        var prefab = new GameObject(MyPluginInfo.PLUGIN_GUID + " Prefab");
        prefab.hideFlags |= HideFlags.HideAndDontSave;
        Object.DontDestroyOnLoad(prefab);
        var networkObject = prefab.AddComponent<NetworkObject>();
        var fieldInfo = typeof(NetworkObject).GetField("GlobalObjectIdHash", BindingFlags.Instance | BindingFlags.NonPublic);
        fieldInfo!.SetValue(networkObject, GetHash(MyPluginInfo.PLUGIN_GUID));

        NetworkManager.Singleton.PrefabHandler.AddNetworkPrefab(prefab);
        return;

        static uint GetHash(string value)
        {
            return value?.Aggregate(17u, (current, c) => unchecked((current * 31) ^ c)) ?? 0u;
        }
    }
}
