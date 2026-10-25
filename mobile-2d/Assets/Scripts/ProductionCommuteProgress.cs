using System;

// Bind one authored trip at a time. A stale binding cannot commit the next step.
// The vehicle calls CommitArrival while faded/locked, before destination presentation
// can release the actor. Save failure leaves the session and fare unchanged.
public sealed class ProductionCommuteProgress : ICommuteProgress
{
    private readonly ProductionCampaignSession session;
    private readonly int mission;
    private readonly string step,choice;
    private readonly bool homebound;
    private readonly Func<bool> missedProvider;
    private string CurrentChoice=>mission==7 && missedProvider!=null?(missedProvider()?"next_bus":"on_time"):choice;
    public int Money=>session.Progress.Money;
    public bool MissedBus=>mission==7 && CurrentChoice=="next_bus";
    public ProductionCommuteProgress(ProductionCampaignSession session,int mission,string step,string choice="",Func<bool> missed=null)
    {
        this.session=session??throw new ArgumentNullException(nameof(session));
        if(!(mission==6 && (step=="take_bus" || step=="return_room")) && !(mission==7 && step=="catch_bus"))
            throw new ArgumentException("Only authored production school trips can bind the bus.");
        if(mission==7 ? missed==null && choice!="on_time" && choice!="next_bus" || missed!=null && choice!="" : choice!="" || missed!=null)
            throw new ArgumentException("Trip choice must match the authored departure.");
        this.mission=mission;this.step=step;this.choice=choice;missedProvider=missed;homebound=step=="return_room";
    }
    public bool CanBoard(bool returning,out string error)
    {
        error="";
        var progress=session.Progress;
        if(returning!=homebound || progress.CurrentMission!=mission || progress.CurrentStep?.id!=step)
        {error="Chuyến xe không khớp mục tiêu hiện tại.";return false;}
        if(progress.CurrentStep.cost!=CampusCommuteController.TicketPrice)
        {error="Giá vé không khớp dữ liệu chuyến xe.";return false;}
        if(Money<CampusCommuteController.TicketPrice)
        {error="Chưa đủ 7.000đ mua vé xe buýt.";return false;}
        return true;
    }
    public bool CommitArrival(bool returning,out string error)
    {
        if(!CanBoard(returning,out error))return false;
        return session.Commit(mission,step,CurrentChoice,out error);
    }
}
