using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отслеживает прогресс квеста: собрать все записки и поговорить с NPC.
/// </summary>
public class QuestManager : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private int totalNotes = 3; // Сколько всего записок на уровне
    [SerializeField] private bool needTalkToNPC = true; // Нужно ли говорить с NPC для завершения

    [Header("UI & World")]
    [SerializeField] private Text questStatusText; // Текстовое поле для отображения прогресса (опционально)
    [SerializeField] private GameObject questCompletePanel; // Панель "Победа" (опционально)
    [SerializeField] private GameObject exitDoor; // Дверь, которая откроется в конце

    private int notesCollected = 0;
    private bool talkedToNPC = false;

    private void Start()
    {
        UpdateQuestUI();
        if (questCompletePanel != null)
            questCompletePanel.SetActive(false);
    }

    // Вызывается из Note.cs при сборе записки
    public void NoteCollected()
    {
        notesCollected++;
        UpdateQuestUI();
        CheckCompletion();
    }

    // Вызывается из NPC.cs после разговора
    public void NPCTalked()
    {
        talkedToNPC = true;
        UpdateQuestUI();
        CheckCompletion();
    }

    private void CheckCompletion()
    {
        bool notesDone = notesCollected >= totalNotes;
        bool talkDone = !needTalkToNPC || talkedToNPC;

        if (notesDone && talkDone)
        {
            QuestComplete();
        }
    }

    private void QuestComplete()
    {
        Debug.Log("Квест выполнен! Уровень пройден.");
        if (questCompletePanel != null)
            questCompletePanel.SetActive(true);

        if (exitDoor != null)
            Destroy(exitDoor); // Убираем преграду (или можно проиграть анимацию открытия)
    }

    private void UpdateQuestUI()
    {
        if (questStatusText != null)
        {
            string status = $"Записки: {notesCollected}/{totalNotes}";
            if (needTalkToNPC)
                status += $"\nРазговор с NPC: {(talkedToNPC ? "?" : "")}";

            questStatusText.text = status;
        }
    }
}