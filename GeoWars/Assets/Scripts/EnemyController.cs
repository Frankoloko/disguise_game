using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private PlayerInstance _playerInstance;
    private EnemyState _enemyState;
    private FieldOfView _fieldOfView;
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float minRoamingDistance = 1f;
    [SerializeField] private float maxRoamingDistance = 10f;

    [SerializeField] private Transform[] waypoints;
    private int _waypointIndex = 0;

    private Vector3 _startingPosition;
    private Vector3 _roamingPosition;
    private Vector3 _lastKnownPosition;

    [SerializeField] private float timeToPause = 3f;

    [SerializeField] private Animator animatorController;
    private static readonly int DrawWeapon = Animator.StringToHash("DrawWeapon");
    private static readonly int SheatheWeapon = Animator.StringToHash("SheatheWeapon");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Awake()
    {
        _enemyState = GetComponent<EnemyState>();
        _fieldOfView = GetComponent<FieldOfView>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _playerInstance = PlayerInstance.Instance;

        _startingPosition = transform.position;

        if (!_enemyState.shouldPatrol)
        {
            _roamingPosition = GetRoamingPosition(_startingPosition);
            _navMeshAgent.SetDestination(_roamingPosition);

            StartCoroutine(Wander());
        }
        else
        {
            _navMeshAgent.SetDestination(waypoints[_waypointIndex].position);

            StartCoroutine(Patrol());
        }
    }

    private void BecomeAwareOnTakeDamage()
    {
        _navMeshAgent.SetDestination(_playerInstance.transform.position);
    }

    private void Update()
    {
        // Need to do this because when the enemy bumps into the player /
        // vice versa it lifts off the navmesh and glitches
        if (transform.position.y > 0.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        IsPlayerInView();
    }

    private void IsPlayerInView()
    {
        switch (_enemyState.enemyState)
        {
            case EnemyState.State.Wandering:
                if (_fieldOfView.VisibleTarget != null)
                {
                    _enemyState.enemyState = EnemyState.State.Chasing;
                    StopAllCoroutines();
                    StartCoroutine(FollowTarget());
                }

                break;
            case EnemyState.State.Patrolling:
                if (_fieldOfView.VisibleTarget != null)
                {
                    _enemyState.enemyState = EnemyState.State.Chasing;
                    StopAllCoroutines();
                    StartCoroutine(FollowTarget());
                }

                break;
            case EnemyState.State.Chasing:
                if (_fieldOfView.VisibleTarget != null)
                {
                    StartCoroutine(IsPlayerInAttackRange());
                }
                else
                {
                    _enemyState.enemyState = EnemyState.State.Searching;
                    StopAllCoroutines();
                    StartCoroutine(Search());
                }

                break;
            case EnemyState.State.Attacking:
                if (_fieldOfView.VisibleTarget == null)
                {
                    _enemyState.enemyState = EnemyState.State.Searching;
                    StopAllCoroutines();
                    StartCoroutine(Search());
                }

                break;
            case EnemyState.State.Searching:
                if (_fieldOfView.VisibleTarget != null)
                {
                    _enemyState.enemyState = EnemyState.State.Chasing;
                    StopAllCoroutines();
                    StartCoroutine(FollowTarget());
                }

                break;
        }
    }

    private Vector3 GetRoamingPosition(Vector3 originPosition)
    {
        float roamingDistance = Random.Range(minRoamingDistance, maxRoamingDistance);

        return originPosition +
               new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * roamingDistance;
    }

    private IEnumerator Wander()
    {
        animatorController.SetBool(DrawWeapon, false);
        animatorController.SetTrigger(SheatheWeapon);

        while (_enemyState.enemyState == EnemyState.State.Wandering)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _navMeshAgent.destination);

            if (distanceToTarget < attackRange)
            {
                yield return new WaitForSeconds(timeToPause);
                _roamingPosition = GetRoamingPosition(_startingPosition);
                _navMeshAgent.SetDestination(_roamingPosition);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Patrol()
    {
        animatorController.SetBool(DrawWeapon, false);
        animatorController.SetTrigger(SheatheWeapon);

        while (_enemyState.enemyState == EnemyState.State.Patrolling)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _navMeshAgent.destination);

            if (distanceToTarget < attackRange)
            {
                yield return new WaitForSeconds(timeToPause);

                if (_waypointIndex < waypoints.Length - 1)
                {
                    _waypointIndex++;
                }
                else
                {
                    _waypointIndex = 0;
                }

                _navMeshAgent.SetDestination(waypoints[_waypointIndex].position);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Search()
    {
        int count = 0;

        while (_enemyState.enemyState == EnemyState.State.Searching)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _navMeshAgent.destination);

            if (distanceToTarget < attackRange && count < 3)
            {
                count++;
                yield return new WaitForSeconds(timeToPause);
                _roamingPosition = GetRoamingPosition(_lastKnownPosition);
                _navMeshAgent.SetDestination(_roamingPosition);
            }
            else if (count >= 3)
            {
                yield return new WaitForSeconds(timeToPause);
                _roamingPosition = _startingPosition;
                _navMeshAgent.SetDestination(_roamingPosition);

                if (!_enemyState.shouldPatrol)
                {
                    _enemyState.enemyState = EnemyState.State.Wandering;
                    StopAllCoroutines();
                    StartCoroutine(Wander());
                }
                else
                {
                    _enemyState.enemyState = EnemyState.State.Patrolling;
                    StopAllCoroutines();
                    StartCoroutine(Patrol());
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator IsPlayerInAttackRange()
    {
        while (_enemyState.enemyState == EnemyState.State.Chasing)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _playerInstance.transform.position);

            if (distanceToTarget <= attackRange)
            {
                _enemyState.enemyState = EnemyState.State.Attacking;
                _navMeshAgent.SetDestination(transform.position);
                StartCoroutine(AttackPlayer());
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator AttackPlayer()
    {
        while (_enemyState.enemyState == EnemyState.State.Attacking)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _playerInstance.transform.position);

            if (distanceToTarget > attackRange)
            {
                _enemyState.enemyState = EnemyState.State.Chasing;
                StopAllCoroutines();
                StartCoroutine(FollowTarget());
            }
            else
            {
                animatorController.SetTrigger(Attack);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FollowTarget()
    {
        animatorController.SetBool(DrawWeapon, true);

        while (_enemyState.enemyState == EnemyState.State.Chasing)
        {
            _lastKnownPosition = _playerInstance.transform.position;
            _navMeshAgent.SetDestination(_lastKnownPosition);
            yield return new WaitForEndOfFrame();
        }
    }
}