using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(BuildPhysicsWorld))]
public class GravitySystem : SystemBase
{
    EntityQuery _query;

    protected override void OnCreate()
    {
        base.OnCreate();

        _query = GetEntityQuery(typeof(VelocityData),
                                typeof(Translation),
                                ComponentType.ReadOnly<MassData>(),
                                ComponentType.ReadOnly<GravityData>());
    }

    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        int G = Spawner.G;
        NativeArray<Translation> sunsT = _query.ToComponentDataArray<Translation>(Allocator.TempJob);
        NativeArray<MassData> sunsM = _query.ToComponentDataArray<MassData>(Allocator.TempJob);

        Entities
            .WithBurst()
            .ForEach((ref VelocityData v, ref Translation t, in MassData m) =>
            {
                float3 totalForce = float3.zero;

                for (int i = 0; i < sunsT.Length; i++)
                {
                    float3 dir = sunsT[i].Value - t.Value;
                    float distance = distance = math.distance(sunsT[i].Value, t.Value);

                    if (distance > 25)
                    {
                        float force = G * m.Mass * sunsM[i].Mass / (distance * distance);
                        float3 acc = math.normalize(dir) * force;
                        totalForce += new float3(acc.x, acc.y, acc.z);
                    }
                }

                v.Velocity += (totalForce * dt) / m.Mass;
                t.Value += v.Velocity * dt;
            })
            .WithDisposeOnCompletion(sunsT)
            .WithDisposeOnCompletion(sunsM)
            .Schedule();
    }
};
