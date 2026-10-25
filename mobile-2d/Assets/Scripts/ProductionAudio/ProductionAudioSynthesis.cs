using System;
using UnityEngine;

namespace ProductionAudio
{
    // Original deterministic procedural sound design. No recorded samples or external music.
    public static class ProductionAudioSynthesis
    {
        public static AudioClip Create(ProductionAudioCue cue)
        {
            switch (cue.source)
            {
                case "DormSound.CreateWater": return DormSound.CreateWater();
                case "DormSound.CreateTyping": return DormSound.CreateTyping();
                case "DormSound.CreateWriting": return DormSound.CreateWriting();
                case "DormSound.CreateFootsteps": return DormSound.CreateFootsteps();
                case "DormSound.CreateBusEngine": return DormSound.CreateBusEngine();
                case "DormSound.CreateHotpot": return DormSound.CreateHotpot();
                case "DormSound.CreateGlassClink": return DormSound.CreateGlassClink();
            }
            if (cue.status == "pending_music") return null;
            if (cue.source.StartsWith("resource:")) return Resources.Load<AudioClip>(cue.source.Substring(9));
            const int rate = 16000;
            var data = new float[Mathf.RoundToInt(rate * cue.duration)];
            int seed = 404; foreach (char c in cue.id) seed = unchecked(seed * 31 + c);
            var random = new System.Random(seed); float low = 0;
            for (int i = 0; i < data.Length; i++)
            {
                float t = (float)i / rate, n = (float)random.NextDouble() * 2 - 1;
                low += .06f * (n - low);
                float pulse = Mathf.Exp(-(t % .18f) * 55);
                float signal;
                switch (cue.source)
                {
                    case "synth:fan": signal = low*.35f + Mathf.Sin(2*Mathf.PI*60*t)*.06f; break;
                    case "synth:wind": signal = low*(.3f+.12f*Mathf.Sin(2*Mathf.PI*t/cue.duration)); break;
                    case "synth:crowd": signal = low*.3f + Mathf.Sin(2*Mathf.PI*173*t)*Mathf.Sin(2*Mathf.PI*3*t)*.015f; break;
                    case "synth:hum": signal = low*.1f + Mathf.Sin(2*Mathf.PI*100*t)*.04f; break;
                    case "synth:alarm": signal = Mathf.Sin(2*Mathf.PI*880*t)*((t%.5f)<.25f?.18f:0); break;
                    case "synth:phone": signal = Mathf.Sin(2*Mathf.PI*135*t)*((t%.6f)<.35f?.17f:0); break;
                    case "synth:keys": signal = (Mathf.Sin(2*Mathf.PI*2400*t)+n*.5f)*pulse*.2f; break;
                    case "synth:cloth": signal = low*Mathf.Sin(Mathf.PI*t/cue.duration)*.7f; break;
                    case "synth:door": signal = low*Mathf.Exp(-t*9)*1.4f + Mathf.Sin(2*Mathf.PI*95*t)*Mathf.Exp(-t*18)*.3f; break;
                    case "synth:knock": signal = (low+Mathf.Sin(2*Mathf.PI*180*t)*.3f)*Mathf.Exp(-(t%.23f)*55); break;
                    case "synth:printer": signal = (low*.4f+Mathf.Sin(2*Mathf.PI*240*t)*.06f)*(.4f+.6f*Mathf.Abs(Mathf.Sin(t*9))); break;
                    case "synth:applause": signal = n*pulse*.2f + low*.3f; break;
                    case "synth:error": signal = Mathf.Sin(2*Mathf.PI*180*t)*Mathf.Exp(-t*12)*.3f; break;
                    case "synth:success": signal = Mathf.Sin(2*Mathf.PI*(t<.12f?660:880)*t)*Mathf.Exp(-t*7)*.25f; break;
                    default: signal = (n*.3f+Mathf.Sin(2*Mathf.PI*1200*t)*.2f)*Mathf.Exp(-t*55); break;
                }
                // Short fades avoid abrupt edges; ambience is low detail, not a field recording.
                float edge = Mathf.Min(1, Mathf.Min(t/.02f, (cue.duration-t)/.03f));
                data[i] = Mathf.Clamp(signal * edge, -.85f, .85f);
            }
            var clip = AudioClip.Create("Production_" + cue.id, data.Length, 1, rate, false);
            clip.SetData(data, 0); return clip;
        }
    }
}
