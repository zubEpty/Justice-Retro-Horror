using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HomeShufflePuzzleController : MonoBehaviour
{
    private const string CardHeart = "Card_H";
    private const string CardDiamond = "Card_D";
    private const string CardCircle = "Card_C";
    private const string CardStar = "Card_S";

    public enum ShuffleCardId
    {
        Card_H,
        Card_D,
        Card_C,
        Card_S
    }

    [Header("Scene References")]
    [SerializeField] private RectTransform shuffleContainer;
    [SerializeField] private GameObject puzzleContainer;
    [SerializeField] private GameObject successContainer;
    [SerializeField] private CanvasGroup hintCanvasGroup;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button downloadLicenseButton;
    [SerializeField] private GameObject licenseButton;
    [SerializeField] private AntivirusDownloadController licenseDownloadController;

    [Header("Puzzle Settings")]
    [SerializeField] private int wrongTriesBeforeHint = 3;
    [SerializeField] private float dropDuration = 0.18f;
    [SerializeField] private string hintMessage = "Hint: Put the suits in this order: Heart, Diamond, Circle, Star.";
    [Tooltip("The order required when the player presses Submit. Reorder this list anytime in the inspector.")]
    [SerializeField] private List<ShuffleCardId> correctOrder = new List<ShuffleCardId>
    {
        ShuffleCardId.Card_H,
        ShuffleCardId.Card_D,
        ShuffleCardId.Card_C,
        ShuffleCardId.Card_S
    };

    private readonly List<HomeShuffleCard> cards = new List<HomeShuffleCard>();
    private RectTransform placeholder;
    private LayoutElement placeholderLayout;
    private HomeShuffleCard draggingCard;
    private Canvas rootCanvas;
    private int consecutiveWrongSubmits;
    private bool solved;

    private void Awake()
    {
        WireSceneReferences();
        InitializeState();
        InitializeCards();
        HookButtons();
    }

    private void OnDestroy()
    {
        if (submitButton != null)
            submitButton.onClick.RemoveListener(SubmitOrder);

        if (downloadLicenseButton != null)
            downloadLicenseButton.onClick.RemoveListener(StartLicenseDownload);
    }

    public void BeginCardDrag(HomeShuffleCard card, int siblingIndex)
    {
        if (solved || shuffleContainer == null)
            return;

        draggingCard = card;
        rootCanvas = GetComponentInParent<Canvas>();

        CreatePlaceholder(card, siblingIndex);
        card.transform.SetParent(rootCanvas != null ? rootCanvas.transform : transform, true);
        card.transform.DOScale(1.06f, 0.12f).SetEase(Ease.OutBack);
    }

    public void UpdateCardDrag(PointerEventData eventData)
    {
        if (solved || placeholder == null || shuffleContainer == null)
            return;

        int targetIndex = GetTargetSiblingIndex(eventData.position, eventData.pressEventCamera);
        placeholder.SetSiblingIndex(targetIndex);
    }

    public void EndCardDrag(HomeShuffleCard card)
    {
        if (shuffleContainer == null)
            return;

        int targetIndex = placeholder != null ? placeholder.GetSiblingIndex() : card.transform.GetSiblingIndex();
        card.transform.SetParent(shuffleContainer, false);
        card.transform.SetSiblingIndex(targetIndex);

        if (placeholder != null)
        {
            placeholder.gameObject.SetActive(false);
            Destroy(placeholder.gameObject);
        }

        placeholder = null;
        placeholderLayout = null;
        draggingCard = null;
        consecutiveWrongSubmits = 0;

        card.transform.DOKill();
        card.transform.localScale = Vector3.one;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(shuffleContainer);
        card.transform.DOPunchScale(Vector3.one * 0.08f, dropDuration, 6, 0.7f);
    }

    private void SubmitOrder()
    {
        if (solved)
            return;

        if (IsCorrectOrder())
        {
            solved = true;
            ShowSuccess();
            return;
        }

        consecutiveWrongSubmits++;
        ShakePuzzle();

        if (consecutiveWrongSubmits >= wrongTriesBeforeHint)
            ShowHint();
    }

    private void StartLicenseDownload()
    {
        if (licenseDownloadController != null && licenseButton != null)
        {
            licenseDownloadController.StartDownload(licenseButton);
            return;
        }

        EnableLicenseButton();
    }

    private void EnableLicenseButton()
    {
        if (licenseButton == null)
            return;

        licenseButton.SetActive(true);
        licenseButton.transform.DOKill();
        licenseButton.transform.localScale = Vector3.one * 0.85f;
        licenseButton.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    private void WireSceneReferences()
    {
        if (puzzleContainer == null)
            puzzleContainer = transform.Find("Container")?.gameObject;

        if (successContainer == null)
            successContainer = transform.Find("Container_Success")?.gameObject;

        if (shuffleContainer == null)
            shuffleContainer = transform.Find("Container/Shuffle_Container") as RectTransform;

        if (hintCanvasGroup == null)
            hintCanvasGroup = transform.Find("Container/Hint_text")?.GetComponent<CanvasGroup>();

        if (hintText == null)
            hintText = transform.Find("Container/Hint_text")?.GetComponent<TMP_Text>();

        if (submitButton == null)
            submitButton = transform.Find("Container/Btn_Submit")?.GetComponent<Button>();

        if (downloadLicenseButton == null)
            downloadLicenseButton = transform.Find("Container_Success/Btn_LicenseKeyDownload")?.GetComponent<Button>();

        if (downloadLicenseButton == null)
            downloadLicenseButton = transform.Find("Container_Success/Btn_Submit")?.GetComponent<Button>();

        if (licenseButton == null)
            licenseButton = FindInactiveObjectByName("Btn_License");

        if (licenseDownloadController == null)
            licenseDownloadController = FindSceneComponent<AntivirusDownloadController>();
    }

    private void InitializeState()
    {
        if (puzzleContainer != null)
            puzzleContainer.SetActive(true);

        if (shuffleContainer != null)
            shuffleContainer.gameObject.SetActive(true);

        if (successContainer != null)
            successContainer.SetActive(false);

        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 0f;
            hintCanvasGroup.interactable = false;
            hintCanvasGroup.blocksRaycasts = false;
        }

        if (hintText != null)
            hintText.text = hintMessage;

        if (licenseButton != null)
            licenseButton.SetActive(false);
    }

    private void InitializeCards()
    {
        cards.Clear();

        if (shuffleContainer == null)
            return;

        RegisterCard(CardHeart);
        RegisterCard(CardDiamond);
        RegisterCard(CardCircle);
        RegisterCard(CardStar);
    }

    private void RegisterCard(string cardName)
    {
        Transform cardTransform = shuffleContainer.Find(cardName);
        if (cardTransform == null)
            return;

        HomeShuffleCard card = cardTransform.GetComponent<HomeShuffleCard>();
        if (card == null)
            card = cardTransform.gameObject.AddComponent<HomeShuffleCard>();

        card.Initialize(this, cardName);
        cards.Add(card);
    }

    private void HookButtons()
    {
        if (submitButton != null)
        {
            submitButton.onClick.RemoveListener(SubmitOrder);
            submitButton.onClick.AddListener(SubmitOrder);
        }

        if (downloadLicenseButton != null)
        {
            downloadLicenseButton.onClick.RemoveListener(StartLicenseDownload);
            downloadLicenseButton.onClick.AddListener(StartLicenseDownload);
        }
    }

    private bool IsCorrectOrder()
    {
        if (shuffleContainer == null)
            return false;

        int cardIndex = 0;

        for (int i = 0; i < shuffleContainer.childCount; i++)
        {
            HomeShuffleCard card = shuffleContainer.GetChild(i).GetComponent<HomeShuffleCard>();
            if (card == null)
                continue;

            if (cardIndex >= correctOrder.Count || card.CardId != correctOrder[cardIndex].ToString())
                return false;

            cardIndex++;
        }

        return cardIndex == correctOrder.Count;
    }

    private void ShowSuccess()
    {
        if (hintCanvasGroup != null)
            hintCanvasGroup.DOFade(0f, 0.15f);

        if (puzzleContainer != null)
            puzzleContainer.SetActive(false);

        if (successContainer == null)
            return;

        successContainer.SetActive(true);
        successContainer.transform.SetAsLastSibling();
        successContainer.transform.DOKill();
        successContainer.transform.localScale = Vector3.one * 0.92f;
        successContainer.transform.DOScale(1f, 0.28f).SetEase(Ease.OutBack);
    }

    private void ShowHint()
    {
        if (hintText != null)
            hintText.text = hintMessage;

        if (hintCanvasGroup == null)
            return;

        hintCanvasGroup.DOKill();
        hintCanvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);
        hintCanvasGroup.transform.DOPunchScale(Vector3.one * 0.04f, 0.22f, 5, 0.7f);
    }

    private void ShakePuzzle()
    {
        if (shuffleContainer != null)
            shuffleContainer.DOShakeAnchorPos(0.18f, new Vector2(12f, 0f), 14, 0f, false, true);

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] != null)
                cards[i].transform.DOPunchRotation(new Vector3(0f, 0f, 5f), 0.18f, 8, 0.7f);
        }
    }

    private void CreatePlaceholder(HomeShuffleCard card, int siblingIndex)
    {
        placeholder = new GameObject("Card_Placeholder", typeof(RectTransform), typeof(LayoutElement)).GetComponent<RectTransform>();
        placeholder.SetParent(shuffleContainer, false);
        placeholder.SetSiblingIndex(siblingIndex);

        RectTransform cardRect = card.GetComponent<RectTransform>();
        placeholder.sizeDelta = cardRect.sizeDelta;

        placeholderLayout = placeholder.GetComponent<LayoutElement>();
        LayoutElement cardLayout = card.GetComponent<LayoutElement>();

        if (cardLayout != null)
        {
            placeholderLayout.preferredWidth = cardLayout.preferredWidth;
            placeholderLayout.preferredHeight = cardLayout.preferredHeight;
            placeholderLayout.minWidth = cardLayout.minWidth;
            placeholderLayout.minHeight = cardLayout.minHeight;
            placeholderLayout.flexibleWidth = cardLayout.flexibleWidth;
            placeholderLayout.flexibleHeight = cardLayout.flexibleHeight;
        }
        else
        {
            placeholderLayout.preferredWidth = cardRect.rect.width;
            placeholderLayout.preferredHeight = cardRect.rect.height;
        }
    }

    private int GetTargetSiblingIndex(Vector2 screenPosition, Camera eventCamera)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(shuffleContainer, screenPosition, eventCamera, out Vector2 localPoint))
            return placeholder.GetSiblingIndex();

        int targetIndex = 0;

        for (int i = 0; i < shuffleContainer.childCount; i++)
        {
            RectTransform child = shuffleContainer.GetChild(i) as RectTransform;
            if (child == null || child == placeholder || child == draggingCard.transform)
                continue;

            if (localPoint.x > child.localPosition.x)
                targetIndex = child.GetSiblingIndex() + 1;
        }

        return Mathf.Clamp(targetIndex, 0, shuffleContainer.childCount - 1);
    }

    private static GameObject FindInactiveObjectByName(string objectName)
    {
        Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();

        for (int i = 0; i < transforms.Length; i++)
        {
            Transform candidate = transforms[i];
            if (candidate.name == objectName && candidate.hideFlags == HideFlags.None && candidate.gameObject.scene.IsValid())
                return candidate.gameObject;
        }

        return null;
    }

    private static T FindSceneComponent<T>() where T : Component
    {
        T[] components = Resources.FindObjectsOfTypeAll<T>();

        for (int i = 0; i < components.Length; i++)
        {
            T candidate = components[i];
            if (candidate != null && candidate.gameObject.scene.IsValid())
                return candidate;
        }

        return null;
    }
}
