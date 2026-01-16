using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DemoHud : MonoBehaviour
    {
        [SerializeField] private Text hudText;
        [SerializeField] private Transform player;

        private readonly StringBuilder builder = new StringBuilder();

        private void Awake()
        {
            if (hudText == null)
            {
                hudText = GetComponentInChildren<Text>();
            }
        }

        private void Update()
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.Find("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            if (hudText == null)
            {
                return;
            }

            builder.Clear();
            builder.AppendLine("演示场景");
            builder.AppendLine("WASD/方向键移动");
            builder.AppendLine("空格跳跃");

            if (player != null)
            {
                Vector3 position = player.position;
                builder.AppendLine($"坐标: {position.x:0.0}, {position.y:0.0}, {position.z:0.0}");
            }

            hudText.text = builder.ToString();
        }
    }
}
