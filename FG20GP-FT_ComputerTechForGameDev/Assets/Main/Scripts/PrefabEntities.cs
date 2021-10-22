using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Main.Scripts
{
    public class PrefabEntities : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        public static Entity[] Entities;
        public GameObject[] prefabs;
        
        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                referencedPrefabs.Add(prefabs[i]);
            }

            Entities = new Entity[prefabs.Length];
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                Entities[i] = conversionSystem.GetPrimaryEntity(prefabs[i]);
            }
        }
    }

    public enum PrefabEntity
    {
        Player,
        Projectile,
        Enemy
    }
}