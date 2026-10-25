using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ProductionAudio;

// Manual only: never runs on import, refresh, startup or scene load.
public static class ProductionAudioChecks
{
    [Serializable] private sealed class MissionFile { public Mission[] missions; }
    [Serializable] private sealed class Mission { public int id; public Step[] steps; }
    [Serializable] private sealed class Step { public string id; }
    [Serializable] private sealed class ClipResult { public string cue, status; public float duration, peak, rms; }
    [Serializable] private sealed class Report { public string utc; public List<string> errors = new List<string>(); public List<ClipResult> clips = new List<ClipResult>(); public int events; }

    [MenuItem("Tools/Production Audio/Validate Catalog and Synthesis (explicit)")]
    public static void Validate()
    {
        var report=new Report { utc=DateTime.UtcNow.ToString("o") };
        var catalog=ProductionAudioCatalog.Load(); report.errors.AddRange(catalog.Validate());
        var definition=Resources.Load<TextAsset>("ProductionCampaign/missions");
        var missionFile=JsonUtility.FromJson<MissionFile>(definition.text);
        var expected=new HashSet<string>();
        foreach(var mission in missionFile.missions) foreach(var step in mission.steps) expected.Add(mission.id+":"+step.id);
        foreach(var e in catalog.events)
        {
            if(!expected.Remove(e.mission+":"+e.step)) report.errors.Add("Unknown/duplicate event: "+e.mission+":"+e.step);
            if(catalog.Find(e.cue).loop) report.errors.Add("Commit mapped to loop: "+e.step);
            report.events++;
        }
        foreach(var missing in expected) report.errors.Add("Unmapped event: "+missing);
        foreach(var cue in catalog.cues)
        {
            var result=new ClipResult {cue=cue.id,status=cue.status}; report.clips.Add(result);
            AudioClip clip=null;
            try
            {
                clip=ProductionAudioSynthesis.Create(cue);
                if(cue.status=="pending_music")
                { if(clip != null) report.errors.Add("Pending music produced a clip: "+cue.id); continue; }
                if(clip == null) { report.errors.Add("Missing clip: "+cue.id); continue; }
                var samples=new float[clip.samples*clip.channels]; clip.GetData(samples,0);
                double energy=0;
                foreach(float sample in samples)
                {
                    if(float.IsNaN(sample) || float.IsInfinity(sample)) report.errors.Add("Non-finite PCM: "+cue.id);
                    result.peak=Mathf.Max(result.peak,Mathf.Abs(sample)); energy+=sample*sample;
                }
                result.rms=(float)Math.Sqrt(energy/samples.Length); result.duration=clip.length;
                if(result.peak>1 || result.rms<=.00001f) report.errors.Add("Clipped/silent PCM: "+cue.id);
                if(Mathf.Abs(result.duration-cue.duration)>.002f) report.errors.Add("Duration mismatch: "+cue.id);
            }
            finally { if(clip != null && !cue.source.StartsWith("resource:")) UnityEngine.Object.DestroyImmediate(clip); }
        }
        string dir=Path.GetFullPath(Path.Combine(Application.dataPath,"../../review/parallel/audio"));
        Directory.CreateDirectory(dir); File.WriteAllText(Path.Combine(dir,"unity-synthesis-report.json"),JsonUtility.ToJson(report,true));
        if(report.errors.Count>0) throw new InvalidOperationException(string.Join("; ",report.errors.ToArray()));
        Debug.Log("Production audio catalog/synthesis validated. This check does not certify listening quality or Play Mode lifecycle.");
    }
}
