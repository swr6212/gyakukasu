
using UnityEngine;

public class GambleExitButton : MonoBehaviour
{
    public enum GambleType
    {
        Pachinko,
        Slot,
        Chinchiro
    }

    [SerializeField] private GambleType gambleType;

    public void Close()
    {
        ChoiceManager manager =
            FindFirstObjectByType<ChoiceManager>(
                FindObjectsInactive.Include
            );

        if (manager == null)
        {
            Debug.LogError("ChoiceManagerÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅI");
            return;
        }

        switch (gambleType)
        {
            case GambleType.Pachinko:
                manager.ClosePachinko();
                break;

            case GambleType.Slot:
                manager.CloseSlot();
                break;

            case GambleType.Chinchiro:
                manager.CloseChinchiro();
                break;
        }
    }
}
