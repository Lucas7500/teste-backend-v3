namespace TheatricalPlayersRefactoringKata.Domain.Models.Genres
{
    public abstract class Genre
    {
        protected int PerformanceAudience { get; }
        protected int PlayLines { get; }
        protected decimal BaseAmount { get; }

        public Genre(int perfAudience, int playLines)
        {
            PerformanceAudience = perfAudience;
            PlayLines = Math.Clamp(playLines, 1000, 4000);
            BaseAmount = (decimal)PlayLines / 10;
        }

        public abstract decimal CalculateAmount();

        public virtual int CalculateCredit()
        {
            const int audienceThreshold = 30;
            const int creditPerExtraViewer = 1;

            var extraAudience = Math.Max(0, PerformanceAudience - audienceThreshold);

            return extraAudience > 0
                ? creditPerExtraViewer * extraAudience
                : 0;
        }
    }
}
