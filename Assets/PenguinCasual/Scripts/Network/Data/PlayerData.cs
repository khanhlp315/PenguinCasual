
using UnityEngine;

namespace Penguin.Network.Data
{
    [SerializeField]
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
        
        public static PlayerData FromJson(string jsonString)
        {
            return JsonUtility.FromJson<PlayerData>(jsonString);
        }
    }
}