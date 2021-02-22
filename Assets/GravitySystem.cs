using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Profiling;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(BuildPhysicsWorld))]
public class GravitySystem : SystemBase
{
    EntityQuery _query;
    ProfilerMarker _marker;

    protected override void OnCreate()
    {
        base.OnCreate();

        _marker = new ProfilerMarker("Gravity System Marker");

        _query = GetEntityQuery(typeof(PhysicsVelocity),
            ComponentType.ReadOnly<PhysicsMass>(),
            ComponentType.ReadOnly<Translation>());
    }

    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        int G = Spawner.G;
        ProfilerMarker marker = _marker;

        NativeArray<Translation> translations = _query.ToComponentDataArray<Translation>(Allocator.TempJob);
        NativeArray<PhysicsMass> masses = _query.ToComponentDataArray<PhysicsMass>(Allocator.TempJob);

        Entities
            .WithBurst()
            .ForEach((ref PhysicsVelocity v, in PhysicsMass m, in Translation t) =>
            {
                marker.Begin();
                float3 totalForce = float3.zero;

                for (int i = 0; i < translations.Length; i++)
                {
                    float3 dir = translations[i].Value - t.Value;
                    float distance = distance = math.distance(translations[i].Value, t.Value);

                    if (distance > 1)
                    {
                        float force = G * (1 / masses[i].InverseMass) / (distance * distance);
                        float3 acc = math.normalize(dir) * force;
                        totalForce += new float3(acc.x, acc.y, acc.z);
                    }
                }

                v.Linear += totalForce * dt;

                marker.End();
            })
            .WithDisposeOnCompletion(translations)
            .WithDisposeOnCompletion(masses)
            .Schedule();
    }
};
