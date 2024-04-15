using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawning : NetworkBehaviour
{
    private HealthControler HealthControlerRef;
    private List<Vector3> SpawnPositions;

    private void Start()
    {
        SpawnPositions = GameObject.FindGameObjectWithTag("SpawnPoints").transform.GetComponentsInChildren<Transform>().ToList().Skip(1).Select((t) => t.position).ToList();

        HealthControlerRef = GetComponent<HealthControler>();
        HealthControlerRef.OnHealthChange += PlayerDealtDamage;

        RandomizePosition();
    }

    private void PlayerDealtDamage(float oldHp, ref float newHp)
    {
        if (newHp <= 0)
        {
            newHp = HealthControlerRef.MaxHp;
            RandomizePosition();
        }
    }

    private void RandomizePosition()
    {
        if (!IsOwner) return;

        transform.position = SpawnPositions[Random.Range(0, SpawnPositions.Count)];
    }
}
