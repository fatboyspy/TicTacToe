using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Класс игровой модели
/// </summary>
public class GameModel : StateMachine
{
    /// <summary>
    /// Текущее состояние модели
    /// </summary>
    GameState currentState;
    /// <summary>
    /// Текущий игрок
    /// </summary>
    Player currentPlayer;
    /// <summary>
    /// Количество игровых клеток
    /// </summary>
    const int fieldsCount=9;
    /// <summary>
    /// Игровое поле
    /// </summary>
    [SerializeField]
    public List<string> gameBoard;
    /// <summary>
    /// Коллекция для хранения игровых выигрышных комбинаций
    /// </summary>
    [SerializeField]
    public List<List<int>> winsCombinations = new List<List<int>>();
    /// <summary>
    /// Первый игрок
    /// </summary>
    [SerializeField]
    Player firstPlayer;
    /// <summary>
    /// Второй игрок
    /// </summary>
    [SerializeField]
    Player secondPlayer;
    /// <summary>
    /// Игровая сложность
    /// </summary>
    [SerializeField]
    GameDifficulty difficulty;
    /// <summary>
    /// Количество игр сыгранных в ничью
    /// </summary>
    int deadheat = 0;
    #region Свойства для полей

    public Player CurrentPlayer
    {
        get
        {
            return currentPlayer;
        }

        set
        {
            currentPlayer = value;
        }
    }

    public Player FirstPlayer
    {
        get
        {
            return firstPlayer;
        }

        set
        {
            firstPlayer = value;
        }
    }

    public Player SecondPlayer
    {
        get
        {
            return secondPlayer;
        }

        set
        {
            secondPlayer = value;
        }
    }

