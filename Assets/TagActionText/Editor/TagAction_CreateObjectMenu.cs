// using UnityEditor;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
//
// namespace UGUITagActionText
// {
//
// 	public static class TagAction_CreateObjectMenu
// 	{
//
// 		static public GameObject FindOrCreateCanvasGO()
// 		{
//
// 			GameObject parentGO = null;
// 			GameObject selectedGo = Selection.activeGameObject;
// 			if (selectedGo)
// 			{
// 				Canvas canvas = selectedGo.GetComponentInParent<Canvas>();
// 				if (canvas != null)
// 				{
// 					if (selectedGo.gameObject.activeInHierarchy)
// 					{
// 						parentGO = selectedGo;
// 					}
// 					else
// 					{
// 						if (canvas.gameObject.activeInHierarchy)
// 							parentGO = canvas.gameObject;
// 					}
// 				}
// 			}
// 			if (parentGO == null)
// 			{
// 				Canvas canvas = Object.FindObjectOfType<Canvas>();
// 				if (canvas != null && canvas.gameObject.activeInHierarchy)
// 				{
// 					parentGO = canvas.gameObject;
// 				}
// 				else
// 				{
// 					GameObject canvasObject = new GameObject("Canvas");
// 					canvas = canvasObject.AddComponent<Canvas>();
// 					canvas.renderMode = RenderMode.ScreenSpaceOverlay;
// 					canvasObject.AddComponent<CanvasScaler>();
// 					canvasObject.AddComponent<GraphicRaycaster>();
// 					Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + canvasObject.name);
// 					parentGO = canvasObject;
// 				}
// 			}
// 			return parentGO;
// 		}
//
// 		[MenuItem("Tools/TagActionText/Create GameObject/UGUI-Text", false, 2001)]
// 		static void CreateTagActionTextObjectPerform(MenuCommand command)
// 		{
// 			GameObject parentGO = FindOrCreateCanvasGO();
// 			GameObject go = new GameObject();
// 			string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parentGO.transform, "TagActionText");
// 			go.name = uniqueName;
// 			//RectTransform goRectTransform = 
// 			go.AddComponent<RectTransform>();
//
// 			Undo.RegisterCreatedObjectUndo((Object)go, "Create " + go.name);
//
// 			// Check if object is being create with left or right click
// 			//GameObject contextObject = command.context as GameObject;
// 			//if (contextObject)
// 			//	GameObjectUtility.SetParentAndAlign(go, contextObject);
// 			//else
// 			GameObjectUtility.SetParentAndAlign(go, parentGO);
//
// 			TagActionText tagActionText = go.AddComponent<TagActionText>();
// 			tagActionText.text = "New Text";
// 			//goRectTransform.sizeDelta = new Vector2(200f, 50f);
//
// 			if (!Object.FindObjectOfType<EventSystem>())
// 			{
// 				GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem));
// 				eventObject.AddComponent<StandaloneInputModule>();
// 				Undo.RegisterCreatedObjectUndo(eventObject, "Create " + eventObject.name);
// 			}
//
// 			Selection.activeGameObject = go;
// 		}
//
// #if (UNITY_2018_3_OR_NEWER)
// 		[MenuItem("Tools/TagActionText/Create GameObject/TextMeshProUGUI", false, 2002)]
// 		static void CreateTagActionTMPUGUIObjectPerform(MenuCommand command)
// 		{
// 			GameObject parentGO = FindOrCreateCanvasGO();
// 			GameObject go = new GameObject();
// 			string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parentGO.transform, "TagActionTMPUGUI");
// 			go.name = uniqueName;
// 			//RectTransform goRectTransform = 
// 			go.AddComponent<RectTransform>();
//
// 			Undo.RegisterCreatedObjectUndo((Object)go, "Create " + go.name);
//
// 			// Check if object is being create with left or right click
// 			//GameObject contextObject = command.context as GameObject;
// 			//if (contextObject)
// 			//	GameObjectUtility.SetParentAndAlign(go, contextObject);
// 			//else
// 			GameObjectUtility.SetParentAndAlign(go, parentGO);
//
// 			TagActionTMPUGUI tagActionTMP = go.AddComponent<TagActionTMPUGUI>();
// 			tagActionTMP.tagActionText = "New Text";
// 			//goRectTransform.sizeDelta = new Vector2(200f, 50f);
//
// 			if (!Object.FindObjectOfType<EventSystem>())
// 			{
// 				GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem));
// 				eventObject.AddComponent<StandaloneInputModule>();
// 				Undo.RegisterCreatedObjectUndo(eventObject, "Create " + eventObject.name);
// 			}
//
// 			Selection.activeGameObject = go;
// 		}
// #endif
//
// 		[MenuItem("Tools/TagActionText/Create GameObject/TagActionManager", false, 2000)]
// 		static void CreateTagActionManagerObjectPerform(MenuCommand command)
// 		{
// 			GameObject go = new GameObject();
// 			string uniqueName = GameObjectUtility.GetUniqueNameForSibling(null, "TagActionManager");
// 			go.name = uniqueName;
// 			go.AddComponent<TagActionManager>();
//
// 			Undo.RegisterCreatedObjectUndo((Object)go, "Create " + go.name);
//
// 			Selection.activeGameObject = go;
// 		}
//
// 		private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
// 		{
// 			// Find the best scene view
// 			SceneView sceneView = SceneView.lastActiveSceneView;
// 			if (sceneView == null && SceneView.sceneViews.Count > 0)
// 				sceneView = SceneView.sceneViews[0] as SceneView;
//
// 			// Couldn't find a SceneView. Don't set position.
// 			if (sceneView == null || sceneView.camera == null)
// 				return;
//
// 			// Create world space Plane from canvas position.
// 			Vector2 localPlanePosition;
// 			Camera camera = sceneView.camera;
// 			Vector3 position = Vector3.zero;
// 			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
// 			{
// 				// Adjust for canvas pivot
// 				localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
// 				localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;
//
// 				localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
// 				localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);
//
// 				// Adjust for anchoring
// 				position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
// 				position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;
//
// 				Vector3 minLocalPosition;
// 				minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
// 				minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;
//
// 				Vector3 maxLocalPosition;
// 				maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
// 				maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;
//
// 				position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
// 				position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
// 			}
//
// 			itemTransform.anchoredPosition = position;
// 			itemTransform.localRotation = Quaternion.identity;
// 			itemTransform.localScale = Vector3.one;
// 		}
//
// 	}
// }
//
