using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace CustomAttributes {

    [CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
    public class HorizontalLine : DecoratorDrawer {
        public override float GetHeight() {
            return GUI.skin.horizontalSlider.fixedHeight;
        }

        public override void OnGUI(Rect position) {
            Rect rect = EditorGUI.IndentedRect(position);
            rect.y += EditorGUIUtility.singleLineHeight / 3.0f;
            rect.height = GUI.skin.horizontalSlider.fixedHeight;

            EditorGUI.DrawRect(rect, new Color32(128, 128, 128, 255));
        }
    }
}
#endif