using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ProductionChapters.Year6;

namespace ProductionChapters.Year6.Editor
{
    // Explicit menu only. No InitializeOnLoad, asset import hooks, scene loads or Play requests.
    public static class Year6ModuleBuilder
    {
        [MenuItem("Production/Chapters/Year6/Create owned module under selection")]
        public static void Build()
        {
            var go=new GameObject("Production Year6 M37–M45");Undo.RegisterCreatedObjectUndo(go,"Create Year6 module");
            if(Selection.activeTransform!=null)go.transform.SetParent(Selection.activeTransform,false);
            var module=go.AddComponent<Year6ChapterModule>();var assets=new List<Year6Art>();
            string[] groups={"C04","C05","P04","P05","E04","E05"};
            foreach(string group in groups)
            {
                string folder="Assets/Sprites/MVP/remaining/"+group;
                foreach(var guid in AssetDatabase.FindAssets("t:Sprite",new[]{folder}))
                {
                    string path=AssetDatabase.GUIDToAssetPath(guid);var sprite=AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    if(sprite!=null)assets.Add(new Year6Art{id=Path.GetFileNameWithoutExtension(path),sprite=sprite});
                }
            }
            Add(assets,"teacher","Assets/Sprites/MVP/year1/teacher.png");
            Add(assets,"advisor_office_background","Assets/Sprites/MVP/remaining/E03/advisor_office_background.png");
            Add(assets,"room_404_backdrop","Assets/Sprites/MVP/environment/room_404_backdrop.png");
            Add(assets,"light_switch_socket","Assets/Sprites/MVP/environment/light_switch_socket.png");
            Add(assets,"keys_generic","Assets/Sprites/MVP/remaining/P01/keys_generic.png");
            Add(assets,"luon_robe_front_idle","Assets/Sprites/MVP/remaining/AN03/luon_robe_front_idle.png");
            module.art=assets.ToArray();Selection.activeGameObject=go;EditorUtility.SetDirty(module);
            Debug.Log("Year6 created. Composition root must Configure(shared ProductionChapterContext), Refresh on commit, Suspend on exit. No session was created.");
        }
        static void Add(List<Year6Art> assets,string id,string path)
        {var sprite=AssetDatabase.LoadAssetAtPath<Sprite>(path);if(sprite!=null)assets.Add(new Year6Art{id=id,sprite=sprite});else throw new InvalidOperationException("Missing Year6 art: "+path);}
    }
}
