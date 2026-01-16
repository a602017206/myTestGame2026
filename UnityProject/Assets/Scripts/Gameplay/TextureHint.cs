using UnityEngine;

namespace Gameplay
{
    public class TextureHint : MonoBehaviour
    {
        [TextArea]
        [SerializeField] private string hint;

        public string Hint => hint;

        public void SetHint(string newHint)
        {
            hint = newHint;
        }
    }
}
