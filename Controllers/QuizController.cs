using Microsoft.AspNetCore.Mvc;

namespace quizapi.Controllers;

[ApiController]
[Route("[controller]")]
public class QuizController : ControllerBase
{
    private static readonly Question[] question_db = new[]
    {
        new Question("Marketing", "Two", new[] {"one", "three", "four"}, "How many legs do you have?"),
        new Question("Marketing", "New Mexico", new[] {"Nigeria", "Hong Kong", "Texas"}, "Where was Scout born?"),
        new Question("Marketing", "Alexus", new[] {"Dad", "Mom", "Blake"}, "Who is the best?")
    };

    private readonly ILogger<QuizController> _logger;

    public QuizController(ILogger<QuizController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetQuiz")]
    public Quiz Get()
    {
        Random random = new Random(DateTime.Now.Millisecond);
        var selected_questions = question_db.OrderBy(x => random.Next()).Take(2);

        return new Quiz("Marketing", selected_questions.ToArray());
    }
}
