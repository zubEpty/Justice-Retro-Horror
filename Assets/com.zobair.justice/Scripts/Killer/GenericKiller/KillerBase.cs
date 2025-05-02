using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class KillerBase : MonoBehaviour
{

    [SerializeField]private List<GameObject> PatrolPoints = new List<GameObject>();
    public List<GameObject> GetPatrolPoints => PatrolPoints;

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
        StateMachine.CurrentState.AnimationTriggerEvent(triggerType);
    }

    #endregion

    #region State Machine Variables

    public StateMachine<KillerBase> StateMachine { get; set; }


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
        StateMachine = new StateMachine<KillerBase>();
        
        IdleState = new KillerIdleState(this, StateMachine);
        PatrolState = new KillerPatrolState(this, StateMachine);
        CaughtPlayerState_ = new KillerCaughtPlayerState(this,StateMachine);
        LeavingClueState = new KillerLeavingClueState(this,StateMachine);
    }

    protected virtual void Start()
    {
        // Initialize Scriptable Objects
        KillerIdleBaseInstance.Initialize(gameObject, this);
        KillerPatrolBaseInstance.Initialize(gameObject, this);
        KillerCaughtPlayerInstance.Initialize(gameObject, this);
        KillerLeavingClueInstance.Initialize(gameObject, this);

        // Set Initial State
        //Add a disabled state at the beginning for all the units including player
        

        // Assign Weapon
        /*if (weaponFactory != null)
        {
            weapon = weaponFactory.CreateWeapon();
        }

        // Perform initial weapon attack (optional behavior)
        weapon.Attack();

        // Initialize Health
        CurrentHealth = MaxHealth;

        // Initialize Scriptable Objects
        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);

        // Set Initial State
        //Add a disabled state at the beginning for all the units including player
        StateMachine.Initialize(AttackState);*/
    }

    protected virtual void Update()
    {
        StateMachine.CurrentState.FrameUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
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
        StateMachine.Initialize(InitialState);
    }
}
