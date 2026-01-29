using UnityEngine;

public class PrefabPlayerJumpScript : MonoBehaviour
{
    [SerializeField] PrefabPlayerController prefabPlayerController = null;

    private LayerMask groundLayer;

    //rayの長さ
    [Header("rayの長さ")]
    [SerializeField] private float rayLength = 0.0f;

    [Header("ジャンプ力")]
    [SerializeField] private float playerJumpPower = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayer = GetComponent<LayerMask>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerJump()
    {
        if (isGround())
        {
            if(prefabPlayerController != null && prefabPlayerController.rigidbody != null)
            {
                prefabPlayerController.rigidbody.AddForce(Vector3.up * playerJumpPower, ForceMode.Impulse);
            }
        }
    }

    /// <summary>
    /// プレイヤーの地面接地判定メソッド
    /// </summary>
    private bool isGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }
}
