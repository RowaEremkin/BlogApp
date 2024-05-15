
using System.Collections.Generic;

namespace Rowa.Blog.Client.Storage
{
    [System.Serializable]
    public class StorageJsonList
    {
        public List<StorageJsonElement> items = new List<StorageJsonElement>();
    }
}