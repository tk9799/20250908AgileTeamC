using UnityEngine;
using DG.Tweening;
using TMPro;

/// <summary>
/// テキストの動きを制御するクラス
/// </summary>
public class TextAnimation : MonoBehaviour
{
    [Header("点滅させるテキスト")]
    [SerializeField] private TextMeshProUGUI moveText = null;

    // -1で無限
    [Header("ループ数")]
    [SerializeField] private int loop = 0;

    [Header("テキストの透明度")]
    [SerializeField] private float textTransparency = 0.0f;

    // とりあえず2.0で設定
    [Header("テキストの透明度が変わる速度")]
    [SerializeField] private float changeTransparencySpeed = 0.0f;

    void Start()
    {
        // テキストを点滅させる処理
        moveText.DOFade(textTransparency, changeTransparencySpeed).SetLoops(loop, LoopType.Yoyo).Play();
    }
}
