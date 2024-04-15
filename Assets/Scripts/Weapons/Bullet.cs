using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public StatsController<BulletStats> BulletStatsController;
    public float CurrentLivetime { private set; get; }
    public float MaxLivetime { private set; get; }

    private Rigidbody2D Rb;
    private Collider2D Coll;
    private BulletStats Stats;
    private bool Collided = false;

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        UpdateStats();
        transform.up = Rb.velocity;
        CurrentLivetime += Time.fixedDeltaTime;
        if (CurrentLivetime > MaxLivetime) Destroy(gameObject);
    }

    private void UpdateStats()
    {
        Stats = BulletStatsController.CurrentStats;
        transform.localScale = new Vector3(Stats.Size, Stats.Size, 1);
        Rb.gravityScale = Stats.Gravity;
        Rb.mass = Stats.Mass;
        MaxLivetime = Stats.Livetime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsOwner || Collided) return;

        collision.gameObject.TryGetComponent(out HealthControler healthControler);
        if (healthControler != null) HandleCollisionClientRpc(Stats.Damage, healthControler.NetworkObject);
        Collided = true;
        NetworkObject.Despawn();
    }

    [ClientRpc]
    private void HandleCollisionClientRpc(float damage, NetworkObjectReference obj)
    {
        obj.TryGet(out NetworkObject netObject);
        netObject.GetComponent<HealthControler>().Hp -= damage;
    }

    public void Setup(Vector2 velocity, BulletStats stats)
    {
        Rb = GetComponent<Rigidbody2D>();
        Rb.velocity = velocity;
        transform.up = velocity;
        Coll = GetComponent<Collider2D>();
        BulletStatsController = new(stats);
    }

    public void IgnoreCollision(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, Coll);
    }
}
