using System;

namespace Rowa.Blog.Client.Storage
{
    public interface IStorage
    {
        void Save(string key, string value, Action<bool> onComplete = null);
        void Load(string key, Action<bool, string> onComplete);
        void Delete(string key, Action<bool> onComplete = null);
    }
}