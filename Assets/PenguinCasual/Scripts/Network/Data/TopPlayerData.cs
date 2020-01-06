using System;
using UnityEngine;

namespace Penguin.Network.Data
{
    [Serializable]
    public class TopPlayerData
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private string nickname;
        [SerializeField]
        private int skin_id;
        [SerializeField]
        private int total_score;

        public int Id => id;
        public string Nickname => nickname;
        public int SkinId => skin_id;
        public int TotalScore => total_score;
    }

    [Serializable]
    public class TopPlayersResponse : ResponseBodyWithMultipleEntities<TopPlayerData>
    {
        public static TopPlayersResponse FromJson(string jsonString)
        {
            return JsonUtility.FromJson<TopPlayersResponse>(jsonString);
        }
    }
}