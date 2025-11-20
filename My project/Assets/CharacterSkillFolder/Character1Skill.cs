using UnityEngine;

public class Character1Skill : PlayerController
{

    [Header("ナイフを跳ね返すガードオブジェクト")]
    [SerializeField] private GameObject reflectionGuard = null;

    [Header("弱スキルの継続時間")]
    [SerializeField] private float weakSkillTime = 0.0f;

    [Header("弱スキルの解除時間")]
    [SerializeField] private float weakSkillEndTime = 0.0f;

    [Header("弱スキル発動判定")]
    [SerializeField] private bool weakSkillEnabled = false;

    [Header("でかいナイフ")]
    [SerializeField] private GameObject bigKnife = null;

    [Header("強スキルの継続時間")]
    [SerializeField] private float strongSkillTime = 0.0f;

    [Header("強スキルの解除時間")]
    [SerializeField] private float strongSkillEndTime = 0.0f;

    [Header("強スキル発動判定")]
    [SerializeField] private bool strongSkillEnabled = false;

    /// <summary>
    /// キャラクター1弱スキル発動メソッド
    /// </summary>
    protected virtual void WeakSkill()
    {

    }

    /// <summary>
    /// キャラクター1の強スキル発動メソッド
    /// </summary>
    protected virtual void StrongSkill()
    {

    }
}
