using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отслеживает прогресс квеста: собрать все записки и поговорить с NPC.
/// </summary>
public class QuestManager : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private int totalNotes = 3; // Сколько записок нужно собрать
    [SerializeField] private bool needTalkToNPC = true; // Нужно ли говорить с NPC

    [Header("UI & World")]
    [SerializeField] private Text questStatusText; // Текст статуса (верхний левый угол)
    [SerializeField] private GameObject questCompletePanel; // Панель "Квест выполнен"
    [SerializeField] private GameObject exitDoor; // Объект, блокирующий выход

    private int notesCollected = 0;
    private bool talkedToNPC = false;
    private bool questFinished = false;

    private void Start()
    {
        UpdateQuestUI();
        if (questCompletePanel != null)
            questCompletePanel.SetActive(false);
    }

    // Вызывается из Note.cs при подборе записки
    public void NoteCollected()
    {
        if (questFinished) return;
        notesCollected++;
        UpdateQuestUI();
        CheckCompletion();
    }

    // Вызывается из NPC.cs после разговора
    public void NPCTalked()
    {
        if (questFinished) return;
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
        questFinished = true;
        Debug.Log(" Квест выполнен! Уровень пройден.");
        if (questCompletePanel != null)
            questCompletePanel.SetActive(true);

        if (exitDoor != null)
            Destroy(exitDoor); // Убираем преграду (или можно проиграть анимацию открытия)
    }

    private void UpdateQuestUI()
    {
        if (questStatusText != null)
        {
            string status = $"?? Записки: {notesCollected}/{totalNotes}";
            if (needTalkToNPC)
                status += $"\n?? Диалог с NPC: {(talkedToNPC ? "?" : "?")}";

            questStatusText.text = status;
        }
    }
}