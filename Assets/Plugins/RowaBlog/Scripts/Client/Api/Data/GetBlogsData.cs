using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowaBlog.Client.Api.Data
{
    [System.Serializable]
    public class GetBlogsData
    {
        public List<BlogElementData> items = new List<BlogElementData>();
    }
}
