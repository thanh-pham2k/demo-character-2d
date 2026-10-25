using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionAudio
{
    [Serializable] public sealed class ProductionAudioCue
    {
        public string id, group, source, rights, status;
        public float duration, volume;
        public bool loop;
    }
    [Serializable] public sealed class ProductionAudioEvent
    {
        public int mission;
        public string step, cue;
    }
    [Serializable] public sealed class ProductionAudioScene
    {
        public string id, ambience, music;
    }
    [Serializable] public sealed class ProductionAudioCatalog
    {
        public ProductionAudioCue[] cues;
        public ProductionAudioEvent[] events;
        public ProductionAudioScene[] scenes;
        public static ProductionAudioCatalog Load()
        {
            var asset = Resources.Load<TextAsset>("ProductionAudio/catalog");
            if (asset == null) throw new InvalidOperationException("Missing ProductionAudio/catalog");
            return JsonUtility.FromJson<ProductionAudioCatalog>(asset.text);
        }
        public ProductionAudioCue Find(string id)
        {
            if (cues != null) foreach (var cue in cues) if (cue.id == id) return cue;
            return null;
        }
        public List<string> Validate()
        {
            var errors = new List<string>(); var ids = new HashSet<string>();
            foreach (var cue in cues ?? new ProductionAudioCue[0])
            {
                if (string.IsNullOrEmpty(cue.id) || !ids.Add(cue.id)) errors.Add("Duplicate/empty cue: " + cue.id);
                if (cue.volume < 0 || cue.volume > 1 || cue.duration <= 0) errors.Add("Invalid levels: " + cue.id);
            }
            var keys = new HashSet<string>();
            foreach (var e in events ?? new ProductionAudioEvent[0])
                if (!keys.Add(e.mission + ":" + e.step) || !ids.Contains(e.cue)) errors.Add("Invalid event: " + e.mission + ":" + e.step);
            foreach (var s in scenes ?? new ProductionAudioScene[0])
                if ((!string.IsNullOrEmpty(s.ambience) && !ids.Contains(s.ambience)) || (!string.IsNullOrEmpty(s.music) && !ids.Contains(s.music))) errors.Add("Invalid scene: " + s.id);
            return errors;
        }
    }
}
