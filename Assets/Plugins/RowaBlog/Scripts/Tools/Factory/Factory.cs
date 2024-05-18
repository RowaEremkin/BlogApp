

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.Blog.Tools.Factory
{
    public class Factory<MN,FD> : IFactory<MN,FD> where MN : MonoBehaviour
    {
        protected readonly MN _prefab;
		protected readonly Transform _container;
        protected List<MN> _list;
        protected Dictionary<MN, FD> _dictinary;
        public List<MN> List => _list;
        public Dictionary<MN, FD> Dictinary => _dictinary;
        public event Action OnListChanged;
        public Factory(MN prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }
        public virtual List<MN> Add(List<FD> data, bool descent = false)
        {
            for (int i = descent ? data.Count - 1 : 0; (descent ? i >= 0 : i < data.Count); i += descent ? -1 : 1)
            {
                MN newMn = Add(data[i], invokeEvent: false);
                _list.Add(newMn);
                _dictinary.Add(newMn, data[i]);
            }
            OnListChanged?.Invoke();
            return _list;
        }
        public virtual List<MN> Create(List<FD> data, bool descent = false)
        {
            Clear(invokeEvent: false);
            _list = new List<MN>();
            _dictinary = new Dictionary<MN, FD>();
            for (int i = descent?data.Count-1:0; (descent? i>=0 : i < data.Count); i+=descent?-1:1)
            {
                MN newMn = Add(data[i], invokeEvent: false);
                _list.Add(newMn);
                _dictinary.Add(newMn, data[i]);

            }
            OnListChanged?.Invoke();
            return _list;
        }
        public virtual void Clear(bool invokeEvent = true)
        {
            if (_list == null) return;
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                if(_list[i] != null)
                {
                    GameObject.Destroy(_list[i].gameObject);
                }
                _list.RemoveAt(i);
            }
            if (invokeEvent) OnListChanged?.Invoke();
        }
        public virtual MN Add(FD data, bool invokeEvent = true)
        {
            MN newMN = GameObject.Instantiate(_prefab, _container);
            if (newMN)
            {
                SetData(newMN, data);
            }
            if (invokeEvent) OnListChanged?.Invoke();
            return newMN;
        }
        public virtual void SetData(MN view, FD data)
        {

        }
        public virtual FD GetData(MN view)
        {
            if (_dictinary.ContainsKey(view))
            {
                return _dictinary[view];
            }
            return default(FD);
        }
    }
}