using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static int G = 9;
    static int _spawnCount;

    [SerializeField] GameObject _prefab;
    [SerializeField] int _spawnQuanity = 5;
    [SerializeField] int _spawnSize = 5;
    [SerializeField] float _initialSpeed = 25;
    [SerializeField] Text _text;
    [SerializeField] float timePerSpawn = 1;
    [SerializeField] KeyCode spawnKey;

    BlobAssetStore _blob;
    Entity _entityPrefab;
    float _timeSinceSpawn;
    EntityManager _entityManager;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _blob = new BlobAssetStore();
        _entityPrefab = CreateEntityFromPrefab();
    }

    void OnDestroy()
    {
        if (_blob != null)
            _blob.Dispose();
    }

    void Update()
    {
        if (Input.GetKey(spawnKey))
        {
            Spawn();
            _text.text = "object count: " + _spawnCount;
        }

        _timeSinceSpawn += Time.deltaTime;
    }

    void Spawn()
    {
        if (_timeSinceSpawn <= timePerSpawn)
            return;

        Vector3 initialVelocity = transform.forward * _initialSpeed;

        int index = 0;
        NativeArray<Entity> cubes = new NativeArray<Entity>(_spawnQuanity, Allocator.TempJob);
        _entityManager.Instantiate(_entityPrefab, cubes);

        for (int i = 0; i < _spawnQuanity; i++)
        {
            Vector3 spawnPosition = transform.position + (Random.insideUnitSphere * _spawnSize);
            Translation translation = new Translation { Value = spawnPosition };

            _entityManager.SetComponentData(cubes[index], translation);
            _entityManager.SetComponentData(cubes[index], new VelocityData { Velocity = initialVelocity });

            index++;
        }

        _spawnCount += _spawnQuanity;
        _timeSinceSpawn = 0;
        cubes.Dispose();
    }

    Entity CreateEntityFromPrefab()
    {
        GameObjectConversionSettings settings = new GameObjectConversionSettings(World.DefaultGameObjectInjectionWorld,
                                                        GameObjectConversionUtility.ConversionFlags.AddEntityGUID,
                                                        _blob);
        Entity entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(_prefab, settings);
        return entity;
    }

}
