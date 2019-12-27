using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class PedestalInfo
    {
        public PedestalInfo(PedestalType o, int s) => (type, slot) = (o, s);

        public PedestalType type;
        public int slot;
    }

    public class PedestalLayer
    {
        public List<PedestalInfo> pedestalInfos;
        public List<Pedestal> pedestales;
        public int level;
        public float height;
        public bool hasDestroy;
    }

    public class Platform : MonoBehaviour
    {
        private const float kAnglePerSlot = 360f/7f;

        [SerializeField] private List<Pedestal> _pedestalePrefabs;
        [SerializeField] private Transform _propInstance;
        
        private List<Transform> _props;
        private GameSetting _gameSetting;
        float _propHeight;
        float _propBottomHeight;
        float _platformAngle;

        PedestalPool _pedestalPool;
        List<PedestalLayer> _pedestalLayers;
        PlatformRule _platformRule;
        private bool _canInteract;
        public bool CanInteract
        {
            get { return _canInteract; }
            set { _canInteract = value; }
        }

        public void Setup(GameSetting gameSetting)
        {
            EventHub.Bind<EventTouchMoved>(OnTouchMoved, true);
            EventHub.Bind<EventPedestalDestroy>(OnPedestalDestroyed);

            _canInteract = false;

            MeshRenderer propMeshRenderer = _propInstance.GetComponent<MeshRenderer>();
            _propHeight = propMeshRenderer.bounds.size.y;
            _propBottomHeight = -_propHeight * 2;

            float propHeight = _propInstance.GetComponent<MeshRenderer>().bounds.size.y;
            Transform propBackBuffer = Instantiate(_propInstance, new Vector3(0, -propHeight * 2f, 0), Quaternion.identity, transform);
            propBackBuffer.localScale = new Vector3(1f, -_propInstance.localScale.y, 1f);

            _props = new List<Transform>();
            _props.Add(_propInstance);
            _props.Add(propBackBuffer);

            _platformAngle = 0;

            _pedestalPool = new PedestalPool(this.transform, _pedestalePrefabs);
            _pedestalLayers = new List<PedestalLayer>();
            _platformRule = new SimplePlatformRule();

            _gameSetting = gameSetting;
            RecycleOldPedestales();
            AddNewPedestales();
        }

        private void OnDestroy()
        {
            UnregisterEvent();
        }

        public void UnregisterEvent()
        {
            EventHub.Unbind<EventTouchMoved>(OnTouchMoved);
            EventHub.Unbind<EventPedestalDestroy>(OnPedestalDestroyed);
        }

        void OnTouchMoved(EventTouchMoved e)
        {
            if (!_canInteract)
                return;

            Camera cam = Camera.main;
            float xMove = e.deltaPos.x / Camera.main.pixelWidth;
            float rotateAngle = xMove * _gameSetting.unitToAngleRotation;
            _platformAngle -= rotateAngle;
            transform.rotation = Quaternion.Euler(0f, _platformAngle, 0f);
        }

        public PedestalType UpdatePenguinPosition(Vector3 position)
        {
            CheckPropAndRecyclePedestales(position);
            return PedestalType.None;
        }

        public void DestroyNextLayer()
        {
            PedestalLayer nextLayer = GetNextPedestalLayer();
            if (nextLayer != null)
            {
                nextLayer.hasDestroy = true;
                foreach (var pedestal in nextLayer.pedestales)
                {
                    pedestal.Fall();
                }

                EventHub.Emit(new EventCharacterPassLayer(nextLayer));
            }
        }

        void CheckPropAndRecyclePedestales(Vector3 position)
        {
            PedestalLayer nextLayer = GetNextPedestalLayer();
            if (nextLayer != null && position.y < nextLayer.height - 0.5f)
            {
                nextLayer.hasDestroy = true;
                foreach (var pedestal in nextLayer.pedestales)
                {
                    pedestal.Fall();
                }

                EventHub.Emit(new EventCharacterPassLayer(nextLayer));
            }

            float centerBotPropY = _propBottomHeight + _propHeight / 2;
            if (position.y < centerBotPropY)
            {
                _propBottomHeight -= _propHeight;
                
                Vector3 propPosition = _props[0].position;
                propPosition.y -= _propHeight * 2;
                _props[0].position = propPosition;

                _props.Reverse();

                RecycleOldPedestales();
                AddNewPedestales();
            }
        }

        void RecycleOldPedestales()
        {
            float propTopHeight = _propBottomHeight + _propHeight * 2;
            var willBeRemovedLayers = _pedestalLayers.FindAll(layer => layer.height > propTopHeight);
            foreach (var layer in willBeRemovedLayers)
            {
                _pedestalLayers.Remove(layer);
            }
        }

        void OnPedestalDestroyed(EventPedestalDestroy e)
        {
            _pedestalPool.Return(e.pedestal);
        }

        void AddNewPedestales()
        {
            int lastLevel = -1;
            float lastPedestalHeight = _gameSetting.pedestalStartPosition + _gameSetting.distancePerPedestalLayer;
            if (_pedestalLayers.Count > 0)
            {
                lastLevel = _pedestalLayers[_pedestalLayers.Count - 1].level;
                lastPedestalHeight = _pedestalLayers[_pedestalLayers.Count - 1].height;
            }

            float pedestalHeight = lastPedestalHeight;
            int pedestalLevel = lastLevel;
            while (pedestalHeight > _propBottomHeight)
            {
                pedestalHeight -= _gameSetting.distancePerPedestalLayer;
                pedestalLevel += 1;

                PedestalLayer layer = new PedestalLayer();
                layer.pedestalInfos = _platformRule.GetPedestalInfos(pedestalLevel);
                layer.pedestales = new List<Pedestal>();
                layer.height = pedestalHeight;
                layer.level = pedestalLevel;
                layer.hasDestroy = false;
                _pedestalLayers.Add(layer);

                foreach (var pedestalInfo in layer.pedestalInfos)
                {
                    Vector3 position = new Vector3(0f, pedestalHeight, 0f);
                    Pedestal pedestal = _pedestalPool.Instantiate(pedestalInfo.type, position, kAnglePerSlot * pedestalInfo.slot);
                    layer.pedestales.Add(pedestal);
                }
            }
        }

        PedestalLayer GetNextPedestalLayer()
        {
            foreach (var layer in _pedestalLayers)
            {
                if (layer.hasDestroy)
                    continue;

                return layer;
            }

            return null;
        }
    }
}

