﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Osblow.Util;

/*
 *	
 *  Manage View's Create And Destory
 *
 *	by Xuanyi
 *
 */

namespace Osblow.App
{
    public class UIManager : MonoBehaviour
    {
        public Dictionary<UIType, GameObject> _UIDict = new Dictionary<UIType,GameObject>();

        private Transform _canvas;

        private void Awake()
        {
            _canvas = GameObject.Find("Canvas").transform;
            foreach (Transform item in _canvas)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        public GameObject GetSingleUI(UIType uiType, bool isCreating = false)
        {
            if(uiType == UIType.TableView)
            {
                int a = 0;
            }
            if(!isCreating && (!_UIDict.ContainsKey(uiType) || !_UIDict[uiType]))
            {
                return null;
            }


            if (_UIDict.ContainsKey(uiType) == false || _UIDict[uiType] == null)
            {
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(uiType.Path)) as GameObject;
                go.transform.SetParent(_canvas, false);
                go.name = uiType.Name;
                _UIDict.AddOrReplace(uiType, go);
                return go;
            }
            return _UIDict[uiType];
        }

        public void DestroySingleUI(UIType uiType)
        {
            if (!_UIDict.ContainsKey(uiType))
            {
                return;
            }

            if (_UIDict[uiType] == null)
            {
                _UIDict.Remove(uiType);
                return;
            }
            _UIDict[uiType].name = "hello";

            _UIDict[uiType].GetComponent<BaseView>().OnExit(null);
            GameObject.DestroyImmediate(_UIDict[uiType]);
            _UIDict.Remove(uiType);
        }
	}
}
