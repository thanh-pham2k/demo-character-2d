using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;

// Runs only after a human clicks Load; never executes checkpoints on import.
[InitializeOnLoad]
public sealed class DeveloperCheckpointWindow : EditorWindow
{
    const string Pending="Room404.CheckpointPicker.Pending";
    const string Phase="Room404.CheckpointPicker.Phase";
    const string ScenePath="Assets/Scenes/Room404_MVP.unity";
    [SerializeField] string search="";
    Vector2 scroll;
    static string status="Chọn một checkpoint rồi bấm Nạp & Play.";
    static DeveloperCheckpointWindow()
    {
        EditorApplication.playModeStateChanged+=OnPlayState;
        EditorApplication.update+=ApplyWhenReady;
    }
    [MenuItem("KTX404/Checkpoint nhanh _F8")]
    public static void Open(){var window=GetWindow<DeveloperCheckpointWindow>("Checkpoint nhanh");window.minSize=new Vector2(430,400);window.Show();}
    void OnGUI()
    {
        EditorGUILayout.LabelField("CHECKPOINT • PHÒNG 404 / NĂM 1",EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Chỉ tải khi bạn bấm nút. Mỗi lần tải sẽ khởi động lại Play Mode để reset phiên cũ. Save checkpoint được tách khỏi save chơi chính.",MessageType.Info);
        search=EditorGUILayout.TextField("Tìm checkpoint",search);
        EditorGUILayout.LabelField(status,EditorStyles.wordWrappedLabel);
        EditorGUILayout.Space(8);scroll=EditorGUILayout.BeginScrollView(scroll);
        string group="";
        foreach(var entry in DeveloperCheckpointCatalog.Entries)
        {
            if(!string.IsNullOrWhiteSpace(search) && (entry.Title+" "+entry.Description+" "+entry.Group).IndexOf(search,StringComparison.OrdinalIgnoreCase)<0)continue;
            if(entry.Group!=group){EditorGUILayout.Space(8);EditorGUILayout.LabelField(entry.Group,EditorStyles.boldLabel);group=entry.Group;}
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(entry.Title,EditorStyles.boldLabel);
            EditorGUILayout.LabelField(entry.Description,EditorStyles.wordWrappedLabel);
            using(new EditorGUI.DisabledScope(EditorApplication.isCompiling || EditorApplication.isUpdating || !string.IsNullOrEmpty(SessionState.GetString(Pending,""))))
                if(GUILayout.Button("Nạp & Play",GUILayout.Height(26)))Request(entry);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.HelpBox("Bạn tự kiểm tra sau khi tải. Để đổi đoạn, chọn checkpoint khác ở cửa sổ này. Để trở lại save chính, Stop rồi Play bình thường và chọn Tiếp tục.",MessageType.None);
    }
    static void Request(DeveloperCheckpointCatalog.Entry entry)
    {
        SessionState.SetString(Pending,entry.Id);status="Đang tải: "+entry.Title;
        if(EditorApplication.isPlayingOrWillChangePlaymode)
        {SessionState.SetString(Phase,"leaving");EditorApplication.isPlaying=false;}
        else BeginPlay();
    }
    static void BeginPlay()
    {
        if(SceneManager.GetActiveScene().path!=ScenePath)
        {
            if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()){Clear("Đã hủy tải checkpoint.");return;}
            EditorSceneManager.OpenScene(ScenePath);
        }
        SessionState.SetString(Phase,"entering");EditorApplication.isPlaying=true;
    }
    static void OnPlayState(PlayModeStateChange change)
    {
        if(string.IsNullOrEmpty(SessionState.GetString(Pending,"")))return;
        if(change==PlayModeStateChange.EnteredEditMode)
        {
            if(SessionState.GetString(Phase,"")=="leaving")EditorApplication.delayCall+=BeginPlay;
            else Clear("Đã dừng tải checkpoint.");
        }
        if(change==PlayModeStateChange.EnteredPlayMode)SessionState.SetString(Phase,"waiting");
    }
    static void ApplyWhenReady()
    {
        if(EditorApplication.isCompiling || EditorApplication.isUpdating || !EditorApplication.isPlaying || SessionState.GetString(Phase,"")!="waiting")return;
        string id=SessionState.GetString(Pending,"");if(string.IsNullOrEmpty(id))return;
        var ui=Object.FindAnyObjectByType<YearOneCampaignUI>();
        // Startup is shown only after the existing milestone controller binds restore handlers.
        if(ui==null || !ui.StartupVisible)return;
        try
        {
            var entry=Array.Find(DeveloperCheckpointCatalog.Entries,e=>e.Id==id);
            if(entry==null)throw new InvalidOperationException("Checkpoint không tồn tại: "+id);
            var controller=Object.FindAnyObjectByType<YearOneCampaignController>();
            var directory=Path.Combine(Application.temporaryCachePath,"Room404-ManualCheckpoints",id,Guid.NewGuid().ToString("N"));
            controller.LoadDeveloperCheckpoint(entry.Progress(),directory);
            ui.RestoreDeveloperCheckpoint();ApplyPresentation(entry,controller);
            Clear("Đã tải: "+entry.Title+" • Save riêng: "+directory);
        }
        catch(Exception error){Clear("Không tải được checkpoint: "+error.Message);Debug.LogException(error);}
    }
    static void ApplyPresentation(DeveloperCheckpointCatalog.Entry entry,YearOneCampaignController controller)
    {
        var view=entry.View;if(view==DeveloperCheckpointCatalog.Presentation.Room)return;
        var player=Object.FindAnyObjectByType<PlayerInteractionStateController>();
        if(view==DeveloperCheckpointCatalog.Presentation.BedLeft || view==DeveloperCheckpointCatalog.Presentation.BedRight)
        {
            var beds=Object.FindObjectsByType<BedInteractable>();Array.Sort(beds,(a,b)=>a.transform.position.x.CompareTo(b.transform.position.x));
            var bed=beds[view==DeveloperCheckpointCatalog.Presentation.BedLeft?0:beds.Length-1];bed.ExecuteImmediateEnter(player);return;
        }
        var chairs=Object.FindObjectsByType<ChairInteractable>();Array.Sort(chairs,(a,b)=>a.SeatPivot.position.x.CompareTo(b.SeatPivot.position.x));
        var chair=chairs[view==DeveloperCheckpointCatalog.Presentation.SeatRight?chairs.Length-1:0];chair.Interact(player);
        var desk=Object.FindAnyObjectByType<DeskActionModal>();
        if(view==DeveloperCheckpointCatalog.Presentation.SeatLeft || view==DeveloperCheckpointCatalog.Presentation.SeatRight)return;
        if(view==DeveloperCheckpointCatalog.Presentation.Study)
        {
            desk.Study();
            var academic=Object.FindAnyObjectByType<YearOneCampaignUI>();
            academic.CourseButtons[controller.Progress.Milestone.Semester==2?3:0].onClick.Invoke();return;
        }
        desk.UseComputer();
        if(desk.Desktop==null || !desk.Desktop.Visible)throw new InvalidOperationException("Không mở được Windows tại checkpoint.");
        switch(view)
        {
            case DeveloperCheckpointCatalog.Presentation.Schedule:desk.Desktop.ShowTab(true);break;
            case DeveloperCheckpointCatalog.Presentation.Gaming:desk.Desktop.Play("LOL");break;
            case DeveloperCheckpointCatalog.Presentation.Makeup:
                // Use the same schedule command as the makeup button, then show its activity view.
                desk.Desktop.StartMakeup(0);break;
        }
    }
    static void Clear(string message)
    {
        SessionState.EraseString(Pending);SessionState.EraseString(Phase);status=message;
        foreach(var window in Resources.FindObjectsOfTypeAll<DeveloperCheckpointWindow>())window.Repaint();
    }
}
