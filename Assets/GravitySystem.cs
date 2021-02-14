using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(BuildPhysicsWorld))]
public class GravitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        int G = Spawner.G;

        Entities
            .WithBurst()
            .ForEach((ref PhysicsMass bodyMass, ref PhysicsVelocity bodyVelocity, in Translation pos) =>
            {
                Vector3 dir = -pos.Value;
                float distance = dir.magnitude;
                float force = G / Mathf.Pow(distance, 2);
                Vector3 acc = dir.normalized * force * dt;
                bodyVelocity.Linear += new float3(acc.x, acc.y, acc.z);

            }).Schedule();
    }
};
