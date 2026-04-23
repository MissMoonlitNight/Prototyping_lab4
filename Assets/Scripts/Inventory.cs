using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Хранит список собранных записок и управляет UI инвентаря.
/// </summary>
public class Inventory : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel; // Панель инвентаря
    [SerializeField] private Transform notesContainer;  // Контейнер, куда будут добавляться кнопки
    [SerializeField] private GameObject noteButtonPrefab; // Префаб кнопки с названием записки

    private List<NoteData> notes = new List<NoteData>();

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        // Открытие/закрытие инвентаря на клавишу "I"
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    /// <summary>
    /// Вызывается из Note.cs при подборе
    /// </summary>
    public void AddNote(NoteData note)
    {
        notes.Add(note);

        // Создаем кнопку для новой записки в UI
        GameObject buttonObj = Instantiate(noteButtonPrefab, notesContainer);

        // Устанавливаем текст кнопки
        buttonObj.GetComponentInChildren<Text>().text = note.title;

        // Вешаем событие клика
        buttonObj.GetComponent<Button>().onClick.AddListener(() => ShowNoteFromInventory(note));
    }

    private void ShowNoteFromInventory(NoteData note)
    {
        inventoryPanel.SetActive(false); // Временно скрываем инвентарь
        NoteUI noteUI = FindObjectOfType<NoteUI>();
        if (noteUI != null)
        {
            // Передаём true, чтобы NoteUI знал, что нужно вернуть инвентарь
            noteUI.ShowNote(note.title, note.content, true);
        }
    }

    private void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);

        Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isActive;
        Time.timeScale = isActive ? 1f : 0f; // Пауза при открытом инвентаре
    }
    /// <summary>
    /// Восстанавливает панель инвентаря после чтения записки.
    /// Вызывается из NoteUI.CloseNote()
    /// </summary>
    public void RestoreInventoryUI()
    {
        inventoryPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}