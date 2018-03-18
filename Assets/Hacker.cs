using UnityEngine;

public class Hacker : MonoBehaviour {
    const int startingLives = 3;
    int currentLives;
    int level; // Game state
    string password;
    enum Screen { MainMenu, Password, Win, Lose };
    Screen currentScreen;
    string[][] availablePasswords = new string[][] {
        new string[] {"red", "purple", "yellow", "pink", "black"},
        new string[] {"teacher", "student", "chef", "programmer", "officer", "firefighter"},
        new string[] {"ChickenWings", "spaghety", "pancakes", "cheesecake", "bolognese"}
    };

	// Use this for initialization
	void Start ()
    {
        ShowMainMenu();
    }

    void ShowMainMenu()
    {
        currentScreen = Screen.MainMenu;
        level = 0;
        password = null;
        currentLives = startingLives;

        // Clear Screen
        Terminal.ClearScreen();

        // Menu
        Terminal.WriteLine("Let's hack some words, please select a topic.");
        Terminal.WriteLine("\n");
        Terminal.WriteLine("Press 1 for the Colors");
        Terminal.WriteLine("Press 2 for the Jobs");
        Terminal.WriteLine("Press 3 for the Food");
        Terminal.WriteLine("Enter your selection:");
    }
    
    void OnUserInput(string input)
    {
        if (input == "menu") {
            ShowMainMenu();
        } else {
            switch (currentScreen)
            {
                case Screen.MainMenu:
                    RunMainMenu(input);
                    break;
                case Screen.Password:
                    CheckPassword(input);
                    break;
                case Screen.Win:
                case Screen.Lose:
                    ShowMainMenu();
                    break;
                default:
                    Debug.LogError("Invalid Screen reached! Screen: " + currentScreen);
                    break;
            }
        }
    }

    void RunMainMenu(string input)
    {
        switch (input)
        {
            case "1":
            case "2":
            case "3":
                OnLevelChoose(input);
                break;
            default:
                Terminal.WriteLine("Please choose a valid level:");
                break;
        }
    }

    void CheckPassword(string input)
    {
        if (input == password)
        {
            DisplayWinScreen();
        }
        else
        {
            WrongPassword();
        }
    }

    private void WrongPassword()
    {
        currentLives--;
        bool hasLive = (currentLives != 0);
        if (hasLive)
        {
            AskForPassword();
        }
        else
        {
            YouLoose();
        }
    }

    private void YouLoose()
    {
        currentScreen = Screen.Lose;
        Terminal.WriteLine("You lose :/");
    }

    private void OnLevelChoose(string input)
    {
        // set current level and it's index
        level = int.Parse(input);

        // Set scene for password guessing
        StartGame();
    }

    private void SetRandomPassword()
    {
        string[] levelPasswords = availablePasswords[level - 1];
        int passwordIndex = Random.Range(0, levelPasswords.Length);
        password = levelPasswords[passwordIndex];
    }

    void StartGame()
    {
        currentScreen = Screen.Password;
        AskForPassword();
    }

    void AskForPassword()
    {
        Terminal.ClearScreen();
        SetRandomPassword();
        Terminal.WriteLine("Enter your password, hint: " + password.Anagram());
        Terminal.WriteLine("You have " + currentLives + " of " + startingLives + " lives");
    }

    void DisplayWinScreen()
    {
        currentScreen = Screen.Win;
        Terminal.ClearScreen();
        ShowLevelReward();
    }

    void ShowLevelReward()
    {
        Terminal.WriteLine("You Win");
        switch (level)
        {
            case 1:
                Terminal.WriteLine(@"
    _..-'      `Y`      '-._
    \ Once upon | Level 1  /
    \\  a time..|    ;-)   //
    \\\         |         ///
     \\\ _..-- -.|.-- -.._///
      \\`_..-- -.Y.-- -.._`//
");
                break;
            case 2:
                Terminal.WriteLine(@"
    (
     \
      )
 ##-------->  Level 2
      )
     /
    (

");
                break;
            case 3:
                Terminal.WriteLine(@" 
            _.-'`'-._
         .-'    _    '-.
          `-.__  `\_.-'
            |  `-``\|
  Level 3   `-.....-A
                    #
                    #
");
                break;
            default:
                Debug.LogError("Invalid level reached");
                break;
        }
    }
}
