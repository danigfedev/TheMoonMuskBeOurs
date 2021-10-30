using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public int poolSize;
        public GameObject poolPrefab;
        public string poolTag; //Pool's tag will match pool base object's tag
        private float prefabHorExtents = -1;

        public void SetPoolTag(int poolIndex)
        {
            if(poolPrefab == null)
            {
                Debug.LogError(string.Format("Pool {0} has no assigned prefab", poolIndex));
                poolTag = "";
                return;
            }
            poolTag = poolPrefab.tag;
        }

        public string GetPoolTag() => poolTag;

        public void SetPrefabExtents(GameObject prefabInstance)
        {
            //IMPORTANT
            //Collider's bounding box has 0 extents if object is inactive or disabled
            //Docs: https://docs.unity3d.com/ScriptReference/Collider-bounds.html

            if (prefabHorExtents > 0) return; //Already set

            Collider col = prefabInstance.transform.
                GetComponentInChildren<Collider>();
            if(col != null)
                prefabHorExtents = col.bounds.extents.x;
        }
        public float GetPrefabExtents() => prefabHorExtents;
    }

    /*[SerializeField]*/ private Transform poolContainer;
    [SerializeField] private List<Pool> poolList;
    [SerializeField] private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolContainer = GameObject.FindGameObjectWithTag(TagList.poolContainerTag).transform;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        InitializePools();
    }

    /// <summary>
    /// Loops through each initialized pool and sets its tag from the assigned prefab
    /// </summary>
    public void UpdatePoolTags()
    {
        for (int i = 0; i < poolList.Count; i++)
            poolList[i].SetPoolTag(i);
    }

    /// <summary>
    /// Populates every pool in pool list
    /// </summary>
    private void InitializePools()
    {
        GameObject parent;
        GameObject tmp;
        foreach(Pool pool in poolList)
        {
            
            string poolTag = pool.GetPoolTag();
            Debug.Log(poolTag);
            poolDictionary.Add(poolTag, new Queue<GameObject>());

            //Create Container object to organize the pool
            parent = new GameObject(poolTag + "_Pool");
            parent.transform.parent = poolContainer;// transform;

            for(int i=0; i< pool.poolSize; i++)
            {
                tmp = Instantiate(pool.poolPrefab, parent.transform);
                tmp.AddComponent<PoolableObject>();
                pool.SetPrefabExtents(tmp);
                tmp.SetActive(false);
                poolDictionary[poolTag].Enqueue(tmp);
            }

        }
    }

    /// <summary>
    /// Sets object pool back to its initial state (all objects hidden and concentrated in the same position)
    /// </summary>
    /// <param name="tag"></param>
    public void ResetPool(string tag)
    {
        if (poolDictionary[tag] == null) return;

        GameObject tmp;
        for (int i = 0; i < poolDictionary[tag].Count; i++)
        {
            tmp = poolDictionary[tag].Dequeue();
            tmp.transform.localPosition = Vector3.zero;
            tmp.SetActive(false);
            poolDictionary[tag].Enqueue(tmp);
        }
    }


    /// <summary>
    /// Retrieves a single object from the pool
    /// </summary>
    /// <param name="tag">the pool we want to retrieve from</param>
    /// <returns></returns>
    public GameObject SpawnSingleElementFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary[tag] == null) return null;

        GameObject spawnObj = poolDictionary[tag].Dequeue();
        spawnObj.SetActive(true);
        spawnObj.transform.position=position;
        spawnObj.transform.localRotation = rotation;
        poolDictionary[tag].Enqueue(spawnObj);
        return spawnObj;
    }


    /// <summary>
    /// Retrieves a pack of objects from the pool
    /// </summary>
    /// <param name="_tag">the pool we want to retrieve from</param>
    /// <returns></returns>
    public GameObject[] SpawnPackFromPool(string _tag, int packSize, Vector3 basePosition, Quaternion rotation, float offset = -1)
    {
        //Debug.LogWarning(_tag);
        if (poolDictionary[_tag] == null)
        {
            Debug.LogError(string.Format("No pool with tag {0}", _tag));
            return null;
        }

        
        //Get preab extents (from collider)
        float horExtents = poolList.Find(p => p.GetPoolTag() == _tag).GetPrefabExtents();

        Vector3[] positions = CalculatePackPositions(basePosition, packSize, horExtents, offset);

        GameObject[] spawnObjPack = new GameObject[packSize];
        for (int i = 0; i < packSize; i++)
        {
            GameObject tmp = poolDictionary[_tag].Dequeue();
            spawnObjPack[i] = tmp;
            tmp.SetActive(true);

            tmp.transform.position = positions[i];
            tmp.transform.localRotation = rotation;

            poolDictionary[_tag].Enqueue(tmp);
        }
        

        return spawnObjPack;
    }

    public GameObject[] SpawnPackFromPool(string _tag, int packSize/*, Vector3 basePosition, Quaternion rotation, float offset = -1*/)
    {
        //Debug.LogWarning(_tag);
        if (poolDictionary[_tag] == null)
        {
            Debug.LogError(string.Format("No pool with tag {0}", _tag));
            return null;
        }

        GameObject[] spawnObjPack = new GameObject[packSize];
        for (int i = 0; i < packSize; i++)
        {
            GameObject tmp = poolDictionary[_tag].Dequeue();
            spawnObjPack[i] = tmp;
            tmp.SetActive(true);

            poolDictionary[_tag].Enqueue(tmp);
        }

        return spawnObjPack;
    }

    /// <summary>
    /// Given a number of items and a base position, linearly distributes the items along the X axis
    /// and around the given position based on their extents and an offset value to add some separation.
    /// </summary>
    /// <param name="basePos"></param>
    /// <param name="packSize"></param>
    /// <param name="horExtents"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3[] CalculatePackPositions(Vector3 basePos, int packSize, float horExtents, float offset)
    {
        if (packSize <= 0)
        {
            Debug.LogError("Invalid pack size. Plece introduce a value higher than 0");
            return null;
        }

        int initialIndex = 0;
        Vector3[] positions = new Vector3[packSize];

        if (packSize % 2 != 0)
        {
            initialIndex = 1;
            positions[0] = basePos;
        }


        for (int i = initialIndex; i <= packSize-2; i+=2)
        {
            float horDisplacement = (i + 1) * (horExtents + offset / 2);
            Vector3 displacement = Vector3.right * horDisplacement;
            positions[i] = basePos + displacement;
            positions[i + 1] = basePos - displacement;
        }

        return positions;
    }
}
