using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.UI;

public class HomeShuffleCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private string cardId;

    private HomeShufflePuzzleController controller;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private int originalSiblingIndex;
    private bool isDragging;

    public string CardId => cardId;

    public void Initialize(HomeShufflePuzzleController owner, string id)
    {
        controller = owner;
        cardId = id;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (controller == null)
            return;

        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        isDragging = true;
        canvasGroup.blocksRaycasts = false;
        transform.DOKill();
        transform.SetAsLastSibling();
        controller.BeginCardDrag(this, originalSiblingIndex);
    }

    private void Update()
    {
        if (!isDragging || IsPointerPressed())
            return;

        FinishDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (controller == null || rectTransform == null)
            return;

        float scaleFactor = canvas != null ? canvas.scaleFactor : 1f;
        rectTransform.anchoredPosition += eventData.delta / scaleFactor;
        controller.UpdateCardDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        FinishDrag();
    }

    private void OnDisable()
    {
        if (isDragging)
            FinishDrag();
    }

    private void FinishDrag()
    {
        if (!isDragging)
            return;

        isDragging = false;
        canvasGroup.blocksRaycasts = true;

        if (controller == null)
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
            return;
        }

        controller.EndCardDrag(this);
    }

    private static bool IsPointerPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.leftButton.isPressed;
#else
        return Input.GetMouseButton(0);
#endif
    }
}
