using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет окном чтения записки и корректно возвращает состояние игры/инвентаря.
/// </summary>
public class NoteUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;
    [SerializeField] private Button closeButton;

    private bool wasInventoryOpen = false; // Флаг: читали ли мы записку из инвентаря

    private void Start()
    {
        notePanel.SetActive(false);
        closeButton.onClick.AddListener(CloseNote);
    }

    /// <summary>
    /// Открывает записку. inventoryWasOpen = true, если вызов идёт из инвентаря.
    /// </summary>
    public void ShowNote(string title, string content, bool inventoryWasOpen = false)
    {
        titleText.text = title;
        contentText.text = content;
        notePanel.SetActive(true);
        wasInventoryOpen = inventoryWasOpen;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false); // Записка гарантированно исчезает

        if (wasInventoryOpen)
        {
            // Если читали из инвентаря → возвращаем панель инвентаря
            Inventory inventory = FindObjectOfType<Inventory>();
            if (inventory != null)
                inventory.RestoreInventoryUI();
        }
        else
        {
            // Если читали сразу при подборе → просто продолжаем игру
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}