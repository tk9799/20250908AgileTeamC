using UnityEngine;

public class PrefabPlayerNomalAttack : MonoBehaviour
{
    [SerializeField] private float knifeSpawnDistance = 0.0f;

    [SerializeField] private float tlanslateSpeed = 0.0f;

    [SerializeField] private PrefabPlayerKnifeList prefabPlayerKnifeList = null;

    [SerializeField] private GameObject knifeObject = null;

    [SerializeField] private PrefabPlayerController prefabPlayerController;

    [SerializeField] private Transform cameraTransform = null;

    public void NormalAttack()
    {
        Vector3 knifeTranslatePossition = transform.position + transform.forward * knifeSpawnDistance;

        if (prefabPlayerKnifeList != null && prefabPlayerKnifeList.knifePossessionList.Count > 0)
        {
            GameObject generateKnife = Instantiate(knifeObject, knifeTranslatePossition, transform.rotation);

            Rigidbody rigidbody = generateKnife.GetComponent<Rigidbody>();

            if(rigidbody != null )
            {
                rigidbody.AddForce(transform.forward*tlanslateSpeed,ForceMode.Impulse);
            }

            prefabPlayerKnifeList.knifePossessionList.RemoveAt(0);
            Debug.Log("ナイフ攻撃");
        }
        else
        {
            Debug.LogError("ナイフを持っていません");
        }
    }

    public void AimNormalAttack()
    {
        Vector3 knifeTranslatePossition = transform.position + transform.forward * knifeSpawnDistance;

        if (prefabPlayerKnifeList != null && prefabPlayerKnifeList.knifePossessionList.Count > 0)
        {
            GameObject generateKnife = Instantiate(knifeObject, knifeTranslatePossition, Quaternion.identity);

            //Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            Vector3 throwDirection = cameraTransform.forward;

            generateKnife.transform.forward = throwDirection;

            Rigidbody rigidbody = generateKnife.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                //Rayの方向（カメラの中央）に向けてナイフを投げる
                rigidbody.AddForce(throwDirection * tlanslateSpeed, ForceMode.Impulse);
            }
        }
    }
}
