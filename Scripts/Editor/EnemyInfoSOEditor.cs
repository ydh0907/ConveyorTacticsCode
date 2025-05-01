using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyInfoSO))]
public class EnemyInfoSOEditor : Editor
{
    private EnemyInfoSO _enemyInfoSo;

    private GUIStyle _labelStyle;
    
    private Editor _rangeInfoSoEditor;

    private void OnEnable()
    {
        _enemyInfoSo = (EnemyInfoSO)target;
        
        if (_labelStyle == null)
        {
            _labelStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                fontStyle = FontStyle.Bold
            };
        }
        
        if (_enemyInfoSo.RangeInfo != null)
        {
            _rangeInfoSoEditor = CreateEditor(_enemyInfoSo.RangeInfo);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.LabelField($"Enemy Info - {target.name}", _labelStyle);

        EditorGUILayout.BeginVertical("helpbox");
        {
            EditorGUILayout.LabelField("체력");
            _enemyInfoSo.Health = EditorGUILayout.IntSlider(_enemyInfoSo.Health, 0, 100);

            EditorGUILayout.LabelField("데미지");
            _enemyInfoSo.Damage = EditorGUILayout.IntSlider(_enemyInfoSo.Damage, 0, 100);
            
            EditorGUILayout.LabelField("방어력");
            _enemyInfoSo.Defense = EditorGUILayout.IntSlider(_enemyInfoSo.Defense, 0, 100);

            EditorGUILayout.LabelField("공격 관통 허용?");
            _enemyInfoSo.IsAllowPenetration = EditorGUILayout.Toggle(_enemyInfoSo.IsAllowPenetration);
            
            EditorGUILayout.LabelField("추가 데미지 허용?");
            _enemyInfoSo.IsAdditionalDamage = EditorGUILayout.Toggle(_enemyInfoSo.IsAdditionalDamage);
            if (_enemyInfoSo.IsAdditionalDamage)
            {
                EditorGUILayout.LabelField("추가 데미지 비율");
                _enemyInfoSo.AdditionalDamagePercent =
                    EditorGUILayout.IntSlider(_enemyInfoSo.AdditionalDamagePercent, 0, 100);
            }
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.LabelField("범위 정보", _labelStyle);
        EditorGUILayout.BeginVertical("helpbox");
        {
            EditorGUILayout.LabelField("범위 정보");
            _enemyInfoSo.RangeInfo = (RangeInfoSO)EditorGUILayout.ObjectField(_enemyInfoSo.RangeInfo, typeof(RangeInfoSO), false);
            
            if (_enemyInfoSo.RangeInfo != null)
            {
                if (_rangeInfoSoEditor != null)
                {
                    _rangeInfoSoEditor.OnInspectorGUI();
                }
                else
                {
                    _rangeInfoSoEditor = CreateEditor(_enemyInfoSo.RangeInfo);
                }
            }
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);
        
        
        if (GUILayout.Button("Save"))
        {
            Debug.Log("[EnemyInfoSOEditor] Saved");
            EditorUtility.SetDirty(_enemyInfoSo);
            EditorUtility.SetDirty(_enemyInfoSo.RangeInfo);
            AssetDatabase.SaveAssets();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void OnDisable()
    {
        if (_rangeInfoSoEditor != null)
        {
            DestroyImmediate(_rangeInfoSoEditor);
        }
    }
}