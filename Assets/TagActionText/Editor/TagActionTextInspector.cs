
using UnityEditor;
using UnityEngine;
using UGUITagActionText;
using System.Text;


[CustomEditor(typeof(TagActionText), true)]
[CanEditMultipleObjects]
public class TagActionTextInspector : Editor
{
	StringBuilder sb;

	public override void OnInspectorGUI()
	{
		TagActionText script = target as TagActionText;
		serializedObject.Update();
		//original code of Text.cs component is here: https://bitbucket.org/Unity-Technologies/ui/src/b580f85dcd84d695b21d8ac9e09942ab4d3bcb50/UnityEngine.UI/UI/Core/Text.cs
		//base.OnInspectorGUI();

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_Text"), false);

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("hold_tagManager"), new GUIContent("Tag Manager"));
		if (script.TagManager != script.hold_tagManager)
			script.TagManager = script.hold_tagManager;

		EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_FontData"));

		//same role the code in TextEditor.cs: https://bitbucket.org/Unity-Technologies/ui/src/b580f85dcd84d695b21d8ac9e09942ab4d3bcb50/UnityEditor.UI/UI/TextEditor.cs
		//see property name from GraphicEditor.cs(inherited TextEditor.cs): https://bitbucket.org/Unity-Technologies/ui/src/9f418c4767c47d0c71f1727eb42a9a9024e9ecc0/UnityEditor.UI/UI/GraphicEditor.cs
		//GraphicAppearanceControlsGUI();
		//RaycastControlsGUI();
		EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_Color"));
		EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_Material"));
		EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_RaycastTarget"));

		if (script.resizeTextForBestFit != true)
		{
			if (script.horizontalOverflow == HorizontalWrapMode.Overflow
				|| script.verticalOverflow == VerticalWrapMode.Overflow)
			{
				string message = "Note: when use Overflow Mode without BestFit, if some text is out of RectTransform area, that part is NOT detected the click by Unity";
				EditorGUILayout.HelpBox(message, MessageType.Warning);
				EditorGUILayout.Space();
			}
		}

		/*
		EditorGUILayout.LabelField("Tag String");
		EditorGUI.BeginChangeCheck();
		script.tagString = EditorGUILayout.TextArea(script.tagString);
		if(EditorGUI.EndChangeCheck())
		{
			if(string.IsNullOrEmpty(script.tagString) != true)
			{
				sb.Clear(); //require higher .Net4.0 in Unity editor settings
				sb.Append(script.tagString);
				if(sb.Length > 20)
				{
					sb.Remove(20, sb.Length - 20);
					script.tagString = sb.ToString();
					EditorUtility.SetDirty(target);
					GUI.FocusControl("");
					Debug.LogWarning("Tag string is needed to 20 characters or less");

					//GUI.SetNextControlName("MyTextField");
					//GUI.FocusControl("MyTextField");
					//EditorGUI.FocusTextInControl("MyTextField");
				}
				if(Regex.IsMatch(script.tagString, @"^[A-Za-z0-9\-_]+$") != true)
				{
					sb.Remove(sb.Length - 1, 1);
					script.tagString = sb.ToString();
					EditorUtility.SetDirty(target);
					GUI.FocusControl("");
					Debug.LogWarning("Tag string is half-width alphanumeric only");
				}
			}

		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("tagTextColor"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("argEvent"));
		*/


		serializedObject.ApplyModifiedProperties();
	}
}


/*
[CustomEditor(typeof(TagActionText), true)]
[CanEditMultipleObjects]
public class TagActionTextInspector : Editor
{
	public override void OnInspectorGUI()
	{
		TagActionText script = target as TagActionText;


		DrawDefaultInspector();
		EditorGUILayout.Space();
		Undo.RecordObject(script, "Inspector");


		EditorGUI.BeginChangeCheck();

		if(EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(target);
		}
	}
}
*/

