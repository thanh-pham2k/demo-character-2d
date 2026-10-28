using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MeasureRoom404Landmarks
{
    [Serializable] sealed class ReferenceData { public int[] image_size_px; public Landmarks landmarks; }
    [Serializable] sealed class Landmarks { public float[] window, door, left_bed_area, right_bed_area, left_desk_area, right_desk_area, left_chair, right_chair; }
    [Serializable] sealed class Row { public string name, hierarchy_path; public float[] reference_approx_px, projected_renderer_bounds_px, delta_px; }
    [Serializable] sealed class Report { public string status, method, warning; public int[] resolution; public Row[] landmarks; }
    static float[] Project(Camera camera, Transform obj)
    {
        var renderers=obj.GetComponentsInChildren<Renderer>();if(renderers.Length==0)throw new Exception("No renderers: "+obj.name);
        float left=float.PositiveInfinity, top=float.PositiveInfinity, right=float.NegativeInfinity, bottom=float.NegativeInfinity;
        foreach(var renderer in renderers)
        {
            Bounds b=renderer.bounds;
            for(int x=0;x<2;x++)for(int y=0;y<2;y++)for(int z=0;z<2;z++)
            {
                Vector3 p=b.center+Vector3.Scale(b.extents,new Vector3(x==0?-1:1,y==0?-1:1,z==0?-1:1));
                Vector3 v=camera.WorldToViewportPoint(p);float px=v.x*768,py=(1-v.y)*512;
                left=Mathf.Min(left,px);top=Mathf.Min(top,py);right=Mathf.Max(right,px);bottom=Mathf.Max(bottom,py);
            }
        }
        return new[]{left,top,right,bottom};
    }
    public static string Main()
    {
        var scene=SceneManager.GetActiveScene();if(scene.path!="Assets/dorm_room/Scenes/Room404_Blockout.unity")throw new Exception("Unexpected scene");
        var data=JsonUtility.FromJson<ReferenceData>(File.ReadAllText("Assets/dorm_room/metadata/reference_landmarks.json"));
        if(data.image_size_px[0]!=1536||data.image_size_px[1]!=1024)throw new Exception("Unexpected reference resolution");
        var cam=GameObject.Find("GameplayCamera").GetComponent<Camera>();var root=GameObject.Find("Room404_Blockout").transform;
        var aspect=cam.aspect;var rect=cam.rect;
        try
        {
            cam.rect=new Rect(0,0,1,1);cam.aspect=1.5f;
            var specs=new[]{("window","WindowPlaceholder",data.landmarks.window),("door","Door/DoorPanel",data.landmarks.door),("left_bed","LeftSet/Bed",data.landmarks.left_bed_area),("right_bed","RightSet/Bed",data.landmarks.right_bed_area),("left_desk","LeftSet/Desk",data.landmarks.left_desk_area),("right_desk","RightSet/Desk",data.landmarks.right_desk_area),("left_chair","LeftSet/Chair",data.landmarks.left_chair),("right_chair","RightSet/Chair",data.landmarks.right_chair)};
            var rows=new Row[specs.Length];
            for(int i=0;i<specs.Length;i++)
            {
                var spec=specs[i];var obj=root.Find(spec.Item2);if(obj==null||spec.Item3==null||spec.Item3.Length!=4)throw new Exception("Missing landmark "+spec.Item1);
                var actual=Project(cam,obj);var target=new float[4];var delta=new float[4];
                for(int j=0;j<4;j++){target[j]=spec.Item3[j]/2f;delta[j]=actual[j]-target[j];}
                rows[i]=new Row{name=spec.Item1,hierarchy_path=spec.Item2,reference_approx_px=target,projected_renderer_bounds_px=actual,delta_px=delta};
            }
            var report=new Report{status="unapproved_measurement",method="Projected world-axis renderer bounds at 3:2; top-left image origin",warning="Reference rectangles are approximate and may include props; bounds are conservative. Not a segmentation, visibility, collision or PASS test.",resolution=new[]{768,512},landmarks=rows};
            const string path="Assets/dorm_room/metadata/blockout_landmark_measurements.json";File.WriteAllText(path,JsonUtility.ToJson(report,true)+"\n");return path;
        }
        finally { cam.rect=rect;cam.aspect=aspect; }
    }
}
