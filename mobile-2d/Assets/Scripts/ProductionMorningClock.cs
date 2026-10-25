using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// A paused gameplay countdown, separate from money/course events. Bound to the
// immutable M01-M06 prefix so reload cannot restart a late morning at120seconds.
public sealed class ProductionMorningClock
{
    [Serializable] private sealed class Checkpoint { public int version=1;public string definition,prefix;public float duration,remaining; }
    private readonly string path,definition,prefix;
    private float savedRemaining;
    public float Duration {get;private set;}
    public float Remaining {get;private set;}
    public bool Missed=>Remaining<=0;
    private static string Hash(string value){using(var sha=SHA256.Create())return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-","");}
    public ProductionMorningClock(string directory,string definitionJson,ProductionCommit[] events,float duration)
    {
        path=Path.Combine(directory,"production-morning-v1.json");definition=Hash(definitionJson);
        var history=new System.Collections.Generic.List<ProductionCommit>();foreach(var e in events)if(e.mission<=6)history.Add(e);
        prefix=Hash(JsonUtility.ToJson(new ProductionCheckpoint{events=history.ToArray()}));
        var existing=Read(path)??Read(path+".bak");
        if(existing==null && (File.Exists(path)||File.Exists(path+".bak")))throw new InvalidDataException("Morning checkpoint is invalid; preserve files and recover explicitly.");
        Duration=existing?.duration??Mathf.Clamp(duration,.1f,3600);Remaining=existing?.remaining??Duration;savedRemaining=Remaining;
        if(existing==null && !Flush(out string error))throw new IOException(error);
    }
    private Checkpoint Read(string file)
    {
        try
        {
            if(!File.Exists(file))return null;var value=JsonUtility.FromJson<Checkpoint>(File.ReadAllText(file));
            if(value==null || value.version!=1 || value.definition!=definition || value.prefix!=prefix || float.IsNaN(value.duration) || float.IsNaN(value.remaining) || value.duration<=0 || value.duration>3600 || value.remaining<0 || value.remaining>value.duration)return null;
            return value;
        }
        catch(Exception e) when(e is IOException || e is ArgumentException || e is UnauthorizedAccessException){return null;}
    }
    public bool Tick(float elapsed,out string error)
    {
        error="";Remaining=Mathf.Max(0,Remaining-Mathf.Max(0,elapsed));
        return savedRemaining-Remaining<1 && !(Remaining==0 && savedRemaining>0) || Flush(out error);
    }
    public bool Flush(out string error)
    {
        error="";string temp=path+".tmp";
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(temp,JsonUtility.ToJson(new Checkpoint{definition=definition,prefix=prefix,duration=Duration,remaining=Remaining},true));
            if(File.Exists(path))File.Replace(temp,path,Read(path)!=null?path+".bak":null);else File.Move(temp,path);
            savedRemaining=Remaining;return true;
        }
        catch(Exception e) when(e is IOException || e is UnauthorizedAccessException){error=e.Message;return false;}
        finally{if(File.Exists(temp))try{File.Delete(temp);}catch(IOException){}}
    }
}
