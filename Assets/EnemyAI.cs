using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private enum State {
        Roaming, 
        ChaseTarget,
    }

    public float moveSpeed = 5f;
    public Vector3 target;
    public float nextWaypointDistance = .5f;
    public float stopChasingRange = 15f;
    public float reachedPositionDistance = 2f;
    public float targetRange = 15f;
    public float attackRange = 5f;
    private State state;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Vector3 startingPosition;
    Vector3 roamPosition;

    Seeker seeker;
    public Rigidbody2D rb;
    public Vector2 lookDir;
    public Animator animator;

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    Vector2 movement;
    public int currentEra;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        target = roamPosition;

        spriteRenderer = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        state = State.Roaming;

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    Vector3 GetRoamingPosition()
    {
        return transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * Random.Range(2f, 5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        seeker.StartPath(rb.position, target, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                MoveTo();

                if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
                {
                    roamPosition = GetRoamingPosition();
                    target = roamPosition;
                }
                FindTarget();
                break;
            case State.ChaseTarget:
                target = GameObject.Find("Player").transform.position;
                MoveTo();

                // Look at player
                lookDir = (Vector2)GameObject.Find("Player").transform.position - rb.position;

                float attackRange = 10f;
                if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < attackRange)
                {
                    // Attack

                }

                if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) > stopChasingRange)
                {
                    // Stop Chasing
                    roamPosition = GetRoamingPosition();
                    target = roamPosition;
                    state = State.Roaming;
                }
                break;
        }
    }

    // Update is called once per frame
    // Probably split this up
    void FixedUpdate()
    {
        movement = rb.velocity;

        currentEra = GameObject.Find("RewindManager").GetComponent<RewindManager>().currentEra;
        spriteRenderer.sprite = sprites[currentEra];

        animator.SetInteger("CurrentEra", currentEra);

        if (movement != Vector2.zero)
            animator.SetInteger("Speed", 1);
        else
            animator.SetInteger("Speed", -1);
    }

    void MoveTo()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void FindTarget()
    {
        if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }
}
