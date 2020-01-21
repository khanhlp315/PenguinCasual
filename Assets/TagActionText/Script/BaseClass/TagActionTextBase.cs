using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UGUITagActionText
{

	public abstract class TagActionTextBase : Text, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler
	{

		const int charVertsNum = 6;
		List<LinkData> linkDataList = new List<LinkData>();
		static readonly ObjectPool<List<UIVertex>> verticesPool = new ObjectPool<List<UIVertex>>(null, l => l.Clear());
		LinkData pointerDownEntry;
		float tapTimer;
		protected bool isChangeTappedColor;
		protected Color tappedColor;
		[SerializeField, HideInInspector] protected string untagText;
		int tappedStartIndex;
		int tappedStrLength;

		class LinkData
		{
			public int startIndex;
			public int targetStrLength;
			public string argStr;
			public bool isChangeTextColor;
			public Color color;
			public ArgUnityEvent onClick;
			public List<Rect> rects;

			public LinkData(int startIndex, int targetStrLength, string argStr, bool isChangeTextColor, Color color, ArgUnityEvent onClick)
			{
				this.startIndex = startIndex;
				this.targetStrLength = targetStrLength;
				this.argStr = argStr;
				this.isChangeTextColor = isChangeTextColor;
				this.color = color;
				this.onClick = onClick;
				rects = new List<Rect>();
			}
		};

		/*
		protected override void Start()
		{
			base.Start();
		}
		*/

		protected void RegisterLinkData(int startIndex, int targetStrLength, string argStr, TagActionManager.TagData tagData)
		{

			if (string.IsNullOrEmpty(argStr))
				return;
			if (startIndex < 0 || targetStrLength <= 0 || startIndex + targetStrLength > text.Length)
				return;

			linkDataList.Add(new LinkData(startIndex, targetStrLength, argStr, tagData.isChangeTextColor, tagData.textColor, tagData.argEvent));
		}

		protected void RemoveAllLinkData()
		{
			linkDataList.Clear();
			pointerDownEntry = null;
		}

		public void RePaint()
		{
			//base.UpdateGeometry();
			base.FontTextureChanged();
		}

		protected void ResetTapped()
		{
			tappedStartIndex = 0;
			tappedStrLength = 0;
		}

		//called whenever changing text
		protected override void OnPopulateMesh(VertexHelper vertexHelper)
		{

			base.OnPopulateMesh(vertexHelper);

			var vertices = verticesPool.Get();
			vertexHelper.GetUIVertexStream(vertices);

			Modify(ref vertices);

			vertexHelper.Clear();
			vertexHelper.AddUIVertexTriangleStream(vertices);
			verticesPool.Release(vertices);
		}

		public override float preferredWidth
		{
			get
			{
				var settings = GetGenerationSettings(Vector2.zero);
				RePaint();
				return cachedTextGeneratorForLayout.GetPreferredWidth(untagText, settings) / pixelsPerUnit;
			}
		}
		public override float preferredHeight
		{
			get
			{
				var settings = GetGenerationSettings(new Vector2(GetPixelAdjustedRect().size.x, 0.0f));
				RePaint();
				return cachedTextGeneratorForLayout.GetPreferredHeight(untagText, settings) / pixelsPerUnit;
			}
		}

		void Modify(ref List<UIVertex> vertices)
		{

			int verticesCount = vertices.Count;

			for (int i = 0, len = linkDataList.Count; i < len; i++)
			{
				var entry = linkDataList[i];

				for (int textIndex = entry.startIndex, endIndex = (entry.startIndex + entry.targetStrLength); textIndex < endIndex; textIndex++)
				{
					int vertexStartIndex = textIndex * charVertsNum;
					if (vertexStartIndex + charVertsNum > verticesCount)
						break;

					var tempMin = Vector2.one * float.MaxValue;
					var tempMax = Vector2.one * float.MinValue;

					for (int vertexIndex = 0; vertexIndex < charVertsNum; vertexIndex++)
					{
						var vertex = vertices[vertexStartIndex + vertexIndex];

						if (isChangeTappedColor && tappedStartIndex <= textIndex && textIndex < (tappedStartIndex + tappedStrLength))
						{
							vertex.color = tappedColor;
						}
						else
						{
							if (entry.isChangeTextColor)
								vertex.color = entry.color;
						}
						vertices[vertexStartIndex + vertexIndex] = vertex;

						var pos = vertices[vertexStartIndex + vertexIndex].position;

						if (pos.x < tempMin.x)
						{
							tempMin.x = pos.x;
						}
						if (pos.y < tempMin.y)
						{
							tempMin.y = pos.y;
						}

						if (pos.x > tempMax.x)
						{
							tempMax.x = pos.x;
						}
						if (pos.y > tempMax.y)
						{
							tempMax.y = pos.y;
						}
					}

					entry.rects.Add(new Rect { min = tempMin, max = tempMax });
				}


				var mergedRects = new List<Rect>();
				foreach (var charRects in SplitRectsByRow(entry.rects))
				{
					mergedRects.Add(CalculateAABB(charRects));
				}

				entry.rects = mergedRects;

			}
		}

		List<List<Rect>> SplitRectsByRow(List<Rect> rects)
		{
			var rectsList = new List<List<Rect>>();
			var rowStartIndex = 0;

			for (int i = 1, rectNum = rects.Count; i < rectNum; i++)
			{
				if (rects[i].xMin < rects[i - 1].xMin)
				{
					rectsList.Add(rects.GetRange(rowStartIndex, i - rowStartIndex));
					rowStartIndex = i;
				}
			}

			if (rowStartIndex < rects.Count)
			{
				rectsList.Add(rects.GetRange(rowStartIndex, rects.Count - rowStartIndex));
			}

			return rectsList;
		}

		Rect CalculateAABB(List<Rect> rects)
		{
			Vector2 tempMin = Vector2.one * float.MaxValue;
			var tempMax = Vector2.one * float.MinValue;

			for (int i = 0, rectNum = rects.Count; i < rectNum; i++)
			{
				if (rects[i].xMin < tempMin.x)
				{
					tempMin.x = rects[i].xMin;
				}
				if (rects[i].yMin < tempMin.y)
				{
					tempMin.y = rects[i].yMin;
				}

				if (rects[i].xMax > tempMax.x)
				{
					tempMax.x = rects[i].xMax;
				}
				if (rects[i].yMax > tempMax.y)
				{
					tempMax.y = rects[i].yMax;
				}
			}

			return new Rect { min = tempMin, max = tempMax };
		}

		Vector3 ToWorldPosition(Vector2 position, Camera camera)
		{
			if (base.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				return position;
			}
			else
			{
				Vector3 worldPos = Vector3.zero;
				if (base.canvas)
					RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out worldPos);
				return worldPos;
			}
		}

		Vector2 ToLocalPosition(Vector2 position, Camera camera)
		{
			if (!base.canvas)
				return Vector2.zero;

			if (base.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				return transform.InverseTransformPoint(position);
			}
			else
			{
				Vector2 localPos = Vector2.zero;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, camera, out localPos);
				return localPos;
			}
		}


		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			/*
			//if use OnPointerUp, when drag start on link and if pixelDragThreshold is none or small, called OnPointerUp() Handler by unity system
			eventData.useDragThreshold = true;
			float screenCoef = 1f;
			if(Screen.width > Screen.height)
				screenCoef = (float)Screen.height / (float)Screen.currentResolution.height;
			else
				screenCoef = (float)Screen.width / (float)Screen.currentResolution.width;
			int threshold = Mathf.CeilToInt(((float)Screen.dpi * 0.2f) * screenCoef);
			EventSystem.current.pixelDragThreshold = threshold;
			*/
			var localPosition = ToLocalPosition(eventData.position, eventData.pressEventCamera);

			for (int i = 0; i < linkDataList.Count; i++)
			{
				for (int j = 0; j < linkDataList[i].rects.Count; j++)
				{
					if (linkDataList[i].rects[j].Contains(localPosition))
					{
						pointerDownEntry = linkDataList[i];
						tapTimer = Time.time;
						break;
					}
				}
			}
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (pointerDownEntry == null)
				return;

			var localPosition = ToLocalPosition(eventData.position, eventData.pressEventCamera);

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
				for (int i = 0; i < linkDataList.Count; i++)
				{
					for (int j = 0; j < linkDataList[i].rects.Count; j++)
					{
						if (linkDataList[i].rects[j].Contains(localPosition))
						{
							if (pointerDownEntry == linkDataList[i])
							{
								if (Time.time - tapTimer < 0.5f)
								{
									if (linkDataList[i].onClick != null)
									{
										//var worldPosition = ToWorldPosition(eventData.position, eventData.pressEventCamera);
										if (linkDataList[i].onClick != null)
											linkDataList[i].onClick.Invoke(linkDataList[i].argStr, eventData.position);

										tappedStartIndex = linkDataList[i].startIndex;
										tappedStrLength = linkDataList[i].targetStrLength;
										RePaint();
									}
								}
								break;
							}
						}
					}
				}
			}
			pointerDownEntry = null;
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			pointerDownEntry = null;
		}

		/*
		//IPointerUpHandler
		//need OnPointerDown() of IPointerDownHandler
		//if LinkActionText on ScrollRect, when drag call start touch on link, also call this without reality pointerUp move, so wrong to work action
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if(pointerDownEntry == null)
				return;

			var localPosition = ToLocalPosition(eventData.position, eventData.pressEventCamera);

			float coef = 0f;
			float screenInch = 0f;
			float dist = Vector2.Distance(eventData.pressPosition, eventData.position);
			if(Screen.width > Screen.height)
			{
				screenInch = (float)Screen.height / (float)Screen.dpi; //(float)Screen.currentResolution.height
				coef = dist / (float)Screen.height;
			}
			else
			{
				screenInch = (float)Screen.width / (float)Screen.dpi; //(float)Screen.currentResolution.width
				coef = dist / (float)Screen.width;
			}
			if((screenInch * coef) < 0.1f)
			{
				for(int i = 0; i < linkDataList.Count; i++)
				{
					for(int j = 0; j < linkDataList[i].rects.Count; j++)
					{
						if(linkDataList[i].rects[j].Contains(localPosition))
						{
							if(pointerDownEntry == linkDataList[i])
							{
								if(Time.time - tapTimer < 0.5f)
								{
									if(linkDataList[i].onClick != null)
									{
										//var worldPosition = ToWorldPosition(eventData.position, eventData.pressEventCamera);
										linkDataList[i].onClick.Invoke(linkDataList[i].argStr, eventData.position);
									}
								}
								break;
							}
						}
					}
				}
			}
			pointerDownEntry = null;
		}

		//IDragHandler 
		//if use OnDrag(), when LinkActionText object on ScrollRect, not scrolling because grab pointer call to ScrollRect by this OnDrag()
		public void OnDrag(PointerEventData eventData)
		{
			if(pointerDownEntry == null)
				return;

			var localPosition = ToLocalPosition(eventData.position, eventData.pressEventCamera);

			bool insideCheck = false;
			for(int j = 0; j < pointerDownEntry.rects.Count; j++)
			{
				if(pointerDownEntry.rects[j].Contains(localPosition))
				{
					insideCheck = true;
					break;
				}
			}
			if(insideCheck != true)
				pointerDownEntry = null;
		}

		//IInitializePotentialDragHandler
		//need OnDrag() of IDragHandler
		//called before drag start
		public void OnInitializePotentialDrag(PointerEventData eventData)
		{

		}
		*/

	}

}
