using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionAudio
{
    [Serializable] public sealed class ProductionAudioSettings
    {
        public float master = 1, ambience = .7f, effects = .8f, music = .6f, ui = .7f;
        public bool muted;
        public void Clamp()
        {
            master=Safe(master); ambience=Safe(ambience); effects=Safe(effects); music=Safe(music); ui=Safe(ui);
        }
        private static float Safe(float v) => float.IsNaN(v) || float.IsInfinity(v) ? 0 : Mathf.Clamp01(v);
    }

    // Composition root owns one instance. No singleton, campaign session, wallet or automatic scene mutation.
    public sealed class ProductionAudioDirector : MonoBehaviour
    {
        private sealed class Voice
        {
            public AudioSource source;
            public ProductionAudioCue cue;
            public string owner;
            public float gain, target;
        }
        private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
        private readonly HashSet<AudioClip> ownedClips = new HashSet<AudioClip>();
        private readonly HashSet<string> pauseOwners = new HashSet<string>();
        private readonly Dictionary<string, float> ducks = new Dictionary<string, float>();
        private readonly List<Voice> voices = new List<Voice>();
        private ProductionAudioCatalog catalog;
        private ProductionAudioSettings settings = new ProductionAudioSettings();
        private bool ready;
        private string scene;
        private float fadeSeconds = .6f;
        public bool IsPaused => pauseOwners.Count != 0;
        public string CurrentScene => scene;
        public string LastUnavailableCue { get; private set; }
        public ProductionAudioCatalog Catalog { get { EnsureReady(); return catalog; } }
        public ProductionAudioSettings Settings => JsonUtility.FromJson<ProductionAudioSettings>(JsonUtility.ToJson(settings));
        private void Awake() => EnsureReady();
        private void EnsureReady()
        {
            if (ready) return;
            catalog = ProductionAudioCatalog.Load();
            var errors = catalog.Validate();
            if (errors.Count != 0) throw new InvalidOperationException(string.Join("; ", errors.ToArray()));
            // 2 ambience + 2 music for crossfades, 4 owned foley loops, 8 bounded one-shots.
            for (int i=0; i<16; i++)
            {
                var child = new GameObject("AudioVoice_" + i); child.transform.SetParent(transform, false);
                var source = child.AddComponent<AudioSource>();
                source.playOnAwake=false; source.spatialBlend=0; source.ignoreListenerPause=true;
                voices.Add(new Voice { source=source });
            }
            ready=true;
        }
        private AudioClip Clip(ProductionAudioCue cue)
        {
            if (clips.TryGetValue(cue.id, out var clip)) return clip;
            clip=ProductionAudioSynthesis.Create(cue);
            if (clip == null) { LastUnavailableCue=cue.id; return null; }
            clips.Add(cue.id,clip);
            if (!cue.source.StartsWith("resource:")) ownedClips.Add(clip);
            return clip;
        }
        public void ApplySettings(ProductionAudioSettings value, bool persist = false)
        {
            if(value == null) throw new ArgumentNullException(nameof(value));
            settings=JsonUtility.FromJson<ProductionAudioSettings>(JsonUtility.ToJson(value)); settings.Clamp();
            if (persist) { PlayerPrefs.SetString("ProductionAudio.Settings.v1",JsonUtility.ToJson(settings)); PlayerPrefs.Save(); }
            UpdateVolumes();
        }
        public void LoadSettings()
        {
            string data=PlayerPrefs.GetString("ProductionAudio.Settings.v1", "");
            if (string.IsNullOrEmpty(data)) return;
            try { ApplySettings(JsonUtility.FromJson<ProductionAudioSettings>(data)); }
            catch (ArgumentException) { ApplySettings(new ProductionAudioSettings()); }
        }
        public void SetPaused(string owner, bool paused)
        {
            RequireOwner(owner); EnsureReady(); bool before=IsPaused;
            if(paused) pauseOwners.Add(owner); else pauseOwners.Remove(owner);
            if (before == IsPaused) return;
            foreach(var v in voices) { if(IsPaused) v.source.Pause(); else v.source.UnPause(); }
        }
        public void SetDuck(string owner, float gain)
        {
            RequireOwner(owner); ducks[owner]=float.IsNaN(gain)?1:Mathf.Clamp01(gain); UpdateVolumes();
        }
        public void ReleaseDuck(string owner) { ducks.Remove(owner); UpdateVolumes(); }
        public void ReleaseOwner(string owner)
        {
            StopLoop(owner); ReleaseDuck(owner); SetPaused(owner,false);
        }
        public bool PlayCue(string cueId)
        {
            EnsureReady(); if(!isActiveAndEnabled) return false; var cue=catalog.Find(cueId);
            if(cue == null || cue.loop) return false;
            var clip=Clip(cue); if(clip == null) return false;
            // Drop on saturation; do not truncate a currently audible event.
            for(int i=8;i<voices.Count;i++)
                if(voices[i].cue == null || (!IsPaused && !voices[i].source.isPlaying))
                { StartVoice(voices[i],cue,clip,null,1,false); return true; }
            return false;
        }
        public bool StartLoop(string owner, string cueId)
        {
            RequireOwner(owner); EnsureReady(); if(!isActiveAndEnabled) return false; var cue=catalog.Find(cueId);
            if(cue == null || !cue.loop || cue.group == "A_BGM") return false;
            for(int i=4;i<8;i++)
                if(voices[i].owner == owner && voices[i].cue != null && voices[i].cue.id == cueId) { voices[i].target=1; return true; }
            var clip=Clip(cue); if(clip == null) return false;
            for(int i=4;i<8;i++)
                if(voices[i].owner == owner || voices[i].cue == null)
                { StartVoice(voices[i],cue,clip,owner,0,true); return true; }
            return false;
        }
        public void StopLoop(string owner)
        {
            foreach(var v in voices) if(v.owner == owner && owner != null) v.target=0;
        }
        public bool SetScene(string id, float seconds = .6f)
        {
            EnsureReady(); if(!isActiveAndEnabled) return false; ProductionAudioScene found=null;
            foreach(var s in catalog.scenes) if(s.id == id) { found=s; break; }
            if(found == null) return false;
            if(scene == id) return true;
            scene=id; fadeSeconds=Mathf.Max(.02f,seconds);
            for(int i=4;i<8;i++) voices[i].target=0;
            Crossfade(0,found.ambience); Crossfade(2,found.music); return true;
        }
        private void Crossfade(int first, string cueId)
        {
            Voice next=voices[first].gain<=voices[first+1].gain?voices[first]:voices[first+1];
            voices[first].target=voices[first+1].target=0;
            if(string.IsNullOrEmpty(cueId)) return;
            var cue=catalog.Find(cueId); var clip=cue == null?null:Clip(cue);
            // A pending score intentionally fades to silence.
            if(clip != null) StartVoice(next,cue,clip,null,0,true);
        }
        private void StartVoice(Voice v, ProductionAudioCue cue, AudioClip clip, string owner, float gain, bool loop)
        {
            v.source.Stop(); v.cue=cue; v.owner=owner; v.gain=gain; v.target=1;
            v.source.clip=clip; v.source.loop=loop; v.source.pitch=1;
            UpdateVolumes(); v.source.Play(); if(IsPaused) v.source.Pause();
        }
        // Invoke ONLY for a successful physical action/committed event; no polling of completed save history.
        public bool OnCommittedEvent(int mission, string step)
        {
            EnsureReady(); foreach(var e in catalog.events)
                if(e.mission == mission && e.step == step) return PlayCue(e.cue);
            return false;
        }
        private void Update()
        {
            if(!ready || IsPaused) return;
            foreach(var v in voices)
            {
                if(v.cue == null) continue;
                v.gain=Mathf.MoveTowards(v.gain,v.target,Time.unscaledDeltaTime/fadeSeconds);
                if(v.target == 0 && v.gain <= 0) { v.source.Stop(); v.source.clip=null; v.cue=null; v.owner=null; }
                else if(!v.source.loop && !v.source.isPlaying) { v.cue=null; v.source.clip=null; }
            }
            UpdateVolumes();
        }
        private void UpdateVolumes()
        {
            float duck=1; foreach(var gain in ducks.Values) duck=Mathf.Min(duck,gain);
            foreach(var v in voices)
            {
                if(v.cue == null) continue;
                float bus=v.cue.group=="A_BGM"?settings.music:v.cue.group=="A_AMBIENCE"?settings.ambience:v.cue.group=="A_UI"?settings.ui:settings.effects;
                float duckGain=(v.cue.group=="A_BGM" || v.cue.group=="A_AMBIENCE")?duck:1;
                v.source.volume=settings.muted?0:settings.master*bus*v.cue.volume*v.gain*duckGain;
            }
        }
        public void StopAll()
        {
            foreach(var v in voices) { v.source.Stop(); v.source.clip=null; v.cue=null; v.owner=null; v.gain=v.target=0; }
            pauseOwners.Clear(); ducks.Clear(); scene=null;
        }
        private void OnApplicationPause(bool paused) { if(ready) SetPaused("application",paused); }
        private void OnDisable() { if(ready) StopAll(); }
        private void OnDestroy() { foreach(var clip in ownedClips) if(clip != null) Destroy(clip); }
        private static void RequireOwner(string owner)
        { if(string.IsNullOrEmpty(owner)) throw new ArgumentException("Audio owner is required",nameof(owner)); }
    }
}
