using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{

}

public enum GameState
{
    Loading = 0,
    Playing = 1,
    Win = 2,
    Lose = 3,
    Pause = 4
}

public class GameplayController : Singleton<GameplayController>
{
    public Level level;
    public BoardUserInput boardUserInput;
    public GameplayUIController gameplayUIController;
    public TutorialController tutorialController;
    public HandTutorialController handTutorialController;
    public CatHomeController catHomeController;

    public GameState gameState;

    public List<int> levelNeedTutorials;
    protected override void OnAwake()
    {
        GameController.Instance.currentScene = SceneType.GamePlay;
        GameplayController.Instance.gameState = GameState.Playing;
        Init();
    }

    private void Start()
    {
        levelNeedTutorials = new List<int>() { 1, 2, 7, 15, 30, 45, 60};
        level.board.SetupTutorial();
        if (!CheckLevelInListTutorial(UseProfile.CurrentLevel))
        {
            level.board.SetupInitialItemPopup();
        }
        else
        {
            level.board.initialItemPopupContainer.GetComponent<Image>().enabled = false;
        }
    }

    public void Init()
    {
        level.Init();
        boardUserInput.Initialize();
        //gameplayUIController.Initialize();
    }

    public bool CheckLevelInListTutorial(int level)
    {
        foreach(var i in levelNeedTutorials)
        {
            if(i == level)
            {
                return true;
            }
        }
        return false;
    }
}

