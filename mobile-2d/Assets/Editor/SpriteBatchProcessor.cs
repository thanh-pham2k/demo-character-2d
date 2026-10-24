using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SpriteBatchProcessor
{
    public static void ProcessBedSheet()
    {
        string srcPath = "/Users/thanhpv/.gemini/antigravity/brain/e793f661-3769-4494-9067-99c22fc9d55c/luon_bed_sheet_8frames_v2_1792862892089.jpg";
        string destFolder = Path.Combine(Application.dataPath, "Sprites/MVP/characters/bed_interaction");
        string rawCopyDest = Path.Combine(Application.dataPath, "../assets/mvp_sprites/interaction/luon_bed_sheet_8frames.png");
        
        Directory.CreateDirectory(destFolder);
        Directory.CreateDirectory(Path.GetDirectoryName(rawCopyDest));

        byte[] fileData = File.ReadAllBytes(srcPath);
        Texture2D srcTex = new Texture2D(2, 2);
        srcTex.LoadImage(fileData);

        int totalWidth = srcTex.width;
        int totalHeight = srcTex.height;
        int cols = 4;
        int rows = 2;
        int cellWidth = totalWidth / cols;
        int cellHeight = totalHeight / rows;

        Texture2D sheetRgba = new Texture2D(totalWidth, totalHeight, TextureFormat.RGBA32, false);
        for (int y = 0; y < totalHeight; y++)
        {
            for (int x = 0; x < totalWidth; x++)
            {
                Color c = srcTex.GetPixel(x, y);
                if (c.r > 0.90f && c.g > 0.90f && c.b > 0.90f)
                {
                    sheetRgba.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
                else
                {
                    sheetRgba.SetPixel(x, y, c);
                }
            }
        }
        sheetRgba.Apply();
        File.WriteAllBytes(rawCopyDest, sheetRgba.EncodeToPNG());

        string[] names = new string[]
        {
            // Bottom row in texture coords (y=0) is Enter poses (Row 2 in grid)
            "luon_bed_enter_01",
            "luon_bed_enter_02",
            "luon_bed_enter_03",
            "luon_bed_enter_04",
            // Top row in texture coords (y=cellHeight) is Lying poses (Row 1 in grid)
            "luon_bed_lying_back",
            "luon_bed_lying_left",
            "luon_bed_lying_right",
            "luon_bed_sleeping"
        };

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                string spriteName = names[r * cols + c];
                Texture2D cellTex = new Texture2D(cellWidth, cellHeight, TextureFormat.RGBA32, false);

                for (int cy = 0; cy < cellHeight; cy++)
                {
                    for (int cx = 0; cx < cellWidth; cx++)
                    {
                        int srcX = c * cellWidth + cx;
                        int srcY = r * cellHeight + cy;
                        Color col = srcTex.GetPixel(srcX, srcY);
                        if (col.r > 0.90f && col.g > 0.90f && col.b > 0.90f)
                        {
                            cellTex.SetPixel(cx, cy, new Color(0, 0, 0, 0));
                        }
                        else
                        {
                            cellTex.SetPixel(cx, cy, col);
                        }
                    }
                }
                cellTex.Apply();
                string outPath = Path.Combine(destFolder, spriteName + ".png");
                File.WriteAllBytes(outPath, cellTex.EncodeToPNG());
            }
        }

        AssetDatabase.Refresh();
        SpriteMetaOptimizer.OptimizeSprites();
        Debug.Log("[SpriteBatchProcessor] Successfully imported pixel-art v2 bed sheet frames!");
    }
}
