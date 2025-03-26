using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Models;
using TheatricalPlayersRefactoringKata.Domain.Models.Genres;

namespace TheatricalPlayersRefactoringKata.Domain.Services.Factories
{
    public static class GenreFactory
    {
        public static Genre Create(GenreEnum genre, int perfAudience, int playLines)
        {
            return genre switch
            {
                GenreEnum.Tragedy => new TragedyGenre(perfAudience, playLines),
                GenreEnum.Comedy => new ComedyGenre(perfAudience, playLines),
                GenreEnum.History => new HistoryGenre(perfAudience, playLines),
                _ => throw new Exception(string.Format("Unknown genre: {0}", genre.ToString())),
            };
        }
    }
}
