using UnityEngine;

/// <summary>
/// Компонент для интерактивной записки. Реагирует на вход/выход игрока из триггера и нажатие кнопки взаимодействия.
/// </summary>
public class Note : MonoBehaviour
{
    [Header("Note Data")]
    [SerializeField] private string noteTitle = "Записка";
    [SerializeField][TextArea(3, 10)] private string noteContent = "Текст записки...";

    [Header("Visual Feedback")]
    [SerializeField] private Material highlightMaterial; // Материал для подсветки
    private Material originalMaterial;
    private Renderer objectRenderer;
    private bool playerInRange = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Highlight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Highlight(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    /// <summary>
    /// Взаимодействие с запиской: показываем UI, добавляем в инвентарь, уведомляем квест-менеджер.
    /// </summary>
    private void Interact()
    {
        // Показываем текст записки через UI
        NoteUI noteUI = FindObjectOfType<NoteUI>();
        if (noteUI != null)
        {
            noteUI.ShowNote(noteTitle, noteContent);
        }

        // Добавляем в инвентарь
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.AddNote(new NoteData(noteTitle, noteContent));
        }

        // Уведомляем квест-менеджер
        QuestManager questManager = FindObjectOfType<QuestManager>();
        if (questManager != null)
        {
            questManager.NoteCollected();
        }

        // Деактивируем записку, чтобы нельзя было собрать дважды
        gameObject.SetActive(false);
    }

    private void Highlight(bool state)
    {
        if (objectRenderer != null && highlightMaterial != null)
        {
            objectRenderer.material = state ? highlightMaterial : originalMaterial;
        }
    }
}