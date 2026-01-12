using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class MainMenuState : IGameState
    {
        private readonly PlayerProfile playerProfile;
        private GameObject menuRoot;

        public MainMenuState(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        public void Enter()
        {
            menuRoot = new GameObject("MainMenu");
            var canvas = menuRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            menuRoot.AddComponent<CanvasScaler>();
            menuRoot.AddComponent<GraphicRaycaster>();

            var textObject = new GameObject("MainMenuText");
            textObject.transform.SetParent(menuRoot.transform, false);

            var text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 32;
            text.color = Color.white;
            text.text = BuildMenuText();

            var rectTransform = text.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.1f, 0.1f);
            rectTransform.anchorMax = new Vector2(0.9f, 0.9f);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        public void Exit()
        {
            if (menuRoot != null)
            {
                Object.Destroy(menuRoot);
            }
        }

        public void Tick()
        {
        }

        private string BuildMenuText()
        {
            if (playerProfile == null)
            {
                return "Main Menu\n\nNo PlayerProfile assigned.";
            }

            return $"Main Menu\n\n" +
                   $"主五行: {playerProfile.primaryElement}\n" +
                   $"副五行: {playerProfile.secondaryElement}\n" +
                   $"特殊属性: {playerProfile.specialAttribute}";
        }
    }
}
