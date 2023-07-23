using System.Collections;
using System.Linq;
using GalacticSystems;
using UnityEngine;
using System;
using SaveSystems;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class GalaxyBehavior : MonoBehaviour
{
    [SerializeField] private int systemsAmount;
    [SerializeField] private int galaxyRadius;
    [SerializeField] private StarConfigs starConfigs;
    [SerializeField] private PlanetConfigs planetConfigs;
    [SerializeField] private LayerMask systemsLayer;
    [SerializeField] private StarSystemBehavior starSystemPrefab;

    private bool _initialized;
    private StarSystemBehavior[] _solarSystemsBehaviors;
    private StarSystemInfo[] _systemInfos;

    public event Action SystemsDataInitialized;
    public event Action SystemsCreated;
    public event Action SystemPositionUpdated;

    public static GalaxyBehavior Instance { get; private set; }
    
    public StarSystemInfo SelectedSystem;
    
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        
        SystemsCreated += () => StartCoroutine(UpdateSystemsPosition());
    }
    
    public void Generate()
    {
        Debug.Log("<color=#508EB7>Galaxy Behavior: </color> Start generating...");
        GenerateSystemsData();
        StartCoroutine(CreateSystems());
    }

    public void Clear()
    {
        if (_solarSystemsBehaviors != null || !_initialized)
        {
            Debug.Log("<color=#508EB7>Galaxy Behavior: </color> Start destroying...");
            StartCoroutine(DestroyAllSystems());
        }
    }

    private IEnumerator DestroyAllSystems()
    {
        for (int i = 0; i < _solarSystemsBehaviors.Length; i++)
        {
            Destroy(_solarSystemsBehaviors[i].gameObject);
            yield return null;
        }

        _solarSystemsBehaviors = null;
        Debug.Log("<color=#508EB7>Galaxy Behavior: </color> Systems destroyed.");
    }

    private void GenerateSystemsData()   
    {
        _initialized = false;
        _systemInfos = new StarSystemInfo[systemsAmount];
        for (int i = 0; i < systemsAmount; i++)
        {
            _systemInfos[i] = new StarSystemInfo
            {
                SystemName = $"Star System #{i}"
            };
        }
        Debug.Log("<color=#508EB7>Galaxy Behavior: </color> Data initialized!");
        SystemsDataInitialized?.Invoke();
    }

    private IEnumerator CreateSystems()
    {
        _solarSystemsBehaviors = new StarSystemBehavior[_systemInfos.Length];
        for (int i = 0; i < _systemInfos.Length; i++)
        {
            // Create star and init
            StarInfo starInfo = new StarInfo();
            StarConfigItem starConfigItem = starConfigs.Configs.FirstOrDefault(starConfig => starConfig.Type == starInfo.Type);
            if (starConfigItem == null)
                continue;
            
            starInfo.Init(starConfigItem);

            // Create planets and init system
            int amount = Random.Range(starConfigItem.PlanetAmounts.Min, starConfigItem.PlanetAmounts.Max);
            PlanetInfo[] planetInfos = planetConfigs.GetRandomPlanets(amount);
            _systemInfos[i].Init(starInfo, planetInfos);

            // Create objects of star system
            _solarSystemsBehaviors[i] = Instantiate(starSystemPrefab, transform);
            _solarSystemsBehaviors[i].Init(_systemInfos[i]);
            Instantiate(starConfigItem.Prefab, _solarSystemsBehaviors[i].transform);

            _solarSystemsBehaviors[i].Selected += OnSystemSelected;
            yield return null;
        }
        Debug.Log("<color=#508EB7>Galaxy Behavior: </color> Systems created!");
        SystemsCreated?.Invoke();
    }

    private void OnSystemSelected(StarSystemInfo starSystemInfo)
    {
        SelectedSystem = starSystemInfo;
    }

    private IEnumerator UpdateSystemsPosition()
    {
        for (int i = 0; i < _solarSystemsBehaviors.Length; i++)
        {
            while (true)
            {
                Vector2 randomPosition = GetRandomPositionInRadius(galaxyRadius);
                Vector3 position = new Vector3(randomPosition.x, Random.Range(-1, 1), randomPosition.y);
                if (!HasObstacles(position, 3))
                {
                    _solarSystemsBehaviors[i].transform.position = position;
                    _solarSystemsBehaviors[i].Position = position;
                    break;
                }
            }

            yield return null;
        }
        Debug.Log("<color=#508EB7>Galaxy Behavior: </color> Galaxy generated!");
        _initialized = true;
        SystemPositionUpdated?.Invoke();
    }

    private Vector2 GetRandomPositionInRadius(float radius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 randomPosition = (Vector2)transform.position + randomDirection * Random.Range(0f, radius);
        return randomPosition;
    }

    private bool HasObstacles(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, systemsLayer);
        return colliders.Length > 0;
    }
}
