using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public static class WorldBuilder
    {
        public static void Build()
        {
            EnsureGround();
            EnsurePlayer();
            EnsureCamera();
            EnsureLight();
            EnsureEnvironmentProps();
            EnsureElementalShrines();
            EnsureHud();
        }

        private static void EnsureGround()
        {
            if (GameObject.Find("Ground") != null)
            {
                return;
            }

            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(5f, 1f, 5f);

            var renderer = ground.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material material = new Material(Shader.Find("Standard"));
                material.color = new Color(0.3f, 0.6f, 0.3f);
                renderer.material = material;
            }

            var hint = ground.AddComponent<TextureHint>();
            hint.SetHint("地面贴图建议：草地/苔藓 + 轻微土壤混合，支持法线与AO。");
        }

        private static void EnsurePlayer()
        {
            if (GameObject.Find("Player") != null)
            {
                return;
            }

            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.transform.position = new Vector3(0f, 1f, 0f);

            var rigidbody = player.AddComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            player.AddComponent<PlayerController>();
        }

        private static void EnsureCamera()
        {
            if (Camera.main != null)
            {
                return;
            }

            GameObject cameraObject = new GameObject("Main Camera");
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            cameraObject.transform.position = new Vector3(0f, 6f, -8f);
            cameraObject.transform.rotation = Quaternion.Euler(20f, 0f, 0f);

            var follow = cameraObject.AddComponent<CameraFollow>();
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                follow.SetTarget(player.transform);
            }
        }

        private static void EnsureLight()
        {
            if (Object.FindObjectOfType<Light>() != null)
            {
                return;
            }

            GameObject lightObject = new GameObject("Directional Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1f, 0.95f, 0.8f);
            light.intensity = 1.1f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static void EnsureEnvironmentProps()
        {
            if (GameObject.Find("EnvironmentProps") != null)
            {
                return;
            }

            GameObject root = new GameObject("EnvironmentProps");

            for (int i = 0; i < 6; i++)
            {
                GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rock.name = $"Rock_{i + 1}";
                rock.transform.SetParent(root.transform, false);
                rock.transform.localScale = new Vector3(1.2f, 0.6f, 1.1f);
                rock.transform.position = new Vector3(-6f + i * 2.4f, 0.3f, 4.5f);

                var renderer = rock.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    Material material = new Material(Shader.Find("Standard"));
                    material.color = new Color(0.4f, 0.4f, 0.45f);
                    renderer.material = material;
                }

                var hint = rock.AddComponent<TextureHint>();
                hint.SetHint("岩石贴图建议：粗糙石块/花岗岩材质，带法线与粗糙度贴图。");
            }

            for (int i = 0; i < 4; i++)
            {
                GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                trunk.name = $"Tree_{i + 1}";
                trunk.transform.SetParent(root.transform, false);
                trunk.transform.localScale = new Vector3(0.4f, 1.2f, 0.4f);
                trunk.transform.position = new Vector3(-4f + i * 2.7f, 1.2f, -4.5f);

                var trunkRenderer = trunk.GetComponent<MeshRenderer>();
                if (trunkRenderer != null)
                {
                    Material trunkMaterial = new Material(Shader.Find("Standard"));
                    trunkMaterial.color = new Color(0.4f, 0.25f, 0.1f);
                    trunkRenderer.material = trunkMaterial;
                }

                var trunkHint = trunk.AddComponent<TextureHint>();
                trunkHint.SetHint("树干贴图建议：深色木材纹理，带法线与高度细节。");

                GameObject canopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                canopy.name = $"Tree_{i + 1}_Canopy";
                canopy.transform.SetParent(root.transform, false);
                canopy.transform.localScale = new Vector3(1.4f, 1.1f, 1.4f);
                canopy.transform.position = trunk.transform.position + new Vector3(0f, 1.6f, 0f);

                var canopyRenderer = canopy.GetComponent<MeshRenderer>();
                if (canopyRenderer != null)
                {
                    Material canopyMaterial = new Material(Shader.Find("Standard"));
                    canopyMaterial.color = new Color(0.2f, 0.45f, 0.2f);
                    canopyRenderer.material = canopyMaterial;
                }

                var canopyHint = canopy.AddComponent<TextureHint>();
                canopyHint.SetHint("树叶贴图建议：绿色树冠/叶片材质，可用半透明或带微光。");
            }
        }

        private static void EnsureElementalShrines()
        {
            if (GameObject.Find("ElementalShrines") != null)
            {
                return;
            }

            GameObject root = new GameObject("ElementalShrines");

            CreateShrine(root.transform, "WoodShrine", new Vector3(-6f, 0f, -1.5f), new Color(0.2f, 0.6f, 0.3f),
                "木属性祭坛贴图建议：木纹、藤蔓与符纹纹理。");
            CreateShrine(root.transform, "FireShrine", new Vector3(-2f, 0f, -1.5f), new Color(0.75f, 0.25f, 0.1f),
                "火属性祭坛贴图建议：赤红石材/熔岩纹理，可加流光贴图。");
            CreateShrine(root.transform, "EarthShrine", new Vector3(2f, 0f, -1.5f), new Color(0.6f, 0.45f, 0.25f),
                "土属性祭坛贴图建议：厚重土石/夯土纹理。");
            CreateShrine(root.transform, "MetalShrine", new Vector3(6f, 0f, -1.5f), new Color(0.6f, 0.6f, 0.65f),
                "金属性祭坛贴图建议：金属/矿石纹理，支持金属度贴图。");
            CreateShrine(root.transform, "WaterShrine", new Vector3(0f, 0f, -6f), new Color(0.2f, 0.4f, 0.7f),
                "水属性祭坛贴图建议：青蓝石材/水纹贴图，可加法线与流动效果。");
        }

        private static void CreateShrine(Transform parent, string name, Vector3 position, Color color, string textureHint)
        {
            GameObject shrine = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shrine.name = name;
            shrine.transform.SetParent(parent, false);
            shrine.transform.position = position + new Vector3(0f, 0.5f, 0f);
            shrine.transform.localScale = new Vector3(1.6f, 1f, 1.6f);

            var renderer = shrine.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material material = new Material(Shader.Find("Standard"));
                material.color = color;
                renderer.material = material;
            }

            var hint = shrine.AddComponent<TextureHint>();
            hint.SetHint(textureHint);

            GameObject banner = GameObject.CreatePrimitive(PrimitiveType.Plane);
            banner.name = $"{name}_Banner";
            banner.transform.SetParent(parent, false);
            banner.transform.position = position + new Vector3(0f, 0.02f, -1.1f);
            banner.transform.localScale = new Vector3(0.15f, 1f, 0.2f);
            banner.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            var bannerRenderer = banner.GetComponent<MeshRenderer>();
            if (bannerRenderer != null)
            {
                Material bannerMaterial = new Material(Shader.Find("Standard"));
                bannerMaterial.color = Color.Lerp(color, Color.white, 0.35f);
                bannerRenderer.material = bannerMaterial;
            }

            var bannerHint = banner.AddComponent<TextureHint>();
            bannerHint.SetHint("祭坛旗帜贴图建议：符箓纹样/织物纹理，可加半透明边缘。");
        }

        private static void EnsureHud()
        {
            if (GameObject.Find("DemoHud") != null)
            {
                return;
            }

            GameObject hudRoot = new GameObject("DemoHud");
            var canvas = hudRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            hudRoot.AddComponent<CanvasScaler>();
            hudRoot.AddComponent<GraphicRaycaster>();

            GameObject textObject = new GameObject("HudText");
            textObject.transform.SetParent(hudRoot.transform, false);
            var text = textObject.AddComponent<UnityEngine.UI.Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 20;
            text.color = Color.white;
            text.alignment = TextAnchor.UpperLeft;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;

            var rectTransform = text.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.02f, 0.7f);
            rectTransform.anchorMax = new Vector2(0.4f, 0.98f);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            hudRoot.AddComponent<UI.DemoHud>();
        }
    }
}
