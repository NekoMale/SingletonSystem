using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace NamelessGames.SingletonSystem
{
    [CustomEditor(typeof(SingletonInstantiator))]
    public class SingletonInstantiatorEditor : Editor
    {
        SerializedProperty _serializedSingletons = null;
        int _currentPage, _maxPage, _startingIndex;
        float _maxElements = 20f;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_serializedSingletons == null)
            {
                _serializedSingletons = serializedObject.FindProperty("_singletons");
                _startingIndex = 0;
                _currentPage = 1;
                _maxPage = Mathf.CeilToInt(_serializedSingletons.arraySize / _maxElements);
            }

            Rect headerPosition = EditorGUILayout.GetControlRect();

            EditorGUI.LabelField(headerPosition, "", new GUIStyle("RL Header"));

            EditorGUI.LabelField(headerPosition, "Persistent Singletons", EditorStyles.boldLabel);

            Rect positionRect = new Rect(headerPosition.x + headerPosition.width - _maxElements, headerPosition.y + 1f, 20f, headerPosition.height);

            GUIContent PlusLabel = EditorGUIUtility.IconContent("Toolbar Plus", "Add to list");
            GUIStyle PlusButton = "RL FooterButton";
            GUIStyle MinusButton = new GUIStyle(PlusButton);
            if (GUI.Button(positionRect, PlusLabel, PlusButton))
            {
                _serializedSingletons.InsertArrayElementAtIndex(_serializedSingletons.arraySize);
                _maxPage = Mathf.CeilToInt(_serializedSingletons.arraySize / _maxElements);
            }
            GUI.enabled = true;

            positionRect.x -= (positionRect.width + 3f);
            if (GUI.Button(positionRect, ">", PlusButton))
            {
                _currentPage = Mathf.Clamp(_currentPage + 1, 1, _maxPage);
            }

            positionRect.x -= (positionRect.width + 3f);
            EditorGUI.LabelField(positionRect, _maxPage.ToString());

            positionRect.x -= (positionRect.width + 3f);
            EditorGUI.LabelField(positionRect, "/");

            positionRect.x -= (positionRect.width + 3f);
            _currentPage = EditorGUI.IntField(positionRect, _currentPage);

            positionRect.x -= (positionRect.width + 3f);
            if (GUI.Button(positionRect, "<", PlusButton))
            {
                _currentPage = Mathf.Clamp(_currentPage - 1, 1, _maxPage);
            }
            _startingIndex = Mathf.Clamp((_currentPage - 1) * (int)_maxElements, 0, _maxPage * (int)_maxElements);

            int maxIndex = Mathf.Min(_serializedSingletons.arraySize, _startingIndex + (int)_maxElements);
            for (int index = _startingIndex; index < maxIndex; index++)
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
                    _serializedSingletons.DeleteArrayElementAtIndex(index);
                    _maxPage = Mathf.CeilToInt(_serializedSingletons.arraySize / _maxElements);
                    _currentPage = Mathf.Clamp(_currentPage, 1, _maxPage);
                    maxIndex = Mathf.Min(_serializedSingletons.arraySize, _startingIndex + (int)_maxElements);
                    continue;
                }

                positionRect.x += 4f;
                positionRect.y += 1f;
                positionRect.width -= 8f;
                positionRect.height -= 3f;
                EditorGUI.PropertyField(positionRect, _serializedSingletons.GetArrayElementAtIndex(index), GUIContent.none);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}