using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за отображение текста записки на UI.
/// </summary>
public class NoteUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        notePanel.SetActive(false);
        closeButton.onClick.AddListener(CloseNote);
    }

    public void ShowNote(string title, string content)
    {
        titleText.text = title;
        contentText.text = content;
        notePanel.SetActive(true);

        // Пауза игры и разблокировка курсора
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}