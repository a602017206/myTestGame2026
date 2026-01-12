using Core;
using Data;
using Gameplay;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    [Header("Elemental Config")]
    [SerializeField] private ElementalWeightsConfig weightsConfig;
    [SerializeField] private TextAsset weightsConfigJson;
    [SerializeField] private PlayerProfile playerProfile;

    [Header("Location Input")]
    [SerializeField, Range(-90f, 90f)] private float latitude;
    [SerializeField, Range(-180f, 180f)] private float longitude;
    [SerializeField] private string terrainTag;
    [SerializeField] private string solarTerm;
    [SerializeField] private DayNightPhase dayNight;

    [Header("Virtual Location")]
    [SerializeField] private bool useVirtualLocation;
    [SerializeField] private string virtualTerrainTag;
    [SerializeField] private string travelMemoryTag;

    private GameStateMachine stateMachine;

    private void Awake()
    {
        Debug.Log("GameEntry initialized");
        InitializeStateMachine();
    }

    private void Start()
    {
        stateMachine.Start<BootState>();
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    public void GenerateAndStoreAffinity()
    {
        var input = new ElementalAffinityInput
        {
            latitude = latitude,
            longitude = longitude,
            terrainTag = terrainTag,
            solarTerm = solarTerm,
            dayNight = dayNight,
            useVirtualLocation = useVirtualLocation,
            virtualTerrainTag = virtualTerrainTag,
            travelMemoryTag = travelMemoryTag
        };

        ElementalWeightsConfig resolvedConfig = weightsConfig != null
            ? weightsConfig
            : ElementalWeightsConfig.FromJson(weightsConfigJson);
        var generator = new ElementalAffinityGenerator(resolvedConfig);
        ElementalAffinityResult result = generator.Generate(input);

        Debug.Log($"Primary Element: {result.primaryElement}, Secondary: {result.secondaryElement}, Special: {result.specialAttribute}");

        if (playerProfile != null)
        {
            playerProfile.ApplyAffinity(result, input);
        }
    }

    private void InitializeStateMachine()
    {
        stateMachine = new GameStateMachine();
        stateMachine.Register(new BootState(stateMachine));
        stateMachine.Register(new GenerateProfileState(this, stateMachine));
        stateMachine.Register(new MainMenuState(playerProfile));
    }
}
