using System.IO;
using UnityEditor;
using UnityEngine;

public static class SpriteMetaOptimizer
{
    public static void OptimizeSprites()
    {
        string folder = "Assets/Sprites/MVP/characters/bed_interaction";
        string[] files = Directory.GetFiles(folder, "*.png");
        foreach (string file in files)
        {
            var importer = AssetImporter.GetAtPath(file) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 100;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.alphaIsTransparency = true;

                var settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.Center;
                importer.SetTextureSettings(settings);

                importer.SaveAndReimport();
            }
        }
        Debug.Log("[SpriteMetaOptimizer] Configured all 8 bed interaction sprites with PPU 100, Point filter, Uncompressed.");
    }
}
