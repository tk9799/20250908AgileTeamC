using UnityEngine;

public class ShineKnife : MonoBehaviour
{
    [SerializeField] private string tipObjectName = "tip";
    [SerializeField] private Color shineColor = Color.white;
    [SerializeField] private float shineIntensity = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform tip = transform.Find(tipObjectName);

        if (tip == null)
        {
            Debug.LogError("tipオブジェクトが見つからない");
            return;
        }

        Renderer renderer = tip.GetComponent<Renderer>();
        if(renderer == null)
        {
            Debug.LogError("Rendererコンポーネントが見つからない");
            return;
        }

        Material material = renderer.material;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", shineColor * shineIntensity);
    }
}
