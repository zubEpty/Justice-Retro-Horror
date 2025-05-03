using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class KillerBase : MonoBehaviour
{
    public List<GameObject> PatrolPoints;

    protected int currentPatrolIndex = 0;

    public Animator anim;

    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Rigidbody2D RB { get; set; }
    public bool IsFacingRight { get; set; } = true;

    #region Player Tracking
    public Transform PlayerTransform { get; private set; } // Reference to the player's Transform
    #endregion

    #region IdleVariables
    // Add any additional variables for idle logic here.
    #endregion

    #region Animation Trigger

    
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        KillerStateMachine.CurrentState.AnimationTriggerEvent(triggerType);
    }

    #endregion

    #region State Machine Variables

    public StateMachine<KillerBase> KillerStateMachine { get; set; }


    public KillerIdleState IdleState { get; set; }
    public KillerPatrolState PatrolState { get; set; }    
    public KillerCaughtPlayerState CaughtPlayerState_ { get; set; }    
    public KillerLeavingClueState LeavingClueState { get; set; }    
    #endregion

    #region ScriptableObject Variables

    [SerializeField] private KillerIdleSOBase KillerIdleBase;
    [SerializeField] private KillerPatrolSOBase KillerPatrolBase;
    [SerializeField] private KillerCaughtPlayerSOBase KillerCaughtPlayerBase;
    [SerializeField] private KillerLeavingClueSOBase KillerLeavingClueBase;
    public KillerIdleSOBase KillerIdleBaseInstance { get; set; }
    public KillerPatrolSOBase KillerPatrolBaseInstance { get; set; }
    public KillerCaughtPlayerSOBase KillerCaughtPlayerInstance { get; set; }
    public KillerLeavingClueSOBase KillerLeavingClueInstance { get; set; }

    
    #endregion

    #region Experimental Variables
    protected bool isAttacking;
    #endregion

    #region NavMeshData
    private NavMeshAgent m_Agent;

    public NavMeshAgent GetNavAgent => m_Agent;
    #endregion


    public virtual void OnEnable()
    {
        RB = GetComponent<Rigidbody2D>();
        m_Agent = GetComponent<NavMeshAgent>();
        
    }
    private void Awake()
    {
        // Instantiate Scriptable Objects
        KillerIdleBaseInstance = Instantiate(KillerIdleBase);
        KillerPatrolBaseInstance = Instantiate(KillerPatrolBase);
        KillerCaughtPlayerInstance = Instantiate(KillerCaughtPlayerBase);
        KillerLeavingClueInstance = Instantiate(KillerLeavingClueBase);

        // Initialize State Machine
        KillerStateMachine = new StateMachine<KillerBase>();
        
        IdleState = new KillerIdleState(this, KillerStateMachine);
        PatrolState = new KillerPatrolState(this, KillerStateMachine);
        CaughtPlayerState_ = new KillerCaughtPlayerState(this, KillerStateMachine);
        LeavingClueState = new KillerLeavingClueState(this, KillerStateMachine);
    }

    protected virtual void Start()
    {
        // Initialize Scriptable Objects
        KillerIdleBaseInstance.Initialize(gameObject, this);
        KillerPatrolBaseInstance.Initialize(gameObject, this);
        KillerCaughtPlayerInstance.Initialize(gameObject, this);
        KillerLeavingClueInstance.Initialize(gameObject, this);      
    }

    protected virtual void Update()
    {
        KillerStateMachine.CurrentState.FrameUpdate();
    }

    protected virtual void FixedUpdate()
    {
        KillerStateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual void ExecuteNavMeshAction()
    {
        Debug.LogError("exicuting NavMesh Action");
    }

    #region Health/Die

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    #endregion

    #region Movement Function

    public void MoveEnemy(Vector2 velocity)
    {
        RB.linearVelocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (velocity.x > 0 && !IsFacingRight || velocity.x < 0 && IsFacingRight)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 rotator = new Vector3(transform.rotation.x, IsFacingRight ? 0 : 180, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }

    #endregion

    #region Gizmos/Tools

    public float DetectPlayerDistanceThreshold = 5f;
    public Color DetectionCircleColor = Color.blue;

    public float AttackDistanceThreshold = 5f;
    public Color AttackCircleColor = Color.red;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = AttackCircleColor;
        Gizmos.DrawWireSphere(transform.position, AttackDistanceThreshold);
        Gizmos.color = DetectionCircleColor;
        Gizmos.DrawWireSphere(transform.position, DetectPlayerDistanceThreshold);
    }

    #endregion

    protected void StateMachineIgnition(StateBase<KillerBase> InitialState)
    {
        KillerStateMachine.Initialize(InitialState);
    }
}

[Serializable]
public class PatrolPointData
{
    public GameObject patrolPointPos;
    public GameObject InteractableObject;
}
