using Penguin.Ads.Settings;
using UnityEngine;

namespace Penguin.Ads
{
    public class AdvertisementManager : MonoBehaviour
    {
        public static AdvertisementManager instance;
        public static AdvertisementManager Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType(typeof(AdvertisementManager)) as AdvertisementManager;

                return instance;
            }
        }
        
        [SerializeField]
        private string _configPath;

        void Awake( )
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            if(Instance != this)
            {
                Destroy(this.gameObject);
            }
            else if(instance == null)
            {
                instance = this;
            }
            
            AdvertisementConfig config = Resources.Load<AdvertisementConfig> (_configPath);
            if (config == null) {
                Debug.LogError ("Please setting up your advertisement config via \"Create->Database->Advertisement Config\"");
                Debug.LogError ("Failed to boots ads system");
                return;
            }
            
            Advertiser.AdvertisementSystem.EnableAdvertise = true;
            Advertiser.AdvertisementSystem.LoadConfig (config);
            Advertiser.AdvertisementSystem.Run ();
        }
    }
}