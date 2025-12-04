using UnityEngine;

public class SkyBoxAnimation : MonoBehaviour
{
    [Header("SkyBoxの回転速度")]
    [SerializeField] private float rotationSpeed = 0.0f;

    [Header("SkyBoxのマテリアル")]
    [SerializeField] private Material skyBoxMaterial = null;

    [Header("回転のリピート")]
    [SerializeField] private float rotationRepeatTime = 0.0f;

    private void Update()
    {
        rotationRepeatTime = Mathf.Repeat(skyBoxMaterial.GetFloat("_Rotation") + rotationSpeed, 360.0f);

        skyBoxMaterial.SetFloat("_Rotation", rotationRepeatTime);

        RenderSettings.skybox = skyBoxMaterial;
    }
}
