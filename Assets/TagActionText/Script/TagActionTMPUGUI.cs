// using System.Collections.Generic;
// using System.Text;
// using TMPro;
// using UnityEngine;
//
// #if (UNITY_EDITOR)
// using UnityEditor;
// #endif
//
// namespace UGUITagActionText
// {
//
// 	//[AddComponentMenu("UI/TagActionText/TagActionTextMeshProUGUI", 99)]
// 	[DisallowMultipleComponent]
// 	[RequireComponent(typeof(RectTransform))]
// 	[RequireComponent(typeof(CanvasRenderer))]
// 	public class TagActionTMPUGUI : TagActionTMPUGUIBase, ITagActionRePaint
// 	{
//
// 		int[] charBuffer;
// 		List<Vector2Int> tmpTagIndexList = new List<Vector2Int>();
// 		Vector2Int forSetVec2Int;
// 		readonly TagActionMatchHelper matchHelper = new TagActionMatchHelper();
// 		List<TagMatch> tagMatchList = new List<TagMatch>();
// 		StringBuilder textSB = new StringBuilder();
// 		StringBuilder linkIdSB = new StringBuilder();
//
// 		[SerializeField, HideInInspector]
// 		string hold_baseText;
// 		[SerializeField, HideInInspector]
// 		[TextArea(3, 10)]
// 		string m_tagActionText;
// 		public string tagActionText
// 		{
// 			set
// 			{
// 				if (m_tagActionText == value)
// 					return;
// 				m_tagActionText = value;
// 				RePaint();
// 			}
// 		}
// 		public string GetText()
// 		{
// 			return m_tagActionText;
// 		}
//
// 		[SerializeField, HideInInspector] TagActionManager m_tagManager;
// 		public TagActionManager TagManager
// 		{
// 			get { return m_tagManager; }
// 			set
// 			{
// 				if (m_tagManager != value)
// 				{
// 					m_tagManager = value;
// 					RePaint();
// 				}
// 				if (m_tagManager != null)
// 					m_tagManager.AddTextComponentList(this);
// 			}
// 		}
//
//
// #if (UNITY_EDITOR)
// 		protected override void OnValidate()
// 		{
// 			if (m_tagManager != null)
// 				m_tagManager.AddTextComponentList(this);
// 			base.OnValidate();
// 		}
// #endif
//
// 		protected override void Start()
// 		{
// 			RePaint();
// 			base.Start();
// 		}
//
// 		public override void SetVerticesDirty()
// 		{
// 			if (base.richText != true)
// 			{
// 				base.richText = true;
// #if (UNITY_EDITOR)
// 				Debug.LogWarning("TagActionTMPUGUI is using RichText to implement the function, so it can not be turned off");
// #endif
// 			}
//
// 			//when changed base.text
// 			if (hold_baseText != base.text)
// 			{
// #if (UNITY_EDITOR)
// 				if (EditorApplication.isPlaying || EditorApplication.isPaused)
// 				{
// 					base.tappedLinkId = string.Empty;
// 					string tempExchText = ExchangeTagText(m_tagActionText, true);
// 					if (tempExchText != base.text)
// 					{
// 						m_tagActionText = base.text;
// 						RePaint();
// 					}
// 					else
// 					{
// 						if (hold_baseText != tempExchText)
// 							hold_baseText = tempExchText;
// 					}
// 				}
// #else
// 				base.tappedLinkId = string.Empty;
// 				m_tagActionText = base.text;
// 				RePaint();
// #endif
// 			}
//
// 			base.SetVerticesDirty();
// 		}
//
// 		public void RePaint()
// 		{
//
// 			hold_baseText = ExchangeTagText(m_tagActionText);
// 			base.text = hold_baseText;
//
// 			/*
// 			base.m_havePropertiesChanged = true;
// 			base.m_isCalculateSizeRequired = true;
// 			base.m_isInputParsingRequired = true;
// 			base.SetVerticesDirty();
// 			base.SetLayoutDirty();
// 			*/
// 		}
//
// 		string ExchangeTagText(string exchangeSourceText)
// 		{
// 			return ExchangeTagText(exchangeSourceText, false);
// 		}
// 		string ExchangeTagText(string exchangeSourceText, bool useTappedColor)
// 		{
//
// 			base.StringToCharArray(exchangeSourceText, ref charBuffer);
// 			int startTMPTagIndex = 0;
// 			int endTMPTagIndex = 0;
// 			tmpTagIndexList.Clear();
// 			base.RemoveAllLinkData();
//
// 			for (int i = 0; charBuffer[i] != 0; i++)
// 			{
// 				int charCode = charBuffer[i];
//
// 				if (base.m_isRichText && charCode == 60)  // '<'
// 				{
// 					startTMPTagIndex = i;
// 					base.m_isParsingText = true;
// 					base.m_textElementType = TMP_TextElementType.Character;
//
// 					// Check if Tag is valid. If valid, skip to the end of the validated tag.
// 					if (base.ValidateHtmlTag(charBuffer, i + 1, out endTMPTagIndex))
// 					{
// 						i = endTMPTagIndex;
// 						forSetVec2Int.Set(startTMPTagIndex, endTMPTagIndex);
// 						tmpTagIndexList.Add(forSetVec2Int);
// 						if (base.m_textElementType == TMP_TextElementType.Character)
// 							continue;
// 					}
// 				}
// 				base.m_isParsingText = false;
// 			}
//
// 			tagMatchList.Clear();
//
// 			if (TagManager != null)
// 			{
// 				matchHelper.CreateTagMatchList(exchangeSourceText, TagManager.tagData, tagMatchList);
// 			}
//
// 			if (tagMatchList.Count > 0)
// 			{
// 				//exclude TagAction tag inside TMP tag charactor string
// 				for (int i = (tagMatchList.Count - 1); i >= 0; i--)
// 				{
// 					int tagStartIndex = tagMatchList[i].matchIndex;
// 					foreach (Vector2Int n in tmpTagIndexList)
// 					{
// 						if (n.x <= tagStartIndex && tagStartIndex <= n.y)
// 						{
// 							tagMatchList.Remove(tagMatchList[i]);
// 							break;
// 						}
// 					}
// 				}
// 			}
// 			if (tagMatchList.Count > 0)
// 			{
// 				int currentIndex = 0;
// #if (NET_2_0 || NET_2_0_SUBSET)
// 				textSB.Remove(0, textSB.Length);
// #else
// 				textSB.Clear(); //require higher .Net4.0 in Unity editor settings
// #endif
// 				int counter = 0;
// 				foreach (TagMatch item in tagMatchList)
// 				{
// 					TagActionManager.TagData data = item.tagData;
// 					if ((item.matchIndex - currentIndex) < 0)
// 						continue;
//
// #if (NET_2_0 || NET_2_0_SUBSET)
// 					linkIdSB.Remove(0, textSB.Length);
// #else
// 					linkIdSB.Clear(); //require higher .Net4.0 in Unity editor settings
// #endif
// 					linkIdSB.Append("TagAction_").Append(counter.ToString()).Append("_").Append(item.tagData.tagString).Append("_").Append(item.argStr);
// 					RegisterLinkData(linkIdSB.ToString(), item.argStr, data);
//
// 					textSB.Append(exchangeSourceText.Substring(currentIndex, item.matchIndex - currentIndex));
// 					bool hasColorTag = false;
// 					if (useTappedColor
// 						&& TagManager.isChangeTappedColor
// 						&& base.tappedLinkId == linkIdSB.ToString())
// 					{
// 						hasColorTag = true;
// 						textSB.Append("<color=#").Append(ColorUtility.ToHtmlStringRGBA(TagManager.tappedColor)).Append(">");
// 					}
// 					else
// 					{
// 						if (item.tagData.isChangeTextColor)
// 						{
// 							hasColorTag = true;
// 							textSB.Append("<color=#").Append(ColorUtility.ToHtmlStringRGBA(item.tagData.textColor)).Append(">");
// 						}
// 					}
// 					textSB.Append("<link=\"").Append(linkIdSB).Append("\">");
// 					textSB.Append(item.taggedText);
// 					textSB.Append("</link>");
// 					if (hasColorTag)
// 						textSB.Append("</color>");
// 					currentIndex = item.matchIndex + item.matchLength;
// 					counter += 1;
// 				}
// 				textSB.Append(exchangeSourceText.Substring(currentIndex));
// 				return textSB.ToString();
// 			}
// 			else
// 			{
// 				return exchangeSourceText;
// 			}
// 		}
//
// 		protected override void ApplyTappedColor(int index)
// 		{
// 			if (TagManager == null || TagManager.isChangeTappedColor != true)
// 				return;
// 			if (base.tappedLinkId == string.Empty)
// 				return;
// 			if (base.tappedLinkId != hold_baseText.Substring(index, base.tappedLinkId.Length))
// 				return;
//
// 			string tempCollateStr = hold_baseText.Substring((index - 24), 8);
// 			if (tempCollateStr == "<color=#")
// 			{
// 				hold_baseText = ExchangeTagText(m_tagActionText);
// 				string tappedColorCode = ColorUtility.ToHtmlStringRGBA(TagManager.tappedColor);
// #if (NET_2_0 || NET_2_0_SUBSET)
// 				textSB.Remove(0, textSB.Length);
// #else
// 				textSB.Clear(); //require higher .Net4.0 in Unity editor settings
// #endif
// 				textSB.Append(hold_baseText).Remove((index - 16), 8).Insert((index - 16), tappedColorCode);
// 				hold_baseText = textSB.ToString();
// 				base.text = hold_baseText;
// 			}
// 		}
//
//
// 		/*
// 		abstract class TMP_Text
// 		public string text
// 		{
// 			get { return m_text; }
// 			set { if(m_text == value) return; m_text = old_text = value; m_inputSource = TextInputSources.String; m_havePropertiesChanged = true; m_isCalculateSizeRequired = true; m_isInputParsingRequired = true; SetVerticesDirty(); SetLayoutDirty(); }
// 		}
// 		[SerializeField]
// 		[TextArea(3, 10)]
// 		protected string m_text;
// 		*/
// 	}
//
// }
