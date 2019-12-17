using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    /// <summary>
    /// Handle logic for in-game main character
    /// </summary>
    public class Character : MonoBehaviour
    {
        [SerializeField]
        private GameObject _model;

        public void SetModel(GameObject characterModel)
        {
            if (_model != null)
            {
                GameObject.Destroy(_model);
            }

            _model = GameObject.Instantiate(characterModel, transform);
            _model.transform.localPosition = Vector3.zero;
            _model.transform.rotation = Quaternion.identity;
        }
    }
}