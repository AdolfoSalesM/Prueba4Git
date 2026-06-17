using UnityEngine;
using Pathfinding;

public class ZombieAICharacterControl : VersionedMonoBehaviour
{
    [SerializeField] private Animator animator;

    private IAstarAI ai;
    private Transform target;

    private void OnEnable ()
    {
        GameEvents.ChestOpened += OnChestOpened;
        GameEvents.GameOver += OnGameOver;

        ai = GetComponent<IAstarAI>();

        if (ai != null)
            ai.onSearchPath += Update;
    }

    private void OnDisable ()
    {
        GameEvents.ChestOpened -= OnChestOpened;
        GameEvents.GameOver -= OnGameOver;

        if (ai != null)
            ai.onSearchPath -= Update;
    }

    private void Awake()
    {
        ai = GetComponent<IAstarAI>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();

        if (player != null)
            target = player.transform;
    }

    private void Update()
    {
        if (target != null && ai != null)
            ai.destination = target.position;

        if (ai != null)
        {
            Vector3 velocity = ai.velocity;
            velocity.y = 0f;

            float speed = velocity.magnitude;

            animator.SetFloat("MoveSpeed", speed);

            if (ai.reachedEndOfPath)
            {
                animator.SetFloat("MoveSpeed", 0f);
            }
        }
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
        ai.isStopped = true;
        animator.SetFloat("MoveSpeed", 0f);
    }
}