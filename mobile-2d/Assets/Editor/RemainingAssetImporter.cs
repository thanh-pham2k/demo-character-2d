using UnityEditor;
using System;
using System.IO;
using UnityEngine;

// Only new production outputs. Existing art/import settings are left untouched.
public sealed class RemainingAssetImporter : AssetPostprocessor
{
    [Serializable] private class SpriteConfig { public float pivotX=.5f,pivotY; public float ppu=100; }
    private void OnPreprocessTexture()
    {
        if(!assetPath.StartsWith("Assets/Sprites/MVP/remaining/") || !assetPath.EndsWith(".png"))return;
        var importer=(TextureImporter)assetImporter;
        importer.textureType=TextureImporterType.Sprite;
        importer.spriteImportMode=SpriteImportMode.Single;
        importer.spritePixelsPerUnit=100;
        var settings=new TextureImporterSettings();importer.ReadTextureSettings(settings);
        settings.spriteAlignment=(int)SpriteAlignment.Custom;
        settings.spriteMeshType=SpriteMeshType.FullRect;
        settings.spritePivot=assetPath.Contains("/E0")?new Vector2(.5f,.5f):new Vector2(.5f,0);
        string configPath=Path.ChangeExtension(assetPath,"sprite.json");
        if(File.Exists(configPath))
        {
            var config=JsonUtility.FromJson<SpriteConfig>(File.ReadAllText(configPath));
            settings.spritePivot=new Vector2(config.pivotX,config.pivotY);importer.spritePixelsPerUnit=config.ppu;
        }
        importer.SetTextureSettings(settings);
        importer.alphaIsTransparency=true;
        importer.filterMode=FilterMode.Point;
        importer.textureCompression=TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled=false;
        importer.maxTextureSize=4096;
    }
}
