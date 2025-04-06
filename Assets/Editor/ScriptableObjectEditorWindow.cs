using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

public class ScriptableObjectEditorWindow : EditorWindow
{
    private string[] scriptableObjectNames;
    private int selectedIndex = 0;
    private List<ScriptableObject> selectedObjects = new List<ScriptableObject>(); // 複数のScriptableObjectを格納するリスト
    private List<Dictionary<string, object>> memberValuesList = new List<Dictionary<string, object>>(); // 複数のScriptableObjectのメンバー値を格納するリスト
    private Vector2 scrollPosition;
    const int WIDTH = 50;

    [MenuItem("Tools/Scriptable Object Editor")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectEditorWindow>("Scriptable Object Editor");
    }

    private void OnEnable()
    {
        // ScriptableObjectのクラス名一覧を取得（Assets/Scripts配下に限定）
        var scriptableObjectTypes = GetScriptableObjectTypesInScriptsFolder();
        scriptableObjectNames = scriptableObjectTypes.Select(t => t.Name).ToArray();
    }

    private void OnGUI()
    {
        // ScriptableObjectのクラス名一覧を表示するプルダウン
        selectedIndex = EditorGUILayout.Popup("Scriptable Object Type", selectedIndex, scriptableObjectNames);

        EditorGUILayout.BeginHorizontal();

        // OKボタン
        if (GUILayout.Button("OK"))
        {
            LoadScriptableObjectData();
        }

        if (GUILayout.Button("Create"))
        {
            CreateScriptableObject();
        }

        EditorGUILayout.EndHorizontal();

        // 選択されたScriptableObjectのメンバーを表示
        if (selectedObjects.Count > 0)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DisplayScriptableObjectMembers();
            EditorGUILayout.EndScrollView();
        }
    }

    private void LoadScriptableObjectData()
    {
        // 選択されたScriptableObjectの型を取得
        var scriptableObjectTypes = GetScriptableObjectTypesInScriptsFolder();
        var selectedType = scriptableObjectTypes[selectedIndex];

        // ScriptableObjectのフォルダを検索
        string[] guids = AssetDatabase.FindAssets("t:" + selectedType.Name);
        selectedObjects.Clear();
        memberValuesList.Clear();
        if (guids.Length > 0)
        {
            // すべてのScriptableObjectを読み込む
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath(path, selectedType) as ScriptableObject;
                selectedObjects.Add(obj);
                LoadMemberValues(obj);
            }
        }
        else
        {
            Debug.LogWarning("No ScriptableObject found of type: " + selectedType.Name);
        }
    }

    private void CreateScriptableObject()
    {
        var scriptableObjectTypes = GetScriptableObjectTypesInScriptsFolder();
        var selectedType = scriptableObjectTypes[selectedIndex];

        if (selectedType == null)
        {
            Debug.LogError("No Scriptable Object Type selected.");
            return;
        }

        ScriptableObject instance = CreateInstance(selectedType);

        string path = "Assets/New" + selectedType.Name + ".asset";
        path = AssetDatabase.GenerateUniqueAssetPath(path);

        AssetDatabase.CreateAsset(instance, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        LoadScriptableObjectData();
    }

    private void LoadMemberValues(ScriptableObject obj)
    {
        Dictionary<string, object> memberValues = new Dictionary<string, object>();
        if (obj == null) return;

        var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            memberValues[field.Name] = field.GetValue(obj);
        }
        memberValuesList.Add(memberValues);
    }

    private void DisplayScriptableObjectMembers()
    {
        if (selectedObjects.Count == 0) return;

        var fields = selectedObjects[0].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        EditorGUILayout.BeginVertical();
        // フィールド名のラベルを作成
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Object Name", GUILayout.Width(150));
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(bool))
            {
                EditorGUILayout.LabelField(field.Name, GUILayout.Width(WIDTH));
            }
            else if (field.FieldType == typeof(int))
            {
                EditorGUILayout.LabelField(field.Name, GUILayout.Width(WIDTH));
            }
            else if (field.FieldType == typeof(ulong))
            {
                EditorGUILayout.LabelField(field.Name, GUILayout.Width(WIDTH));
            }
            else if (field.FieldType == typeof(float))
            {
                EditorGUILayout.LabelField(field.Name, GUILayout.Width(WIDTH));
            }
            else if (field.FieldType == typeof(string))
            {
                EditorGUILayout.LabelField(field.Name, GUILayout.Width(150));
            }
            else if (field.FieldType == typeof(Sprite))
            {
                EditorGUILayout.LabelField(field.Name, GUILayout.Width(150));
            }
            // Enum型の場合
            else if (field.FieldType.IsEnum)
            {
                EditorGUILayout.LabelField(field.Name, GUILayout.Width(WIDTH));
            }
            else
            {
                EditorGUILayout.LabelField("Unsupported Type", GUILayout.Width(WIDTH));
            }
        }
        EditorGUILayout.EndHorizontal();

        // データを作成
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(selectedObjects[i].name, GUILayout.Width(150));
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(bool))
                {
                    bool value = (bool)memberValuesList[i][field.Name];
                    bool newValue = EditorGUILayout.Toggle(value, GUILayout.Width(WIDTH));
                    if (newValue != value)
                    {
                        field.SetValue(selectedObjects[i], newValue);
                        memberValuesList[i][field.Name] = newValue;
                        EditorUtility.SetDirty(selectedObjects[i]);
                    }
                }
                else if (field.FieldType == typeof(int))
                {
                    int value = (int)memberValuesList[i][field.Name];
                    int newValue = EditorGUILayout.IntField(value, GUILayout.Width(WIDTH));
                    if (newValue != value)
                    {
                        field.SetValue(selectedObjects[i], newValue);
                        memberValuesList[i][field.Name] = newValue;
                        EditorUtility.SetDirty(selectedObjects[i]);
                    }
                }
                else if (field.FieldType == typeof(ulong))
                {
                    ulong value = (ulong)memberValuesList[i][field.Name];
                    string stringValue = value.ToString();
                    string newStringValue = EditorGUILayout.TextField(stringValue, GUILayout.Width(WIDTH));
                    if (ulong.TryParse(newStringValue, out ulong newValue) && newValue != value)
                    {
                        field.SetValue(selectedObjects[i], newValue);
                        memberValuesList[i][field.Name] = newValue;
                        EditorUtility.SetDirty(selectedObjects[i]);
                    }
                }
                else if (field.FieldType == typeof(float))
                {
                    float value = (float)memberValuesList[i][field.Name];
                    float newValue = EditorGUILayout.FloatField(value, GUILayout.Width(WIDTH));
                    if (newValue != value)
                    {
                        field.SetValue(selectedObjects[i], newValue);
                        memberValuesList[i][field.Name] = newValue;
                        EditorUtility.SetDirty(selectedObjects[i]);
                    }
                }
                else if (field.FieldType == typeof(string))
                {
                    string value = (string)memberValuesList[i][field.Name];
                    string newValue = EditorGUILayout.TextField(value, GUILayout.Width(150));
                    if (newValue != value)
                    {
                        field.SetValue(selectedObjects[i], newValue);
                        memberValuesList[i][field.Name] = newValue;
                        EditorUtility.SetDirty(selectedObjects[i]);
                    }
                }
                else if (field.FieldType == typeof(Sprite))
                {
                    Sprite value = (Sprite)memberValuesList[i][field.Name];
                    Sprite newValue = (Sprite)EditorGUILayout.ObjectField(value, typeof(Sprite), false, GUILayout.Width(150));
                    if (newValue != value)
                    {
                        field.SetValue(selectedObjects[i], newValue);
                        memberValuesList[i][field.Name] = newValue;
                        EditorUtility.SetDirty(selectedObjects[i]);
                    }
                }
                // Enum型の場合
                else if (field.FieldType.IsEnum)
                {
                    System.Enum value = (System.Enum)memberValuesList[i][field.Name];
                    System.Enum newValue = EditorGUILayout.EnumPopup(value, GUILayout.Width(WIDTH));
                    if (!newValue.Equals(value))
                    {
                        field.SetValue(selectedObjects[i], newValue);
                        memberValuesList[i][field.Name] = newValue;
                        EditorUtility.SetDirty(selectedObjects[i]);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("Unsupported Type", GUILayout.Width(WIDTH));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private List<System.Type> GetScriptableObjectTypesInScriptsFolder()
    {
        List<System.Type> types = new List<System.Type>();
        string scriptsFolderPath = "Assets/Scripts";

        // Assets/Scripts フォルダ内のすべてのC#ファイルを検索
        string[] scriptFiles = Directory.GetFiles(scriptsFolderPath, "*.cs", SearchOption.AllDirectories);

        foreach (string scriptFile in scriptFiles)
        {
            // ファイル名からクラス名を取得
            string className = Path.GetFileNameWithoutExtension(scriptFile);

            // アセンブリから型を取得
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(className);
                if (type != null && type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsAbstract)
                {
                    types.Add(type);
                }
            }
        }
        return types;
    }
}
