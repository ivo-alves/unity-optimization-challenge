using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct VelocityData : IComponentData
{
    public float3 Velocity;
}

public class VelocityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Vector3 _initialVelocity;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new VelocityData { Velocity = _initialVelocity });
    }
}
