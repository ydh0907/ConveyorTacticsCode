using System;
using UnityEngine;

[Serializable]
public struct SkillSet
{
    public Sprite skillIcon;
    public string skillName;
    [TextArea()] public string skillDescription;
    public int activePower;
}

[CreateAssetMenu(menuName = "SO/PlayerInfo")]
public class PlayerInfoSO : ScriptableObject
{
    public SkillSet skill_1;
    public SkillSet skill_2;
}
