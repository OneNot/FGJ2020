using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public List<GameObject> Levels;
    public static EnemyHandler defaultInstance;

    private void Awake()
    {
        defaultInstance = this;
    }

    private void Start()
    {
        foreach(GameObject level in Levels)
            for(int i = 0; i < level.transform.childCount; i++)
                level.transform.GetChild(i).gameObject.SetActive(false);

        ActivateLevelEnemies(0);
    }

    public void ActivateLevelEnemies(int level)
    {
        for(int i = 0; i < Levels[level].transform.childCount; i++)
            Levels[level].transform.GetChild(i).gameObject.SetActive(true);
    }
}
