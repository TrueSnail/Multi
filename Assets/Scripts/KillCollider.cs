using UnityEngine;

public class KillCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthControler hc))
        {
            hc.Hp = 0;
        }
    }
}
