using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.Blog.Tools.Factory
{
    public interface IFactory<MN, FD> where MN : MonoBehaviour
    {
        public List<MN> List { get; }
        public Dictionary<MN, FD> Dictinary { get; }
        public List<MN> Add(List<FD> data, bool descent = false);
        public List<MN> Create(List<FD> data, bool descent = false);
        public MN Add(FD data);
        public void SetData(MN view, FD data);
        public FD GetData(MN view);
    }
}