using TheatricalPlayersRefactoringKata.API.Enums;
using TheatricalPlayersRefactoringKata.Domain.Models;

namespace TheatricalPlayersRefactoringKata.API.Requests
{
    public class StatementRequest
    {
        public string? Id { get; set; }
        public required Invoice Invoice { get; set; }
        public required Dictionary<string, Play> Plays { get; set; }
        public required string Format { get; set; }
        public RequestStatusEnum Status { get; set; } = RequestStatusEnum.Pending;
        public string? ErrorMessage { get; set; }
    }
}
