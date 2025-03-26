using System.Text;

namespace TheatricalPlayersRefactoringKata.API.Extensions
{
    public static class FileExtensions
    {
        public static MemoryStream ToFile(this string content)
        {
            return new(Encoding.UTF8.GetBytes(content));
        }
    }
}
