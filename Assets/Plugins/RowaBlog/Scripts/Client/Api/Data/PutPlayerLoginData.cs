
namespace Rowa.Blog.Client.Api.Data
{
    [System.Serializable]
    public class PutPlayerLoginData
    {
        public string Login;
        public string Password;

        public PutPlayerLoginData(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}