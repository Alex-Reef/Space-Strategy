using System.Collections;
using System.Linq;
using GalacticSystems;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GalacticBehavior : MonoBehaviour
{
    [SerializeField] private int systemsAmount;
    [SerializeField] private StarConfigs starConfigs;

    [SerializeField] private LayerMask systemsLayer;
    private StarSystemBehavior[] _solarSystemsBehaviors;
    private SolarSystemInfo[] _systemInfos;

    public event Action SystemsDataInited;
    public event Action SystemsCreated;
    public event Action SystemPositionUpdated;
    
    private void Awake()
    {
        SystemsCreated += () => StartCoroutine(UpdateSystemsPosition());
        
        InitSystemsData();
        StartCoroutine(CreateSystems());
    }

    private void InitSystemsData()
    {
        _systemInfos = new SolarSystemInfo[systemsAmount];
        for (int i = 0; i < systemsAmount; i++)
        {
            _systemInfos[i] = new SolarSystemInfo();
        }
        SystemsDataInited?.Invoke();
    }

    private IEnumerator CreateSystems()
    {
        _solarSystemsBehaviors = new StarSystemBehavior[_systemInfos.Length];
        for (int i = 0; i < _systemInfos.Length; i++)
        {
            StarInfo starInfo = (StarInfo)_systemInfos[i].Star;
            StarConfigItem starConfigItem = starConfigs.Configs.FirstOrDefault(starConfig => starConfig.Type == starInfo.Type);
            if (starConfigItem == null)
                continue;
            
            StarSystemBehavior behaviorPrefab = starConfigItem.PrefabVariables[0];
            if (starConfigItem.PrefabVariables.Length > 1)
                behaviorPrefab = starConfigItem.PrefabVariables[Random.Range(0, starConfigItem.PrefabVariables.Length)];

            _solarSystemsBehaviors[i] = Instantiate(behaviorPrefab, transform);
            _solarSystemsBehaviors[i].Init(_systemInfos[i]);
            
            yield return null;
        }
        SystemsCreated?.Invoke();
    }

    private IEnumerator UpdateSystemsPosition()
    {
        for (int i = 0; i < _solarSystemsBehaviors.Length; i++)
        {
            while (true)
            {
                Vector2 randomPosition = GetRandomPositionInRadius(30);
                Vector3 position = new Vector3(randomPosition.x, Random.Range(-1, 1), randomPosition.y);
                if (!HasObstacles(position, 3))
                {
                    _solarSystemsBehaviors[i].transform.position = position;
                    break;
                }
            }

            yield return null;
        }
        SystemPositionUpdated?.Invoke();
    }

    Vector2 GetRandomPositionInRadius(float radius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 randomPosition = (Vector2)transform.position + randomDirection * Random.Range(0f, radius);
        return randomPosition;
    }

    bool HasObstacles(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, systemsLayer);
        return colliders.Length > 0;
    }
}
