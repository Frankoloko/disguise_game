using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum State
    {
        Wandering,
        Patrolling,
        Chasing,
        Attacking,
        Searching
    };

    public State enemyState;

    [Tooltip("If set to true the enemy will patrol between two points, " +
             "otherwise it will wander within a particular range of where it's placed / spawned")]
    [SerializeField]
    public bool shouldPatrol = false;

    private void Awake()
    {
        enemyState = shouldPatrol ? State.Patrolling : State.Wandering;
    }
}