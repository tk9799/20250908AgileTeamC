using UnityEngine;

public class Character2Skill : PlayerController
{
    [Header("弱スキルの継続時間")]
    [SerializeField] private float weakSkillTime = 0.0f;

    [Header("弱スキルの解除時間")]
    [SerializeField] private float weakSkillEndTime = 0.0f;

    [Header("弱スキル発動判定")]
    [SerializeField] private bool weakSkillEnabled = false;

    [Header("強スキルの継続時間")]
    [SerializeField] private float strongSkillTime = 0.0f;

    [Header("強スキルの解除時間")]
    [SerializeField] private float strongSkillEnable = 0.0f;

    [Header("強スキル発動判定")]
    [SerializeField] private bool strongSkillEnabled = false;

    /// <summary>
    /// キャラクター2弱スキル発動メソッド
    /// </summary>
    protected override void WeakSkill()
    {

    }

    /// <summary>
    /// キャラクター2の強スキル発動メソッド
    /// </summary>
    protected override void StrongSkill()
    {

    }
}
