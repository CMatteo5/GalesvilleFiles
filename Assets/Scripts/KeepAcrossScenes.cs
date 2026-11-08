using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-1000)]
public class KeepAcrossScenes : MonoBehaviour
{
    [Tooltip("Unique name for this persistent object. Leave empty to use the GameObject's name.")]
    public string key;

    static readonly Dictionary<string, KeepAcrossScenes> alive = new Dictionary<string, KeepAcrossScenes>();

    public bool IsDuplicate { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetRegistry()
    {
        alive.Clear();
    }

    void Awake()
    {
        if (string.IsNullOrEmpty(key))
        {
            key = gameObject.name;
        }

        if (alive.TryGetValue(key, out KeepAcrossScenes existing) && existing != null && existing != this)
        {

            IsDuplicate = true;
            gameObject.SetActive(false); 
            Destroy(gameObject);
            return;
        }
        alive[key] = this;
        if (transform.parent != null)
        {
            Debug.LogWarning("KeepAcrossScenes: '" + name + "' was not a root object, so it was unparented.");
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        if (!IsDuplicate && alive.TryGetValue(key, out KeepAcrossScenes current) && current == this)
        {
            alive.Remove(key);
        }
    }
}