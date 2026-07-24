using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Maintains the task buttons shown in the OS footer for minimized windows.</summary>
public class MinimizedProgramManager : MonoBehaviour
{
    private const int MaximumMinimizedPrograms = 5;

    public static MinimizedProgramManager Instance { get; private set; }

    [SerializeField] private GameObject minimizedProgramFooterPrefab;

    private readonly List<MinimizedProgram> minimizedPrograms = new List<MinimizedProgram>();

    private void Awake() => Instance = this;

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Minimize(GameObject window)
    {
        if (window == null) return;

        if (FindProgram(window) == null)
        {
            while (minimizedPrograms.Count >= MaximumMinimizedPrograms)
                RemoveProgram(minimizedPrograms[0]);

            GameObject footerButton = Instantiate(minimizedProgramFooterPrefab, transform);
            TMP_Text label = footerButton.GetComponentInChildren<TMP_Text>(true);
            if (label != null) label.text = window.name;

            MinimizedProgram program = new MinimizedProgram(window, footerButton);
            minimizedPrograms.Add(program);

            Button restoreButton = footerButton.GetComponent<Button>();
            if (restoreButton != null) restoreButton.onClick.AddListener(() => Restore(window));
        }

        window.SetActive(false);
    }

    public void Restore(GameObject window)
    {
        MinimizedProgram program = FindProgram(window);
        if (program != null) RemoveProgram(program);

        if (window == null) return;
        window.SetActive(true);
        window.transform.SetAsLastSibling();
    }

    // Desktop icons call SetActive directly, so remove the matching footer item when that happens.
    public void NotifyWindowOpened(GameObject window)
    {
        MinimizedProgram program = FindProgram(window);
        if (program != null) RemoveProgram(program);
    }

    private MinimizedProgram FindProgram(GameObject window) =>
        minimizedPrograms.Find(program => program.Window == window);

    private void RemoveProgram(MinimizedProgram program)
    {
        minimizedPrograms.Remove(program);
        if (program.FooterButton != null) Destroy(program.FooterButton);
    }

    private sealed class MinimizedProgram
    {
        public GameObject Window { get; }
        public GameObject FooterButton { get; }

        public MinimizedProgram(GameObject window, GameObject footerButton)
        {
            Window = window;
            FooterButton = footerButton;
        }
    }
}
