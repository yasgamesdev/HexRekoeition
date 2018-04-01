using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    private static GameInstance instance = null;

    public static GameInstance Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SLGCore.Instance.GenerateWorld(HexMetrics.chunkCountX * HexMetrics.chunkSizeX, HexMetrics.chunkCountZ * HexMetrics.chunkSizeZ, HexMetrics.worldSize);
    }
}
