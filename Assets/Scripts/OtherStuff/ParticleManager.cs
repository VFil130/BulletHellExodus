using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [System.Serializable]
    public class ParticleEffect
    {
        public string name;
        public GameObject prefab;
    }

    [SerializeField] private ParticleEffect[] particleEffects;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public static void CreateParticle(string effectName, Vector3 position)
    {
        if (Instance == null)
        {
            Debug.LogError("ParticleManager не найден на сцене!");
            return;
        }

        GameObject effectPrefab = Instance.GetPrefabByName(effectName);

        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, position, Quaternion.identity);
        }
    }
    public static void CreateParticle(string effectName, Transform spawnPoint)
    {
        CreateParticle(effectName, spawnPoint.position);
    }

    public static GameObject CreateParticleAttached(string effectName, Transform parent, Vector3 offset = default)
    {
        if (Instance == null)
        {
            Debug.LogError("ParticleManager не найден на сцене!");
            return null;
        }

        GameObject effectPrefab = Instance.GetPrefabByName(effectName);

        if (effectPrefab != null)
        {
            GameObject particle = Instantiate(effectPrefab, parent.position + offset, Quaternion.identity);
            particle.transform.SetParent(parent);
            particle.transform.localPosition = offset;
            return particle;
        }

        return null;
    }

    private GameObject GetPrefabByName(string name)
    {
        foreach (var effect in particleEffects)
        {
            if (effect.name == name)
            {
                return effect.prefab;
            }
        }

        Debug.LogError($"Эффект '{name}' не найден в ParticleManager!");
        return null;
    }
}