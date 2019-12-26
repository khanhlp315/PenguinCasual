
using System;
using UnityEngine;

namespace Penguin.Network.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private string nickname;
        [SerializeField]
        private int skin_id;
        [SerializeField]
        private int highest_score;
        [SerializeField]
        private int rank;
        [SerializeField]
        private string created_at;
        [SerializeField]
        private string updated_at;

        public int Id => id;
        public string Nickname => nickname;
        public int SkinId => skin_id;
        public int HighestScore => highest_score;
        public int Rank => rank;
        public string CreatedAt => created_at;
        public string UpdatedAt => updated_at;
    }

    [Serializable]
    public class PlayerDataResponse : ResponseBodyWithSingleEntity<PlayerData>
    {
        public static PlayerDataResponse FromJson(string jsonString)
        {
            return JsonUtility.FromJson<PlayerDataResponse>(jsonString);
        }
    }
}