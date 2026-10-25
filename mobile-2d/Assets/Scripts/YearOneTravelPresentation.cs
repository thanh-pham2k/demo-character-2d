using UnityEngine;

// Shared text rules keep the expired departure clock out of the recovery phase.
public static class YearOneTravelPresentation
{
    public static string Objective(YearOneProgress progress,bool busAtStop)
    {
        if(progress.Milestone.MissableBus && progress.Has("day:4:missed") && !progress.Has(progress.TripId(false)))
            return busAtStop?"Lên chuyến sau • Vé 7.000đ [E]":"Tới trạm 33 và chờ chuyến sau";
        return "Chuẩn bị balo, WC và đi học";
    }
    public static string Countdown(YearOneProgress progress)
    {
        if(!progress.Milestone.MissableBus || progress.Has(progress.TripId(false)) || progress.Has("day:4:missed"))return "";
        return " • Xe: "+Mathf.CeilToInt(progress.morningSecondsRemaining)+" phút game";
    }
}
