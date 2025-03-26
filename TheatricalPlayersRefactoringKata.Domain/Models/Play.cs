using TheatricalPlayersRefactoringKata.Domain.Enums;

namespace TheatricalPlayersRefactoringKata.Domain.Models;

public class Play
{
    private string _name;
    private int _lines;
    private GenreEnum _type;

    public string Name { get => _name; set => _name = value; }
    public int Lines { get => _lines; set => _lines = value; }
    public GenreEnum Type { get => _type; set => _type = value; }

    public Play(string name, int lines, GenreEnum type) {
        _name = name;
        _lines = lines;
        _type = type;
    }
}
