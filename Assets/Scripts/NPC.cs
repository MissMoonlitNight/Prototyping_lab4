using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC с диалогом. Взаимодействие определяется по расстоянию до игрока.
/// </summary>
public class NPC : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] private string npcName = "Незнакомец";
    [SerializeField][TextArea(2, 5)] private string dialogueLine = "Привет, путник!";

    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 2.5f; // Радиус взаимодействия

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text nameText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Button closeButton;

    private bool playerInRange = false;
    private Transform playerTransform;

    private void Start()
    {
        Debug.Log("=== NPC СКРИПТ ЗАПУЩЕН ===");

        // Ищем игрока по тегу один раз при старте
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
        else
            Debug.LogError("Не найден объект с тегом Player!");

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDialogue);
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Проверяем расстояние до игрока
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool inRange = distance <= interactionRange;

        // Отслеживаем вход/выход из зоны
        if (inRange && !playerInRange)
        {
            playerInRange = true;
            Debug.Log("[NPC] Игрок вошёл в зону взаимодействия!");
        }
        else if (!inRange && playerInRange)
        {
            playerInRange = false;
            Debug.Log("[NPC] Игрок покинул зону.");
            CloseDialogue(); // Автоматически закрываем, если отошли
        }

        // Обработка нажатия E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[NPC] Нажата E. Открываем диалог.");
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        if (nameText != null) nameText.text = npcName;
        if (dialogueText != null) dialogueText.text = dialogueLine;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("[NPC] Диалог открыт, игра на паузе.");

        QuestManager questManager = FindObjectOfType<QuestManager>();
        if (questManager != null)
            questManager.NPCTalked();
    }

    private void CloseDialogue()
    {
        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("[NPC] Диалог закрыт.");
        }
    }
}