using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WordSpawner : MonoBehaviour
{
    public GameObject word;
    private static float startTime;
   // private TextMesh tm;
    private List<string> wordsHistory = new List<string>();
    private int position;
    private bool gameOver;
    private string curretWord;
    private bool isWordSet;
    
    private List<GameObject> _gameObjects = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        //TEMPORRALY 
        var word1 = CreateWord();
        var word2 = CreateWord();
        var word3 = CreateWord();
        var word4 = CreateWord();
        _gameObjects.Add(word1);
        _gameObjects.Add(word2);
        _gameObjects.Add(word3);
        _gameObjects.Add(word4);
        
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //SpamWord();
        var input = GetInput();
            
        if (input != null)
        { 
            if (!isWordSet)
            {
                GetWordFromList(input, wordsHistory);
            }
            CheckWord(input, curretWord);
        }
    }

    //this is used to destroy specific object 
    private GameObject GetGameObjectByName(string name)
    {
        foreach (var i in _gameObjects)
        {
            var tm = i.GetComponent<TextMesh>();
            
            if (tm.text == name)
            {
                Debug.Log("GetGameObject is returning object at position :" + i.transform.position);
                return i;
            }
        }

        Debug.Log("NO OBJECT BY NAME FOUND");
        return null;
    }
    

    private void SetWord(string givenWord)
    {
        if (!isWordSet)
        {
            curretWord = givenWord;
            isWordSet = true;
            Debug.Log("Set word: " + curretWord);
        }
    }

    private void CheckWord(string input, string givenWord)
    {
        // if (wordsHistory.Count > 0)
        // {
            if (givenWord != null)
            {
                 CheckInput(givenWord,input);
            }
            else
            {
                Debug.Log("CheckWord : Given word is null");
            }
        // }
    }

    private void CheckInput(string givenWord, string input)
    {
        //check if position isn't bigger than word 
        var isCorrect = CheckInputAtPosition(input, givenWord, position);
                
        if (isCorrect)
        {
            Debug.Log("input: " + input + " was correct");
            IncreasePosition(givenWord);

            //RemoveLetter(GetGameObjectByName(givenWord));
        }
        else
        {
            position = 0;
            Debug.Log("input: " + input + " wasn't correct = GAME OVER" );
            gameOver = true;
        }
    }

    private void IncreasePosition(string givenWord)
    {
        position++;
        
         if (position == givenWord.Length)
         {
             position = 0;
             wordsHistory.Remove(givenWord);
             isWordSet = false;
             Debug.Log("WORD DONE");

             var objectToDestroy = GetGameObjectByName(curretWord);
             _gameObjects.Remove(objectToDestroy);
             Destroy(objectToDestroy);
         }
    }
    
    private void GetWordFromList(string character, List<string> strings)
    {
        foreach (var valueToCheck in strings)
        {
            if (valueToCheck.StartsWith(character))
            {
                SetWord(valueToCheck);
                break;
            }
            
            Debug.Log("NO MATCHES");
            gameOver = true;
        }
    }

    private void RemoveLetter(GameObject gameObject)
    {
       var textMesh = gameObject.GetComponent<TextMesh>();
       textMesh.text =  textMesh.text.Remove(0, 1);
       //curretWord.Remove(0, 1);

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

    void SpamWord()
    {
        var currentTime =  Time.time;
        if (currentTime - startTime > 4f && gameOver == false)
        {
            var newWord = CreateWord();
            startTime = currentTime;
            
            Destroy(newWord, 10f); //destroy after 10s -- maybe too much 
        }
    }

    private bool CheckInputAtPosition(string input, string givenWord, int position)
    {
        return input == givenWord[position].ToString();
    }
    

    private GameObject CreateWord()
    {
        GameObject newWord = Instantiate(word);
        GetRandomPosition(newWord);
        
        var tm = newWord.GetComponent<TextMesh>();
        var text = GetRandomString(Random.Range(5,9));
        tm.text = text;
        
        wordsHistory.Add(text);
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
    
}
