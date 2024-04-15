using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    private void Start()
    {
        if (!NetworkObject.IsOwner) gameObject.SetActive(false);
    }
}
