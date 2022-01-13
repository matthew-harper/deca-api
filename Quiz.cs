namespace quizapi;

public class Quiz
{
    public Quiz(string? category, Question[]? questions)
    {
        Category = category;
        Questions = questions;
    }
    public string? Category { get; set; }

    public Question[]? Questions{ get; set; }

}
