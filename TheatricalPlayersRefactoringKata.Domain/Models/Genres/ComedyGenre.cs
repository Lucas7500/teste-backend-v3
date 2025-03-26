namespace TheatricalPlayersRefactoringKata.Domain.Models.Genres
{
    public class ComedyGenre(int perfAudience, int playLines) : Genre(perfAudience, playLines)
    {
        public override decimal CalculateAmount()
        {
            const int audienceThreshold = 20;
            const int viewerCost = 3;
            const int extraViewerCost = 5;

            var amount = BaseAmount + (viewerCost * PerformanceAudience);
            var extraAudience = Math.Max(0, PerformanceAudience - audienceThreshold);

            if (extraAudience > 0)
            {
                amount += (decimal)(extraViewerCost * extraAudience) + 100;
            }

            return amount;
        }

        public override int CalculateCredit()
        {
            var baseCredit = base.CalculateCredit();
            var additionalCredit = (int)Math.Floor((decimal)PerformanceAudience / 5);

            return baseCredit + additionalCredit;
        }
    }
}
