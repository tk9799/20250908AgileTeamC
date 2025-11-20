using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    [Header("点滅させるテキスト")]
    [SerializeField] private TextMeshProUGUI moveText = null;

    [Header("テキストの透明度")]
    [SerializeField] private float textTransparency = 0.0f;

    // -1で無限
    [Header("ループ数")]
    [SerializeField] private int loop = 0;

    // とりあえず2.0で設定
    [Header("テキストの透明度が変わる速度")]
    [SerializeField] private float changeTransparencySpeed = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // テキストを点滅させる処理
        moveText.DOFade(textTransparency, changeTransparencySpeed).SetLoops(loop, LoopType.Yoyo).Play();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
