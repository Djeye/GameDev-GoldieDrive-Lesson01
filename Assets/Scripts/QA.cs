using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QA : MonoBehaviour
{
    [SerializeField]
    private GameObject qapanel;
    TextMeshProUGUI qText;
    List<Button> buttons = new List<Button>();
    List<TextMeshProUGUI> answerText = new List<TextMeshProUGUI>();

    private string[] questions = new string[3] {
        "Можно ли Вам въехать на мост первым?",
        "Разрешен ли вам выезд на дорогу с асфальтированным покрытием?",
        "Вы намерены повернуть налево. Кому вы должны уступить дорогу?" };
    private string[,] answers = new string[3, 4] {
        { "Можно, если вы не затрудните въезд встречному автомобилю", "Можно", "Нельзя", "3"},
        { "Разрешен", "Разрешен только при технической неисправности транспортного средства", "Запрещен" , "1"},
        { "Только пешеходам", "Только автобусу", "Автобусу и пешеходам", "3"}};


    // Start is called before the first frame update

    private void Start()
    {
        qText = qapanel.transform.Find("Questions").GetComponent<TextMeshProUGUI>();
        buttons.Add(qapanel.transform.Find("Answer_1").GetComponent<Button>());
        buttons.Add(qapanel.transform.Find("Answer_2").GetComponent<Button>());
        buttons.Add(qapanel.transform.Find("Answer_3").GetComponent<Button>());

        answerText.Add(buttons[0].GetComponentInChildren<TextMeshProUGUI>());
        answerText.Add(buttons[1].GetComponentInChildren<TextMeshProUGUI>());
        answerText.Add(buttons[2].GetComponentInChildren<TextMeshProUGUI>());

        qapanel.SetActive(false);
    }

    public void showQA(string name)
    {
        qapanel.SetActive(true);
        int i = int.Parse(name.Substring(name.Length - 1)) - 1;
        
        qText.text = questions[i];
        for (int j=0; j < 3; j++)
        {
            answerText[j].text = answers[i, j];
            buttons[j].GetComponent<Answer>().setCorrectAnswer(false);
        }

        buttons[int.Parse(answers[i, 3])-1].GetComponent<Answer>().setCorrectAnswer(true);
        
    }
}
