using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SkillManager : MonoBehaviour
{
    // ���� �ڵ� ������ �Ұ��Ͽ� ���� �־���.
    private PlayerInfoSO playerInfo = null;
    public void SetPlayerInfo(PlayerInfoSO info) => playerInfo = info;
    public PlayerInfoSO GetPlayerInfo() => playerInfo;
}
