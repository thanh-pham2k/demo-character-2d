using System;
using UnityEngine;

// Stable runtime lookup for generated art; no narrative or progression side effects.
[CreateAssetMenu(menuName="Room404/Remaining Asset Catalog")]
public sealed class RemainingAssetCatalog : ScriptableObject
{
    [Serializable] public sealed class Entry
    {
        public string id;
        public Sprite sprite;
        public string batch;
        public int[] missions;
    }
    [Serializable] public sealed class ClipEntry
    {
        public string id;
        public AnimationClip clip;
        public Sprite[] frames;
        public float fps=8;
    }
    public Entry[] entries=Array.Empty<Entry>();
    public ClipEntry[] animations=Array.Empty<ClipEntry>();
    public bool TryGetAnimation(string id,out ClipEntry animation)
    {
        foreach(var item in animations)if(item.id==id){animation=item;return item.clip!=null;}
        animation=null;return false;
    }
    public bool TryGet(string id,out Sprite sprite)
    {
        foreach(var item in entries)if(item.id==id){sprite=item.sprite;return sprite!=null;}
        sprite=null;return false;
    }
}
