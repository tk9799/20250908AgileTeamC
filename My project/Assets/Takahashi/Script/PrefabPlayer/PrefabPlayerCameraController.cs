using UnityEngine;

public class PrefabPlayerCameraController : MonoBehaviour
{
    public Transform targetTransform;

    //プレイヤーとカメラの距離
    private Vector3 playerDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDistance = transform.position - targetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetTransform.position + playerDistance;
    }
}
