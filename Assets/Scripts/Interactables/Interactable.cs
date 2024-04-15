using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private static List<Interactable> InteractablesInRange = new();

    public event System.Action<GameObject> OnInteraction;
    public GameObject OnSelectObject;
    public bool CanInteract = true;
    private bool _IsSelected = false;
    public bool IsSelected
    {
        get => _IsSelected;
        private set
        {
            OnSelectObject.SetActive(value);
            _IsSelected = value;
        }
    }

    private GameObject Player;

    private void Awake()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.excludeLayers = ~LayerMask.GetMask("Players");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsLocalPlayer(collision.gameObject)) return;
        Player = collision.gameObject;
        InteractablesInRange.Add(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsLocalPlayer(collision.gameObject)) return;
        if (IsClosestInteractable()) IsSelected = true;
        else IsSelected = false;
    }

    private bool IsClosestInteractable()
    {
        Interactable closestInteractable = InteractablesInRange
            .Where((I) => I.CanInteract)
            .OrderBy((I) => Vector3.Distance(I.transform.position, Player.transform.position))
            .FirstOrDefault();
        return closestInteractable != null && closestInteractable == this;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsLocalPlayer(collision.gameObject)) return;
        InteractablesInRange.Remove(this);
        IsSelected = false;
    }

    private bool IsLocalPlayer(GameObject gameObject)
    {
        return gameObject.GetComponent<NetworkObject>().IsOwner;
    }

    //Invoked by unity input system
    private void OnInteract()
    {
        if (IsSelected) OnInteraction?.Invoke(Player);
    }
}
