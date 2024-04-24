using HierarchyIcons;
using UnityEngine;
using UnityEditor;

namespace CGHierarchyIconsEditor
{
    [InitializeOnLoad]
    public class ShowIconOnHierarchyView
    {
        const float MAX_ICON_SIZE = 16;
        //static Material tintMat;

        static ShowIconOnHierarchyView()
        {
            ////Shader shade = Shader.Find("Shader Graphs/IconTint");
            ////if(shade == null)
            ////{
            ////    Debug.LogError("Could not find shader named: " + "Shader Graphs/IconTint");
            ////}
            ////else
            ////{
            ////    Debug.Log("Shader found: " + shade.name);
            ////}

            //tintMat = new Material(shade);
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
        }

        static void HierarchyWindowItemCallback(int instanceID, Rect selectionRect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject == null)
                return;

            HierarchyIcon[] hierarchyIcons = gameObject.GetComponents<HierarchyIcon>();
            foreach (HierarchyIcon hierarchyIcon in hierarchyIcons)
            {
                Texture2D icon = hierarchyIcon.icon ? hierarchyIcon.icon : TextureHelper.NO_ICON;
                float width = Mathf.Min(icon.width, MAX_ICON_SIZE);
                float height = Mathf.Min(icon.height, MAX_ICON_SIZE);

                // get the starting x position based on the direction
                float x = selectionRect.x;
                if (hierarchyIcon.direction == HierarchyIcon.Direction.LeftToRight)
                    x += hierarchyIcon.position * MAX_ICON_SIZE;
                else
                    x += (selectionRect.width - width) - hierarchyIcon.position * MAX_ICON_SIZE;

                //draw the icon
                Rect rect = new Rect(x, selectionRect.y, width, height);
                GUI.DrawTexture(rect, icon);




                //// Convert screen coordinates to GUI space coordinates
                //Vector2 guiPosition = EditorGUIUtility.GUIToScreenPoint(new Vector2(x, selectionRect.y));
                //Rect rect = new Rect(guiPosition.x, guiPosition.y, width, height);

                //// Set the texture and color properties of the material
                //tintMat.SetTexture("_MainTexture", icon);
                //tintMat.SetColor("_TintColor", Color.blue);

                //// Draw the Quad with the material
                //GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture, ScaleMode.ScaleToFit, true, 0, Color.blue, 0, 0);
                //Graphics.DrawTexture(rect, EditorGUIUtility.whiteTexture, tintMat);




                // set link cursor
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

                // and draw a button for change the icon and display the tooltip
                if (GUI.Button(rect, new GUIContent(string.Empty, hierarchyIcon.tooltip), EditorStyles.label))
                    PopupWindow.Show(rect, new PickIconWindow(hierarchyIcon));


                //Texture2D icon = hierarchyIcon.icon ? hierarchyIcon.icon : TextureHelper.NO_ICON;

                //// Calculate the position based on the direction and icon size
                //float x = selectionRect.x;
                //if (hierarchyIcon.direction == HierarchyIcon.Direction.RightToLeft)
                //    x += selectionRect.width - MAX_ICON_SIZE - hierarchyIcon.position * MAX_ICON_SIZE;
                //else
                //    x += hierarchyIcon.position * MAX_ICON_SIZE;

                //// Convert screen coordinates to GUI space coordinates
                //Vector2 guiPosition = EditorGUIUtility.GUIToScreenPoint(new Vector2(x, selectionRect.y));
                //Rect rect = new Rect(guiPosition.x, guiPosition.y, MAX_ICON_SIZE, MAX_ICON_SIZE);

                //// Draw the icon using the material
                //Graphics.DrawTexture(rect, icon, tintMat);
            }
        }
    }
}