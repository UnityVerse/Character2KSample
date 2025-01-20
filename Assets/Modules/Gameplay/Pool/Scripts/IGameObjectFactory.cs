using UnityEngine;

namespace Modules.Gameplay
{
    public interface IGameObjectFactory
    {
        GameObject Instantiate(
            string name,
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent
        );
    }
}