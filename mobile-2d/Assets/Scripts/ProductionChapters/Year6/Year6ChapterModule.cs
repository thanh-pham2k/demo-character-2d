using System;
using UnityEngine;

namespace ProductionChapters.Year6
{
    [Serializable] public sealed class Year6Art
    {
        public string id;
        public Sprite sprite;
    }
    [DisallowMultipleComponent]
    public sealed class Year6ChapterModule : MonoBehaviour,IProductionChapterModule,IInteractable
    {
        public Year6Art[] art=Array.Empty<Year6Art>();
        public int FirstMission=>37;
        public int LastMission=>45;
        public bool CanInteract=>active && context!=null && context.Progress.CurrentMission>=37 && context.Progress.CurrentMission<=45 && !context.Progress.Finished;
        public string InteractionPrompt=>"Mở công việc năm cuối";
        public Transform GetTransform()=>transform;
        ProductionChapterContext context;
        Year6Attempt attempt=new Year6Attempt();
        Year6Workbench bench;
        Year6Activity activity;
        GameObject stage;
        bool active,opened,ownsPlayer,creditsStarted,savePending;
        float creditsElapsed;
        string error="",current="";
        Vector2 scroll;
        SpriteRenderer[] playerRenderers=Array.Empty<SpriteRenderer>();
        bool[] rendererEnabled=Array.Empty<bool>();
        static readonly string[] DefenseQuestions={
            "GVHD: Đề tài nào em theo đuổi từ đầu đến cuối?",
            "Chủ tịch: Hệ thống suy ra lỗi thiết bị từ nguồn nào?",
            "Giám khảo A: Làm sao tránh vòng lặp khi mạng mất kết nối?",
            "Giám khảo B: Em bảo vệ API bằng những lớp kiểm tra nào?",
            "Giám khảo C: Ca kiểm thử nào chứng minh đã sửa crash cuối?"
        };
        public void Configure(ProductionChapterContext injected)
        {
            Suspend();context=injected??throw new ArgumentNullException(nameof(injected));
            active=true;Refresh();
        }
        public void Refresh()
        {
            if(context==null || !active)return;
            if(!CanInteract){Close();ClearStage();return;}
            string step=context.Progress.CurrentStep.id;
            if(step==current)return;
            current=step;activity=Year6Activities.Find(step);attempt.Reset();error="";scroll=Vector2.zero;
            creditsElapsed=0;creditsStarted=false;savePending=false;
            bench=new Year6Workbench();bench.Restore(context.Progress);
            if(activity==null){error="Year6 chưa có activity: "+step;return;}
            RebuildStage();
        }
        public void Interact()
        {
            if(!CanInteract || opened)return;
            if(context.Player!=null)
            {
                if(!context.Player.TryEnterState(PlayerInteractionState.Transitioning,null,this,false))return;
                ownsPlayer=true;
                playerRenderers=context.Player.GetComponentsInChildren<SpriteRenderer>(true);
                rendererEnabled=new bool[playerRenderers.Length];
                for(int i=0;i<playerRenderers.Length;i++){rendererEnabled[i]=playerRenderers[i].enabled;playerRenderers[i].enabled=false;}
            }
            opened=true;
        }
        void Close()
        {
            opened=false;
            for(int i=0;i<playerRenderers.Length;i++)if(playerRenderers[i]!=null)playerRenderers[i].enabled=rendererEnabled[i];
            playerRenderers=Array.Empty<SpriteRenderer>();rendererEnabled=Array.Empty<bool>();
            if(ownsPlayer && context!=null && context.Player!=null && context.Player.IsInteractingWith(this))context.Player.ExitInteraction();
            ownsPlayer=false;
        }
        public void Suspend(){active=false;Close();ClearStage();current="";activity=null;creditsStarted=false;}
        void OnDisable()=>Suspend();
        void OnDestroy()=>Suspend();
        void Update()
        {
            if(!active || context==null)return;
            Refresh();
            if(opened && Input.GetKeyDown(KeyCode.Escape)){Close();return;}
            if(opened && creditsStarted && Time.timeScale>0)
            {
                creditsElapsed+=Time.deltaTime;
                if(creditsElapsed>=activity.Seconds){creditsStarted=false;Choose("Xem credits");}
            }
        }
        void Choose(string op)
        {
            if(!CanInteract || !opened || activity==null || Time.timeScale<=0)return;
            if(attempt.Confidence==0)return;
            bool passed=attempt.Choose(activity,op);
            // Apply only accepted operations. A rejected choice cannot mutate the network.
            if(attempt.Feedback.StartsWith("Đúng",StringComparison.Ordinal) || passed)bench.Apply(op);
            if(!passed)return;
            if(!bench.Validate(activity.Step,out error))return;
            if(activity.Step=="submit_project" && Year6Activities.SubmissionSeconds(context.Progress)>=3600){error="Cổng đã đóng. Quay checkpoint kiểm thử trước khi nộp.";return;}
            savePending=true;Save();
        }
        void Save()
        {
            if(!savePending || !CanInteract || context.Progress.CurrentStep.id!=current)return;
            int mission=context.Progress.CurrentMission;string step=current;
            // Commit callback can synchronously Refresh this module, so never mutate new activity after it.
            if(!context.Commit(mission,step,"",out var failure)){error="Lưu thất bại: "+failure;return;}
            savePending=false;Close();Refresh();
        }
        void Rehearse()
        {
            attempt.Reset();bench=new Year6Workbench();bench.Restore(context.Progress);error="";creditsStarted=false;creditsElapsed=0;
        }
        void OnGUI()
        {
            if(!CanInteract)return;
            float scale=Mathf.Clamp(Screen.width/1100f,.65f,1.8f);
            var prior=GUI.matrix;GUI.matrix=Matrix4x4.Scale(new Vector3(scale,scale,1));
            float w=Screen.width/scale,h=Screen.height/scale;
            string selected=null;bool retrySave=false,rehearse=false,close=false;
            if(!opened)
            {
                if(GUI.Button(new Rect(20,h-76,330,52),"Năm cuối · "+context.Progress.CurrentStep.label))Interact();
                GUI.matrix=prior;return;
            }
            GUILayout.BeginArea(new Rect(22,22,w-44,h-44),GUI.skin.box);
            GUILayout.Label("M"+context.Progress.CurrentMission+" · "+context.Progress.CurrentStep.zone+" · "+context.Progress.Credits+"/150 tín chỉ");
            if(context.Progress.CurrentMission==42)
            {
                var clock=TimeSpan.FromSeconds(22*3600+45*60+Year6Activities.SubmissionSeconds(context.Progress));
                GUILayout.Label("Đồng hồ công việc "+clock.ToString(@"hh\:mm\:ss")+" · hạn 23:45 · thời gian mô phỏng theo công việc đã lưu");
            }
            GUILayout.Label(activity==null?error:activity.Prompt);
            if(activity!=null)
            {
                if(activity.Step=="pass_defense_qa" && attempt.Position<DefenseQuestions.Length)GUILayout.Label(DefenseQuestions[attempt.Position]);
                if(context.Progress.CurrentMission==44)GUILayout.Label("Góc người mới 1, 2, 3: giữ nguyên · Đèn: "+(context.Progress.Has("action:44:turn_off_light")?"tắt":"bật")+" · Cửa: "+(context.Progress.Has("action:44:close_room_door")?"đã khép":"mở"));
                GUILayout.Label("Thao tác "+attempt.Position+"/"+activity.Required.Length+" · tự tin "+attempt.Confidence+"/3");
                scroll=GUILayout.BeginScrollView(scroll);
                GUI.enabled=Time.timeScale>0 && attempt.Confidence>0 && !attempt.Passed && !creditsStarted;
                foreach(var op in activity.Operations)
                {
                    if(GUILayout.Button(op,GUILayout.MinHeight(42)))
                    {
                        selected=op;
                    }
                }
                GUI.enabled=true;
                if(creditsStarted)GUILayout.Label("Credits · "+Mathf.FloorToInt(creditsElapsed)+" / "+activity.Seconds+" giây");
                GUILayout.EndScrollView();
                GUILayout.Label(attempt.Feedback);
                if(error.Length>0)GUILayout.Label(error);
                GUI.enabled=Time.timeScale>0;
                if(savePending){if(GUILayout.Button("Thử lưu lại checkpoint",GUILayout.Height(38)))retrySave=true;}
                else if(GUILayout.Button("Tập dượt lại thao tác này",GUILayout.Height(38)))rehearse=true;
                GUI.enabled=true;
            }
            if(GUILayout.Button("Đóng / tạm dừng",GUILayout.Height(38)))close=true;
            GUILayout.EndArea();GUI.matrix=prior;
            // Execute after finishing IMGUI layout: commit may synchronously change the chapter.
            if(close)Close();
            else if(retrySave)Save();
            else if(rehearse)Rehearse();
            else if(selected!=null){if(activity.Seconds>0){creditsStarted=true;creditsElapsed=0;}else Choose(selected);}
        }
        Sprite Find(string id){foreach(var a in art)if(a!=null && a.id==id)return a.sprite;return null;}
        void ClearStage(){if(stage!=null){stage.SetActive(false);Destroy(stage);}stage=null;}
        void Place(string id,float x,float y,float height,int order)
        {
            Sprite sprite=Find(id);if(sprite==null)return;
            var go=new GameObject(id);go.transform.SetParent(stage.transform,false);go.transform.localPosition=new Vector3(x,y,0);
            var r=go.AddComponent<SpriteRenderer>();r.sprite=sprite;r.sortingOrder=order;
            float size=sprite.bounds.size.y;if(size>0)go.transform.localScale=Vector3.one*(height/size);
        }
        void RebuildStage()
        {
            ClearStage();stage=new GameObject("Year6 owned stage");stage.transform.SetParent(context.WorldRoot!=null?context.WorldRoot:transform,false);
            if(context.View!=null)stage.transform.position=new Vector3(context.View.transform.position.x,context.View.transform.position.y,0);
            string zone=context.Progress.CurrentStep.zone;
            if(zone=="DefenseRoom")
            {
                Place("defense_room_background",0,0,8,-5);Place("projector_screen_blank",2,1,2,1);
                Place("committee_table",0,-1.2f,1.5f,4);
                string[] people={"teacher","committee_chair","committee_examiner_a","committee_examiner_b","committee_examiner_c"};
                for(int i=0;i<5;i++)Place(people[i],-4+i*1.4f,-.3f,1.8f,2);
                Place("luon_presenting",3,-1.5f,2.3f,5);Place("podium",3,-2,1.3f,6);
            }
            else if(zone=="GraduationHall")
            {
                Place("a5_graduation_hall_background",0,0,8,-5);Place("graduation_backdrop_blank",0,1,3,0);
                Place("luon_robe_front_idle",0,-1,2.7f,3);Place("mother_luon",-1,-1,2.5f,2);Place("father_luon",1,-1,2.5f,2);Place("bouquet",0,-1,1,3);
            }
            else if(zone=="BinhDong")
            {
                Place("binh_dong_night_background",0,0,8,-5);Place("beer_table",0,-2.8f,1,1);
                string[] people={"luon_reunion_arm","dam_reunion_arm","long_reunion_arm","to_reunion_arm"};
                for(int i=0;i<4;i++)Place(people[i],-2.4f+i*1.6f,-1.3f,2.8f,3);
            }
            else if(zone=="AdvisorOffice"){Place("advisor_office_background",0,0,8,-5);Place("teacher",2,-1,2.5f,2);}
            else if(context.Progress.CurrentMission==38){Place("library_background",0,0,8,-5);}
            else if(context.Progress.CurrentMission==44)
            {
                Place("room_404_backdrop",0,0,8,-5);
                Place("luon_packing",-3,-1,2.5f,3);
                Place("light_switch_socket",-4,1,.6f,2);
                Place("bedding_freshman_a",0,-1,1.2f,2);Place("bedding_freshman_b",2,-1,1.2f,2);Place("bedding_freshman_c",4,-1,1.2f,2);
                // Freshmen objects stay present for every step; only Luon's owned item changes.
                if(!context.Progress.Has("room:luon_packed"))Place("thesis_book_blank",-3,-2,.7f,4);
            }
            else if(zone=="Reception")Place("keys_generic",0,-1,.6f,2);
            else {Place("room_404_backdrop",0,0,8,-5);Place("thesis_book_blank",2,-1,1.8f,2);}
            if(context.Progress.CurrentMission==44 && context.Progress.Has("action:44:turn_off_light"))
                foreach(var r in stage.GetComponentsInChildren<SpriteRenderer>())r.color=new Color(.22f,.25f,.4f,1);
        }
    }
}
