using System.Collections;
using TMPro;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public sealed class PlatformerGameController : MonoBehaviour
{
    [SerializeField] private RectTransform player;
    [SerializeField] private RectTransform goal;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private RectTransform collapsingPlatform;
    [SerializeField] private RectTransform[] platforms;
    [SerializeField] private GameObject gameWindow;
    [SerializeField] private GameObject nextGameWindow;

    [SerializeField] private float moveSpeed = 360f;
    [SerializeField] private float slowMoveSpeed = 90f;
    [SerializeField] private float jumpVelocity = 560f;
    [SerializeField] private float gravity = 1350f;
    [SerializeField] private float collapseSpeed = 980f;
    [SerializeField] private float goalGateSpeed = 720f;
    [SerializeField] private float goalGateRiseDistance = 145f;
    [SerializeField] private float goalGateJumpRiseDistance = 245f;
    [SerializeField] private float goalGateJumpSpeedThreshold = 80f;
    [SerializeField] private float goalGateSlowTouchSpeedThreshold = 120f;
    [SerializeField] private float goalGateResetDistance = 120f;
    [SerializeField] private int slowWalkHintFailedAttempts = 3;
    [SerializeField] private float resetDelay = 0.45f;
    [SerializeField] private float nextGameDelay = 0.45f;

    private Vector2 velocity;
    private Vector2 startPosition;
    private Vector2 goalStartPosition;
    private Vector2 collapsingPlatformStartPosition;
    private Color statusTextStartColor;
    private bool grounded;
    private bool won;
    private bool collapsing;
    private bool goalGateTriggered;
    private bool goalGateReturning;
    private bool resetting;
    private bool handoffStarted;
    private bool slowWalkHintUnlocked;
    private int goalGateFailedAttempts;
    private float resetTimer;
    private float goalGateCurrentRiseDistance;

    public void Configure(RectTransform playerRect, RectTransform goalRect, TextMeshProUGUI statusLabel, RectTransform trapPlatform, GameObject currentWindow, GameObject nextWindow, params RectTransform[] platformRects)
    {
        player = playerRect;
        goal = goalRect;
        statusText = statusLabel;
        collapsingPlatform = trapPlatform;
        gameWindow = currentWindow;
        nextGameWindow = nextWindow;
        platforms = platformRects;
        startPosition = player.anchoredPosition;
        goalStartPosition = goal.anchoredPosition;
        collapsingPlatformStartPosition = collapsingPlatform.anchoredPosition;
        goalGateCurrentRiseDistance = goalGateRiseDistance;

        if (statusText != null)
        {
            statusTextStartColor = statusText.color;
        }
    }

    private void Update()
    {
        if (player == null || goal == null || collapsingPlatform == null || platforms == null)
        {
            return;
        }

        float deltaTime = Time.unscaledDeltaTime;

        if (resetting)
        {
            resetTimer -= deltaTime;

            if (resetTimer <= 0f)
            {
                ResetPlayer();
            }

            return;
        }

        Vector2 previousPosition = player.anchoredPosition;
        Vector2 position = previousPosition;
        float horizontalInput = ReadHorizontalInput();
        float currentMoveSpeed = ReadSlowInput() ? slowMoveSpeed : moveSpeed;

        velocity.x = horizontalInput * currentMoveSpeed;

        if (grounded && ReadJumpInput())
        {
            velocity.y = jumpVelocity;
            grounded = false;
        }

        velocity.y -= gravity * deltaTime;
        position += velocity * deltaTime;

        grounded = false;
        UpdateCollapsingPlatform(deltaTime);
        UpdateGoalGate(deltaTime);
        TryTriggerCollapsingPlatform(position);
        ResolveVerticalPlatformCollision(previousPosition, ref position);
        ClampToGameArea(ref position);

        player.anchoredPosition = position;
        TryStartResetIfFallen();
        UpdateGoalState();
    }

    private float ReadHorizontalInput()
    {
        float input = 0f;

#if ENABLE_INPUT_SYSTEM
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            return 0f;
        }

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            input -= 1f;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            input += 1f;
        }
