using UnityEngine;

public class FreakbookFoundStep : MonoBehaviour
{
   public void CheckJaberFreakAccount()
   {
       if(GetComponent<ResultUI>().titleText.text == "Freakbook | Jaber Molla")
       {
            FindFirstObjectByType<TutorialManager>()?.SetConditionMetExternally();
       }

   }
}
