using Unity.Netcode;
using UnityEngine;

public class HealthControler : NetworkBehaviour
{
    public delegate void HealthChangeDelegate(float oldHp, ref float newHp);
    public event HealthChangeDelegate OnHealthChange;

    public bool DestroyOnDeath;
    public float MaxHp;

    private float _Hp = new ();
    public float Hp
    {
        get => _Hp;
        set
        {
            value = Mathf.Clamp(value, 0, MaxHp);
            OnHealthChange?.Invoke(_Hp, ref value);
            if (value == 0 && DestroyOnDeath) DespawnObjectServerRpc();
            _Hp = value;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnObjectServerRpc() => NetworkObject.Despawn();

    private void Awake()
    {
        _Hp = MaxHp;
    }
}
