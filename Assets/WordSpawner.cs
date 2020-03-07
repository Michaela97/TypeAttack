using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WordSpawner : MonoBehaviour
{
    public GameObject word;
    private static float startTime;
    private TextMesh tm;
    private List<GameObject> wordsHistory = new List<GameObject>();
    private GameObject curretWord;
    private bool gameOver;
    private bool isWordSet;
    
    private string GUImessage;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateWord();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        SpamWord();
        
        var input = GetInput();
            
        if (input != null && !gameOver)
        { 
            if (!isWordSet)
            {
                GetWordFromList(input, wordsHistory);
            }
            CheckInput(curretWord,input);
        }
    }
    

    private void SetWord(GameObject givenWord)
    {
        if (!isWordSet)
        {
            curretWord = givenWord;
            isWordSet = true;
            
            tm = curretWord.GetComponent<TextMesh>();
            Debug.Log("Set word: " + tm.text);
        }
    }

    private void CheckInput(GameObject givenWord, string input)
    {
        if (givenWord == null) { return; }
        
        tm = givenWord.GetComponent<TextMesh>();

        var isCorrect = CheckInputAtPosition(input, tm.text);
                
        if (isCorrect)
        {
            if (tm.text.Length == 1)
            {
                wordsHistory.Remove(curretWord);
                isWordSet = false;
                Destroy(curretWord);
            }
            RemoveLetter(curretWord);
        }
        else
        {
            GameOver(GameMessages.wrongInput);
        }
    }

    private void GetWordFromList(string character, List<GameObject> wordObjects)
    {
        foreach (var wordToCheck in wordObjects)
        {
            tm = wordToCheck.GetComponent<TextMesh>();
            if (tm.text.StartsWith(character))
            {
                SetWord(wordToCheck);
                return;
            }
        }
        GameOver(GameMessages.noWordMatch);
    }

    private void RemoveLetter(GameObject gameObject)
    { 
        tm = gameObject.GetComponent<TextMesh>();
        tm.text =  tm.text.Remove(0, 1);
    }
    
    private string GetInput()
    {
        var input = Input.inputString;
        
        if (input != null && !"".Equals(input))
        {
            return input;
        }
        return null;
    }
    
    private void GameOver(string message)
    {
        GUImessage = message;
        gameOver = true;
        foreach (var wordObject in wordsHistory)
        {
            Destroy(wordObject);
        }
    }

    void SpamWord()
    {
        var currentTime =  Time.time;
        if (currentTime - startTime > 4f && gameOver == false)
        {
            var newWord = CreateWord();
            startTime = currentTime;
            
            Destroy(newWord, 10f); 
            //if destroyed == game over 
            //ASK JACOB
        }
    }

    private bool CheckInputAtPosition(string input, string givenWord)
    {
        return input == givenWord[0].ToString();
    }
    
    private GameObject CreateWord()
    {
        GameObject newWord = Instantiate(word);
        GetRandomPosition(newWord);
        
        tm = newWord.GetComponent<TextMesh>();
        var text = GetRandomString(Random.Range(5,9));
        tm.text = text;
        
        wordsHistory.Add(newWord);
        Debug.Log(text + " added to history");
        
        return newWord;
    }
    
    private void GetRandomPosition(GameObject gameObject)
    {
        var x = Random.Range(-9f, 4f);
        var z = Random.Range(-5f, 5f);
        gameObject.transform.position = new Vector3(x, 0, z);
    }

    private string GetRandomString(int length)
    {
        const string chars = "qwertyuiopasdfghjklzxcvbnm";
        
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Range(0,s.Length)]).ToArray());
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(0,0,300,100), GUImessage);
    }
    
}
