using UnityEngine;

/// <summary>
/// skyboxを回転させるクラス
/// </summary>
public class SkyBoxAnimation : MonoBehaviour
{
    [Header("SkyBoxの回転速度")]
    [SerializeField] private float rotationSpeed = 0.0f;

    [Header("SkyBoxのマテリアル")]
    [SerializeField] private Material skyBoxMaterial = null;

    [Header("回転のリピート")]
    [SerializeField] private float rotationRepeatTime = 0.0f;

    /// <summary>
    /// skyboxを回転させるメソッド
    /// </summary>
    private void Update()
    {
        // skyboxの回転
        rotationRepeatTime = Mathf.Repeat(skyBoxMaterial.GetFloat("_Rotation") + rotationSpeed, 360.0f);

        // skyboxのマテリアルに回転を反映
        skyBoxMaterial.SetFloat("_Rotation", rotationRepeatTime);

        // skyboxにマテリアルを反映
        RenderSettings.skybox = skyBoxMaterial;
    }
}
