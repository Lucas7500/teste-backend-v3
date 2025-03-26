namespace TheatricalPlayersRefactoringKata.Domain.Models.Genres
{
    public class HistoryGenre(int perfAudience, int playLines) : Genre(perfAudience, playLines)
    {
        public override decimal CalculateAmount()
        {
            var tragedyGenre = new TragedyGenre(PerformanceAudience, PlayLines);
            var comedyGenre = new ComedyGenre(PerformanceAudience, PlayLines);

            return tragedyGenre.CalculateAmount() + comedyGenre.CalculateAmount();
        }
    }
}
