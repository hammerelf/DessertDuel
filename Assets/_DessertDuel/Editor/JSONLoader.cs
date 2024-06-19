//Created by: Ryan King

//Example json:
//    {
//      "id": null,
//      "itemName": null,
//      "cost": 0,
//      "power": 0,
//      "description": null,
//      "state": 0,
//      "itemImagePath": null
//    }

using HammerElf.Games.DessertDuel;
using HammerElf.Tools.Utilities;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HammerElf.Editor.DessertDuel
{
    //This will take a json file representing a placeable and generate a prefab of that item.
    public class JSONLoader : OdinEditorWindow
    {
        [SerializeField]
        private GameObject defensePrefab, offensePrefab;

        private string jsonOutputFolderParent = "Assets/_DessertDuel/Resources";
        private string jsonOutputFolderPath = "Assets/_DessertDuel/Resources/JSON/";
        private string prefabOutputFolderParent = "Assets/_DessertDuel/Prefabs/Items";
        private string prefabOutputFolderPath = "Assets/_DessertDuel/Prefabs/Items/JSON/";

        [MenuItem("HammerElf/Load Placeable")]
        private static void OpenWindow()
        {
            EditorWindow.GetWindow<JSONLoader>().Show();
        }

        //public Placeable LoadPlaceable(string filePath)
        //{
        //    PlaceableJSON deserializedObject = JsonConvert.DeserializeObject<PlaceableJSON>(filePath);

        //    PrefabUtility.CreatePrefab(prefabPath, defensePrefab);

        //    return deserializedObject;
        //}

        //public Placeable ConvertJSONToPlaceable(string json)
        //{
        //    if (json == null || json.Equals(""))
        //    {
        //        ConsoleLog.LogError("Json data is empty or null.");
        //        return null;
        //    }
        //    return new Placeable();
        //}

        //Takes in a json file and creates either a DefensePlaceable or OffensePlaceable depending
        //on the isDefense bool in the file. Then sets the values of that prefab to the values from
        //the file.
        [Button]
        public void CreatePlaceableFromJSON(TextAsset jsonFile)
        {
            if (!AssetDatabase.IsValidFolder(prefabOutputFolderPath))
            {
                AssetDatabase.CreateFolder(prefabOutputFolderParent, "JSON");
                ConsoleLog.LogWarning("Prefab folder didn't exist. Try again.");
                return;
            }

            //PlaceableJSON deserializedObject = JsonConvert.DeserializeObject<PlaceableJSON>(jsonFile.text);
            dynamic deserializedObject = JsonConvert.DeserializeObject(jsonFile.text);
            //ConsoleLog.Log(deserializedObject.itemName);

            bool isDef = deserializedObject.isDefense.Value;
            Type placeableType = isDef ? typeof(DefensePlaceableJSON) : typeof(OffensePlaceableJSON);
            //PlaceableJSON placeableObject = (PlaceableJSON)Activator.CreateInstance(placeableType, deserializedObject);

            string savePath = prefabOutputFolderPath + deserializedObject.itemName + ".prefab";
            savePath = AssetDatabase.GenerateUniqueAssetPath(savePath);

            if(isDef)
            {
                PlaceableJSON pJson = new DefensePlaceableJSON(deserializedObject.id.Value,
                                                               deserializedObject.itemName.Value,
                                                               (int)deserializedObject.cost.Value,
                                                               (int)deserializedObject.power.Value,
                                                               deserializedObject.description.Value,
                                                               (PlaceableState)((int)deserializedObject.state.Value),
                                                               deserializedObject.itemImagePath.Value,
                                                               (int)deserializedObject.health.Value,
                                                               (int)deserializedObject.damage.Value,
                                                               (int)deserializedObject.attackRate.Value);

                GameObject go = PrefabUtility.SaveAsPrefabAsset(defensePrefab, savePath);
                go.GetComponent<DefensePlaceable>().SetFromJSON((DefensePlaceableJSON)pJson);
                EditorUtility.SetDirty(go);
                PrefabUtility.SavePrefabAsset(go);
            }
            else
            {
                GameObject go = PrefabUtility.SaveAsPrefabAsset(offensePrefab, savePath);
                go.GetComponent<OffensePlaceable>().SetFromJSON(deserializedObject as OffensePlaceableJSON);
                EditorUtility.SetDirty(go);
                PrefabUtility.SavePrefabAsset(go);
            }
        }

        [Button(ButtonSizes.Large)]
        public void SerializePlaceable(Placeable deserializedObject)
        {
            if(!AssetDatabase.IsValidFolder(jsonOutputFolderPath))
            {
                AssetDatabase.CreateFolder(jsonOutputFolderParent, "JSON");
                ConsoleLog.LogWarning("Json folder didn't exist. Try again.");
                return;
            }

            string jsonData;
            string path = jsonOutputFolderPath + "Unnamed" + ".json";
            string type = "";

            if (deserializedObject.GetType() == typeof(DefensePlaceable))
            {
                jsonData = JsonConvert.SerializeObject((deserializedObject as DefensePlaceable).ToJSON());
                path = jsonOutputFolderPath + "DP_" + deserializedObject.id + ".json";
                type = "defense ";
            }
            else if (deserializedObject.GetType() == typeof(OffensePlaceable))
            {
                jsonData = JsonConvert.SerializeObject((deserializedObject as OffensePlaceable).ToJSON());
                path = jsonOutputFolderPath + "OP_" + deserializedObject.id + ".json";
                type = "offense ";
            }
            else
            {
                ConsoleLog.LogError("Placeable is not Offense or Defense. Type is: " + deserializedObject.GetType());
                return;
            }

            path = AssetDatabase.GenerateUniqueAssetPath(path);
            File.WriteAllText(path, jsonData);
            AssetDatabase.Refresh();
            ConsoleLog.Log("Json " + type + "data for id \"" + deserializedObject.id + "\" created at path: " + path);
        }

        public static string GetIdFromFileName(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('_') + 1, fileName.LastIndexOf('.') - fileName.LastIndexOf('_'));
        }
    }
}
