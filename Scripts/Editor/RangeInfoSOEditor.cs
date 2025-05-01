using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RangeInfoSO))]
public class RangeInfoSOEditor : Editor
{
    private RangeInfoSO so;

    private void OnEnable()
    {
        so = (RangeInfoSO)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical("helpbox");
        {
            EditorGUILayout.LabelField("필드 크기");
            int tempMoveRange = so.MoveRange;
            so.MoveRange = EditorGUILayout.IntField(so.MoveRange);
            if (so.MoveBlock == null || tempMoveRange != so.MoveRange)
                so.SetMoveArray();
            EditorGUILayout.LabelField("움직임 범위");
            SetMoveUI(so.MoveBlock, Color.yellow);

            EditorGUILayout.LabelField("필드 크기");
            int tempAttackRange = so.AttackRange;
            so.AttackRange = EditorGUILayout.IntField(so.AttackRange);
            if (so.AttackBlock == null || tempAttackRange != so.AttackRange)
                so.SetAttackArray();
            EditorGUILayout.LabelField("공격 범위");
            SetAttackUI(so.AttackBlock, Color.red);

            if (GUILayout.Button("Save"))
            {
                Debug.Log("[RangeInfoSOEditor] Saved");
                EditorUtility.SetDirty(so);
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void SetAttackUI(bool[] block, Color guiColor)
    {
        for (int i = 0; i < so.AttackRange; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < so.AttackRange; j++)
            {
                int idx = i * so.AttackRange + j;
                if (block[idx])
                    GUI.color = guiColor;
                else
                    GUI.color = Color.white;
                block[idx] = EditorGUILayout.Toggle(block[idx]);
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;
        }
    }

    private void SetMoveUI(bool[] block, Color guiColor)
    {
        for (int i = 0; i < so.MoveRange; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < so.MoveRange; j++)
            {
                int idx = i * so.MoveRange + j;
                if (block[idx])
                    GUI.color = guiColor;
                else
                    GUI.color = Color.white;
                block[idx] = EditorGUILayout.Toggle(block[idx]);
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;
        }
    }
}