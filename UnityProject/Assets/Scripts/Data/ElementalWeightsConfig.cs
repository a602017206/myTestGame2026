using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Game/Elemental Weights Config", fileName = "ElementalWeights")]
    public class ElementalWeightsConfig : ScriptableObject
    {
        public ElementalWeights baseWeights = ElementalWeights.Default;
        public List<TerrainWeightEntry> terrainWeights = new List<TerrainWeightEntry>();
        public List<SolarTermModifier> solarTermModifiers = new List<SolarTermModifier>();
        public List<YinYangModifier> yinYangModifiers = new List<YinYangModifier>();
        public List<SpecialAttributeTrigger> specialAttributeTriggers = new List<SpecialAttributeTrigger>();

        public static ElementalWeightsConfig FromJson(TextAsset jsonAsset)
        {
            if (jsonAsset == null || string.IsNullOrWhiteSpace(jsonAsset.text))
            {
                return null;
            }

            var config = CreateInstance<ElementalWeightsConfig>();
            JsonUtility.FromJsonOverwrite(jsonAsset.text, config);
            return config;
        }
    }

    [Serializable]
    public class TerrainWeightEntry
    {
        public string terrainTag;
        public ElementalWeights weights = ElementalWeights.Default;
    }

    [Serializable]
    public class SolarTermModifier
    {
        public string solarTerm;
        public ElementalWeights weightsDelta = new ElementalWeights();
    }

    [Serializable]
    public class YinYangModifier
    {
        public DayNightPhase phase;
        public ElementalWeights weightsDelta = new ElementalWeights();
    }

    [Serializable]
    public class SpecialAttributeTrigger
    {
        public string attributeName;
        [Range(0f, 1f)] public float baseProbability = 0.05f;
        public ElementalType associatedElement;
        [Range(0f, 1f)] public float bonusIfPrimaryMatches = 0.05f;
        [Range(0f, 1f)] public float bonusIfSecondaryMatches = 0.02f;
    }
}
