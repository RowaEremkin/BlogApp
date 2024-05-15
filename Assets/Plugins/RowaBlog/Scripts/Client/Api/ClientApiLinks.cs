
using System.Collections.Generic;

namespace Rowa.Blog.Client.Api
{
    public partial class ClientApi
    {
        private const string Protocol = "http://";//"https://";
        private const string Domain = "localhost:5079";//"some.site";
        private const string Version = "";//"v1/";
        private const bool DeleteDash = false;

        private const string ApiPath = "/api/" + Version;
        private const string PlayerPath = "Player/";
        private const string InventoryPath = "Inventory/";
        private const string BlogPath = "Blog/";
        private const string RoomPath = "Room/";
        private const string CasesPath = "Cases/";
        private const string ShopPath = "Shop/";

        #region Get

        private const string GetRoomPath =  ApiPath + RoomPath + "Get/№";
        private const string GetBlogPath = ApiPath + BlogPath + "Get/#";

        #endregion

        #region Put

        private const string PutPlayerLoginPath = ApiPath + PlayerPath + "Login/#";
        private const string PutPlayerRegisterPath = ApiPath + PlayerPath + "Register/#";
        private const string PutBlogLikePath = ApiPath + BlogPath + "Like/#";

        #endregion

        #region Post

        private const string PostBlogCreatePath = ApiPath + BlogPath + "Create/#";

        #endregion

        #region Delete

        private const string DeletePlayerLogoutPath = ApiPath + PlayerPath + "Logout/#";

        #endregion


        private static string GetFullUrl(string path, string query = null, string value = null, string page = null, Dictionary<string, string> param = null)
        {

            if (DeleteDash) path = path.Replace("-", "");
            if (query != null) path = path.Replace("#", query);
            if (value != null) path = path.Replace("*", value);
            if (page != null) path = path.Replace("№", page);
            if (param != null && param.Count > 0)
            {
                path += "?";
                bool first = true;
                foreach (var pair in param)
                {
                    if (!first) path += "&"; else first = false;
                    path += pair.Key + "=" + pair.Value;
                }
            }

            return Protocol + Domain + path;
        }
    }
}