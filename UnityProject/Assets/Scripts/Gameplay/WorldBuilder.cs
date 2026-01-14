using UnityEngine;

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
    }
}
