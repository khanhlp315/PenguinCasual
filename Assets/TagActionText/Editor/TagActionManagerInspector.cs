using UGUITagActionText;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;


[CustomPropertyDrawer(typeof(TagActionManager.TagData))]
public class TagDataDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{

		using (new EditorGUI.PropertyScope(position, label, property))
		{

			EditorGUIUtility.labelWidth = 75;
			float thisWidth = position.width - 10f;
			position.height = EditorGUIUtility.singleLineHeight;
			float halfWidth = thisWidth * 0.5f;
			SerializedProperty tempProp = null;
			bool regexCheck = false;
			bool colorCheck = false;

			var tagRect = new Rect(position)
			{
				width = thisWidth,
				x = position.x
			};

			int tagTypeIndex = property.FindPropertyRelative("m_tagType").enumValueIndex;
			tempProp = property.FindPropertyRelative("m_tagString");
			SerializedProperty dispTagStrProp = property.FindPropertyRelative("disp_tagString");
			string currentStr = dispTagStrProp.stringValue = EditorGUI.TextField(tagRect, dispTagStrProp.stringValue);
			if (tagTypeIndex == 0 || tagTypeIndex == 1)
			{
				if (Regex.IsMatch(currentStr, @"^[A-Za-z0-9\-_]+$"))
				{
					tempProp.stringValue = currentStr;
				}
				else
				{
					if (string.IsNullOrEmpty(currentStr) != true)
						regexCheck = true;
					tempProp.stringValue = string.Empty;
				}
			}
			else if (tagTypeIndex == 2)
			{
				if (Regex.IsMatch(currentStr, @"[\\]+$") != true)
				{
					tempProp.stringValue = currentStr;
				}
				else
				{
					if (string.IsNullOrEmpty(currentStr) != true)
						regexCheck = true;
					tempProp.stringValue = string.Empty;
				}
			}
			tempProp = null;

			float helpBoxHeight = 0f;
			if (regexCheck)
			{
				if (tagTypeIndex == 0 || tagTypeIndex == 1)
				{
					if (thisWidth > 320)
						helpBoxHeight = 25f;
					else
						helpBoxHeight = 35f;
				}
				else if (tagTypeIndex == 2)
				{
					if (thisWidth > 230)
						helpBoxHeight = 25f;
					else
						helpBoxHeight = 35f;
				}
			}

			var helpBoxRect = new Rect(tagRect)
			{
				width = thisWidth,
				height = helpBoxHeight,
				x = position.x,
				y = tagRect.y + EditorGUIUtility.singleLineHeight + 2
			};

			var tagTypeLabelRect = new Rect(tagRect)
			{
				width = thisWidth,
				y = tagRect.y + EditorGUIUtility.singleLineHeight + helpBoxHeight
			};

			var tagTypeRect = new Rect(tagTypeLabelRect)
			{
				width = thisWidth,
				y = tagTypeLabelRect.y + EditorGUIUtility.singleLineHeight
			};

			var changeColorToggleRect = new Rect(tagTypeRect)
			{
				width = halfWidth,
				y = tagTypeRect.y + EditorGUIUtility.singleLineHeight + 8
			};

			var changeColorRect = new Rect(changeColorToggleRect)
			{
				width = halfWidth,
				x = position.x + halfWidth,
				y = changeColorToggleRect.y
			};

			var argEventRect = new Rect(changeColorRect)
			{
				width = thisWidth,
				x = position.x,
				y = changeColorToggleRect.y + EditorGUIUtility.singleLineHeight + 2
			};


			if (regexCheck)
			{
				string message = string.Empty;
				if (tagTypeIndex == 0 || tagTypeIndex == 1)
					message = "in this TagType, Tag string is half-width alphanumeric only";
				else if (tagTypeIndex == 2)
					message = "contains character that can not be used";
				EditorGUI.HelpBox(helpBoxRect, message, MessageType.Error);
			}

			tempProp = property.FindPropertyRelative("m_tagType");
			LinkActionTagType tagType = (LinkActionTagType)System.Enum.ToObject(typeof(LinkActionTagType), tagTypeIndex);
			int newTagTypeIndex = (int)(LinkActionTagType)EditorGUI.EnumPopup(tagTypeRect, tagType);
			tempProp.enumValueIndex = newTagTypeIndex;
			tempProp = null;

			string exampleStr = string.Empty;
			if (newTagTypeIndex == 0)
				exampleStr = "<Tag>xxx</Tag>";
			else if (newTagTypeIndex == 1)
				exampleStr = "<Tag=\"xxx\"></Tag>";
			else if (newTagTypeIndex == 2)
				exampleStr = "Word Itself";
			EditorGUI.LabelField(tagTypeLabelRect, " TagType: " + exampleStr);

			tempProp = property.FindPropertyRelative("m_isChangeTextColor");
			colorCheck = tempProp.boolValue = EditorGUI.Toggle(changeColorToggleRect, "Text Color", tempProp.boolValue);
			tempProp = null;

			if (colorCheck)
			{
				tempProp = property.FindPropertyRelative("m_textColor");
				tempProp.colorValue = EditorGUI.ColorField(changeColorRect, tempProp.colorValue);
				tempProp = null;
			}

			tempProp = property.FindPropertyRelative("m_argEvent");
			EditorGUI.PropertyField(argEventRect, tempProp, new GUIContent("Action"));
			tempProp = null;

		}

	}

}





