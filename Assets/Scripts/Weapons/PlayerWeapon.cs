using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerWeaponStats))]
public class PlayerWeapon : Weapon
{
    public event System.Action OnPressedActionButton;
    public event System.Action OnCancelActionButton;

    public StatsController<PlayerWeaponStats> PlayerWeaponStatsController;

    private Quaternion BaseRotation = Quaternion.identity;
    private float TargetAngle;
    private float NoiseRandomness = -1.5f;
    private bool NoiseDirection = true;
    private float CurrentNoise = 0;
    private float CurrentNoiseSpeed;
    private float CurrentRecoil = 0;
    private PlayerWeaponStats Stats { get => PlayerWeaponStatsController.CurrentStats; }

    protected override void Awake()
    {
        base.Awake();
        PlayerWeaponStatsController = new(GetComponent<PlayerWeaponStats>());
    }

    protected virtual void Start()
    {
        CurrentNoiseSpeed = Stats.AccuracySpeed + Random.Range(-NoiseRandomness, NoiseRandomness);
        OnAfterShot += AddRecoil;
    }

    override protected void Update()
    {
        base.Update();
        UpdateCurrentNoise();
        UpdateTargetAngle();
        UpdateCurrentRecoil();
        UpdateRotation();
    }

    private void UpdateCurrentNoise()
    {
        float noiseMaxDegrees = -45 * Stats.TurningAccuracy + 45;
        CurrentNoise = Mathf.Clamp(CurrentNoise, -noiseMaxDegrees, noiseMaxDegrees);
        noiseMaxDegrees = NoiseDirection ? noiseMaxDegrees : -noiseMaxDegrees;

        CurrentNoise = Mathf.MoveTowards(CurrentNoise, noiseMaxDegrees, CurrentNoiseSpeed * Time.deltaTime);
        if (CurrentNoise == noiseMaxDegrees)
        {
            NoiseDirection = !NoiseDirection;
            CurrentNoiseSpeed = Stats.AccuracySpeed + Random.Range(-NoiseRandomness, NoiseRandomness);
        }
    }

    protected virtual void UpdateTargetAngle()
    {
        if (!IsOwner) return;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        targetPosition.z = 0;
        TargetAngle = Vector2.SignedAngle(Vector2.right, targetPosition - transform.position);
    }

    private void UpdateCurrentRecoil()
    {
        float currentDecay = Stats.RecoilDecay * Time.deltaTime + (CurrentRecoil * Random.Range(0f, 0.01f) / 20);
        CurrentRecoil = Mathf.Max(0, CurrentRecoil - currentDecay);
    }

    private void UpdateRotation()
    {
        BaseRotation = Quaternion.RotateTowards(BaseRotation, Quaternion.Euler(0, 0, TargetAngle), Stats.TurningSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, BaseRotation.eulerAngles.z + CurrentNoise + CurrentRecoil);
    }

    private void AddRecoil() => CurrentRecoil += Stats.RecoilStrenght;

    //Invoked by unity action system
    private void OnAction(InputValue value)
    {
        if (!IsOwner) return;

        if (value.isPressed) OnPressedActionButton?.Invoke();
        else OnCancelActionButton?.Invoke();
    }
}
