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
            if (prefabHorExtents > 0) return; //Already set

            prefabHorExtents = prefabInstance.transform.
                GetComponentInChildren<Collider>().bounds.extents.x;

            Debug.Log(string.Format("Pool extents: {0}", prefabHorExtents));
        }
        public float GetPrefabExtents() => prefabHorExtents;
    }

    [SerializeField] private Transform poolParent;
    [SerializeField] private List<Pool> poolList;
    [SerializeField] private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        InitializePools();
    }


    private void Start()
    {
        //InitializePools();
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
            poolDictionary.Add(poolTag, new Queue<GameObject>());

            //Create Container object to organize the pool
            parent = new GameObject(poolTag + "_Pool");
            parent.transform.parent = poolParent;// transform;

            for(int i=0; i< pool.poolSize; i++)
            {
                tmp = Instantiate(pool.poolPrefab, parent.transform);
                pool.SetPrefabExtents(tmp);
                tmp.SetActive(false);
                poolDictionary[poolTag].Enqueue(tmp);
            }

        }
    }

    /// <summary>
    /// Retrieves a single object from the pool
    /// </summary>
    /// <param name="tag">the pool we want to retrieve from</param>
    /// <returns></returns>
    public GameObject SpawnSingleElementFromPool(string tag, Vector3 position)
    {
        if (poolDictionary[tag] == null) return null;

        GameObject spawnObj = poolDictionary[tag].Dequeue();
        spawnObj.SetActive(true);
        spawnObj.transform.position=position;
        poolDictionary[tag].Enqueue(spawnObj);
        return spawnObj;
    }


    /// <summary>
    /// Retrieves a pack of objects from the pool
    /// </summary>
    /// <param name="_tag">the pool we want to retrieve from</param>
    /// <returns></returns>
    public GameObject[] SpawnPackFromPool(string _tag, int packSize, Vector3 basePosition , float offset = -1)
    {
        if (poolDictionary[_tag] == null)
        {
            Debug.LogError(string.Format("No pool with tag {0}", _tag));
            return null;
        }

        //Get preab extents (from collider)
        float horExtents = poolList.Find(p => p.GetPoolTag() == _tag).GetPrefabExtents();
            //GetItemHorizontalExtents(_tag);

        Vector3[] positions = CalculatePackPositions(basePosition, packSize, horExtents, offset);

        GameObject[] spawnObjPack = new GameObject[packSize];
        for (int i = 0; i < packSize; i++)
        {
            GameObject tmp = poolDictionary[_tag].Dequeue();
            spawnObjPack[i] = tmp;
            tmp.SetActive(true);

            tmp.transform.position = positions[i];

            poolDictionary[_tag].Enqueue(tmp);
        }

        return spawnObjPack;
    }

    public void ResetPool(string tag)
    {
        if (poolDictionary[tag] == null) return;

        GameObject tmp;
        for (int i = 0; i < poolDictionary[tag].Count; i++)
        {
            tmp = poolDictionary[tag].Dequeue();
            //tmp.transform.position
            tmp.SetActive(false);
        }
    }

    private float GetItemHorizontalExtents(string poolTag)
    {
        GameObject tmp = poolList.Find(p => p.GetPoolTag() == poolTag).poolPrefab;

        //IMPORTANT
        //Collider's bounding box has 0 extents if object is inactive or disabled
        //Docs: https://docs.unity3d.com/ScriptReference/Collider-bounds.html

        //GameObject tmp = poolDictionary[poolTag].Dequeue();
        //Debug.Log(tmp.name);
        //tmp.transform.GetChild(0);
        float radius = tmp.transform.GetChild(0).GetComponent<SphereCollider>().radius / 2;
        
        //Debug.Log(string.Format("Radius: {0}", radius));
        float horExtents = radius;
        //tmp.GetComponentInChildren<Collider>().bounds.extents.x;
        //poolDictionary[poolTag].Enqueue(tmp);
        return horExtents;
    }

    private Vector3[] CalculatePackPositions(Vector3 basePos, int packSize, float horExtents, float offset)
    {
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
