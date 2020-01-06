// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
//
// namespace Penguin
// {
//     public static class TelegramCrashlytics
//     {
//         const string baseURL = "https://api.telegram.org/bot993980375:AAG1WqtVV0-0BHjlSb9-8ovJmv_Xhi083zM/sendMessage?chat_id=-357310364&text=";
//         static HashSet<int> sentContents;
//
//         [RuntimeInitializeOnLoadMethod]
//         static void HandleLog()
//         {
//             if (Application.isPlaying)
//             {
//                 sentContents = new HashSet<int>();
//                 Application.logMessageReceived += OnLogReceived;
//             }
//         }
//
//         static void OnLogReceived(string condition, string stackTrace, LogType type)
//         {
//             if (type != LogType.Exception)
//                 return;
//             
//             string content = UnityWebRequest.EscapeURL(condition + "\n\n" + stackTrace);
//             int hashContent = content.GetHashCode();
//             if (sentContents.Add(hashContent))
//             {
//                 var request = UnityWebRequest.Get(baseURL + content);
//                 request.SendWebRequest();
//             }
//         }
//     }
// }