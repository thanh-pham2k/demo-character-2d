using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ProductionChapters.Years45
{
    [DisallowMultipleComponent]
    public sealed class Years45ChapterModule : MonoBehaviour,IProductionChapterModule,IInteractable
    {
        public int FirstMission=>24;
        public int LastMission=>36;
        [Header("Owned staging; original room visibility is composition-root responsibility")]
        public Transform staging;
        public SpriteRenderer backdrop, focalActor;
        public Sprite labBackground,courtyardBackground,labActor,graduateActor,officeActor;
        public Sprite[] oldRoommates=new Sprite[3],freshmen=new Sprite[3],graduates=new Sprite[3];
        public SpriteRenderer[] visitingFriends=new SpriteRenderer[3];
        public SpriteRenderer[] roomSlots=new SpriteRenderer[3];
        public Transform actionAnchor;
        // Room desk, door/travel, bed/alarm, WC, H6 desk, lab/courtyard.
        public Transform[] activityAnchors=new Transform[6];
        public float interactionDistance=2.2f;
        ProductionChapterContext context;
        GameObject hud,modal;
        Text status,feedback,holdText;
        Font font;
        Years45Activity activity;
        int inputIndex;
        float holdSeconds;
        ProductionHoldButton holdButton;
        Slider pan,zoom;
        RectTransform photoSubjects;
        bool configured, suspended=true;
        string displayedKey,modalKey;
        public bool CanInteract=>configured&&!suspended&&modal==null&&InRange()&&IsOurs;
        public string InteractionPrompt=>activity==null?"":activity.Prompt;
        public Transform GetTransform()=>actionAnchor!=null?actionAnchor:transform;
        bool IsOurs=>context!=null&&context.Progress.CurrentMission>=FirstMission&&context.Progress.CurrentMission<=LastMission;
        string Key=>context==null?"":context.Progress.CurrentMission+":"+context.Progress.CurrentStep?.id;
        bool InRange()=>context!=null&&context.Player!=null&&Vector2.Distance(context.Player.transform.position,GetTransform().position)<=interactionDistance;
        public void Configure(ProductionChapterContext injected)
        {
            Suspend();context=injected??throw new ArgumentNullException(nameof(injected));
            configured=true;suspended=false;font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");Refresh();
        }
        public void Refresh()
        {
            if(!configured||!isActiveAndEnabled)return;
            suspended=false;
            if(!IsOurs){Suspend();return;}
            if(modal!=null&&modalKey!=Key)CloseModal();
            activity=Years45Activities.Find(context.Progress.CurrentMission,context.Progress.CurrentStep.id);
            int anchorIndex=AnchorIndex(context.Progress.CurrentMission,context.Progress.CurrentStep.id);
            if(activityAnchors!=null&&activityAnchors.Length>anchorIndex&&activityAnchors[anchorIndex]!=null)actionAnchor=activityAnchors[anchorIndex];
            if(staging!=null)staging.gameObject.SetActive(true);
            RefreshStage();
            if(hud==null)
            {
                hud=Panel(context.UiRoot,"Years45 HUD",new Vector2(.03f,.02f),new Vector2(.97f,.17f),new Color(.04f,.07f,.12f,.95f));
                status=Label(hud.transform,"",new Vector2(.02f,.12f),new Vector2(.77f,.94f),18);
                Button(hud.transform,"Tương tác / E",new Vector2(.79f,.18f),new Vector2(.98f,.83f),Interact);
            }
            status.text="M"+context.Progress.CurrentMission+" • "+context.Progress.CurrentStep.label+"\n"+
                "Long: "+Years45Activities.Occupant(context.Progress,0)+" | Tộ: "+Years45Activities.Occupant(context.Progress,1)+" | Đạm: "+Years45Activities.Occupant(context.Progress,2);
            displayedKey=Key;
        }
        public static int AnchorIndex(int mission,string step)
        {
            if(step=="morning_wc")return 3;
            if(step=="wake_immediately"||step=="set_alarm")return 2;
            if(mission==35||step=="study_days"||step=="arrive_class_on_time")return 4;
            if(mission==28&&step!="return_room"||mission==24&&(step=="deliver_book"||step=="listen_lab_talk"))return 5;
            if(step.Contains("goodbye")||step.Contains("leaves")||step.Contains("moves_in")||step.Contains("return")||step.Contains("travel")||step=="open_for_friends"||step=="see_to_to_work")return 1;
            return 0;
        }
        void RefreshStage()
        {
            int m=context.Progress.CurrentMission;
            bool lab=m==24&&context.Progress.Has("action:24:travel_hcmus")&&!context.Progress.Has("action:24:return_room");
            bool yard=m==28;
            if(backdrop!=null){backdrop.sprite=lab?labBackground:yard?courtyardBackground:null;backdrop.enabled=backdrop.sprite!=null;
                if(backdrop.sprite!=null)backdrop.transform.localScale=Vector3.one*(8f/backdrop.sprite.bounds.size.y);}
            if(focalActor!=null){focalActor.sprite=lab?labActor:yard?graduateActor:officeActor;focalActor.enabled=lab||m==26;}
            for(int i=0;i<3;i++)
                if(roomSlots!=null&&i<roomSlots.Length&&roomSlots[i]!=null)
                {
                    string who=Years45Activities.Occupant(context.Progress,i);
                    Sprite[] source=yard?graduates:who.StartsWith("Người mới",StringComparison.Ordinal)?freshmen:oldRoommates;
                    roomSlots[i].sprite=source!=null&&source.Length>i?source[i]:null;
                    roomSlots[i].enabled=!lab&&(yard||who!="Trống")&&roomSlots[i].sprite!=null;
                }
            bool visiting=m==34&&context.Progress.Has("action:34:open_for_friends")&&!context.Progress.Has("action:34:say_goodbye_visitors");
            for(int i=0;i<3;i++)if(visitingFriends!=null&&i<visitingFriends.Length&&visitingFriends[i]!=null)
            {
                visitingFriends[i].sprite=oldRoommates!=null&&oldRoommates.Length>i?oldRoommates[i]:null;
                visitingFriends[i].enabled=visiting&&visitingFriends[i].sprite!=null;
            }
        }
        public void Interact()
        {
            if(!CanInteract||activity==null||context.UiRoot==null)return;
            if(!context.Player.TryEnterState(PlayerInteractionState.Sitting,null,this,false))return;
            modalKey=Key;inputIndex=0;holdSeconds=0;
            modal=Panel(context.UiRoot,"Years45 activity",new Vector2(.1f,.2f),new Vector2(.9f,.94f),new Color(.06f,.09f,.15f,.99f));
            Label(modal.transform,"M"+activity.Mission+" • "+context.Progress.CurrentStep.label,new Vector2(.04f,.88f),new Vector2(.95f,.98f),23);
            Label(modal.transform,activity.Prompt,new Vector2(.04f,.65f),new Vector2(.95f,.88f),20);
            feedback=Label(modal.transform,"",new Vector2(.04f,.07f),new Vector2(.74f,.18f),17);
            Button(modal.transform,"Hủy / Esc",new Vector2(.76f,.03f),new Vector2(.96f,.13f),CloseModal);
            if(activity.Hold)
            {
                var b=Button(modal.transform,"Giữ: Gỡ game",new Vector2(.2f,.34f),new Vector2(.8f,.52f),()=>{});
                holdButton=b.gameObject.AddComponent<ProductionHoldButton>();
                holdText=Label(modal.transform,"0 / 2 giây",new Vector2(.3f,.2f),new Vector2(.7f,.32f),20);
            }
            else if(activity.Camera)
            {
                var frame=Panel(modal.transform,"Khung ảnh",new Vector2(.04f,.2f),new Vector2(.49f,.64f),new Color(.16f,.2f,.23f));
                frame.AddComponent<RectMask2D>();
                var subjects=Panel(frame.transform,"Ba bạn",Vector2.zero,Vector2.one,Color.clear);photoSubjects=(RectTransform)subjects.transform;
                for(int i=0;i<3;i++)
                {
                    var portrait=Panel(subjects.transform,"Bạn "+(i+1),new Vector2(.05f+i*.3f,.06f),new Vector2(.35f+i*.3f,.94f),Color.white);
                    var im=portrait.GetComponent<Image>();im.sprite=graduates!=null&&graduates.Length>i?graduates[i]:null;im.preserveAspect=true;im.raycastTarget=false;
                }
                pan=MakeSlider("Căn ngang",.15f,.55f);
                zoom=MakeSlider("Thu phóng",.85f,.4f);
                pan.onValueChanged.AddListener(_=>UpdatePhotoPreview());zoom.onValueChanged.AddListener(_=>UpdatePhotoPreview());UpdatePhotoPreview();
                DrawInputButtons();
            }
            else DrawInputButtons();
        }
        void DrawInputButtons()
        {
            if(modal==null)return;
            var old=modal.transform.Find("Inputs");if(old!=null){old.gameObject.SetActive(false);Destroy(old.gameObject);}
            var root=Panel(modal.transform,"Inputs",new Vector2(activity.Camera ? .53f : .04f,.19f),new Vector2(.95f,activity.Camera ? .35f : .64f),Color.clear);
            // All authored cards remain selectable; wrong order never commits an event.
            int count=activity.Inputs.Length+1;
            for(int i=0;i<count;i++)
            {
                string token=i==activity.Inputs.Length?Distractor():activity.Inputs[i];
                float top=1f-(float)i/count;
                Button(root.transform,token,new Vector2(.02f,top-.88f/count),new Vector2(.98f,top-.04f/count),()=>Submit(token));
            }
        }
        string Distractor()
        {
            if(activity.Mission==35)return activity.Step=="pass_operating_systems"?"Bỏ qua đồng bộ":activity.Step=="pass_data_structures"?"Ngăn xếp LIFO":"Học HĐH trước CTDL";
            if(activity.Mission==36||activity.Mission==27)return "Đăng ký đồ án khi chưa đủ điều kiện";
            if(activity.Mission==25)return "Gửi bản CV chưa xuất PDF";
            if(activity.Mission>=29&&activity.Mission<=31)return "Dọn cả ba giường";
            if(activity.Mission==32)return "Đặt 08:30 và bỏ lịch học";
            return "Bỏ qua việc đang làm";
        }
        public bool Submit(string token)
        {
            if(modal==null||activity==null||activity.Hold||modalKey!=Key||Time.timeScale<=0)return false;
            if(!activity.Accept(inputIndex,token)){feedback.text="Chưa đúng thao tác/thứ tự. Hãy đọc yêu cầu và thử lại.";return false;}
            if(activity.Camera&&(Mathf.Abs(pan.value-.5f)>.12f||Mathf.Abs(zoom.value-.5f)>.12f))
            {feedback.text="Chưa đủ ba bạn trong khung. Đưa hai thanh về vùng giữa.";return false;}
            inputIndex++;feedback.text="Đã làm "+inputIndex+" / "+activity.Inputs.Length;
            if(inputIndex==activity.Inputs.Length)return CommitActivity();
            return true;
        }
        bool CommitActivity()
        {
            int mission=activity.Mission;string step=activity.Step;
            if(!Years45Activities.Gate(context.Progress,mission,step,out var error)){if(feedback!=null)feedback.text=error;return false;}
            // Release only our lock before the callback can switch modules. Failed writes reopen safely.
            if(context.Player!=null&&context.Player.IsInteractingWith(this))context.Player.ExitInteraction();
            if(!context.Commit(mission,step,"",out error))
            {
                if(feedback!=null)feedback.text="Chưa lưu được: "+error+". Hủy rồi thử lại.";
                inputIndex=0;holdSeconds=0;if(holdButton!=null)holdButton.Held=false;
                if(context.Player!=null)context.Player.TryEnterState(PlayerInteractionState.Sitting,null,this,false);
                return false;
            }
            CloseModal();Refresh();return true;
        }
        void Update()
        {
            if(!configured||suspended)return;
            if(!IsOurs){Suspend();return;}
            if(displayedKey!=Key)Refresh();
            if(modal==null){if(Input.GetKeyDown(KeyCode.E))Interact();return;}
            if(Input.GetKeyDown(KeyCode.Escape)){CloseModal();return;}
            if(Time.timeScale<=0){if(holdButton!=null)holdButton.Held=false;holdSeconds=0;return;}
            if(activity!=null&&activity.Hold&&holdButton!=null)
            {
                holdSeconds=holdButton.Held?holdSeconds+Time.deltaTime:0;
                holdText.text=holdSeconds.ToString("0.0")+" / 2 giây";
                if(holdSeconds>=2f){holdButton.Held=false;CommitActivity();}
            }
        }
        void OnApplicationFocus(bool focused){if(!focused){holdSeconds=0;if(holdButton!=null)holdButton.Held=false;}}
        public void CloseModal()
        {
            holdSeconds=0;inputIndex=0;holdButton=null;pan=null;zoom=null;photoSubjects=null;modalKey=null;
            if(modal!=null){modal.SetActive(false);Destroy(modal);modal=null;}
            if(context?.Player!=null&&context.Player.IsInteractingWith(this))context.Player.ExitInteraction();
        }
        public void Suspend()
        {
            suspended=true;CloseModal();
            if(hud!=null){hud.SetActive(false);Destroy(hud);hud=null;}
            if(staging!=null)staging.gameObject.SetActive(false);
        }
        void OnDisable()=>Suspend();
        void OnDestroy()=>Suspend();
        void UpdatePhotoPreview()
        {
            if(photoSubjects==null||pan==null||zoom==null)return;
            photoSubjects.anchoredPosition=new Vector2((pan.value-.5f)*photoSubjects.rect.width*1.6f,0);
            photoSubjects.localScale=Vector3.one*Mathf.Lerp(.5f,1.5f,zoom.value);
        }
        Slider MakeSlider(string caption,float value,float y)
        {
            Label(modal.transform,caption,new Vector2(.55f,y+.04f),new Vector2(.95f,y+.09f),16);
            var go=Panel(modal.transform,caption,new Vector2(.55f,y),new Vector2(.95f,y+.035f),new Color(.22f,.25f,.3f));
            var s=go.AddComponent<Slider>();
            var handle=Panel(go.transform,"Handle",Vector2.zero,Vector2.one,new Color(.4f,.8f,.8f));
            var hr=(RectTransform)handle.transform;hr.anchorMin=new Vector2(.5f,0);hr.anchorMax=new Vector2(.5f,1);hr.sizeDelta=new Vector2(22,0);
            s.handleRect=hr;s.targetGraphic=handle.GetComponent<Image>();s.direction=Slider.Direction.LeftToRight;s.value=value;return s;
        }
        static GameObject Panel(Transform parent,string name,Vector2 min,Vector2 max,Color color)
        {
            var go=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image));go.transform.SetParent(parent,false);
            var rt=(RectTransform)go.transform;rt.anchorMin=min;rt.anchorMax=max;rt.offsetMin=rt.offsetMax=Vector2.zero;
            var im=go.GetComponent<Image>();im.color=color;im.raycastTarget=color.a>0;return go;
        }
        Text Label(Transform parent,string text,Vector2 min,Vector2 max,int size)
        {
            var go=new GameObject("Text",typeof(RectTransform),typeof(CanvasRenderer),typeof(Text));go.transform.SetParent(parent,false);
            var rt=(RectTransform)go.transform;rt.anchorMin=min;rt.anchorMax=max;rt.offsetMin=rt.offsetMax=Vector2.zero;
            var t=go.GetComponent<Text>();t.text=text;t.font=font;t.fontSize=size;t.color=Color.white;t.alignment=TextAnchor.MiddleLeft;t.raycastTarget=false;return t;
        }
        Button Button(Transform parent,string text,Vector2 min,Vector2 max,UnityEngine.Events.UnityAction action)
        {
            var go=Panel(parent,text,min,max,new Color(.16f,.28f,.38f));var b=go.AddComponent<Button>();b.targetGraphic=go.GetComponent<Image>();
            var label=Label(go.transform,text,new Vector2(.025f,0),new Vector2(.98f,1),17);label.alignment=TextAnchor.MiddleCenter;
            b.onClick.AddListener(action);return b;
        }
    }
}
