using UnityEngine;

public class EventObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player1"))
            return;

        ChoiceManager choiceManager = FindFirstObjectByType<ChoiceManager>();

        if (choiceManager != null)
        {
            // êÊÇ…ÉQÅ[ÉÄÇí‚é~
            Time.timeScale = 0f;

            // ÇªÇÃÇ†Ç∆UIÇï\é¶
            choiceManager.OpenChoice();
        }

        Destroy(gameObject);
    }
}