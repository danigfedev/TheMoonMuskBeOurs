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
    }

    [SerializeField] private Transform poolParent;
    [SerializeField] private List<Pool> poolList;
    [SerializeField] private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
    }


    private void Start()
    {
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
            poolDictionary.Add(poolTag, new Queue<GameObject>());

            //Create Container object to organize the pool
            parent = new GameObject(poolTag + "_Pool");
            parent.transform.parent = poolParent;// transform;

            for(int i=0; i< pool.poolSize; i++)
            {
                tmp = Instantiate(pool.poolPrefab, parent.transform);
                tmp.SetActive(false);
                poolDictionary[poolTag].Enqueue(tmp);
            }

        }
    }

    /// <summary>
    /// Retrieves an object from the pool
    /// </summary>
    /// <param name="tag">the pool we want to retrieve from</param>
    /// <returns></returns>
    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        if (poolDictionary[tag] == null) return null;

        GameObject spawnObj = poolDictionary[tag].Dequeue();
        spawnObj.SetActive(true);
        spawnObj.transform.position=position;
        poolDictionary[tag].Enqueue(spawnObj);
        return spawnObj;
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

}
