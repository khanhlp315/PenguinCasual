using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUITagActionText
{

	public abstract class TagActionTMPUGUIBase : TextMeshProUGUI, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler
	{

		List<LinkData> linkDataList = new List<LinkData>();
		int pointerDownLinkIndex;
		string pointerDownLinkIdStr;
		float tapTimer;
		protected string tappedLinkId;

		public class LinkData
		{
			public string linkId;
			public string argStr;
			public bool isChangeTextColor;
			public Color color;
			public ArgUnityEvent onClick;

			public LinkData(string linkId, string argStr, bool isChangeTextColor, Color color, ArgUnityEvent onClick)
			{
				this.linkId = linkId;
				this.argStr = argStr;
				this.isChangeTextColor = isChangeTextColor;
				this.color = color;
				this.onClick = onClick;
			}
		};

		protected void RegisterLinkData(string linkId, string argStr, TagActionManager.TagData tagData)
		{

			if (string.IsNullOrEmpty(linkId) || string.IsNullOrEmpty(argStr))
				return;

			linkDataList.Add(new LinkData(linkId, argStr, tagData.isChangeTextColor, tagData.textColor, tagData.argEvent));
		}

		protected void RemoveAllLinkData()
		{
			linkDataList.Clear();
		}

		protected abstract void ApplyTappedColor(int index);

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			int index = TMP_TextUtilities.FindIntersectingLink(this, eventData.position, eventData.pressEventCamera);
			if (index == -1)
				return;
			pointerDownLinkIndex = index;
			var linkInfo = base.textInfo.linkInfo[index];
			pointerDownLinkIdStr = linkInfo.GetLinkID();
			tapTimer = Time.time;
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (pointerDownLinkIndex == -1)
				return;
			if (pointerDownLinkIdStr == string.Empty)
				return;

			int index = TMP_TextUtilities.FindIntersectingLink(this, eventData.position, eventData.pressEventCamera);
			if (index == -1)
			{
				pointerDownLinkIndex = -1;
				pointerDownLinkIdStr = string.Empty;
				return;
			}
			var linkInfo = base.textInfo.linkInfo[index];
			string linkIdStr = linkInfo.GetLinkID();
			if (pointerDownLinkIndex == index && pointerDownLinkIdStr == linkIdStr)
			{
				float coef = 0f;
				float screenInch = 0f;
				float dist = Vector2.Distance(eventData.pressPosition, eventData.position);
				if (Screen.width > Screen.height)
				{
					screenInch = (float)Screen.height / (float)Screen.dpi; //(float)Screen.currentResolution.height
					coef = dist / (float)Screen.height;
				}
				else
				{
					screenInch = (float)Screen.width / (float)Screen.dpi; //(float)Screen.currentResolution.width
					coef = dist / (float)Screen.width;
				}
				if ((screenInch * coef) < 0.1f)
				{
					if (Time.time - tapTimer < 0.5f)
					{
						for (int i = 0; i < linkDataList.Count; i++)
						{
							if (linkDataList[i].linkId == linkIdStr)
							{
								if (linkDataList[i].onClick != null)
									linkDataList[i].onClick.Invoke(linkDataList[i].argStr, eventData.position);
								tappedLinkId = linkIdStr;
								ApplyTappedColor(linkInfo.linkIdFirstCharacterIndex);
							}
						}
					}
				}
			}
			pointerDownLinkIndex = -1;
			pointerDownLinkIdStr = string.Empty;
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			pointerDownLinkIndex = -1;
			pointerDownLinkIdStr = string.Empty;
		}

	}

}
