namespace Rowa.Blog.Client.Api.Data
{
    [System.Serializable]
    public class PutPlayerRegisterData
    {
        public string Login;
        public string Password;

        public PutPlayerRegisterData(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
