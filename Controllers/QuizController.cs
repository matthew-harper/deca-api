using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Microsoft.AspNetCore.Mvc;

namespace quizapi.Controllers;

[ApiController]
[Route("[controller]")]
public class QuizController : ControllerBase
{
    private static readonly Question[] question_db = new[]
    {
        new Question("Marketing", "Sole proprietorship", new[] 
            { 
                "Merger", 
                "Partnership", 
                "Corporation"
            }, 
            "After only her seventh month in business, Martha realized that her new business venture was paying off, and she would be taking home a $1,000 profit for the month. Finally, she could reap the financial rewards of being the boss. This is an advantage of what type of business ownership?"),
        new Question("Marketing", "A bibliography", new[] 
            {
                "A table of contents", 
                "A professional-looking website design", 
                "A date of modification less than one year old"
            }, 
            "For her new job, Tasha has been asked to write a report. She has found a website with information regarding her topic. Which of the following would help her identify whether the information on this site is accurate:?"),
        new Question("Marketing", "Read step one and then perform step one. Repeat this process in sequential order until all of the steps are completed and you have removed the paper. ", new[] 
            {
                "Perform the first step, and then verify that you have performed it correctly in the manual. Repeat this process in sequential order until all the steps are completed. ",
                "Read the entire manual in sequential order and then perform all of the steps in reverse order toremove the jammed paper. ",
                "Briefly skim the manual in reverse order and then follow the steps in sequential order to remove the jammed paper."
            }, 
            "The office photocopier is jammed again! Fortunately, there is an instruction manual to guide you through the process of removing the jammed paper. What is the best way to use the manual to clear the photocopier?"),
        new Question("Marketing", "Past Experiences", new[]
            {
                "Attention span",
                "Age differences",
                "Language skills"
            },
            "The employee didn't listen to the manager's explanation about how to perform a certain task because s/he had done a similar job before. Which of the following factors caused the employee not to listen effectively?"),
        new Question("Marketing", "Make comments such as “yes” or “I see” occasionally", new[]
            {
                "Avoid making any noise while the speaker is talking",
                "Interrupt the speaker to debate what s/he has said ",
                "Clap as the speaker makes each of her/his main points "
            },
            "What is an effective way to support and encourage someone who is talking to you?"),
        new Question("Marketing", "Tone of voice", new[]
            {
                "Tempo",
                "Accuracy",
                "Economy of speech"
            },
            "Which of the following adds meaning to the words a speaker uses?"),
        new Question("Marketing", "Anger", new[]
            {
                "Sadness",
                "Joy",
                "Nervousness"
            },
            "If a message recipient is frowning and has clenched fists, the emotional response that s/he is most likely displaying is ?"),
        new Question("Marketing", "Giving directions out of order", new[]
            {
                "Giving directions that are too challenging ",
                "Using too many big words",
                "Using too much negative language"
            },
            "Ashley is helping Laia make soup. Ashley says, “Chop up your carrots. But first, find your cutting board. And before that, make sure you have all your ingredients.” What mistake is Ashley making in giving directions?"),
        new Question("Marketing", "Critical feedback ", new[]
            {
                "Nonverbal support ",
                "Additional responsibility ",
                "Personal attack"
            },
            "What should employees be willing to accept when defending their ideas objectively?"),
        new Question("Marketing", "File the information", new[]
            {
                "Contact a supervisor",
                "Write an inquiry ",
                "Thank the caller"
            },
            "What should a business employee do immediately after taking a telephone message for a coworker?"),
        new Question("Marketing", "organize information", new[]
            {
                "revise facts",
                "access files",
                "verify accuracy"
            },
            "Writing key points on notecards and then placing the cards in order of their importance is one way to?")
    };

    private readonly ILogger<QuizController> _logger;

    public QuizController(ILogger<QuizController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetQuiz")]
    public Quiz Get()
    {
        var awsCredentials = new BasicAWSCredentials("AKIARODRDWF3ZP6GPSQ4", "LFK+yTJejcxt4b1HtGcUNHtY2sgZpS8OBoKW9LFN");
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(awsCredentials, Amazon.RegionEndpoint.USEast1);
        var request = new QueryRequest
        {
            TableName = "deca-questions",
            KeyConditionExpression = "category = :v_Cat",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
        {":v_Cat", new AttributeValue { S =  "Marketing" }}}
        };

        var response = client.QueryAsync(request).Result;

        List<Question> allquestions = new List<Question>();
        foreach (Dictionary<string, AttributeValue> item in response.Items)
        {
            // Process the result.
            PrintItem(item);
            allquestions.Add(new Question(item["category"].S, item["correctanswer"].S, item["incorrectanswers"].SS.ToArray(), item["text"].S));
        }

        Random random = new Random(DateTime.Now.Millisecond);
        var selected_questions = allquestions.OrderBy(x => random.Next()).Take(1);

        return new Quiz("Marketing", selected_questions.ToArray());
    }

    private static void PrintItem(
            Dictionary<string, AttributeValue> attributeList)
    {
        foreach (KeyValuePair<string, AttributeValue> kvp in attributeList)
        {
            string attributeName = kvp.Key;
            AttributeValue value = kvp.Value;

            Console.WriteLine(
                attributeName + " " +
                (value.S == null ? "" : "S=[" + value.S + "]") +
                (value.N == null ? "" : "N=[" + value.N + "]") +
                (value.SS == null ? "" : "SS=[" + string.Join(",", value.SS.ToArray()) + "]") +
                (value.NS == null ? "" : "NS=[" + string.Join(",", value.NS.ToArray()) + "]")
                );
        }
        Console.WriteLine("************************************************");
    }

}
