using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionChapters.Year3
{
    public sealed class Year3ChapterModule : MonoBehaviour,IProductionChapterModule,IInteractable
    {
        public Sprite cafe,lake,repairPose,jacketPose,crush,boyfriend,bench,tea,food,sign;
        public Sprite[] friends;
        ProductionChapterContext context;
        GameObject panel,stage;
        Text title,body;
        Transform buttons;
        string stepKey,choice="",feedback="";
        int mission,cursor;
        bool opened,suspended;
        Vector3 cameraPosition;
        bool cameraSaved;
        public int FirstMission=>16;
        public int LastMission=>23;
        public bool CanInteract=>!suspended && InRange && !opened;
        public string InteractionPrompt=>"Năm 3 • "+(context?.Progress.CurrentStep?.label??"");
        bool InRange=>context!=null && context.Progress.CurrentMission>=FirstMission && context.Progress.CurrentMission<=LastMission;
        public Transform GetTransform()=>transform;
        public void Configure(ProductionChapterContext value) { Suspend();context=value;suspended=false;Refresh(); }
        public void Refresh()
        {
            if(!InRange || suspended){Close();return;}
            if(opened && (mission!=context.Progress.CurrentMission || stepKey!=context.Progress.CurrentStep.id)) Close();
        }
        public void Interact()
        {
            if(!CanInteract || context.Player==null)return;
            if(!context.Player.TryEnterState(PlayerInteractionState.Transitioning,null,this,false))return;
            opened=true;mission=context.Progress.CurrentMission;stepKey=context.Progress.CurrentStep.id;cursor=0;choice="";feedback="";
            BuildUI();BuildStage();Draw();
        }
        void Update()
        {
            Refresh();
            if(opened && Input.GetKeyDown(KeyCode.Escape))Close();
            else if(CanInteract && Input.GetKeyDown(KeyCode.E))Interact();
        }
        void BuildUI()
        {
            panel=new GameObject("Year3 activity",typeof(RectTransform),typeof(Image));panel.transform.SetParent(context.UiRoot,false);
            var rect=(RectTransform)panel.transform;rect.anchorMin=new Vector2(.08f,.08f);rect.anchorMax=new Vector2(.92f,.92f);rect.offsetMin=rect.offsetMax=Vector2.zero;
            panel.GetComponent<Image>().color=new Color(.06f,.09f,.13f,.97f);
            var layout=panel.AddComponent<VerticalLayoutGroup>();layout.padding=new RectOffset(24,24,14,14);layout.spacing=5;layout.childControlHeight=true;layout.childForceExpandHeight=false;
            title=Label(panel.transform,28,52);body=Label(panel.transform,20,140);
            var go=new GameObject("Actions",typeof(RectTransform),typeof(VerticalLayoutGroup));go.transform.SetParent(panel.transform,false);buttons=go.transform;
            var list=go.GetComponent<VerticalLayoutGroup>();list.spacing=4;list.childForceExpandHeight=false;
            var flex=go.AddComponent<LayoutElement>();flex.flexibleHeight=1;
            Button(panel.transform,"Hủy • giữ checkpoint",Close);
        }
        Text Label(Transform root,int size,int height)
        {
            var go=new GameObject("Text",typeof(RectTransform),typeof(Text),typeof(LayoutElement));go.transform.SetParent(root,false);
            var text=go.GetComponent<Text>();text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.fontSize=size;text.color=Color.white;text.horizontalOverflow=HorizontalWrapMode.Wrap;
            go.GetComponent<LayoutElement>().preferredHeight=height;return text;
        }
        void Button(Transform root,string caption,Action action)
        {
            var go=new GameObject(caption,typeof(RectTransform),typeof(Image),typeof(Button),typeof(LayoutElement));go.transform.SetParent(root,false);
            go.GetComponent<Image>().color=new Color(.19f,.29f,.36f);go.GetComponent<LayoutElement>().preferredHeight=44;
            var label=Label(go.transform,18,40);label.text=caption;label.alignment=TextAnchor.MiddleCenter;
            var r=(RectTransform)label.transform;r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;
            go.GetComponent<Button>().onClick.AddListener(()=>action());
        }
        void Draw()
        {
            foreach(Transform child in buttons) {child.gameObject.SetActive(false);Destroy(child.gameObject);}
            var step=context.Progress.CurrentStep;
            title.text="M"+mission+" • "+step.label+"     Ví "+context.Progress.Money.ToString("N0")+"đ • "+context.Progress.Credits+" TC";
            if(stepKey=="answer_mother")
            {
                body.text="Mẹ gọi hỏi học hành. Chọn cách trả lời; mẹ có phản ứng riêng.\n"+feedback;
                foreach(var option in step.choices) {var captured=option;Button(buttons,captured.label,()=>{
                    choice=captured.id;feedback=choice=="honest"?"Mẹ: Cảm ơn con nói thật. Mình chia lịch học lại cho vừa sức.":"Mẹ: Mẹ cùng con tính khoản học lại. Đừng bỏ bữa để làm thêm. Hỏi học vụ nếu cần giãn phí.";
                    foreach(Transform child in buttons){child.gameObject.SetActive(false);Destroy(child.gameObject);}body.text=feedback;
                    Button(buttons,"Nói với mẹ: con sẽ lên lịch học lại",Commit);
                });}return;
            }
            var activity=Year3Activities.Get(stepKey);
            if(activity==null){body.text="Chưa có adapter cho bước này. Hủy để giữ checkpoint.";return;}
            body.text=activity.Prompt+"\n"+cursor+"/"+activity.Actions.Length+" • "+feedback;
            if(cursor==activity.Actions.Length){Button(buttons,"Lưu kết quả thao tác",Commit);Button(buttons,"Làm lại phần này",()=>{cursor=0;feedback="";Draw();});return;}
            // All uncompleted actions stay available, so ordering is validated by
            // the minigame rather than the UI silently supplying the answer.
            for(int i=activity.Actions.Length-1;i>=cursor;i--){int index=i;Button(buttons,activity.Actions[i],()=>Select(index));}
            foreach(var wrong in activity.Distractors)Button(buttons,wrong,()=>Select(-1));
        }
        void Select(int index)
        {
            if(index!=cursor){feedback="Sai thứ tự hoặc thao tác không an toàn. Kiểm tra lại checklist; chưa ghi thay đổi.";Draw();return;}
            cursor++;feedback="Đã kiểm tra.";Draw();
        }
        void Commit()
        {
            if(!opened || context.Progress.CurrentMission!=mission || context.Progress.CurrentStep.id!=stepKey)return;
            if(stepKey!="answer_mother" && cursor!=Year3Activities.Get(stepKey)?.Actions.Length)return;
            if(!context.Commit(mission,stepKey,choice,out var error)){feedback="Lưu chưa thành công: "+error;Draw();return;}
            Close();context.Notify("Đã lưu thao tác. "+(InRange?"Nhấn E để tiếp tục năm 3.":"Đạm đang đợi sách ở lab."));
        }
        void BuildStage()
        {
            stage=new GameObject("Year3 native staging");stage.transform.SetParent(context.WorldRoot,false);
            var zone=context.Progress.CurrentStep.zone;
            bool outdoors=zone=="Cafe" || zone=="Lake";
            if(outdoors && context.View!=null){cameraPosition=context.View.transform.position;cameraSaved=true;context.View.transform.position=new Vector3(150,0,cameraPosition.z);}
            var origin=outdoors?new Vector3(150,0,0):transform.position;
            if(zone=="Cafe"){Place(cafe,origin,0,14);Place(crush,origin+new Vector3(1,-1,0),4,2);Place(boyfriend,origin+new Vector3(3,-1,0),4,2);Place(jacketPose,origin+new Vector3(-2,-1,0),4,2);}
            else if(zone=="Lake"){
                Place(lake,origin,0,14);Place(bench,origin+new Vector3(0,-2,0),2,6);
                if(friends!=null)for(int i=0;i<friends.Length;i++)Place(friends[i],origin+new Vector3(-3+i*2,-1,0),4,2);
                Place(tea,origin+new Vector3(-.5f,-2,0),5,.6f);Place(food,origin+new Vector3(.5f,-2,0),5,.7f);
            }else {Place(repairPose,origin+new Vector3(2,0,0),10,2);if(context.Progress.Has("action:18:post_service_sign"))Place(sign,origin+new Vector3(3,1,0),11,1);}
        }
        void Place(Sprite sprite,Vector3 position,int order,float width)
        {
            if(sprite==null)return;var go=new GameObject(sprite.name);go.transform.SetParent(stage.transform,false);go.transform.position=position;
            var renderer=go.AddComponent<SpriteRenderer>();renderer.sprite=sprite;renderer.sortingOrder=order;go.transform.localScale=Vector3.one*(width/Mathf.Max(.01f,sprite.bounds.size.x));
        }
        public void Suspend(){suspended=true;Close();}
        void Close()
        {
            opened=false;
            if(context?.Player!=null && context.Player.IsInteractingWith(this))context.Player.ExitInteraction();
            if(cameraSaved && context?.View!=null)context.View.transform.position=cameraPosition;
            cameraSaved=false;
            if(panel!=null){panel.SetActive(false);Destroy(panel);}if(stage!=null){stage.SetActive(false);Destroy(stage);}
        }
        void OnDisable()=>Close();
        void OnDestroy()=>Close();
    }
}
