using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : MonoBehaviour
{
    private bool isCorrect = false;
    [SerializeField] private GameObject qapanel;
    [SerializeField] private CarController car;

    [SerializeField] GameObject error;
    [SerializeField] GameObject approve;

    public void setCorrectAnswer(bool correct)
    {
        isCorrect = correct;
    }

    public void checkCorrect()
    {
        if (isCorrect)
        {
            approve.GetComponent<AudioSource>().Play();

            car.updateRestartPosition();
            switch (car.getLastCheckPointName())
            {
                case "Question_1":
                    car.showQuest("Пропустите грузовик, идущий впереди");
                    TruckSpawner.available = true;
                    break;
                case "Question_2":
                    car.showQuest("Поверните направо. Направляйтесь в город");
                    break;
                case "Question_3":
                    car.showQuest("Поверните налево");
                    PeopleSpawnerScript.available = true;
                    break;
            }
        }
        else
        {
            error.GetComponent<AudioSource>().Play();
            car.showQuest("Неверно. Попробуйте еще раз!");
            car.Restart();
        }

        car.setStopeed(false);
        qapanel.SetActive(false);
    }
}
