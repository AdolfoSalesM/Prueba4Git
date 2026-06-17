using UnityEngine;

/// <summary>
/// PlayerCollectable:
/// Detecta objetos coleccionables al entrar en trigger.
/// </summary>
public class PlayerCollectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectable>(out var collectable))
        {
            collectable.Collect();
        }
    }
}