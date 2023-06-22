[System.Serializable]

public class QuestionHandler
{
    public string Question;
    public string[] Answers;
    public int CorrectAnswer;

    public QuestionHandler(string Question, string[] Answers, int CorrectAnswer) {
        string[] Anw = new string[Answers.Length];
        this.Question = Question;
        for(int i=0; i<Answers.Length; i++) {
            Anw[i] = Answers[i];
        }
        this.Answers = Anw;
        this.CorrectAnswer = CorrectAnswer;

    }

    public QuestionHandler Clone() {
        return new QuestionHandler(Question, Answers, CorrectAnswer);

    }
   
}

