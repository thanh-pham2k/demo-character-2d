public static class DeskPoseTiming
{
    // Key taps, wrist rest, mouse reach/click, then a short thinking pause.
    private static readonly float[] Holds={.14f,.11f,.18f,.22f,.10f,.45f};
    public static float Hold(int frame)=>Holds[frame%Holds.Length];
    public static bool IsKeyOrClick(int frame)=>frame%6==0 || frame%6==1 || frame%6==4;
}
