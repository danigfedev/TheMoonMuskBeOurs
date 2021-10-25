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

    /// <summary>
    /// Loops through each initialized pool and sets its tag from the assigned prefab
    /// </summary>
    public void UpdatePoolTags()
    {
        for (int i = 0; i < poolList.Count; i++)
            poolList[i].SetPoolTag(i);
    }


    public List<Pool> poolList;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        
    }

}
