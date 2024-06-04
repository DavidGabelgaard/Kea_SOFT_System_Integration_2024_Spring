using System.Text;

namespace StringExtensions
{
    public static class StringExtensions
    {
        public static ArraySegment<byte> ToArraySegment(this string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            return new ArraySegment<byte>(bytes, 0, bytes.Length);
        }
    }
}