    public int Deadheat
    {
        get
        {
            return deadheat;
        }

        set
        {
            deadheat = value;
        }
    } 
    #endregion
    /// <summary>
    /// Конструктор Игровой модели
    /// </summary>
    /// <param name="fPlayer">Первый игрок</param>
    /// <param name="sPlayer">Второй игрок</param>
    /// <param name="difficulty">Сложность игры</param>
    public GameModel( Player fPlayer, Player sPlayer, GameDifficulty difficulty)
    {
        #region Заполняем игровое поле
        gameBoard = new List<string>(fieldsCount);
        for (int i = 0; i < fieldsCount; i++)
            gameBoard.Add("");
        #endregion

        #region Заполняем возможные выигрышные комбинации
        winsCombinations.Add(new List<int>() { 0, 1, 2 });
        winsCombinations.Add(new List<int>() { 3, 4, 5 });
        winsCombinations.Add(new List<int>() { 6, 7, 8 });
        winsCombinations.Add(new List<int>() { 0, 3, 6 });
        winsCombinations.Add(new List<int>() { 1, 4, 7 });
        winsCombinations.Add(new List<int>() { 2, 5, 8 });
        winsCombinations.Add(new List<int>() { 0, 4, 8 });
        winsCombinations.Add(new List<int>() { 2, 4, 6 }); 
        #endregion

        //очищаем игровое поле
        //на всякий случай
        ClearBord();
        //прописываем игроков
        FirstPlayer = fPlayer;
        SecondPlayer = sPlayer;
        //текущий игрок у нас первый
        currentPlayer = FirstPlayer;
        currentState = GameState.FirstPlayerTurn;
        //задаем сложность игры
        this.difficulty = difficulty;
    }
    /// <summary>
    /// Метод для очистки игрового поля
    /// </summary>
    public void ClearBord()
    {
        for (int i = 0; i < gameBoard.Count; i++)
            gameBoard[i] = "";     
    }
    /// <summary>
    /// Метод для проверки игрового поля
    /// </summary>
    /// <returns>Текущее состояние игры</returns>
    public GameState CheckBoard()
    {
        //пробегаемся по всем выигрышным комбинациям
        for (int i = 0; i < winsCombinations.Count; i++)
        {
            int num = 0;
            //подсчитываем количество одинаково заполненных ячеек
            //значения в которых соответствуют значениям для текущего игрока
            num= winsCombinations[i].Where(w => gameBoard[w] == currentPlayer.CellValue).Count();
            //если их - 3
            if (num == 3)
                //если текущий игрок у нас первый то возвращаем статус "Победил первый" если нет то "Победил второй"
                return (FirstPlayer==currentPlayer)?GameState.FirstPlayerWin:GameState.SecondPlayerWin; 
        }
        //если мы дошли сюда и все ячейки игрового поля заполнены
        if (gameBoard.Where(gb => string.IsNullOrEmpty(gb)).Count() == 0)
            //значит у нас "Ничья"
            return GameState.DeadHeat;
        //если мы дошли сюда и у нас еще ничего не заполнено
        if (gameBoard.Where(gb => !string.IsNullOrEmpty(gb)).Count() == 0)
            //если текущий игрок у нас первый то возвращаем статус "Ходит первый" если нет то "Ходит второй"
            return (FirstPlayer == currentPlayer) ? GameState.FirstPlayerTurn : GameState.SecondPlayerTurn;
        //ну и наконец, если мы дошли сюда и текущий игрок у нас первый то возвращаем статус "Ходит второй" если нет то"Ходит первый" 
        return (FirstPlayer == currentPlayer) ? GameState.SecondPlayerTurn : GameState.FirstPlayerTurn;
    }
    /// <summary>
    /// Метод для получения текущего статуса игры
    /// </summary>
    /// <returns>Текущий статус игры</returns>
    public GameState GetState()
    {
        return currentState;
    }
    /// <summary>
    /// Метод для осуществления хода
    /// </summary>
    /// <param name="fieldIndex">Номер ячейки для хода</param>
    public void TakeTurn(int fieldIndex)
    {
        //записываем значение ячейки
        gameBoard[fieldIndex] = currentPlayer.CellValue;
        //переключаем состояние
        ToggleState();
        //если текущий игрок АИ и состояние игры не равно ни одному из перечисленных
        if (currentPlayer.Type == PlayerType.AI && currentState != GameState.DeadHeat && currentState != GameState.FirstPlayerWin && currentState != GameState.SecondPlayerWin)
        {
            //Ходит АИ
            AITurn();
        }
    }
    /// <summary>
    /// Метод осуществления хода АИ
    /// </summary>
    public void AITurn()
    {
        //проверяем уровень сложности
        switch (difficulty)
        {
            //если простой
            case GameDifficulty.Easy:
                List<int> indexOfEmpty = new List<int>();
                //тянм все индексы ячеек которые пустые
                for (int i = 0; i < gameBoard.Count; i++)
                {
                    if (string.IsNullOrEmpty(gameBoard[i]))
                    {
                        indexOfEmpty.Add(i);
                    }
                }
                //делаем рандомный ход
                TakeTurn(Random.Range(0, indexOfEmpty.Count - 1));
                break;
                //если средний
            case GameDifficulty.Medium:
                //выполняем поиск вариантов
                AISeek(2);
                break;
                //если тяжелый
            case GameDifficulty.Hard:
                //выполняем поиск вариантов с опцией пробы блокировки ходов противника
                AISeek(2, true);
                break;
        }
        
    }
    /// <summary>
    /// Метод поиска вариантов для ходов АИ
    /// </summary>
    /// <param name="friendlyCount">Минимум ячеек для контроля</param>
    /// <param name="seekEnemy">Блокировать ли противника</param>
    private void AISeek(int friendlyCount, bool seekEnemy = false)
    {
        //если пустой центр и ходов еще не было
        if (string.IsNullOrEmpty(gameBoard[4]) && gameBoard.Where(gb => !string.IsNullOrEmpty(gb)).Count() == 0)
        {
            //занимаем центр
            TakeTurn(4);
        }
        else
        {
            //если нужно блокировать противника
            if (seekEnemy)
            {
                //пробегаемся по всем комбинацям
                for (int i = 0; i < winsCombinations.Count; i++)
                {
                    //получаем список ячеек противника
                    List<int> enemyIndexes = winsCombinations[i].Where(w => !string.IsNullOrEmpty(gameBoard[w]) && gameBoard[w] != currentPlayer.CellValue).ToList<int>();
                    //получаем список своих ячеек
                    List<int> friendlyIndexes = winsCombinations[i].Where(w => gameBoard[w] == currentPlayer.CellValue).ToList<int>();
                    //получаем список пустых ячеек
                    List<int> emptyIndexes = winsCombinations[i].Where(w => string.IsNullOrEmpty(gameBoard[w])).ToList<int>();
                    //если количество вражеских ячеек больше или равно контрольным и есть пустые ячейки
                    if (enemyIndexes.Count >= friendlyCount && emptyIndexes.Count > 0)
                    {
                        //делаем ход в первую пустую
                        TakeTurn(emptyIndexes.FirstOrDefault());
                        break;
                    }
                }
            }
            else
            {
                //пробегаемся по всем комбинацям
                for (int i = 0; i < winsCombinations.Count; i++)
                {

                    //получаем список ячеек противника
                    List<int> enemyIndexes = winsCombinations[i].Where(w => !string.IsNullOrEmpty(gameBoard[w]) && gameBoard[w] != currentPlayer.CellValue).ToList<int>();
                    //получаем список своих ячеек
                    List<int> friendlyIndexes = winsCombinations[i].Where(w => gameBoard[w] == currentPlayer.CellValue).ToList<int>();
                    //получаем список пустых ячеек
                    List<int> emptyIndexes = winsCombinations[i].Where(w => string.IsNullOrEmpty(gameBoard[w])).ToList<int>();
                    //если дружеских больше или равно контрольным и (враждебных ячеек нет или на поле только одна ячейка свободна) и есть пустые
                    if (friendlyIndexes.Count >= friendlyCount && (enemyIndexes.Count == 0 || gameBoard.Where(gb => string.IsNullOrEmpty(gb)).ToList<string>().Count == 1) && emptyIndexes.Count > 0)
                    {
                        //если дружеских 2
                        if (friendlyIndexes.Count == 2)
                        {
                            //и они идут подряд
                            if (winsCombinations[i][winsCombinations[i].IndexOf(friendlyIndexes[0]) + 1] == friendlyIndexes[1])
                            {
                                TakeTurn(emptyIndexes.FirstOrDefault());
                                break;
                            }
                        }
                        //если меньше 2
                        if (friendlyIndexes.Count < 2)
                        {
                            TakeTurn(emptyIndexes.FirstOrDefault());
                            break;
                        }
                    }
                }
            }
            //если текущий пользователь все еще АИ и игра не окончена
            if (currentPlayer.Type == PlayerType.AI && currentState != GameState.FirstPlayerWin && currentState != GameState.SecondPlayerWin)
                //рекурсивно вызываем еще раз сами себя
                AISeek(friendlyCount - 1, seekEnemy);
        }
    }
    /// <summary>
    /// Метод для переключения состояния игры
    /// </summary>
    public void ToggleState()
    {
        //проверяем игровое поле
        currentState = CheckBoard();
        switch (currentState)
        {
            case GameState.FirstPlayerTurn:
                currentPlayer = FirstPlayer;
                break;
            case GameState.SecondPlayerTurn:
                currentPlayer = SecondPlayer;
                break;
                //если ничья
            case GameState.DeadHeat:
                //меняем значения текущего игрока на противоположное
                currentPlayer = (FirstPlayer.CellValue == "X") ? SecondPlayer : FirstPlayer;
                Deadheat++;
                break;
            case GameState.FirstPlayerWin:
                FirstPlayer.WinsCount++;
                SecondPlayer.LosesCount++;
                break;
            case GameState.SecondPlayerWin:
                FirstPlayer.LosesCount++;
                SecondPlayer.WinsCount++;
                break;
        }
    }
    /// <summary>
    /// Новая игра
    /// </summary>
    public void NewGame()
    {
        //очищаем поле
        ClearBord();
        //меняем местами значения ячеек для игроков, если необходимо
        FirstPlayer.CellValue = (currentPlayer == SecondPlayer) ? "O" : "X";
        SecondPlayer.CellValue = (currentPlayer == SecondPlayer) ? "X" : "O";

    }
    /// <summary>
    /// Метод для изменения сложности игры
    /// </summary>
    /// <param name="newDifficulty"></param>
    public void ChangeDifficulty(GameDifficulty newDifficulty)
    {
        difficulty = newDifficulty;
    }

    
}
public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    }