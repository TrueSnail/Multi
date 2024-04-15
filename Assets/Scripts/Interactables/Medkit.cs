using Unity.Netcode;
using UnityEngine;

public class Medkit : NetworkBehaviour
{
    public int HealAmount = 50;

    private void Start()
    {
        GetComponent<Interactable>().OnInteraction += UseMedkit;
    }

    private void UseMedkit(GameObject player)
    {
        HealthControler hc = player.GetComponent<HealthControler>();
        if (hc.Hp == hc.MaxHp) return;

        hc.Hp += HealAmount;
        DestroyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyServerRpc()
    {
        NetworkObject.Despawn();
    }
}
