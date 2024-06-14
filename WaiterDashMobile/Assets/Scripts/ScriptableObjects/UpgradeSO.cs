using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class UpgradeSO : ScriptableObject
{
    [HideLabel]
    public string Name;

    [PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 164)]
    [HideLabel]
    public Sprite icon;

    [Space(10)]
    [Title("Upgrade Levels")]
    [OnInspectorGUI("AutoIncrementLevels")]
    public List<UpgradeLevel> levels;

    [Serializable]
    public class UpgradeLevel
    {
        [FoldoutGroup("@GetFoldoutGroupName()", Expanded = true)]
        [LabelWidth(70)]
        [HideLabel]
        public string name;

        [PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 64)]
        [HideLabel]
        public GameObject UpgradeablePrefab;

        [LabelWidth(70)]
        [LabelText("Level")]
        public int level;

        [LabelWidth(70)]
        [LabelText("Cost")]
        public int upgradeCost;

        [TableList(AlwaysExpanded = true)]
        public List<UpgradeDescription> descriptions;

        public List<UpgradeProperty> properties;

        public string GetFoldoutGroupName()
        {
            return $"Level {level}";
        }
    }

    [Serializable]
    public class UpgradeDescription
    {
        [HorizontalGroup("Description", Width = 0.75f)]
        [TextArea]
        [HideLabel]
        public string description;

        [HorizontalGroup("Description", Width = 0.25f)]
        [PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 48)]
        [HideLabel]
        public Sprite icon;
    }

    private void AutoIncrementLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].level = i + 1;
            levels[i].upgradeCost = (i+1) * 100;
            levels[i].name = Name;
        }
    }

    public UpgradeLevel GetUpgradeLevel(int level)
    {
        return levels.Find(l => l.level == level);
    }
}
