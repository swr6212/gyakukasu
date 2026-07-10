using UnityEngine;

public class EventObject : MonoBehaviour
{
    public GameObject choicePanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            choicePanel.SetActive(true);

            // ÉvÉåÉCÉÑÅ[Ç™ìÆÇØÇ»Ç¢ÇÊÇ§Ç…Ç∑ÇÈÇ»ÇÁÇ±Ç±Ç≈í‚é~
            Time.timeScale = 0f;

            Destroy(gameObject);
        }
    }
}