#else
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            input -= 1f;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            input += 1f;
        }
#endif

        return input;
    }

    private bool ReadJumpInput()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard keyboard = Keyboard.current;
        return keyboard != null
            && (keyboard.spaceKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
#endif
    }

    private bool ReadSlowInput()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard keyboard = Keyboard.current;
        return keyboard != null
            && (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed);
#else
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
#endif
    }

    private void ResolveVerticalPlatformCollision(Vector2 previousPosition, ref Vector2 position)
    {
        Rect playerRect = GetRect(player, position);
        Rect previousPlayerRect = GetRect(player, previousPosition);

        ResolveVerticalPlatformCollision(collapsingPlatform, previousPlayerRect, ref playerRect, ref position);

        foreach (RectTransform platform in platforms)
        {
            ResolveVerticalPlatformCollision(platform, previousPlayerRect, ref playerRect, ref position);
        }
    }

    private void ResolveVerticalPlatformCollision(RectTransform platform, Rect previousPlayerRect, ref Rect playerRect, ref Vector2 position)
    {
        if (platform == null || (platform == collapsingPlatform && collapsing))
        {
            return;
        }

        Rect platformRect = GetRect(platform, platform.anchoredPosition);
        bool movingDown = velocity.y <= 0f;
        bool wasAbovePlatform = previousPlayerRect.yMin >= platformRect.yMax - 2f;
        bool overlapsHorizontally = playerRect.xMax > platformRect.xMin && playerRect.xMin < platformRect.xMax;
        bool crossesTop = playerRect.yMin <= platformRect.yMax && previousPlayerRect.yMin >= platformRect.yMax - 8f;

        if (movingDown && wasAbovePlatform && overlapsHorizontally && crossesTop)
        {
            position.y = platformRect.yMax + player.rect.height * 0.5f;
            velocity.y = 0f;
            grounded = true;
            playerRect = GetRect(player, position);
        }
    }

    private void ClampToGameArea(ref Vector2 position)
    {
        RectTransform root = (RectTransform)transform;
        Rect rootRect = root.rect;
        float halfWidth = player.rect.width * 0.5f;

        position.x = Mathf.Clamp(position.x, rootRect.xMin + halfWidth, rootRect.xMax - halfWidth);
    }

    private void TryTriggerCollapsingPlatform(Vector2 playerPosition)
    {
        if (collapsing || won)
        {
            return;
        }

        Rect playerRect = GetRect(player, playerPosition);
        Rect platformRect = GetRect(collapsingPlatform, collapsingPlatform.anchoredPosition);
        bool closeToTop = playerRect.yMin <= platformRect.yMax + 6f && playerRect.yMin >= platformRect.yMax - 20f;
        bool overTrap = playerRect.xMax > platformRect.xMin + 18f && playerRect.xMin < platformRect.xMax - 18f;

        if (!closeToTop || !overTrap)
        {
            return;
        }

        collapsing = true;
    }

    private void UpdateCollapsingPlatform(float deltaTime)
    {
        if (!collapsing)
        {
            return;
        }

        collapsingPlatform.anchoredPosition += Vector2.down * collapseSpeed * deltaTime;
    }

    private void UpdateGoalGate(float deltaTime)
    {
        if (!goalGateTriggered)
        {
            return;
        }

        float raisedY = goalStartPosition.y + goalGateCurrentRiseDistance;
        bool playerFarFromHouse = Vector2.Distance(player.anchoredPosition, goalStartPosition) >= goalGateResetDistance;

        if (goalGateReturning && !playerFarFromHouse)
        {
            goalGateReturning = false;
        }

        Vector2 targetPosition = goalGateReturning
            ? goalStartPosition
            : new Vector2(goalStartPosition.x, raisedY);

        goal.anchoredPosition = Vector2.MoveTowards(goal.anchoredPosition, targetPosition, goalGateSpeed * deltaTime);

        if (!goalGateReturning && Mathf.Approximately(goal.anchoredPosition.y, raisedY) && playerFarFromHouse)
        {
            goalGateReturning = true;
        }

        if (goalGateReturning && Mathf.Approximately(goal.anchoredPosition.y, goalStartPosition.y))
        {
            goalGateTriggered = false;
            goalGateReturning = false;
        }
    }

    private void TryStartResetIfFallen()
    {
        RectTransform root = (RectTransform)transform;
        float deathY = root.rect.yMin - player.rect.height;

        if (player.anchoredPosition.y > deathY)
        {
            return;
        }

        if (goalGateTriggered)
        {
            goalGateFailedAttempts++;
            slowWalkHintUnlocked = goalGateFailedAttempts >= slowWalkHintFailedAttempts;
        }

        resetting = true;
        resetTimer = resetDelay;
        velocity = Vector2.zero;
    }

    private void ResetPlayer()
    {
        resetting = false;
        collapsing = false;
        goalGateTriggered = false;
        goalGateReturning = false;
        goalGateCurrentRiseDistance = goalGateRiseDistance;
        grounded = false;
        won = false;
        velocity = Vector2.zero;
        player.anchoredPosition = startPosition;
        goal.anchoredPosition = goalStartPosition;
        collapsingPlatform.anchoredPosition = collapsingPlatformStartPosition;
        UpdateSlowWalkHint();
    }

    private void UpdateGoalState()
    {
        if (won)
        {
            return;
        }

        bool touchingGoal = GetRect(player, player.anchoredPosition).Overlaps(GetRect(goal, goal.anchoredPosition));

        if (!touchingGoal)
        {
            return;
        }

        bool tryingJumpCatch = !grounded && Mathf.Abs(velocity.y) >= goalGateJumpSpeedThreshold;
        bool touchingTooFast = velocity.magnitude >= goalGateSlowTouchSpeedThreshold;

        if (tryingJumpCatch)
        {
            TriggerGoalGate(goalGateJumpRiseDistance);
            return;
        }

        if (touchingTooFast)
        {
            TriggerGoalGate(goalGateRiseDistance);
            return;
        }

        if (goalGateTriggered)
        {
            return;
        }

        won = true;
        velocity = Vector2.zero;

        if (statusText != null)
        {
            statusText.text = "Welcome.";
            statusText.color = new Color(0.35f, 1f, 0.52f, 1f);
        }

        StartNextGameHandoff();
    }

    private void UpdateSlowWalkHint()
    {
        if (!slowWalkHintUnlocked || statusText == null)
        {
            return;
        }

        statusText.richText = true;
        statusText.color = statusTextStartColor;
        statusText.text = "oops forgot to add another instruction, here u go\n<color=#ff2020>HOLD SHIFT</color>\n<color=#37ff37>SLOW WALK</color>";
    }

    private void TriggerGoalGate(float riseDistance)
    {
        goalGateTriggered = true;
        goalGateReturning = false;
        goalGateCurrentRiseDistance = Mathf.Max(goalGateCurrentRiseDistance, riseDistance);
    }

    private void StartNextGameHandoff()
    {
        if (handoffStarted)
        {
            return;
        }

        handoffStarted = true;
        StartCoroutine(NextGameHandoffRoutine());
    }

    private IEnumerator NextGameHandoffRoutine()
    {
        if (nextGameDelay > 0f)
        {
            yield return new WaitForSecondsRealtime(nextGameDelay);
        }

        if (nextGameWindow != null)
        {
            nextGameWindow.SetActive(true);
            nextGameWindow.transform.SetAsLastSibling();
        }

        if (gameWindow != null)
        {
            gameWindow.SetActive(false);
        }
    }

    private static Rect GetRect(RectTransform rectTransform, Vector2 anchoredPosition)
    {
        Vector2 size = rectTransform.rect.size;
        return new Rect(anchoredPosition.x - size.x * 0.5f, anchoredPosition.y - size.y * 0.5f, size.x, size.y);
    }
}
