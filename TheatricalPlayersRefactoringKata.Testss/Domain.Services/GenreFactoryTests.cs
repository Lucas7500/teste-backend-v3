using TheatricalPlayersRefactoringKata.Domain.Models.Genres;
using TheatricalPlayersRefactoringKata.Domain.Services.Factories;

namespace TheatricalPlayersRefactoringKata.Tests.Domain.Services
{
    public class GenreFactoryTests
    {
        [Theory]
        [InlineData("tragedy", typeof(TragedyGenre))]
        [InlineData("comedy", typeof(ComedyGenre))]
        [InlineData("history", typeof(HistoryGenre))]
        public void Create_GivenValidGenreType_ReturnsCorrectGenre(string genreType, Type expectedType)
        {
            const int audience = 100;
            const int lines = 20;

            var genre = GenreFactory.Create(genreType, audience, lines);

            Assert.IsType(expectedType, genre);
        }

        [Fact]
        public void Create_GivenUnknownGenreType_ThrowsException()
        {
            const int audience = 100;
            const int lines = 20;
            string unknownGenre = "unknown";

            var exception = Assert.Throws<Exception>(() => GenreFactory.Create(unknownGenre, audience, lines));
            Assert.Equal("Unknown genre: unknown", exception.Message);
        }
    }
}
