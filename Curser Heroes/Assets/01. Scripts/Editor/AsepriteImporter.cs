using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AsepriteImporter : EditorWindow
{
    public TextAsset jsonFile;
    public Texture2D spriteSheet;
    public string savePath = "Assets/Animations";

    [MenuItem("Tools/Aseprite JSON Importer")]
    public static void ShowWindow()
    {
        GetWindow<AsepriteImporter>("Aseprite Importer (JsonUtility)");
    }

    private void OnGUI()
    {
        GUILayout.Label("Aseprite JSON + PNG Importer", EditorStyles.boldLabel);

        jsonFile = (TextAsset)EditorGUILayout.ObjectField("Aseprite JSON", jsonFile, typeof(TextAsset), false);
        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("Sprite Sheet", spriteSheet, typeof(Texture2D), false);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Import Animation"))
        {
            if (jsonFile != null && spriteSheet != null)
            {
                Import(jsonFile, spriteSheet, savePath);
            }
            else
            {
                Debug.LogError("JSON과 Sprite Sheet를 모두 지정해야 합니다.");
            }
        }
    }

    void Import(TextAsset json, Texture2D texture, string path)
    {
        Debug.Log("[Import] JSON 파일 파싱 시작");

        AsepriteRoot data = JsonUtility.FromJson<AsepriteRoot>(json.text);

        Debug.Log("[Import] 파싱 완료: " + data.frames.Count + "프레임");

        List<SpriteMetaData> metas = new List<SpriteMetaData>();

        foreach (var frame in data.frames)
        {
            var r = frame.frame;
            Rect rect = new Rect(r.x, texture.height - r.y - r.h, r.w, r.h);

            metas.Add(new SpriteMetaData
            {
                name = frame.filename,
                rect = rect,
                alignment = (int)SpriteAlignment.Center,
                pivot = new Vector2(0.5f, 0.5f)
            });
        }

        string texPath = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.SaveAndReimport();

        AssetDatabase.Refresh();
        var allSprites = AssetDatabase.LoadAllAssetsAtPath(texPath).OfType<Sprite>().ToList();

        foreach (var tag in data.meta.frameTags)
        {
            AnimationClip clip = new AnimationClip();
            clip.frameRate = 60f;

            EditorCurveBinding binding = new EditorCurveBinding
            {
                type = typeof(SpriteRenderer),
                path = "",
                propertyName = "m_Sprite"
            };

            ObjectReferenceKeyframe[] keys = new ObjectReferenceKeyframe[tag.to - tag.from + 1];
            float elapsed = 0f;

            for (int i = tag.from; i <= tag.to; i++)
            {
                float time = elapsed / 1000f;
                elapsed += data.frames[i].duration;

                keys[i - tag.from] = new ObjectReferenceKeyframe
                {
                    time = time,
                    value = allSprites[i]
                };
            }

            AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);

            Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(clip, $"{path}/{tag.name}.anim");
        }

        Debug.Log("애니메이션 생성 완료");
        Debug.Log($"[Sprite Load] Loaded Sprites: {allSprites.Count}");

    }

    [System.Serializable]
    public class AsepriteFrameRect
    {
        public int x;
        public int y;
        public int w;
        public int h;
    }

    [System.Serializable]
    public class AsepriteFrame
    {
        public string filename;
        public AsepriteFrameRect frame;
        public int duration;
    }

    [System.Serializable]
    public class AsepriteTag
    {
        public string name;
        public int from;
        public int to;
    }

    [System.Serializable]
    public class AsepriteMeta
    {
        public List<AsepriteTag> frameTags;
    }

    [System.Serializable]
    public class AsepriteRoot
    {
        public List<AsepriteFrame> frames;
        public AsepriteMeta meta;
    }
}
