namespace TheatricalPlayersRefactoringKata.Domain.Models.Genres
{
    public class TragedyGenre(int perfAudience, int playLines) : Genre(perfAudience, playLines)
    {
        public override decimal CalculateAmount()
        {
            const int audienceThreshold = 30;
            const int additionalViewerCost = 10;

            var extraAudience = Math.Max(0, PerformanceAudience - audienceThreshold);
            var additionalAmount = extraAudience * additionalViewerCost;

            return BaseAmount + additionalAmount;
        }
    }
}
