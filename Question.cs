namespace quizapi;

public class Question
{
    public Question(string? category, string? correctAnswer, string[]? incorrectAnswers, string? text)
    {
        Category = category;
        CorrectAnswer = correctAnswer;
        IncorrectAnswers = incorrectAnswers;
        Text = text;
    }

    public string? Category { get; set; }

    public string? CorrectAnswer { get; set; }

    public string[]? IncorrectAnswers { get; set; }

    public string Difficulty
    {
        get
        {
            return "normal";
        }
    }

    public string? Text { get; set; }
    public string? Type 
    { 
        get
        {
            return "multiple";
        }
    }
}
