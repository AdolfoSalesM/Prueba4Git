using UnityEngine;

public class GhostAICharacterControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform target;


    private void OnEnable ()
    {
        GameEvents.ChestOpened += OnChestOpened;
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable ()
    {
        GameEvents.ChestOpened -= OnChestOpened;
        GameEvents.GameOver -= OnGameOver;
    }

    private void Start()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();

        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnChestOpened()
    {
        Stop();
    }

    private void OnGameOver()
    {
        Stop();
    }

    private void Stop()
    {
        moveSpeed = 0f;
    }
}