using System;
using UnityEngine;
using UnityEngine.UI;

// Shared injection contract for separately authored chapters. The composition
// root activates one chapter at a time; modules never create a second session.
public interface IProductionChapterModule
{
    int FirstMission {get;}
    int LastMission {get;}
    void Configure(ProductionChapterContext context);
    void Refresh();
    void Suspend();
}
public sealed class ProductionChapterContext
{
    public ProductionCampaignSession Session {get;}
    public IProductionCampaignView Progress=>Session.Progress;
    public PlayerInteractionStateController Player {get;}
    public Camera View {get;}
    public Transform WorldRoot {get;}
    public RectTransform UiRoot {get;}
    public Image Fade {get;}
    private readonly Action<string> notify;
    private readonly Action changed;
    public ProductionChapterContext(ProductionCampaignSession session,PlayerInteractionStateController player,Camera view,Transform worldRoot,RectTransform uiRoot,Image fade,Action<string> notify,Action changed)
    {
        Session=session??throw new ArgumentNullException(nameof(session));Player=player;View=view;WorldRoot=worldRoot;UiRoot=uiRoot;Fade=fade;this.notify=notify;this.changed=changed;
    }
    // Call only after the physical interaction/minigame succeeds. A failed disk
    // commit publishes no new state and must keep/recover the module's safe UI.
    public bool Commit(int mission,string step,string choice,out string error)
    {
        if(!Session.Commit(mission,step,choice,out error))return false;
        changed?.Invoke();return true;
    }
    public void Notify(string message)=>notify?.Invoke(message);
}
