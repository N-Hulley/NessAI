using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Status : MonoBehaviour
{
    public List<LogEntry> History;
    public enum Importance
    {
        NotImportant,
        SlightlyImportant,
        SomewhatImportant,
        Important,
        VeryImportant,
        Critical
    }
    public TextMeshPro status;
    private static Status Instance;
    private static Importance LogLevel = Importance.SomewhatImportant;
    private void Awake()
    {
        History = new List<LogEntry>();

        Instance = this;
        
    }
    public void instanceLog(string message, Importance importance = Importance.Critical) {
        Log(message, importance);
    }
    public static void Log(string message, Importance importance = Importance.Critical)
    {
        LogEntry entry = LogEntry.NewEntry(message, importance);
        Log(entry);
    }
    public static void Log(LogEntry log)
    {
        Instance.History.Add(log);
        if (LogLevel <= log.importance)
        {
            Instance.status.text = log.GetMessage() + Instance.status.text;
        }
    }
    public static void ChangeLogLevel(Importance importance)
    {
        LogLevel = importance;
        Instance.status.text = "";
        for (int i = Instance.History.Count - 1; i >= 0; i--) {
            if (LogLevel <= Instance.History[i].importance)
            Instance.status.text += Instance.History[i].GetMessage();
        }
    }
    public static Importance GetLogLevel()
    {
        return LogLevel;
    }
    public void instanceLogBoard()
    {
        logBoard();
    }
        public static void logBoard()
    {
        GamePiece SelectedPiece = ChessManager.Instance.PManager.SelectedPiece;

        if (SelectedPiece != null) Log("\nSelected:  " + SelectedPiece.position.ToString());

        Log(ChessManager.BoardToString());

    }
}
public class LogEntry
{
    public string OriginalMessage { get; set; }
    public DateTime TimeStamp { get; set; }
    public Status.Importance importance { get; set; }
    public string GetMessage()
    {
        return "\n<color=#FFFFFF55><b>(" + importance.ToString() + ")</b> " + TimeStamp.ToString("H:mm:ss.f") + "</color>   " + OriginalMessage;
    }
    public static LogEntry NewEntry(string Message, Status.Importance importance)
    {
        LogEntry entry = new LogEntry();
        entry.TimeStamp = DateTime.Now;
        entry.OriginalMessage = Message;
        entry.importance = importance;
        return entry;
    }
}