using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct MassData : IComponentData
{
    public float Mass;
}

public class MassAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float _mass;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new MassData { Mass = _mass });
    }
}
