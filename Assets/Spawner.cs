using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static int G = 45000;
    static int _spawnCount;

    [SerializeField] GameObject _prefab;
    [SerializeField] int _spawnQuanity = 5;
    [SerializeField] float _initialSpeed = 25;
    [SerializeField] Text _text;
    [SerializeField] Mesh _mesh;
    [SerializeField] UnityEngine.Material _material;
    [SerializeField] float timePerSpawn = 1;

    BlobAssetStore _blob;
    Entity _entityPrefab;
    float _timeSinceSpawn;

    private void Start()
    {
        _blob = new BlobAssetStore();
        _entityPrefab = CreateEntityFromPrefab();
    }

    private void OnDestroy()
    {
        _blob.Dispose();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            G += 5000;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            G -= 5000;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Time.timeScale = 1;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Time.timeScale = 2;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            Time.timeScale = 5;

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Backspace))
            G *= -1;

        if (Input.GetKey(KeyCode.Space))
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

        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Vector3 initialVelocity = transform.forward * _initialSpeed;

        int total = _spawnQuanity * _spawnQuanity;
        int index = 0;
        NativeArray<Entity> cubes = new NativeArray<Entity>(total, Allocator.TempJob);
        manager.Instantiate(_entityPrefab, cubes);

        for (int i = 0; i < _spawnQuanity; i++)
        {
            for (int j = 0; j < _spawnQuanity; j++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(0, i * 2, -j * 2);

                manager.SetComponentData(cubes[index], new Translation { Value = spawnPosition });
                manager.SetComponentData(cubes[index], new PhysicsVelocity { Linear = initialVelocity });
                index++;
            }
        }

        _spawnCount += total;
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

    //Entity CreateCubeEntity()
    //{
    //    EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

    //    EntityArchetype archetype = manager.CreateArchetype
    //    (
    //        typeof(Translation),
    //        typeof(Rotation),
    //        typeof(LocalToWorld),
    //        typeof(RenderMesh),
    //        typeof(RenderBounds),
    //        typeof(PhysicsVelocity),
    //        typeof(PhysicsDamping),
    //        typeof(PhysicsGravityFactor),
    //        typeof(PhysicsCollider),
    //        typeof(PhysicsMass)
    //    );

    //    Entity entity = manager.CreateEntity(archetype);

    //    manager.AddComponentData(entity, new Translation { });
    //    manager.AddComponentData(entity, new Rotation { });
    //    manager.AddComponentData(entity, new LocalToWorld());
    //    manager.SetComponentData(entity, new RenderBounds { Value = _mesh.bounds.ToAABB() });
    //    manager.AddSharedComponentData(entity, new RenderMesh
    //    {
    //        mesh = _mesh,
    //        material = _material
    //    });

    //    // Physics
    //    Vector3 initialVelocity = transform.forward * _initialSpeed;
    //    manager.AddComponentData(entity, new PhysicsVelocity
    //    {
    //        Linear = new float3(initialVelocity),
    //        Angular = new float3(0, 0, 0)
    //    });

    //    manager.AddComponentData(entity, new PhysicsDamping()
    //    {
    //        Linear = 0.00f,
    //        Angular = 0.005f
    //    });

    //    manager.AddComponentData(entity, new PhysicsGravityFactor { Value = 0 });

    //    PhysicsCollider collider = new PhysicsCollider
    //    {
    //        Value = Unity.Physics.BoxCollider.Create(new BoxGeometry
    //        {
    //            Center = new float3(0f, 0.5f, 0f),
    //            Orientation = quaternion.identity,
    //            Size = new float3(1f, 1f, 1f)
    //        })
    //    };

    //    manager.AddComponentData(entity, collider);
    //    manager.AddComponentData(entity, PhysicsMass.CreateDynamic(collider.MassProperties, 1f));

    //    return entity;
    //}

}
