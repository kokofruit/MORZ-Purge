using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    public static PersistentCanvas instance;
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
