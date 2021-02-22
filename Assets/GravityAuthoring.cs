using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GravityData : IComponentData
{

}

public class GravityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new VelocityData());
    }
}
