using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace NamelessGames.SingletonSystem
{
    [CustomEditor(typeof(AppSingletonInstantiator))]
    public class AppSingletonInstantiatorEditor : Editor
    {
        GUIContent _headerLabel = new GUIContent("Persistent Singleton Behaviours", "Drag here Singleton GameObjects who has to exists for all application lifecycle");
        SerializedProperty _serializedSingletonBehaviours = null;
        int _currentPageSB, _maxPageSB, _startingIndexSB;
        float _maxElements = 20f;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_serializedSingletonBehaviours == null)
            {
                _serializedSingletonBehaviours = serializedObject.FindProperty("_singletonBehaviours");
                _startingIndexSB = 0;
                _currentPageSB = 1;
                _maxPageSB = Mathf.CeilToInt(_serializedSingletonBehaviours.arraySize / _maxElements);
            }

            Rect headerPosition = EditorGUILayout.GetControlRect();

            EditorGUI.LabelField(headerPosition, "", new GUIStyle("RL Header"));

            headerPosition.x += 6f;
            headerPosition.width -= 6f;
            EditorGUI.LabelField(headerPosition, _headerLabel, EditorStyles.boldLabel);

            Rect positionRect = new Rect(headerPosition.x + headerPosition.width - _maxElements, headerPosition.y + 1f, 20f, headerPosition.height);

            GUIContent PlusLabel = EditorGUIUtility.IconContent("Toolbar Plus", "Add to list");
            GUIStyle PlusButton = "RL FooterButton";
            GUIStyle MinusButton = new GUIStyle(PlusButton);
            if (GUI.Button(positionRect, PlusLabel, PlusButton))
            {
                _serializedSingletonBehaviours.InsertArrayElementAtIndex(_serializedSingletonBehaviours.arraySize);
                _maxPageSB = Mathf.CeilToInt(_serializedSingletonBehaviours.arraySize / _maxElements);
            }
            GUI.enabled = true;

            positionRect.x -= (positionRect.width + 3f);
            if (GUI.Button(positionRect, ">", PlusButton))
            {
                _currentPageSB = Mathf.Clamp(_currentPageSB + 1, 1, _maxPageSB);
            }

            positionRect.x -= (positionRect.width + 3f);
            EditorGUI.LabelField(positionRect, _maxPageSB.ToString());

            positionRect.x -= (positionRect.width + 3f);
            EditorGUI.LabelField(positionRect, "/");

            positionRect.x -= (positionRect.width + 3f);
            _currentPageSB = EditorGUI.IntField(positionRect, _currentPageSB);

            positionRect.x -= (positionRect.width + 3f);
            if (GUI.Button(positionRect, "<", PlusButton))
            {
                _currentPageSB = Mathf.Clamp(_currentPageSB - 1, 1, _maxPageSB);
            }
            _startingIndexSB = Mathf.Clamp((_currentPageSB - 1) * (int)_maxElements, 0, _maxPageSB * (int)_maxElements);

            int maxIndex = Mathf.Min(_serializedSingletonBehaviours.arraySize, _startingIndexSB + (int)_maxElements);
            for (int index = _startingIndexSB; index < maxIndex; index++)
            {
                positionRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight + 4f);
                positionRect.x -= 1f;
                positionRect.height += 2f;
                EditorGUI.DrawRect(positionRect, new Color(0.2f, 0.2f, 0.2f));
                positionRect.x += 2f;
                positionRect.y += 1f;
                positionRect.width -= 24f;
                positionRect.height -= 2f;
                EditorGUI.DrawRect(positionRect, new Color(0.29f, 0.29f, 0.29f));

                Rect buttonRect = new Rect(positionRect);
                //buttonRect.y += 5f;
                buttonRect.x += positionRect.width;
                buttonRect.width = 20f;
                MinusButton.fixedHeight = positionRect.height;
                if (GUI.Button(buttonRect, ReorderableList.defaultBehaviours.iconToolbarMinus, MinusButton))
                {
                    _serializedSingletonBehaviours.DeleteArrayElementAtIndex(index);
                    _maxPageSB = Mathf.CeilToInt(_serializedSingletonBehaviours.arraySize / _maxElements);
                    _currentPageSB = Mathf.Clamp(_currentPageSB, 1, _maxPageSB);
                    maxIndex = Mathf.Min(_serializedSingletonBehaviours.arraySize, _startingIndexSB + (int)_maxElements);
                    continue;
                }

                positionRect.x += 4f;
                positionRect.y += 1f;
                positionRect.width -= 8f;
                positionRect.height -= 3f;
                EditorGUI.PropertyField(positionRect, _serializedSingletonBehaviours.GetArrayElementAtIndex(index), GUIContent.none);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}