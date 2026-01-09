using System;
using Gameplay;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Game/Player Profile", fileName = "PlayerProfile")]
    public class PlayerProfile : ScriptableObject
    {
        public ElementalType primaryElement;
        public ElementalType secondaryElement;
        public string specialAttribute;
        public ElementalAffinityInput lastAffinityInput;
        public DateTime lastUpdatedUtc;

        public void ApplyAffinity(ElementalAffinityResult result, ElementalAffinityInput input)
        {
            primaryElement = result.primaryElement;
            secondaryElement = result.secondaryElement;
            specialAttribute = result.specialAttribute;
            lastAffinityInput = input;
            lastUpdatedUtc = DateTime.UtcNow;
        }
    }
}
