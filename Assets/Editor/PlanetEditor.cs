using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor {
    private Planet planet;
    private Editor shapeEditor;
    private Editor colourEditor;

    public override void OnInspectorGUI() {
        // 使所有的更新都在该脚本中完成，取代了OnValidate()的更新
        using (var check = new EditorGUI.ChangeCheckScope()) {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }
        // 添加一个按钮，当自动更新关闭后，手动点击按钮使其更新
        if (GUILayout.Button("生成行星"))
        {
            planet.GeneratePlanet();
        }
        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdate, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdate, ref planet.colourSettingsFoldout, ref colourEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdate, ref bool foldout, ref Editor editor) {
        if (settings != null) {

            // 在细节面板中添加标题，设置为true使不可折叠
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope()) {
                if (foldout) {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                    if (check.changed) {
                        if (onSettingsUpdate != null) {
                            onSettingsUpdate();
                        }
                    }
                }
            }
        }

    }

    private void OnEnable() {
        planet = (Planet)target;
    }

}
