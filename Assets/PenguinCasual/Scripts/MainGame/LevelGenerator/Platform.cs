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
        public List<PedestalInfo> pedestalLayers;
        public List<Pedestal> pedestales;
        public int level;
        public float height;
        public bool hasDestroy;
    }

    public class Platform : MonoBehaviour
    {
        const float kAnglePerSlot = 360f/7f;

        [SerializeField] List<Pedestal> _pedestalePrefabs;
        [SerializeField] float _unitToAngleRotation;
        [SerializeField] float _heightPerPedestalLayer;
        [SerializeField] Transform _propInstance;
        List<Transform> _props;
        float _propHeight;
        float _propBottomHeight;
        float _platformAngle;

        PedestalPool _pedestalPool;
        List<PedestalLayer> _pedestalLayers;
        PlatformRule _platformRule;

        void Awake()
        {
            EventHub.Bind<EventTouchMoved>(OnTouchMoved, true);
            EventHub.Bind<EventPedestalDestroy>(OnPedestalDestroyed);
            
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

            RecycleOldPedestales();
            AddNewPedestales();
        }

        private void OnDestroy()
        {
            EventHub.Unbind<EventTouchMoved>(OnTouchMoved);

        }

        public void UnregisterEvent()
        {
            EventHub.Unbind<EventTouchMoved>(OnTouchMoved);
            EventHub.Unbind<EventPedestalDestroy>(OnPedestalDestroyed);
        }

        void OnTouchMoved(EventTouchMoved e)
        {
            Camera cam = Camera.main;
            float xMove = e.deltaPos.x / Camera.main.pixelWidth;
            float rotateAngle = xMove * _unitToAngleRotation;
            _platformAngle -= rotateAngle;
            transform.rotation = Quaternion.Euler(0f, _platformAngle, 0f);
        }

        public PedestalType UpdatePenguinPosition(Vector3 position)
        {
            CheckPropAndRecyclePedestales(position);
            return PedestalType.None;
        }

        void CheckPropAndRecyclePedestales(Vector3 position)
        {
            foreach (var layer in _pedestalLayers)
            {
                if (layer.hasDestroy)
                    continue;
                
                if (position.y < layer.height - 0.5f)
                {
                    layer.hasDestroy = true;
                    foreach (var pedestal in layer.pedestales)
                    {
                        pedestal.Fall();
                    }

                    EventHub.Emit(new EventCharacterPassLayer(layer));
                }

                break;
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
                // foreach (var pedestal in layer.pedestales)
                // {
                //     _pedestalPool.Return(pedestal);
                // }

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
            float lastPedestalHeight = 0;
            if (_pedestalLayers.Count > 0)
            {
                lastLevel = _pedestalLayers[_pedestalLayers.Count - 1].level;
                lastPedestalHeight = _pedestalLayers[_pedestalLayers.Count - 1].height;
            }

            float pedestalHeight = lastPedestalHeight;
            int pedestalLevel = lastLevel;
            while (pedestalHeight > _propBottomHeight)
            {
                pedestalHeight -= _heightPerPedestalLayer;
                pedestalLevel += 1;

                PedestalLayer layer = new PedestalLayer();
                layer.pedestalLayers = _platformRule.GetPedestalInfos(pedestalLevel);
                layer.pedestales = new List<Pedestal>();
                layer.height = pedestalHeight;
                layer.level = pedestalLevel;
                layer.hasDestroy = false;
                _pedestalLayers.Add(layer);

                foreach (var pedestalInfo in layer.pedestalLayers)
                {
                    Vector3 position = new Vector3(0f, pedestalHeight, 0f);
                    Pedestal pedestal = _pedestalPool.Instantiate(pedestalInfo.type, position, kAnglePerSlot * pedestalInfo.slot);
                    layer.pedestales.Add(pedestal);
                }
            }
        }
    }
}

