using TheatricalPlayersRefactoringKata.Domain.Models.Genres;

namespace TheatricalPlayersRefactoringKata.Domain.Services.Factories
{
    public static class GenreFactory
    {
        public static Genre Create(string type, int perfAudience, int playLines)
        {
            return type switch
            {
                "tragedy" => new TragedyGenre(perfAudience, playLines),
                "comedy" => new ComedyGenre(perfAudience, playLines),
                "history" => new HistoryGenre(perfAudience, playLines),
                _ => throw new Exception(string.Format("Unknown genre: {0}", type)),
            };
        }
    }
}
