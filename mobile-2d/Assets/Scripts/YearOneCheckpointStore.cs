using System;
using System.IO;
using UnityEngine;

public sealed class YearOneCheckpointStore
{
    private readonly string path;
    public YearOneCheckpointStore(string folder){path=Path.Combine(folder,"year-one-v2.json");}
    public bool Exists=>File.Exists(path) || File.Exists(path+".bak");
    public bool HasLegacy=>File.Exists(Path.Combine(Path.GetDirectoryName(path),"year-one-v1.json")) || File.Exists(Path.Combine(Path.GetDirectoryName(path),"year-one-v1.json.bak"));
    public string Error {get;private set;}
    public bool Save(YearOneProgress progress)
    {
        Error="";
        try
        {
            if(progress==null || !progress.IsValid()){Error="Invalid v2 checkpoint";return false;}
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string temporary=path+".tmp";File.WriteAllText(temporary,JsonUtility.ToJson(progress,true));
            if(File.Exists(path))File.Replace(temporary,path,path+".bak");else File.Move(temporary,path);
            return true;
        }
        catch(Exception e) when(e is IOException || e is UnauthorizedAccessException || e is ArgumentException){Error=e.Message;return false;}
    }

    public YearOneProgress Load()
    {
        Error="";
        foreach(string candidate in new[]{path,path+".bak"})
        {
            try{if(!File.Exists(candidate))continue;var state=JsonUtility.FromJson<YearOneProgress>(File.ReadAllText(candidate));if(state!=null && state.IsValid())return state;}
            catch(Exception e) when(e is IOException || e is UnauthorizedAccessException || e is ArgumentException){Error=e.Message;}
        }
        if(string.IsNullOrEmpty(Error))Error="Không tìm thấy save v2 hợp lệ. Save v1 vẫn được giữ nguyên.";
        return null;
    }
    public void NewGame(){if(File.Exists(path))File.Delete(path);if(File.Exists(path+".bak"))File.Delete(path+".bak");}
}
