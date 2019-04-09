using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.VFX;
using UnityEditor;
using GameplayIngredients;
using UnityEngine.Playables;

namespace GameplayIngredients.Editor
{

    [InitializeOnLoad]
    public static class HierarchyHints
    {
        const string kMenuPath = "Edit/Advanced Hierarchy View %.";
        public const int kMenuPriority = 230;

        [MenuItem(kMenuPath, priority = kMenuPriority, validate = false)]
        static void Toggle()
        {
            if (Active)
                Active = false;
            else
                Active = true;
        }

        [MenuItem(kMenuPath, priority = kMenuPriority, validate = true)]
        static bool ToggleCheck()
        {
            Menu.SetChecked(kMenuPath, Active);
            return SceneView.sceneViews.Count > 0;
        }


        static readonly string kPreferenceName = "GameplayIngredients.HierarchyHints";

        public static bool Active
        {
            get
            {
                return EditorPrefs.GetBool(kPreferenceName, false);
            }

            set
            {
                EditorPrefs.SetBool(kPreferenceName, value);
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }

        static HierarchyHints()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyOnGUI;

            foreach (var kvp in s_Definitions)
            {
                Contents.AddIcon(kvp.Key, kvp.Value);
            }
        }

        static Dictionary<Type, string> s_Definitions = new Dictionary<Type, string>()
        {
            { typeof(MonoBehaviour), "cs Script Icon"},
            { typeof(Camera), "Camera Icon"},
            { typeof(MeshRenderer), "MeshRenderer Icon"},
            { typeof(SkinnedMeshRenderer), "SkinnedMeshRenderer Icon"},
            { typeof(BoxCollider), "BoxCollider Icon"},
            { typeof(SphereCollider), "SphereCollider Icon"},
            { typeof(CapsuleCollider), "CapsuleCollider Icon"},
            { typeof(MeshCollider), "MeshCollider Icon"},
            { typeof(AudioSource), "AudioSource Icon"},
            { typeof(Animation), "Animation Icon"},
            { typeof(Animator), "Animator Icon"},
            { typeof(PlayableDirector), "PlayableDirector Icon"},
            { typeof(Light), "Light Icon"},
            { typeof(LightProbeGroup), "LightProbeGroup Icon"},
            { typeof(LightProbeProxyVolume), "LightProbeProxyVolume Icon"},
            { typeof(ReflectionProbe), "ReflectionProbe Icon"},
            { typeof(VisualEffect), "VisualEffect Icon"},
            { typeof(ParticleSystem), "ParticleSystem Icon"},
            { typeof(Canvas), "Canvas Icon"},
            { typeof(Image), "Image Icon"},
            { typeof(Text), "Text Icon"},
            { typeof(Button), "Button Icon"},
        };

        static void HierarchyOnGUI(int instanceID, Rect selectionRect)
        {
            if (!Active) return;

            var fullRect = selectionRect;
            fullRect.xMin = 24;
            fullRect.xMax = EditorGUIUtility.currentViewWidth;
            GameObject o = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (o == null) return;
            
            var c = GUI.color;
            if (o.isStatic)
            {
                GUI.Label(fullRect, " S");
                EditorGUI.DrawRect(fullRect, Colors.dimGray);
            }

            foreach(var type in s_Definitions.Keys)
            {
                if (o.GetComponents(type).Length > 0) selectionRect = DrawIcon(selectionRect, Contents.GetContent(type), Color.white);
            }

            GUI.color = c;
        }

        
        static Rect DrawIcon(Rect rect, GUIContent content, Color color, int size = 16)
        {
            GUI.color = color;
            GUI.Label(rect, content, Styles.icon);
            rect.width = rect.width - size;
            return rect;
        }

        static class Contents
        {
            static Dictionary<Type, GUIContent> s_Icons = new Dictionary<Type, GUIContent>();

            public static void AddIcon(Type type, string IconName)
            {
                s_Icons.Add(type, EditorGUIUtility.IconContent(IconName));
            }

            public static GUIContent GetContent(Type t)
            {
                return s_Icons[t];
            }
        }

        static class Colors
        {
            public static Color orange = new Color(1.0f, 0.7f, 0.1f);
            public static Color red = new Color(1.0f, 0.4f, 0.3f);
            public static Color yellow = new Color(0.8f, 1.0f, 0.1f);
            public static Color green = new Color(0.2f, 1.0f, 0.1f);
            public static Color blue = new Color(0.5f, 0.8f, 1.0f);
            public static Color violet = new Color(0.8f, 0.5f, 1.0f);
            public static Color purple = new Color(1.0f, 0.5f, 0.8f);
            public static Color dimGray = new Color(0.4f, 0.4f, 0.4f, 0.2f);
        }

        static class Styles
        {
            public static GUIStyle rightLabel;
            public static GUIStyle icon;

            static Styles()
            {
                rightLabel = new GUIStyle(EditorStyles.label);
                rightLabel.alignment = TextAnchor.MiddleRight;
                rightLabel.normal.textColor = Color.white;
                rightLabel.onNormal.textColor = Color.white;

                rightLabel.active.textColor = Color.white;
                rightLabel.onActive.textColor = Color.white;

                icon = new GUIStyle(rightLabel);
                icon.padding = new RectOffset();
                icon.margin = new RectOffset();
            }
        }

    }
}
