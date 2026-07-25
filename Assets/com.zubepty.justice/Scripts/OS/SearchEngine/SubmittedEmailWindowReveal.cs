using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SubmittedEmailWindowReveal : MonoBehaviour
{
    [SerializeField] private Button lindaButton;

    private Coroutine revealRoutine;

    private void OnEnable()
    {
        if (revealRoutine != null)
            StopCoroutine(revealRoutine);

        revealRoutine = StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        yield return new WaitForEndOfFrame();

        transform.SetAsLastSibling();

        if (lindaButton == null)
            lindaButton = FindSceneButton("Btn_Linda");

        if (lindaButton != null)
        {
            lindaButton.gameObject.SetActive(true);
            lindaButton.interactable = true;
            lindaButton.transform.SetAsLastSibling();
            lindaButton.transform.DOKill();
            lindaButton.transform.localScale = Vector3.one * 0.88f;
            lindaButton.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        }

        revealRoutine = null;
    }

    private static Button FindSceneButton(string objectName)
    {
        GameObject[] sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject sceneObject in sceneObjects)
        {
            if (sceneObject.name == objectName && sceneObject.scene.IsValid())
                return sceneObject.GetComponent<Button>();
        }

        return null;
    }
}
