using UnityEngine;

namespace CompanyCruiserConfig.Utils;

internal class DestroyObject : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject);
    }
}
