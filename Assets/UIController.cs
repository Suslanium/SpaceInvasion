using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

public class HeroComparer : IEqualityComparer<Hero>
{
    public bool Equals(Hero x, Hero y)
    {
        return
            x.Name == y.Name &&
            x.HealthPoints == y.HealthPoints &&
            x.ArmorPoints == y.ArmorPoints &&
            x.DamageMultiplier.Equals(y.DamageMultiplier) &&
            x.MovementSpeedMultiplier.Equals(y.MovementSpeedMultiplier) &&
            x.DominantAbility == y.DominantAbility &&
            x.RecessiveAbility == y.RecessiveAbility;
    }

    public int GetHashCode(Hero obj)
    {
        return obj.GetHashCode();
    }
}

public class UIController : MonoBehaviour
{
    public Label TankHeroStats;
    public Label FighterHeroStats;
    public Label MarksmanHeroStats;
    public Label ChosenHeroesLabel;
    public Button CrossFighterHeroButton;
    public Button CrossTankHeroButton;
    public Button CrossMarksmanHeroButton;
    public Button PlayButton;

    private TankHero _tankHero = new();
    private FighterHero _fighterHero = new();
    private MarksmanHero _marksmanHero = new();
    
    private HashSet<Hero> _chosenHeroes = new(new HeroComparer());

    [CanBeNull] private Hero _firstChosenHero;
    [CanBeNull] private Hero _secondChosenHero;
    private string _firstChosenHeroLabel = "None";
    private string _secondChosenHeroLabel = "None";

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        TankHeroStats = root.Q<Label>("TankHeroStats");
        FighterHeroStats = root.Q<Label>("FighterHeroStats");
        MarksmanHeroStats = root.Q<Label>("MarksmanHeroStats");
        ChosenHeroesLabel = root.Q<Label>("ChosenHeroesLabel");
        
        CrossFighterHeroButton = root.Q<Button>("CrossFighterHeroButton");
        CrossTankHeroButton = root.Q<Button>("CrossTankHeroButton");
        CrossMarksmanHeroButton = root.Q<Button>("CrossMarksmanHeroButton");

        CrossFighterHeroButton.clicked += OnFighterClick;
        CrossTankHeroButton.clicked += OnTankClick;
        CrossMarksmanHeroButton.clicked += OnMarksmanClick;

        PlayButton = root.Q<Button>("PlayButton");
        
        PlayButton.clicked += OnPlayButtonClick;
    }
    
    void Update()
    {
        TankHeroStats.text = _tankHero.ToString();
        FighterHeroStats.text = _fighterHero.ToString();
        MarksmanHeroStats.text = _marksmanHero.ToString();
        
        PlayButton.SetEnabled(_chosenHeroes.Count == 2);

        _firstChosenHero = _chosenHeroes.Count > 0 ? GetChosenHeroAtIndex(0) : null;
        _secondChosenHero = _chosenHeroes.Count > 1 ? GetChosenHeroAtIndex(1) : null;

        _firstChosenHeroLabel = _firstChosenHero == null ? "None" : _firstChosenHero.Name;
        _secondChosenHeroLabel = _secondChosenHero == null ? "None" : _secondChosenHero.Name;
        
        ChosenHeroesLabel.text = $"Heroes chosen: {_firstChosenHeroLabel} + {_secondChosenHeroLabel}";
    }

    private Hero GetChosenHeroAtIndex(int index)
    {
        if (_chosenHeroes.Count <= index)
        {
            throw new IndexOutOfRangeException();
        }
        var list = _chosenHeroes.ToList();
        return list[index];
    }

    private void OnFighterClick()
    {
        if (_chosenHeroes.Contains(_fighterHero))
        {
            return;
        }

        if (_chosenHeroes.Count >= 2)
        {
            _chosenHeroes.Remove(_tankHero);
        }
        _chosenHeroes.Add(_fighterHero);
    }
    
    private void OnTankClick()
    {
        if (_chosenHeroes.Contains(_tankHero))
        {
            return;
        }

        if (_chosenHeroes.Count >= 2)
        {
            _chosenHeroes.Remove(_marksmanHero);
        }
        _chosenHeroes.Add(_tankHero);
    }
    
    private void OnMarksmanClick()
    {
        if (_chosenHeroes.Contains(_marksmanHero))
        {
            return;
        }

        if (_chosenHeroes.Count >= 2)
        {
            _chosenHeroes.Remove(_fighterHero);
        }
        _chosenHeroes.Add(_marksmanHero);
    }

    private void OnPlayButtonClick()
    {
        PlayerRepository.CreatePlayerHero(_firstChosenHero, _secondChosenHero);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.deltaTime * Time.timeScale;
    }
}