[CustomEditor(typeof(TagActionManager), true)]
[CanEditMultipleObjects]
public class TagActionManagerInspector : Editor
{

	TagActionManager script;
	SerializedProperty tagDataProp;
	ReorderableList reorderableList;

	void OnEnable()
	{
		script = target as TagActionManager;
		tagDataProp = serializedObject.FindProperty("tagData");
		reorderableList = new ReorderableList(serializedObject, tagDataProp);
	}

	void DrawElements()
	{

		reorderableList.drawHeaderCallback = (rect) =>
		{
			EditorGUI.LabelField(rect, "Tag Action"); //prop.displayName
		};


		reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			var element = tagDataProp.GetArrayElementAtIndex(index);
			rect.y += 8;
			EditorGUI.PropertyField(rect, element);
			if (index > 0)
			{
				var lineRect = new Rect(rect)
				{
					width = rect.width,
					height = 2,
					x = rect.x,
					y = rect.y - 7f
				};
				EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));
			}
		};

		reorderableList.elementHeightCallback = (int index) =>
		{
			var elementCount = script.tagData[index].argEvent.GetPersistentEventCount();
			if (elementCount > 0)
			{
				elementCount -= 1;
			}

			float height = (elementCount * (EditorGUIUtility.singleLineHeight * 2.7f))
				+ (EditorGUIUtility.singleLineHeight * 11f);
			if (index >= (script.tagData.Count - 1))
				height += EditorGUIUtility.singleLineHeight * 0.5f;
			return height;
		};

	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		DrawElements();
		GUILayout.Space(10);
		reorderableList.DoLayoutList();
		GUILayout.Space(10);
		EditorGUILayout.BeginHorizontal();
		script.isChangeTappedColor = EditorGUILayout.Toggle("Tapped Text Color", script.isChangeTappedColor);
		if (script.isChangeTappedColor)
			script.tappedColor = EditorGUILayout.ColorField(script.tappedColor);
		EditorGUILayout.EndHorizontal();
		Undo.RecordObject(script, "Inspector");
		GUILayout.Space(10);
		serializedObject.ApplyModifiedProperties();
	}

}



/*

[CustomEditor(typeof(TagActionManager), true)]
[CanEditMultipleObjects]
public class TagActionManagerInspector : Editor
{

	SerializedProperty tagDataProp;

	void OnEnable()
	{
		tagDataProp = serializedObject.FindProperty("tagData");
	}

	public override void OnInspectorGUI()
	{
		TagActionManager script = target as TagActionManager;

		serializedObject.Update();
		//DrawDefaultInspector();

		if(tagDataProp != null)
		{

			//EditorGUILayout.PropertyField(tagDataProp, true);
			int listSize = tagDataProp.arraySize;
			if(listSize > 0)
			{
				for(int i = 0; i < listSize; i++)
				{
					SerializedProperty elementProp = tagDataProp.GetArrayElementAtIndex(i);

					//EditorGUILayout.PropertyField(elementProp, true);
					//elementProp.isExpanded = true;

					EditorGUILayout.Space();
					SerializedProperty tempProp = null;
					bool colorCheck = false;
					tempProp = elementProp.FindPropertyRelative("tagString");
					if(tempProp != null)
					{
						EditorGUILayout.PropertyField(tempProp, true);
						tempProp = null;
					}
					tempProp = elementProp.FindPropertyRelative("isChangeTextColor");
					if(tempProp != null)
					{
						colorCheck = tempProp.boolValue;
						EditorGUILayout.PropertyField(tempProp, true);
						tempProp = null;
					}
					if(colorCheck)
					{
						tempProp = elementProp.FindPropertyRelative("textColor");
						if(tempProp != null)
						{
							EditorGUILayout.PropertyField(tempProp, true);
							tempProp = null;
						}
					}
					tempProp = elementProp.FindPropertyRelative("argEvent");
					if(tempProp != null)
					{
						EditorGUILayout.PropertyField(tempProp, true);
						tempProp = null;
					}
					EditorGUILayout.Space();
				}
			}
		}

		serializedObject.ApplyModifiedProperties();





		//Undo.RecordObject(script, "Inspector");

		EditorGUI.BeginChangeCheck();

		if(EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(target);
		}
	}
}
*/
