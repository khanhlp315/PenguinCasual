using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;


namespace UGUITagActionText
{

	//[AddComponentMenu("UI/TagActionText/UGUI-TagActionText", 99)]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasRenderer))]
	public class TagActionText : TagActionTextBase, ITagActionRePaint
	{

		[SerializeField, HideInInspector]
		string originText;

#if (UNITY_EDITOR)
		[HideInInspector] public TagActionManager hold_tagManager;
#endif
		[SerializeField, HideInInspector] TagActionManager m_tagManager;
		public TagActionManager TagManager
		{
			get { return m_tagManager; }
			set
			{
				if (m_tagManager != value)
				{
					m_tagManager = value;
					if (m_tagManager != null)
						m_tagManager.AddTextComponentList(this);
					base.RePaint();
				}
#if (UNITY_EDITOR)
				if (hold_tagManager != m_tagManager)
					hold_tagManager = m_tagManager;
#endif
			}
		}


#if (UNITY_EDITOR)
		protected override void OnValidate()
		{
			if (m_tagManager != null)
				m_tagManager.AddTextComponentList(this);
			base.OnValidate();
		}
#endif


		readonly TagActionMatchHelper matchHelper = new TagActionMatchHelper();
		List<TagMatch> tagMatchList = new List<TagMatch>();
		StringBuilder textSB = new StringBuilder();

		protected override void OnPopulateMesh(VertexHelper vertexHelper)
		{

			base.RemoveAllLinkData();
			if (originText != m_Text)
			{
				originText = m_Text;
				base.ResetTapped();
			}
			tagMatchList.Clear();

			if (TagManager != null)
			{
				base.isChangeTappedColor = TagManager.isChangeTappedColor;
				base.tappedColor = TagManager.tappedColor;
				matchHelper.CreateTagMatchList(originText, TagManager.tagData, tagMatchList);
			}

			if (tagMatchList.Count > 0)
			{
				int totalTagStrLength = 0;
				int currentIndex = 0;
#if (NET_2_0 || NET_2_0_SUBSET)
				textSB.Remove(0, textSB.Length);
#else
				textSB.Clear(); //require higher .Net4.0 in Unity editor settings
#endif
				foreach (TagMatch item in tagMatchList)
				{
					TagActionManager.TagData data = item.tagData;
					if ((item.matchIndex - currentIndex) < 0)
						continue;
					textSB.Append(originText.Substring(currentIndex, item.matchIndex - currentIndex));
					textSB.Append(item.taggedText);
					base.RegisterLinkData(item.matchIndex - totalTagStrLength, item.taggedText.Length, item.argStr, data);
					currentIndex = item.matchIndex + item.matchLength;
					totalTagStrLength += item.tagLength;
				}
				textSB.Append(originText.Substring(currentIndex));

				base.untagText = m_Text = textSB.ToString();
				base.OnPopulateMesh(vertexHelper);
				m_Text = originText;
			}
			else
			{
				base.OnPopulateMesh(vertexHelper);
			}

		}

	}

}
