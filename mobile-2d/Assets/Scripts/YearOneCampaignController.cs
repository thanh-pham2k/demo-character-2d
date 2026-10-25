using System;
using UnityEngine;
public sealed partial class YearOneCampaignController : MonoBehaviour
{
    [SerializeField] private DormJourneyController journey;
    private YearOneCheckpointStore store;
    public YearOneProgress Progress {get;private set;}=new YearOneProgress();
    public bool Initialized {get;private set;}
    public bool HasCheckpoint=>Store.Exists;
    public bool HasLegacyCheckpoint=>Store.HasLegacy;
    public bool MenuPausesClock {get;set;}
    public string SaveError {get;private set;}="";
    public Func<bool> WorldPaused;
    public event Action Changed;
    private float saveTimer;
    private YearOneCheckpointStore Store=>store??(store=new YearOneCheckpointStore(Application.persistentDataPath));
    public void Configure(DormJourneyController dorm)=>journey=dorm;
    public bool Transact(Func<YearOneProgress,bool> action)
    {
        if(!Initialized)return false;
        var candidate=JsonUtility.FromJson<YearOneProgress>(JsonUtility.ToJson(Progress));
        if(!action(candidate))return false;
        if(!Store.Save(candidate)){SaveError=Store.Error;return false;}
        Progress=candidate;SaveError="";Changed?.Invoke();return true;
    }
    public bool BeginNewGame()
    {
        CloseComputer();var candidate=new YearOneProgress();
        if(!Store.Save(candidate)){SaveError=Store.Error;return false;}
        Progress=candidate;Initialized=true;SaveError="";Changed?.Invoke();return true;
    }
    public bool ContinueGame()
    {var loaded=Store.Load();if(loaded==null){SaveError=Store.Error;return false;}Progress=loaded;Initialized=true;Changed?.Invoke();return true;}
    public void UseStoreForTests(string directory)
    {if(Initialized)throw new InvalidOperationException("Select save sandbox before campaign initialization.");store=new YearOneCheckpointStore(directory);}
#if UNITY_EDITOR
    public void LoadDeveloperCheckpoint(YearOneProgress progress,string directory)
    {
        if(Initialized || progress==null || !progress.IsValid())throw new InvalidOperationException("Fresh Play Mode and a valid v2 checkpoint required.");
        store=new YearOneCheckpointStore(directory);if(!store.Save(progress))throw new InvalidOperationException(store.Error);
        Progress=progress;Initialized=true;Changed?.Invoke();
    }
#endif
    public bool CompleteOpening(string activity)=>Transact(p=>p.CompleteOpening(activity));
    public bool CompleteCourse(string course,YearOneSchedule.Work work)=>Transact(p=>p.Complete(course,work));
    public bool CompleteClassStory()=>Transact(p=>p.CompleteClassStory());
    public bool CompleteEvening(string activity)=>Transact(p=>p.MarkEvening(activity));
    public bool CompleteTrip(bool returning,bool nextBus=false)=>Transact(p=>p.CommitTrip(returning,nextBus));
    public bool FinishNight(bool longSleeping)=>CommitTransition(Progress.PreviewTransition());
    public bool CommitTransition(YearOneTransition preview)=>Transact(p=>p.CommitTransition(preview));
    public bool StartCourseExam(string course)=>Transact(p=>p.StartExam(course));
    public bool AnswerCourseExam(int answer)=>Transact(p=>p.AnswerExam(answer));
    public bool SubmitCourseExam()=>Transact(p=>p.SubmitExam());
    public bool PrepareFeast()=>Transact(p=>p.PrepareFeast());
    public bool CompleteFeast()=>Transact(p=>p.FinishFeast());
    public bool EnterYearTwo()=>Transact(p=>p.EnterYearTwo());
    // Kept for scene compatibility: the feast no longer advances the year without sleep.
    public bool CompleteFeastAndEnterYearTwo()=>CompleteFeast();
    public void Save()
    {if(!Initialized)return;if(Store.Save(Progress)){SaveError="";Changed?.Invoke();}else SaveError=Store.Error;}
    public void ReachedBusStop()
    {
        if(Initialized && Progress.Milestone.MissableBus && !Progress.Has("day:4:stop"))Transact(p=>{p.completed.Add("day:4:stop");p.safeLocation="Campus";return true;});
    }
    private void Update()
    {
        if(!Initialized || Progress.Has("year:two"))return;
        bool paused=!string.IsNullOrEmpty(SaveError) || MenuPausesClock || (WorldPaused?.Invoke()??false);
        if(Progress.exam!=null)
        {
            if(!paused){Progress.exam.Seconds=Mathf.Max(0,Progress.exam.Seconds-Time.deltaTime);if(Progress.exam.Seconds<=0)AnswerCourseExam(-1);}
        }
        else if(!paused && !(computerUI!=null && computerUI.StudyVisible) && !(computerDesk!=null && computerDesk.ComputerVisible))
        {
            Progress.AdvanceComputerTime(Time.deltaTime,IsComputerGaming);
            if(IsComputerGaming)ComputerSessionSeconds+=Time.deltaTime;
            TickComputer();

        }
        saveTimer+=Time.unscaledDeltaTime;if(saveTimer>=1){saveTimer=0;Save();}
    }
}
