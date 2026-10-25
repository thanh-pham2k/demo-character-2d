using UnityEngine;

// Original procedural foley, avoiding external audio dependencies.
public static class DormSound
{
    public static AudioClip CreateWater() => Build("Dorm_WashWater", false);
    public static AudioClip CreateTyping() => Build("Dorm_Keyboard", true);
    public static AudioClip CreateWriting() => MakeFoley("Pen_Paper", 0);
    public static AudioClip CreateFootsteps() => MakeFoley("Concrete_Steps", 1);
    public static AudioClip CreateBusEngine() => MakeFoley("Bus33_Engine", 2);
    public static AudioClip CreateHotpot() => MakeFoley("Hotpot_Bubbling", 3);
    public static AudioClip CreateGlassClink() => MakeFoley("Soda_Glass_Clink", 4);
    private static AudioClip MakeFoley(string name, int type)
    {
        const int rate = 16000;
        var data = new float[rate]; var random = new System.Random(33);
        float filtered = 0f;
        for (int i=0;i<rate;i++)
        {
            float t = (float)i / rate;
            float noise = (float)random.NextDouble()*2f-1f;
            filtered = Mathf.Lerp(filtered,noise,.08f);
            float step = (i % 6400) / 6400f;
            data[i] = type == 0 ? filtered * (.2f + .4f*Mathf.Abs(Mathf.Sin(t*19f))) :
                type == 1 ? (filtered*.8f + Mathf.Sin(t*400f)*.25f) * Mathf.Exp(-step*25f) :
                type == 2 ? Mathf.Sin(t*2f*Mathf.PI*65f)*.12f + Mathf.Sin(t*2f*Mathf.PI*97f)*.08f + filtered*.13f :
                type == 3 ? filtered*(.3f+.5f*Mathf.Pow(Mathf.Max(0,Mathf.Sin(t*51f)),8)) :
                (Mathf.Sin(t*2f*Mathf.PI*2200f)+Mathf.Sin(t*2f*Mathf.PI*3300f)*.4f)*Mathf.Exp(-t*9f)*.25f;
        }
        var clip = AudioClip.Create(name,rate,1,rate,false);clip.SetData(data,0);return clip;
    }
    private static AudioClip Build(string name, bool typing)
    {
        const int rate = 16000;
        var samples = new float[rate];
        var random = new System.Random(404);
        float filtered = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            float noise = (float)random.NextDouble() * 2f - 1f;
            filtered = Mathf.Lerp(filtered, noise, .17f);
            float phase = (i % 2000) / 2000f;
            samples[i] = typing ? noise * Mathf.Exp(-phase * 85f) * .45f :
                (filtered * .65f + noise * .15f) * (.7f + .3f * Mathf.Sin(i * .0015f));
        }
        AudioClip clip = AudioClip.Create(name, samples.Length, 1, rate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
