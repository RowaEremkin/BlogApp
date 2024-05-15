namespace Rowa.Blog.Tools.Hasher
{
    public interface IHasher
    {
        string ToHash(string value);
    }
}