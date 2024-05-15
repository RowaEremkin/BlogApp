using Rowa.Blog.Client.Api;
using Rowa.Blog.Client.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowa.Blog.Client.Device
{
    public class DeviceIdLoader : IDeviceIdLoader
    {
        private readonly IClientApi _clientApi;
        private readonly IStorage _storage;
        public DeviceIdLoader(
            IClientApi clientApi,
            IStorage storage
            )
        {
            _clientApi = clientApi;
            _storage = storage;
            Start();
        }
        public void Start()
        {
            _storage.Load(EStorageEnum.DeviceId.ToString(), OnDeviceIdLoadComplete);
        }
        private void OnDeviceIdLoadComplete(bool load, string deviceId)
        {
            if (load)
            {
                _clientApi.SetDeviceId(deviceId);
            }
            else
            {
                string newDeviceId = _clientApi.GenerateNewDeviceId();
                _storage.Save(EStorageEnum.DeviceId.ToString(), newDeviceId);
            }
        }
    }
}
