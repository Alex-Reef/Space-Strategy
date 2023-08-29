using System;
using System.Collections;
using System.Collections.Generic;
using GalaxySystem.Configs;
using GalaxySystem.Models;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;

namespace GalaxySystem.Behavior
{
    [DisallowMultipleComponent]
    public class GalaxyBehavior : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private int yPosOffset;
        [SerializeField, Range(1, 100)] private int galaxyRadius;
        [SerializeField] private LayerMask starSystemLayer;
        [SerializeField, Range(1, 10)] private int distanceBetweenStars;
        [SerializeField, Range(0, 1000)] private int starSystemsCount;
        
        [SerializeField] private StarConfig starConfig;
        [SerializeField] private PlanetConfig planetConfig;
        
        [SerializeField] private GameObject starSystemPrefab;
        
        private Dictionary<StarType, StarConfigItem> _cashedStarConfigs;
        private Dictionary<PlanetsType, PlanetConfigItem> _cashedObjectsConfigs;
        public Dictionary<int, GameObject> SystemsBehaviors { get; private set; }
        
        private StarSystemData[] _starSystemsData;
        public StarSystemData[] StarSystemsData => _starSystemsData;

        private Transform _galaxyTransform;
        private Vector3 _centerPosition;

        private WaitForEndOfFrame _waitForEndOfFrame;

        public event Action<int> OnStarSelected; 
        public static GalaxyBehavior Instance { get; private set; }
        
        private bool _locked = false;
        public bool Locked => _locked;
        
        private void Awake()
        {
            Instance = this;
            CachingConfig();
        }

        public void GenerateGalaxy()
        {
            if (_locked) return;
            
            _locked = true;
            Observable.FromMicroCoroutine(GenerateSystemsData). 
                Subscribe(_ => SpawnSystems());
        }

        private void CachingConfig()
        {
            _cashedStarConfigs = new Dictionary<StarType, StarConfigItem>();
            int length = starConfig.Items.Length;
            for (int i = 0; i < length; i++)
            {
                _cashedStarConfigs.Add(starConfig.Items[i].Type, starConfig.Items[i]);
            }
            
            _cashedObjectsConfigs = new Dictionary<PlanetsType, PlanetConfigItem>();
            length = planetConfig.Items.Length;
            for (int i = 0; i < length; i++)
            {
                _cashedObjectsConfigs.Add(planetConfig.Items[i].Type, planetConfig.Items[i]);
            }
        }

        public void SpawnSystems()
        {
            _galaxyTransform = transform;
            _centerPosition = _galaxyTransform.position;
            Observable.FromMicroCoroutine(SpawnStarSystems).Subscribe();
        }

        private IEnumerator GenerateSystemsData()
        {
            int systemsCount = starSystemsCount;
            _starSystemsData = new StarSystemData[systemsCount];
            for (int i = 0; i < systemsCount; i++)
            {
                Array array = Enum.GetValues(typeof(StarType));
                var starType = (StarType)Random.Range(0, array.Length);
                if(_cashedStarConfigs.TryGetValue(starType, out var config))
                {
                    int planetCount = Random.Range(config.planetsCount.Min, config.planetsCount.Max);
                    GeneratePlanets(planetCount, (planets) =>
                    {
                        var starSystem = new StarSystemData()
                        {
                            systemId = i,
                            type = starType,
                            position = Vector3.zero,
                            systemName = $"System #{i + 1}",
                            spaceObjectsData = planets
                        };
                        _starSystemsData[i] = starSystem;
                    });
                }
            }

            yield return null;
        }

        private void GeneratePlanets(int count, Action<PlanetData[]> onCompleted)
        {
            PlanetData[] planetsData = new PlanetData[count];
            System.Random random = new System.Random();
            
            Array array = Enum.GetValues(typeof(PlanetsType));
            for (int i = 0; i < count; i++)
            {
                var objectType = (PlanetsType)random.Next(0, array.Length);
                if(_cashedObjectsConfigs.TryGetValue(objectType, out var config))
                {
                    planetsData[i] = new PlanetData()
                    {
                        type = objectType,
                        objectName = $"Planet #{i + 1}",
                        objectVariableId = random.Next(0, config.Objects.Length)
                    };   
                }
            }
            
            onCompleted?.Invoke(planetsData);
        }

        public void LoadSystemsData(StarSystemData[] starSystemsData)
        {
            _starSystemsData = starSystemsData;
        }

        private IEnumerator SpawnStarSystems()
        {
            int length = _starSystemsData.Length;
            SystemsBehaviors = new Dictionary<int, GameObject>();
            for (int i = 0; i < length; i++)
            {
                if (_starSystemsData[i].position == Vector3.zero)
                {
                    while (true)
                    {
                        Vector3 position = GetRandomPosition(_centerPosition);
                        if (!HasObstacles(position))
                        {
                            var data = _starSystemsData[i];
                            data.position = position;
                            _starSystemsData[i] = data;
                            break;
                        }
                    }
                }

                SpawnStar(_starSystemsData[i]);
                
                yield return null;
            }
            _locked = false;
        }

        public void SelectSystem(int id)
        {
            if (SystemsBehaviors.ContainsKey(id))
            {
                OnStarSelected?.Invoke(id);
            }
        }

        public void ClearSelected()
        {
            OnStarSelected?.Invoke(-1);
        }

        private void SpawnStar(StarSystemData data)
        {
            if (_cashedStarConfigs.TryGetValue(data.type, out StarConfigItem configItem))
            {
                var behavior = Instantiate(starSystemPrefab, _galaxyTransform);
                behavior.GetComponent<StarSystemView>().Init(data.systemName);
                behavior.transform.position = data.position;
                behavior.GetComponent<ClickableObject>().OnClicked += () => OnSystemSelected(data.systemId);
                Instantiate(configItem.Object, behavior.transform);
                SystemsBehaviors.Add(data.systemId, behavior);
            }
        }

        private void OnSystemSelected(int systemId)
        {
            OnStarSelected?.Invoke(systemId);
        }

        public void Clear()
        {
            if(_locked) return;
            _locked = true;
            Observable.FromMicroCoroutine(RemoveStars).Subscribe();
        }

        private IEnumerator RemoveStars()
        {
            int count = SystemsBehaviors.Count;
            for (int i = 0; i < count; i++)
            {
                Destroy(SystemsBehaviors[i].gameObject);
                yield return null;
            }

            _locked = false;
        }

        private bool HasObstacles(Vector3 position)
        {
            return Physics.OverlapSphere(position, distanceBetweenStars, starSystemLayer).Length > 0;
        }

        private Vector3 GetRandomPosition(Vector3 position)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector2 randomPosition = (Vector2)position + randomDirection * Random.Range(0f, galaxyRadius);
            float randomY = Random.Range(-yPosOffset, yPosOffset);
            Vector3 spawnPosition = new Vector3(randomPosition.x, randomY, randomPosition.y) + _centerPosition;
            return spawnPosition;
        }
    }
}
