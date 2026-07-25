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

    [SerializeField] private float moveSpeed = 360f;
    [SerializeField] private float jumpVelocity = 560f;
    [SerializeField] private float gravity = 1350f;
    [SerializeField] private float collapseSpeed = 980f;
    [SerializeField] private float goalGateSpeed = 720f;
    [SerializeField] private float goalGateDropDistance = 145f;
    [SerializeField] private float goalGateGraceTime = 0.18f;
    [SerializeField] private float resetDelay = 0.45f;

    private Vector2 velocity;
    private Vector2 startPosition;
    private Vector2 goalStartPosition;
    private Vector2 collapsingPlatformStartPosition;
    private bool grounded;
    private bool won;
    private bool collapsing;
    private bool goalGateTriggered;
    private bool goalGateReturning;
    private bool goalGateReady;
    private bool resetting;
    private float resetTimer;
    private float goalGateGraceTimer;

    public void Configure(RectTransform playerRect, RectTransform goalRect, TextMeshProUGUI statusLabel, RectTransform trapPlatform, params RectTransform[] platformRects)
    {
        player = playerRect;
        goal = goalRect;
        statusText = statusLabel;
        collapsingPlatform = trapPlatform;
        platforms = platformRects;
        startPosition = player.anchoredPosition;
        goalStartPosition = goal.anchoredPosition;
        collapsingPlatformStartPosition = collapsingPlatform.anchoredPosition;
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

        velocity.x = horizontalInput * moveSpeed;

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
        if (!goalGateTriggered || goalGateReady)
        {
            return;
        }

        if (goalGateGraceTimer > 0f)
        {
            goalGateGraceTimer -= deltaTime;
        }

        float closedY = goalStartPosition.y - goalGateDropDistance;
        Vector2 targetPosition = goalGateReturning
            ? goalStartPosition
            : new Vector2(goalStartPosition.x, closedY);

        goal.anchoredPosition = Vector2.MoveTowards(goal.anchoredPosition, targetPosition, goalGateSpeed * deltaTime);

        if (!goalGateReturning && Mathf.Approximately(goal.anchoredPosition.y, closedY))
        {
            goalGateReturning = true;
        }

        if (goalGateReturning && Mathf.Approximately(goal.anchoredPosition.y, goalStartPosition.y))
        {
            goalGateReady = true;
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
        goalGateReady = false;
        grounded = false;
        won = false;
        goalGateGraceTimer = 0f;
        velocity = Vector2.zero;
        player.anchoredPosition = startPosition;
        goal.anchoredPosition = goalStartPosition;
        collapsingPlatform.anchoredPosition = collapsingPlatformStartPosition;
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

        if (!goalGateTriggered)
        {
            goalGateTriggered = true;
            goalGateGraceTimer = goalGateGraceTime;
            return;
        }

        if (!goalGateReady)
        {
            if (goalGateGraceTimer <= 0f)
            {
                BeginReset();
            }

            return;
        }

        won = true;
        velocity = Vector2.zero;

        if (statusText != null)
        {
            statusText.text = "Welcome.";
            statusText.color = new Color(0.35f, 1f, 0.52f, 1f);
        }
    }

    private void BeginReset()
    {
        if (resetting)
        {
            return;
        }

        resetting = true;
        resetTimer = resetDelay;
        velocity = Vector2.zero;
    }

    private static Rect GetRect(RectTransform rectTransform, Vector2 anchoredPosition)
    {
        Vector2 size = rectTransform.rect.size;
        return new Rect(anchoredPosition.x - size.x * 0.5f, anchoredPosition.y - size.y * 0.5f, size.x, size.y);
    }
}
