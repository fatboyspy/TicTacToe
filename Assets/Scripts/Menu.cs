using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс-скрипт для работы с главным меню
/// </summary>
public class Menu : MonoBehaviour {
    /// <summary>
    /// Кнопка запуска игры
    /// </summary>
    [SerializeField]
    Button play;
    /// <summary>
    /// Комбобокс для регулирования сложности игры
    /// </summary>
    [SerializeField]
    Dropdown difficulty;
    /// <summary>
    /// Комбобокс для установления режима игры
    /// </summary>
    [SerializeField]
    Dropdown mode;
    /// <summary>
    /// Ну и собственно выход из игры
    /// </summary>
    [SerializeField]
    Button exit;

    /// <summary>
    /// Тут все начинается
    /// </summary>
    void Start () {
        //тянем значение для уровня сложности игры из глобального класса
        difficulty.value = (DataAdapter.GameData.difficulty!=null)?(int)DataAdapter.GameData.difficulty:difficulty.value;
        //пытаемся вытянуть также и режим игры
        mode.value = (DataAdapter.GameData.enemyType != null) ? (int)DataAdapter.GameData.enemyType : mode.value;
    }
    /// <summary>
    /// Обработчик нажатия на кнопку начала игры
    /// </summary>
    public void Play()
    {
        //перебираем значения уровня сложности
        switch(difficulty.value)
        {
            case 0:DataAdapter.GameData.difficulty = GameDifficulty.Easy;break;
            case 1:DataAdapter.GameData.difficulty = GameDifficulty.Medium;break;
            case 2: DataAdapter.GameData.difficulty = GameDifficulty.Hard;break;
        }
        //перебираем значения для режима игры
        switch(mode.value)
        {
            case 0:DataAdapter.GameData.enemyType = PlayerType.Human;break;
            case 1:DataAdapter.GameData.enemyType = PlayerType.AI;break;
        }
        //загружаем игровую сцену
        SceneManager.LoadScene("game");
        
    }

    /// <summary>
    /// Обработчик кнопки "Выход"
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
