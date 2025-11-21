using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
{
    private List<PoolInfo> _objectPools = new();

    private List<Transform> _poolParents = new();

    protected override void Awake()
    {
        base.Awake();
        
        for (int i = 0; i < Enum.GetNames(typeof(PoolType)).Length; i++)
        {
            var poolParentObject = new GameObject(((PoolType)i).ToString()).transform;
            poolParentObject.SetParent(transform);
            _poolParents.Add(poolParentObject);
        }
    }

    public GameObject Get(GameObject objectToGet, PoolType poolType = PoolType.Others, Action<GameObject> onCreate = null, Action<GameObject> onGet = null)
    {
        var pool = _objectPools.FirstOrDefault(p => p.PoolName == objectToGet.name);

        if (pool == null)
        {
            pool = new PoolInfo() { PoolName = objectToGet.name };
            _objectPools.Add(pool);
        }

        var result = pool.ObjectsInPool.FirstOrDefault();

        if (!result)
        {
            result = Instantiate(objectToGet);
            SetParentObject(result, poolType);
            onCreate?.Invoke(result);
        }

        else
        {
            result.SetActive(true);
            pool.ObjectsInPool.Remove(result);
            onGet?.Invoke(result);
        }

        return result;
    }

    public void Release(GameObject objectToRelease, Action<GameObject> onRelease = null)
    {
        var objName = objectToRelease.name.Substring(0, objectToRelease.name.Length - 7);
        var pool = _objectPools.Find(p =>
            p.PoolName == objName);

        if (pool == null)
        {
            Debug.LogWarning($"No pool found for name: {objName}");
            return;
        }

        objectToRelease.SetActive(false);
        pool.ObjectsInPool.Add(objectToRelease);
        onRelease?.Invoke(objectToRelease);
    }

    private void SetParentObject(GameObject obj, PoolType poolType)
    {
        if (_poolParents[(int)poolType] != null)
            obj.transform.SetParent(_poolParents[(int)poolType]);
    }
}

public class PoolInfo
{
    public string PoolName;
    public List<GameObject> ObjectsInPool = new();
}

public enum PoolType
{
    Enemy,
    Others
}