using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class AssetStorage : MonoBehaviour 
{
    public GameObject[] rewardPrefabs;
    public GameObject[] enemyPrefabs;

    public void loadRewardAsset(string name,Action<GameObject> onAssetLoaded)
    {
        GameObject loadedObj = rewardPrefabs.Single(x => x.name.Equals(name));
        if (onAssetLoaded != null)
            onAssetLoaded(loadedObj);
    }

    public void loadEnemyAsset(string name, Action<GameObject> onAssetLoaded)
    {
        GameObject loadedObj = enemyPrefabs.Single(x => x.name.Equals(name));
        if (onAssetLoaded != null)
            onAssetLoaded(loadedObj);
    }

    private void Start()
    {
        test(name, (x) =>
        {
            x = 2;
            
            onMissed(x);
        });

        //test(name, onMissed);
    }


    public void test(string testName, Action<int> onMiss)
    {
        Debug.Log(testName);
        
        onMiss(3);
    }

    private void onMissed(int test)
    {
        Debug.Log("Result: " + test);
    }
}
