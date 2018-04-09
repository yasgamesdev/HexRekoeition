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

    void Update()
    {
        Person playerPerson = PersonRepository.Instance.GetPlayerPerson();
        if(playerPerson.GetComponent<CommandComponent>().HaveCommand())
        {
            System.Random rand = new System.Random();
            CastleRepository.Instance.GetAllCastle().ForEach(x => x.AddSoldiers(rand));

            FactionRepository.Instance.GetAllFaction().ForEach(x => x.DeclareWar());

            PersonRepository.Instance.Update();
        }
    }
}
