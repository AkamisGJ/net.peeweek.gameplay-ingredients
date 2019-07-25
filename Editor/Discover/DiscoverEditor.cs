using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.VFX;
using UnityEngine.Timeline;

namespace GameplayIngredients.Editor
{
    [CustomEditor(typeof(Discover))]
    public class DiscoverEditor : UnityEditor.Editor
    {
        const string kEditPreferenceName = "GameplayIngredients.DiscoverEditor.Editing";

        static bool editing
        {
            get { return EditorPrefs.GetBool(kEditPreferenceName, false); }
            set { if (value != editing) EditorPrefs.SetBool(kEditPreferenceName, value); }
        }

        Discover m_Discover;

        private void OnEnable()
        {
            m_Discover = serializedObject.targetObject as Discover;
            if (m_Discover.transform.hideFlags != HideFlags.HideInInspector)
                m_Discover.transform.hideFlags = HideFlags.HideInInspector;
        }

        public override void OnInspectorGUI()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                editing = GUILayout.Toggle(editing, "Edit", EditorStyles.miniButton, GUILayout.Width(48));
            }

            if (editing)
                DrawDefaultInspector();
            else
                DrawDiscoverContentGUI(m_Discover);
        }


        public static void DrawDiscoverContentGUI(Discover discover)
        {

            GUILayout.Label(discover.Category, DiscoverWindow.Styles.subHeader);
            GUILayout.Label(discover.Name, DiscoverWindow.Styles.header);

            using (new GUILayout.VerticalScope(DiscoverWindow.Styles.indent))
            {
                if (discover.Description != null && discover.Description != string.Empty)
                {
                    GUILayout.Label(discover.Description, DiscoverWindow.Styles.body);
                }

                GUILayout.Space(8);

                foreach (var section in discover.Sections)
                {
                    SectionGUI(section);
                    GUILayout.Space(16);
                }
            }
        }

        public static void SectionGUI(DiscoverSection section)
        {
            using (new DiscoverWindow.GroupLabelScope(section.SectionName))
            {
                using (new GUILayout.VerticalScope(DiscoverWindow.Styles.slightIndent))
                {
                    GUILayout.Label(section.SectionContent, DiscoverWindow.Styles.body);

                    if (section.Actions != null && section.Actions.Length > 0)
                    {
                        GUILayout.Space(8);

                        using (new GUILayout.VerticalScope(GUI.skin.box))
                        {
                            foreach (var action in section.Actions)
                            {
                                using (new GUILayout.HorizontalScope())
                                {
                                    GUILayout.Label(action.Description);
                                    GUILayout.FlexibleSpace();
                                    using (new GUILayout.HorizontalScope(GUILayout.MinWidth(160)))
                                    {
                                        ActionButtonGUI(action.Target);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static void ActionButtonGUI(UnityEngine.Object target)
        {
            Type t = target.GetType();

            if (t == typeof(GameObject))
            {
                GameObject go = target as GameObject;
                
                if (GUILayout.Button("  Select  ", DiscoverWindow.Styles.buttonLeft))
                {
                    Selection.activeObject = go;
                }

                if(PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.NotAPrefab)
                {
                    if (GUILayout.Button("  Go to  ", DiscoverWindow.Styles.buttonRight))
                    {

                        Selection.activeObject = go;
                        SceneView.lastActiveSceneView.FrameSelected();
                    }
                }
                else
                {
                    if (GUILayout.Button("  Open  ", DiscoverWindow.Styles.buttonRight))
                    {
                        AssetDatabase.OpenAsset(go);
                    }
                }


            }
            else if (t == typeof(VisualEffectAsset))
            {
                if (GUILayout.Button("Open VFX Graph"))
                {
                    VisualEffectAsset graph = target as VisualEffectAsset;
                    AssetDatabase.OpenAsset(graph);
                }
            }
            else if (t == typeof(Animation))
            {
                if (GUILayout.Button("Open Animation"))
                {
                    Animation animation = target as Animation;
                    AssetDatabase.OpenAsset(animation);
                }
            }
            else if (t == typeof(TimelineAsset))
            {
                if (GUILayout.Button("Open Timeline"))
                {
                    TimelineAsset timeline = target as TimelineAsset;
                    AssetDatabase.OpenAsset(timeline);
                }
            }
            else if (t == typeof(Shader))
            {
                if (GUILayout.Button("Open Shader"))
                {
                    Shader shader = target as Shader;
                    AssetDatabase.OpenAsset(shader);
                }
            }
            else
            {
                if (GUILayout.Button("Select"))
                {
                    Selection.activeObject = target;
                }
            }
        }
    }
}

