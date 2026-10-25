using UnityEngine;
using UnityEngine.UI;
using System;
namespace ProductionChapters.Year2
{
    public sealed class Year2Module : MonoBehaviour,IProductionChapterModule
    {
        public int FirstMission=>10; public int LastMission=>15;
        public PlayerInteractionStateController Player=>context?.Player;
        public Year2Challenge Challenge {get;private set;}=new Year2Challenge();
        public GameObject pluggedCooker,visiblePot,hiddenPot,trashLayer,guard;
        public int MorningEnergy => context==null?0:context.Progress.Has("choice:gaming_yes")?25:85;
        public bool MorningAttendanceMissed => context!=null && context.Progress.Has("choice:snooze");
        public Transform bedPose,deskPose;
        private ProductionChapterContext context;
        private GameObject panel;
        private Text body;
        private RectTransform options;
        private Year2WorldAction owner;
        private string shown="";
        private float elapsed;
        private bool running,paused;
        private int observedMission;
        public string Status {get;private set;}="";
        private int Mission=>context?.Progress.CurrentMission??0;
        private string Step=>context?.Progress.CurrentStep?.id??"";
        private bool InRange=>Mission>=10 && Mission<=15;
        public void Configure(ProductionChapterContext value){Suspend();context=value??throw new ArgumentNullException(nameof(value));Challenge=new Year2Challenge();observedMission=0;running=true;Refresh();}
        public bool CanOpen(string station)=>running && InRange && owner==null && station==StationFor(Mission,Step);
        public static string StationFor(int mission,string step)
        {
            if(mission==14)return "H6";
            if(mission==11 || step=="pretend_sleep" || step=="end_night")return "Giường";
            if(mission==12)return step=="unplug_cooker"?"Ổ điện":step=="hide_pot"?"Tủ":"Cửa";
            return "Laptop";
        }
        public void Open(Year2WorldAction action)
        {
            if(action==null || !CanOpen(action.station) || Player==null)return;
            owner=action;
            var pose=action.station=="Giường"?bedPose:action.station=="Laptop"?deskPose:null;
            var state=action.station=="Giường"?PlayerInteractionState.Sleeping:PlayerInteractionState.Sitting;
            if(!Player.TryEnterState(state,pose,action)){owner=null;return;}
            EnsureUi();panel.SetActive(true);shown="";Refresh();
        }
        public void Refresh()
        {
            if(context==null)return;
            if(observedMission!=Mission){observedMission=Mission;elapsed=0;}
            int order=Step=="hide_pot"?1:Step=="pretend_sleep"?2:Step=="resolve_inspection"?3:0;
            Challenge.RestoreHideOrder(order);
            if(guard!=null)guard.SetActive(Mission==12 && Step!="hear_guard");
            if(pluggedCooker!=null)pluggedCooker.SetActive(Mission==12 && order==0);
            if(visiblePot!=null)visiblePot.SetActive(Mission==12 && order<2);
            if(hiddenPot!=null)hiddenPot.SetActive(Mission==12 && order>=2);
            if(trashLayer!=null)trashLayer.SetActive(context.Progress.Has("choice:gaming_yes"));
            if(!InRange){Close();return;}
            if(owner!=null && shown!=Step){shown=Step;elapsed=0;Draw();}
        }
        private void Update()
        {
            if(!running || paused || context==null || !InRange)return;
            float dt=Time.deltaTime;elapsed+=dt;
            Challenge.Tick(dt,Mission==12 && Step!="hear_guard" && Step!="resolve_inspection",Mission==13);
            if(owner!=null && body!=null){body.text=Status+"\n"+ClockText();}
            if(Mission==12 && Challenge.Caught && owner!=null && shown!="caught"){shown="caught";Clear();Status="Bảo vệ bắt gặp. Cảnh cáo: diễn tập lại từ thao tác đang lưu.";Add("Thử lại 10 giây",()=>{Challenge.RetryStealth();shown="";Refresh();});}
            if(Mission==13 && Challenge.DeadlineMissed && owner!=null && shown!="late"){shown="late";Clear();Status="Quá hạn 23:59:59. Chưa nộp. Thử lại buổi làm bài tại checkpoint.";Add("Làm lại",()=>{Challenge.RetryAssignment();shown="";Refresh();});}
        }
        public string ClockText()=>Mission==12?"Bảo vệ: "+Challenge.StealthRemaining.ToString("0.0")+"s":Mission==13?"23:"+(40+(int)Challenge.AssignmentSeconds/60).ToString("00")+":"+((int)Challenge.AssignmentSeconds%60).ToString("00"):"";
        private bool Commit(string expected,string choice="")
        {
            if(Step!=expected || Mission<10 || Mission>15)return false;
            int before=Mission;
            if(!context.Commit(before,expected,choice,out string error)){context.Notify(error);return false;}
            Close();Refresh();return true;
        }
        private void Draw()
        {
            Clear();Status=context.Progress.CurrentStep.label;
            switch(Step)
            {
                case "choose_night": Status="23:40 — bạn rủ chơi thêm. Chọn kết quả đêm nay.";Add("YES — chơi tiếp",()=>Commit("choose_night","play"));Add("NO — đi ngủ",()=>Commit("choose_night","sleep"));break;
                case "end_night":
                    bool gaming=context.Progress.Has("choice:gaming_yes");Status=gaming?"Time-lapse 23:40 → 03:10. Rác tăng; sáng mệt, khó tập trung.":"Tắt laptop, ngủ từ 23:40. Sáng tỉnh táo và còn thời gian chuẩn bị.";
                    Add(gaming?"Kết thúc ván rồi ngủ":"Ngủ đủ giấc",()=>{if(elapsed>=3)Commit("end_night");else context.Notify("Đợi chuyển đêm 3 giây.");});break;
                case "choose_alarm":Status="06:30 — báo thức. "+(context.Progress.Has("choice:gaming_yes")?"Năng lượng thấp sau đêm chơi.":"Năng lượng tốt sau giấc ngủ.");Add("Dậy ngay",()=>Commit("choose_alarm","wake"));Add("Snooze 20 phút",()=>Commit("choose_alarm","snooze"));break;
                case "show_morning_consequence":Status=context.Progress.Has("choice:snooze")?"06:50 — muộn lớp 06:45; mất điểm danh, vẫn tới buổi học. Đây không phải buổi thi.":"06:30 — chuẩn bị và đến lớp 06:45 đúng giờ; được điểm danh.";Add("Xem lịch tối hôm khác",()=>{if(elapsed>=2)Commit("show_morning_consequence");});break;
                case "hear_guard":Status="Nồi đang cắm điện. Có tiếng chìa khóa ngoài cửa: 10 giây rút phích → cất nồi → giả ngủ.";Add("Nghe bước chân",()=>Commit("hear_guard"));break;
                case "unplug_cooker":Add("Rút phích điện",()=>Hide(0,"unplug_cooker"));break;
                case "hide_pot":Add("Nhấc nồi vào tủ",()=>Hide(1,"hide_pot"));break;
                case "pretend_sleep":Add("Nằm xuống, giả ngủ",()=>Hide(2,"pretend_sleep"));break;
                case "resolve_inspection":Status="Bảo vệ soi phòng rồi rời đi. Cất bếp xong; tin nhắc bài C++ tới.";Add("Đợi bảo vệ đi",()=>{if(elapsed>=2)Commit("resolve_inspection");});break;
                case "compile_assignment":Status="int sum(int* a,int n) { int s=0; for(int i=0;i<=n;i++) s+=a[i]; return s; }";Add("Compile / Run",()=>Commit("compile_assignment"));break;
                case "find_error":Status="AddressSanitizer: heap-buffer-overflow tại a[i], i == n. Chọn dòng lỗi.";Add("for: i <= n",()=>{Commit("find_error");});Add("return s",()=>context.Notify("Log chỉ tới truy cập mảng, không phải return."));break;
                case "patch_assignment":Status="Đổi điều kiện vòng lặp để không đọc vượt mảng.";Add("i < n",()=>{Commit("patch_assignment");});Add("i <= n + 1",()=>context.Notify("Vẫn vượt mảng."));break;
                case "test_assignment":Status="Bản sửa i < n. Test []→0; [1]→1; [1,2,3]→6; kiểm tra biên.";Add("Chạy 4 test",()=>{Commit("test_assignment");});break;
                case "submit_assignment":Status="4/4 test pass. Bài nộp muộn trong đêm nên điểm thành phần thấp; hạn 23:59:59.";Add("Upload bài C++",()=>{if(!Challenge.DeadlineMissed)Commit("submit_assignment");});break;
                case "arrive_exam":Status="H6 — đã đủ tư cách thi. Vào phòng và nhận đề.";Add("Nhận đề",()=>Commit("arrive_exam"));break;
                case "answer_known_sections":DrawQuestion();break;
                case "submit_exam":if(Challenge.ExamAnswered<5){DrawQuestion();break;}Status="Bài thi đã làm. "+(Challenge.ExamAnswered>0?"Đúng "+Challenge.ExamCorrect+"/5 ("+Challenge.ExamPercent+"%).":"Đã phục hồi bài tại checkpoint nộp.")+" Điểm học phần công bố sau.";Add("Nộp bài",()=>Commit("submit_exam"));break;
                case "open_grade_portal":Status="Portal — thông báo điểm cuối năm 2 đã công bố.";Add("Đăng nhập bảng điểm",()=>Commit("open_grade_portal"));break;
                case "read_failed_courses":Status="Kết quả học phần: Giải tích 2 F; Vật lý 1 F; Kỹ thuật lập trình F. Tổng tích lũy sau các học phần khác: 50/150. Điểm bài thi luyện tập không bị ép về 20%; F là kết quả học phần cả kỳ theo baseline.";Add("Đọc chi tiết 3 môn",()=>{if(elapsed>=2)Commit("read_failed_courses");});break;
                case "read_prerequisite_graph":Status="KTLT (F) → CTDL (khóa) → HĐH (khóa). Cần pass KTLT trước khi mở CTDL; pass CTDL trước HĐH.";Add("Xem sơ đồ môn",()=>{Commit("read_prerequisite_graph");});break;
                case "open_retaking_invoice":Status="Hóa đơn học lại: GT2 + VL1 + KTLT. Chưa thu tiền tại màn này. Mẹ đang gọi; trao đổi trước khi nhận việc sửa máy.";Add("Mở hóa đơn và nhận tin cuộc gọi",()=>{Commit("open_retaking_invoice");});break;
            }
            Add("Đóng / trở lại phòng",Close);
            body.text=Status;
        }
        private void Hide(int order,string step)
        {
            if(Challenge.Caught || Challenge.HideOrder!=order)return;
            // Advance local animation/state only after durable commit; disk failure is retryable.
            Commit(step);
        }
        private void DrawQuestion()
        {
            if(Challenge.ExamAnswered>=Year2Challenge.Questions.Length){Status="Đã làm 5 câu. Đúng "+Challenge.ExamCorrect+"/5 ("+Challenge.ExamPercent+"%).";Add("Hoàn tất phần kiến thức",()=>{if(Step=="answer_known_sections")Commit("answer_known_sections");else Draw();});return;}
            int q=Challenge.ExamAnswered;Status="Câu "+(q+1)+"/5: "+Year2Challenge.Questions[q];
            for(int i=0;i<2;i++){int answer=i;Add(Year2Challenge.Answers[q][i],()=>{Challenge.Answer(answer);Draw();});}
        }
        private void EnsureUi()
        {
            if(panel!=null)return;
            panel=new GameObject("Year2 interaction",typeof(RectTransform),typeof(Image));panel.transform.SetParent(context.UiRoot,false);
            var rect=(RectTransform)panel.transform;rect.anchorMin=new Vector2(.15f,.1f);rect.anchorMax=new Vector2(.85f,.9f);rect.offsetMin=rect.offsetMax=Vector2.zero;
            panel.GetComponent<Image>().color=new Color(.04f,.07f,.12f,.97f);
            var textObject=new GameObject("Content",typeof(RectTransform),typeof(Text));textObject.transform.SetParent(panel.transform,false);body=textObject.GetComponent<Text>();body.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");body.fontSize=22;body.color=Color.white;
            var tr=(RectTransform)textObject.transform;tr.anchorMin=new Vector2(.04f,.5f);tr.anchorMax=new Vector2(.96f,.96f);tr.offsetMin=tr.offsetMax=Vector2.zero;
            var buttons=new GameObject("Actions",typeof(RectTransform),typeof(VerticalLayoutGroup));buttons.transform.SetParent(panel.transform,false);options=(RectTransform)buttons.transform;options.anchorMin=new Vector2(.04f,.04f);options.anchorMax=new Vector2(.96f,.48f);options.offsetMin=options.offsetMax=Vector2.zero;
            var layout=buttons.GetComponent<VerticalLayoutGroup>();layout.spacing=8;layout.childForceExpandHeight=true;
        }
        private void Clear(){if(options!=null)for(int i=options.childCount-1;i>=0;i--){var child=options.GetChild(i).gameObject;child.SetActive(false);Destroy(child);}}
        private void Add(string label,Action action)
        {
            var go=new GameObject(label,typeof(RectTransform),typeof(Image),typeof(Button));go.transform.SetParent(options,false);go.GetComponent<Image>().color=new Color(.17f,.26f,.38f);go.GetComponent<Button>().onClick.AddListener(()=>action());
            var text=new GameObject("Label",typeof(RectTransform),typeof(Text));text.transform.SetParent(go.transform,false);var t=text.GetComponent<Text>();t.font=body.font;t.fontSize=19;t.color=Color.white;t.alignment=TextAnchor.MiddleCenter;t.text=label;var r=(RectTransform)text.transform;r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;
        }
        public void Close(){if(Player!=null && owner!=null && Player.IsInteractingWith(owner))Player.ExitInteraction();owner=null;if(panel!=null)panel.SetActive(false);shown="";}
        public void SetPaused(bool value){paused=value;if(value)Close();}
        public void Suspend(){running=false;Close();}
        private void OnDisable(){Suspend();}
        private void OnDestroy(){Close();if(panel!=null)Destroy(panel);}
    }
}
