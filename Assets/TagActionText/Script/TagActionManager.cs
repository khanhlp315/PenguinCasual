using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


namespace UGUITagActionText
{

	public class TagActionManager : MonoBehaviour
	{

		[SerializeField, HideInInspector] List<ITagActionRePaint> textComponentList = new List<ITagActionRePaint>();
		public void AddTextComponentList(ITagActionRePaint item)
		{
			if (textComponentList.Contains(item) != true)
				textComponentList.Add(item);
		}
		public void RemoveTextComponentList(ITagActionRePaint item)
		{
			textComponentList.Remove(item);
		}

#if (UNITY_EDITOR)
		void OnValidate()
		{
			RePaint();
		}
#endif

		void Awake()
		{
			rePaintAction = RePaint;
			rePaintAllAction -= rePaintAction;
			rePaintAllAction += rePaintAction;
		}
		void OnDestroy()
		{
			rePaintAllAction -= rePaintAction;
		}

		[HideInInspector, SerializeField] bool m_isChangeTappedColor = true;
		public bool isChangeTappedColor
		{
			get { return m_isChangeTappedColor; }
			set
			{
				if (m_isChangeTappedColor != value)
				{
					m_isChangeTappedColor = value;
					if (TagActionManager.rePaintAllAction != null)
						TagActionManager.rePaintAllAction.Invoke();
				}
			}
		}
		[HideInInspector, SerializeField] Color m_tappedColor = new Color(0.7f, 0.4f, 0.8f, 1.0f);
		public Color tappedColor
		{
			get { return m_tappedColor; }
			set
			{
				if (m_tappedColor != value)
				{
					m_tappedColor = value;
					if (TagActionManager.rePaintAllAction != null)
						TagActionManager.rePaintAllAction.Invoke();
				}
			}
		}

		Action rePaintAction;
		public static Action rePaintAllAction;

		void RePaint()
		{
			if (textComponentList != null && textComponentList.Count > 0)
			{
				foreach (var n in textComponentList)
				{
					if (n == null)
						textComponentList.Remove(n);
					else
						n.RePaint();
				}
			}

		}

		public List<TagData> hold_tagData = new List<TagData>();
		[HideInInspector] public List<TagData> tagData = new List<TagData>();

		[System.Serializable]
		public class TagData
		{

			[SerializeField] LinkActionTagType m_tagType;
			public LinkActionTagType tagType
			{
				get { return m_tagType; }
				set
				{
					if (m_tagType != value)
					{
						m_tagType = value;
						if (TagActionManager.rePaintAllAction != null)
							TagActionManager.rePaintAllAction.Invoke();
					}
				}
			}

#if (UNITY_EDITOR)
			[SerializeField] string disp_tagString;
#endif
			[SerializeField] string m_tagString;
			public string tagString
			{
				get { return m_tagString; }
				set
				{
					if (m_tagString != value)
					{
						m_tagString = value;
						if (TagActionManager.rePaintAllAction != null)
							TagActionManager.rePaintAllAction.Invoke();
					}
				}
			}

			[SerializeField] bool m_isChangeTextColor = true;
			public bool isChangeTextColor
			{
				get { return m_isChangeTextColor; }
				set
				{
					if (m_isChangeTextColor != value)
					{
						m_isChangeTextColor = value;
						if (TagActionManager.rePaintAllAction != null)
							TagActionManager.rePaintAllAction.Invoke();
					}
				}
			}

			[SerializeField] Color m_textColor = Color.cyan;
			public Color textColor
			{
				get { return m_textColor; }
				set
				{
					if (m_textColor != value)
					{
						m_textColor = value;
						if (TagActionManager.rePaintAllAction != null)
							TagActionManager.rePaintAllAction.Invoke();
					}
				}
			}

			[SerializeField] ArgUnityEvent m_argEvent = new ArgUnityEvent();
			public ArgUnityEvent argEvent
			{
				get { return m_argEvent; }
				set
				{
					if (m_argEvent != value)
					{
						m_argEvent = value;
						if (TagActionManager.rePaintAllAction != null)
							TagActionManager.rePaintAllAction.Invoke();
					}
				}
			}
		}

	}


	[System.Serializable]
	public class ArgUnityEvent : UnityEvent<string, Vector2>
	{
	}

	public enum LinkActionTagType
	{
		enclosure = 0,
		addition,
		self
	}

	public interface ITagActionRePaint
	{
		void RePaint();
	}


}
