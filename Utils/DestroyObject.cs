using UnityEngine;

namespace CompanyCruiserConfig.Utils;

public class DestroyObject : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject);
    }
}
