using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

// Records actual Play Mode renders with real game timestamps, not sprite previews.
[InitializeOnLoad]
public static class YearOneMotionCapture
{
    private static string directory;
    private static float start,next;
    private static int frame;
    private static StringBuilder times;
    static YearOneMotionCapture(){EditorApplication.update+=Tick;}
    public static void Begin(string name)
    {
        End();directory=Path.GetFullPath(Path.Combine(Application.dataPath,"../Temp/CampaignMotion",name));
        Directory.CreateDirectory(directory);foreach(var path in Directory.GetFiles(directory,"*.png"))File.Delete(path);
        start=Time.time;next=start;frame=0;times=new StringBuilder("file,time\n");
    }
    private static void Tick()
    {
        if(directory==null)return;
        if(!EditorApplication.isPlaying){End();return;}
        if(Time.time<next || Camera.main==null)return;
        string name="frame-"+frame.ToString("D5")+".png";
        MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.Combine(directory,name));
        times.Append(name).Append(',').Append((Time.time-start).ToString("F6",CultureInfo.InvariantCulture)).Append('\n');
        frame++;next=Time.time+1f/15;
    }
    public static void End()
    {
        if(directory==null)return;
        File.WriteAllText(Path.Combine(directory,"timestamps.csv"),times.ToString());
        File.WriteAllText(Path.Combine(directory,"duration.txt"),(Time.time-start).ToString("F6",CultureInfo.InvariantCulture));
        directory=null;times=null;
    }
}
