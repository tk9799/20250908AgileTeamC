using UnityEngine;

public class Character1Skill : PlayerController
{
    private KnifeControllertr knifeControllertr = new KnifeControllertr();

    [Header("ナイフのダメージ値を1.5倍に設定")]
    [SerializeField] private float knifeDamageMultiplier = 0.0f;

    [Header("ナイフを跳ね返すガードオブジェクト")]
    [SerializeField] private GameObject reflectionGuard = null;

    [Header("弱スキルの継続時間")]
    [SerializeField] private float weakSkillTime = 0.0f;

    [Header("弱スキルの解除時間")]
    [SerializeField] private float weakSkillEndTime = 0.0f;

    [Header("弱スキル発動判定")]
    [SerializeField] private bool weakSkillEnabled = false;

    [Header("サイズが大きいナイフ")]
    [SerializeField] private GameObject bigKnife = null;

    [Header("強スキルの継続時間")]
    [SerializeField] private float strongSkillTime = 0.0f;

    [Header("強スキルの解除時間")]
    [SerializeField] private float strongSkillEnable = 0.0f;

    [Header("強スキル発動判定")]
    [SerializeField] private bool strongSkillEnabled = false;

    /// <summary>
    /// キャラクター1弱スキル発動メソッド
    /// </summary>
    protected override void WeakSkill()
    {
        // ナイフの当たり判定をでかくする
    }

    /// <summary>
    /// キャラクター1の強スキル発動メソッド
    /// </summary>
    protected override void StrongSkill()
    {
        // ナイフコントローラー内のダメージ値を1.5倍にして変更
    }
}
