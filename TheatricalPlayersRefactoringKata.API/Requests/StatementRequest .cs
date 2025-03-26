﻿using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Models;

namespace TheatricalPlayersRefactoringKata.API.Requests
{
    public record StatementRequest(Invoice Invoice, Dictionary<string, Play> Plays, string Format);
}
