[System.Serializable]

//Classe usata per la questione delle domande del quiz

public class QuestionHandler
{
    //Variabili 
    public string Question;
    public string[] Answers;
    public int CorrectAnswer;

    //Costruttore
    public QuestionHandler(string Question, string[] Answers, int CorrectAnswer) {

        // Creo un nuovo array di stringhe "Anw" con la stessa lunghezza di "Answers".
        string[] Anw = new string[Answers.Length];
        this.Question = Question;
        for(int i=0; i<Answers.Length; i++) {
            Anw[i] = Answers[i];
        }
        // Assegno l'array "Anw" all'attributo "Answers" dell'oggetto "QuestionHandler".
        this.Answers = Anw;
        this.CorrectAnswer = CorrectAnswer;

    }

    //Funzione che restituisce una copia dell'oggetto "QuestionHandler" corrente.
    public QuestionHandler Clone() {
        return new QuestionHandler(Question, Answers, CorrectAnswer);

    }
   
}

