using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BulletStats))]
[RequireComponent(typeof(WeaponStats))]
public class Weapon : NetworkBehaviour
{
    public event System.Action OnCooldownEnd;
    public event System.Action OnAfterShot;
    public event System.Action OnBeforeShot;
    public event System.Action<Bullet> OnBulletSpawn;

    public StatsController<WeaponStats> WeaponStatsController;
    public StatsController<BulletStats> BulletStatsController;
    public float RemainingCooldown { private set; get; } = 0;

    [SerializeField]
    private GameObject BulletSpawnPoint;
    [SerializeField]
    private Rigidbody2D ShootingRigidbody;
    [SerializeField]
    private GameObject Bullet;

    private WeaponStats Stats { get => WeaponStatsController.CurrentStats; }

    protected virtual void Awake()
    {
        WeaponStatsController = new(GetComponent<WeaponStats>());
        BulletStatsController = new(GetComponent<BulletStats>());
    }

    protected virtual void Update()
    {
        if (RemainingCooldown > 0) UpdateCooldown();
    }

    private void UpdateCooldown()
    {
        RemainingCooldown -= Time.deltaTime;
        if (RemainingCooldown <= 0)
        {
            RemainingCooldown = 0;
            OnCooldownEnd?.Invoke();
        }
    }

    protected virtual void Shoot(Vector2 direction, bool ignoreCooldown = false)
    {
        if (RemainingCooldown > 0 && !ignoreCooldown) return;

        OnBeforeShot?.Invoke();
        for (int i = 0; i < Stats.BulletCount; i++)
        {
            float bulletSpread = Random.Range(-Stats.ShootSpread / 2, Stats.ShootSpread / 2);
            direction = Quaternion.Euler(0, 0, bulletSpread) * direction.normalized;

            SpawnBulletServerRpc(direction, BulletSpawnPoint.transform.position);

            ShootingRigidbody.AddForce(-direction * (Stats.ShootKnockback / Stats.BulletCount));
        }
        OnAfterShot?.Invoke();

        SetCooldown(Stats.FireRate);
    }

    protected virtual void SetCooldown(float fireRate)
    {
        if (fireRate <= 0)
        {
            Debug.LogWarning($"Detected invalid fire rate ({fireRate}), this weapon will cese to shoot!");
            RemainingCooldown = float.PositiveInfinity;
        }
        else
        {
            RemainingCooldown = 1 / fireRate;
        }
    }

    [ServerRpc]
    protected virtual void SpawnBulletServerRpc(Vector2 direction, Vector3 SpawnPos)
    {
        GameObject bulletObj = Instantiate(Bullet, SpawnPos, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Setup(direction * Stats.ShotSpeed, (BulletStats)BulletStatsController.CurrentStats.Clone());
        OnBulletSpawn?.Invoke(bullet);

        List<Collider2D> colliders = new();
        ShootingRigidbody.GetAttachedColliders(colliders);
        colliders.ForEach(c => bullet.IgnoreCollision(c));
        
        bulletObj.GetComponent<NetworkObject>().Spawn();
    }
}
