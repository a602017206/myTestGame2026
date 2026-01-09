using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public enum ElementalType
    {
        Wood,
        Fire,
        Earth,
        Metal,
        Water
    }

    public enum DayNightPhase
    {
        Day,
        Night
    }

    [Serializable]
    public struct ElementalWeights
    {
        [Min(0f)] public float wood;
        [Min(0f)] public float fire;
        [Min(0f)] public float earth;
        [Min(0f)] public float metal;
        [Min(0f)] public float water;

        public static ElementalWeights Default => new ElementalWeights
        {
            wood = 1f,
            fire = 1f,
            earth = 1f,
            metal = 1f,
            water = 1f
        };

        public void Add(ElementalWeights other)
        {
            wood += other.wood;
            fire += other.fire;
            earth += other.earth;
            metal += other.metal;
            water += other.water;
        }

        public void Scale(float multiplier)
        {
            wood *= multiplier;
            fire *= multiplier;
            earth *= multiplier;
            metal *= multiplier;
            water *= multiplier;
        }

        public void Normalize()
        {
            float total = wood + fire + earth + metal + water;
            if (total <= 0f)
            {
                this = Default;
                total = wood + fire + earth + metal + water;
            }

            wood /= total;
            fire /= total;
            earth /= total;
            metal /= total;
            water /= total;
        }

        public float Get(ElementalType type)
        {
            return type switch
            {
                ElementalType.Wood => wood,
                ElementalType.Fire => fire,
                ElementalType.Earth => earth,
                ElementalType.Metal => metal,
                ElementalType.Water => water,
                _ => 0f
            };
        }

        public void Add(ElementalType type, float amount)
        {
            switch (type)
            {
                case ElementalType.Wood:
                    wood += amount;
                    break;
                case ElementalType.Fire:
                    fire += amount;
                    break;
                case ElementalType.Earth:
                    earth += amount;
                    break;
                case ElementalType.Metal:
                    metal += amount;
                    break;
                case ElementalType.Water:
                    water += amount;
                    break;
            }
        }

        public (ElementalType primary, ElementalType secondary) GetTopTwo()
        {
            var values = new List<(ElementalType type, float value)>
            {
                (ElementalType.Wood, wood),
                (ElementalType.Fire, fire),
                (ElementalType.Earth, earth),
                (ElementalType.Metal, metal),
                (ElementalType.Water, water)
            };

            values.Sort((a, b) => b.value.CompareTo(a.value));
            return (values[0].type, values[1].type);
        }
    }

    [Serializable]
    public class ElementalAffinityInput
    {
        [Range(-90f, 90f)] public float latitude;
        [Range(-180f, 180f)] public float longitude;
        public string terrainTag;
        public string solarTerm;
        public DayNightPhase dayNight;
        public bool useVirtualLocation;
        public string virtualTerrainTag;
        public string travelMemoryTag;
    }

    [Serializable]
    public class ElementalAffinityResult
    {
        public ElementalType primaryElement;
        public ElementalType secondaryElement;
        public string specialAttribute;
        public ElementalWeights normalizedWeights;
    }

    public class ElementalAffinityGenerator
    {
        private readonly Data.ElementalWeightsConfig _config;

        public ElementalAffinityGenerator(Data.ElementalWeightsConfig config)
        {
            _config = config;
        }

        public ElementalAffinityResult Generate(ElementalAffinityInput input)
        {
            ElementalWeights weights = _config != null ? _config.baseWeights : ElementalWeights.Default;

            if (_config != null)
            {
                ApplyTerrainWeights(input, ref weights);
                ApplySolarTermModifiers(input, ref weights);
                ApplyDayNightModifiers(input, ref weights);
            }

            ApplyLocationInfluence(input, ref weights);
            ApplyMemoryInfluence(input, ref weights);

            weights.Normalize();
            var (primary, secondary) = weights.GetTopTwo();

            return new ElementalAffinityResult
            {
                primaryElement = primary,
                secondaryElement = secondary,
                specialAttribute = ResolveSpecialAttribute(primary, secondary),
                normalizedWeights = weights
            };
        }

        private void ApplyTerrainWeights(ElementalAffinityInput input, ref ElementalWeights weights)
        {
            string targetTag = input.useVirtualLocation && !string.IsNullOrWhiteSpace(input.virtualTerrainTag)
                ? input.virtualTerrainTag
                : input.terrainTag;

            if (string.IsNullOrWhiteSpace(targetTag))
            {
                return;
            }

            foreach (var entry in _config.terrainWeights)
            {
                if (string.Equals(entry.terrainTag, targetTag, StringComparison.OrdinalIgnoreCase))
                {
                    weights.Add(entry.weights);
                    break;
                }
            }
        }

        private void ApplySolarTermModifiers(ElementalAffinityInput input, ref ElementalWeights weights)
        {
            if (string.IsNullOrWhiteSpace(input.solarTerm))
            {
                return;
            }

            foreach (var modifier in _config.solarTermModifiers)
            {
                if (string.Equals(modifier.solarTerm, input.solarTerm, StringComparison.OrdinalIgnoreCase))
                {
                    weights.Add(modifier.weightsDelta);
                    break;
                }
            }
        }

        private void ApplyDayNightModifiers(ElementalAffinityInput input, ref ElementalWeights weights)
        {
            foreach (var modifier in _config.yinYangModifiers)
            {
                if (modifier.phase == input.dayNight)
                {
                    weights.Add(modifier.weightsDelta);
                    break;
                }
            }
        }

        private void ApplyLocationInfluence(ElementalAffinityInput input, ref ElementalWeights weights)
        {
            float latFactor = Mathf.InverseLerp(0f, 90f, Mathf.Abs(input.latitude));
            float lonFactor = Mathf.InverseLerp(0f, 180f, Mathf.Abs(input.longitude));

            weights.Add(ElementalType.Water, latFactor * 0.4f);
            weights.Add(ElementalType.Metal, latFactor * 0.2f);
            weights.Add(ElementalType.Fire, (1f - latFactor) * 0.3f);
            weights.Add(ElementalType.Wood, (1f - latFactor) * 0.2f);
            weights.Add(ElementalType.Earth, lonFactor * 0.3f);
        }

        private void ApplyMemoryInfluence(ElementalAffinityInput input, ref ElementalWeights weights)
        {
            if (string.IsNullOrWhiteSpace(input.travelMemoryTag))
            {
                return;
            }

            string memory = input.travelMemoryTag.ToLowerInvariant();
            if (memory.Contains("mountain") || memory.Contains("山"))
            {
                weights.Add(ElementalType.Earth, 0.4f);
            }
            else if (memory.Contains("forest") || memory.Contains("林"))
            {
                weights.Add(ElementalType.Wood, 0.4f);
            }
            else if (memory.Contains("river") || memory.Contains("water") || memory.Contains("水"))
            {
                weights.Add(ElementalType.Water, 0.4f);
            }
            else if (memory.Contains("volcano") || memory.Contains("火"))
            {
                weights.Add(ElementalType.Fire, 0.4f);
            }
            else if (memory.Contains("metal") || memory.Contains("金"))
            {
                weights.Add(ElementalType.Metal, 0.4f);
            }
        }

        private string ResolveSpecialAttribute(ElementalType primary, ElementalType secondary)
        {
            if (_config == null || _config.specialAttributeTriggers.Count == 0)
            {
                return string.Empty;
            }

            foreach (var trigger in _config.specialAttributeTriggers)
            {
                float probability = trigger.baseProbability;
                if (trigger.associatedElement == primary)
                {
                    probability += trigger.bonusIfPrimaryMatches;
                }
                else if (trigger.associatedElement == secondary)
                {
                    probability += trigger.bonusIfSecondaryMatches;
                }

                if (UnityEngine.Random.value <= probability)
                {
                    return trigger.attributeName;
                }
            }

            return string.Empty;
        }
    }
}
