using System;
using UnityEngine;

public sealed partial class YearOneCampaignController
{
    public enum ComputerState { Off, Desktop, Gaming, Makeup }
    public ComputerState Computer { get; private set; }
    public bool IsComputerGaming => Computer == ComputerState.Gaming;
    public string ComputerGame { get; private set; }
    public float ComputerSessionSeconds { get; private set; }
    public float MakeupSeconds { get; private set; }
    public const float MakeupDuration = 15f;
    public static readonly string[] ComputerGames = { "LOL", "FC Online", "Valorant", "CS2" };
    private DeskActionModal computerDesk;
    private YearOneCampaignUI computerUI;
    private string makeupCourse;
    private YearOneSchedule.Work makeupWork;
    private float computerSaveTimer;
    public bool OpenComputer(DeskActionModal desk)
    {
        if (!Initialized || desk == null || !desk.IsOpen || Progress.Has("year:two")) return false;
        computerDesk = desk; computerUI=FindAnyObjectByType<YearOneCampaignUI>(); Computer = ComputerState.Desktop; ComputerSessionSeconds = 0; Progress.EnsureClock(); return true;
    }
    public bool PlayComputerGame(string game)
    {
        if (Computer != ComputerState.Desktop || computerDesk == null || !computerDesk.IsOpen || Array.IndexOf(ComputerGames, game) < 0 || Progress.day < 0 || Progress.Has("year:two")) return false;
        ComputerGame = game; ComputerSessionSeconds = 0; Computer = ComputerState.Gaming; MenuPausesClock = false; return true;
    }
    public void StopComputerGame()
    {
        if (Computer != ComputerState.Gaming) return;
        Computer = ComputerState.Desktop; ComputerGame = null; Save();
    }
    public bool BeginComputerMakeup(string course, YearOneSchedule.Work work)
    {
        var c = YearOneSchedule.Find(course);
        if (Computer != ComputerState.Desktop || computerDesk == null || !computerDesk.IsOpen || c == null || !Progress.NeedsMakeup(c, work)) return false;
        if (work == YearOneSchedule.Work.Lesson2 && !Progress.Has(c.Activity(YearOneSchedule.Work.Lesson1))) return false;
        makeupCourse = course; makeupWork = work; MakeupSeconds = 0; Computer = ComputerState.Makeup; return true;
    }
    public void CancelComputerActivity()
    {
        if (Computer == ComputerState.Gaming) StopComputerGame();
        if (Computer == ComputerState.Makeup) { Computer = ComputerState.Desktop; MakeupSeconds = 0; Save(); }
    }
    public void CloseComputer()
    {
        if (Computer == ComputerState.Off) return;
        CancelComputerActivity(); Computer = ComputerState.Off; computerDesk = null; if (Initialized) Save();
    }
    private void TickComputer()
    {
        if (Computer == ComputerState.Off) return;
        if (computerDesk == null || !computerDesk.IsOpen || computerDesk.CurrentAction != DeskActionModal.DeskAction.Computer) { CloseComputer(); return; }
        if (MenuPausesClock || (computerUI != null && (computerUI.StartupVisible || computerUI.StudyVisible))) return;
        if (Computer == ComputerState.Makeup)
        {
            MakeupSeconds += Time.deltaTime;
            if(MakeupSeconds>=MakeupDuration)
            {
                if(Transact(p=>p.CompleteMakeup(makeupCourse,makeupWork)))Computer=ComputerState.Desktop;
            }
        }
    }
    private void OnApplicationPause(bool paused) { if (paused && Initialized) Save(); }
    private void OnDisable() { CloseComputer(); }
    private void OnApplicationQuit() { if (Initialized) Save(); }
}
