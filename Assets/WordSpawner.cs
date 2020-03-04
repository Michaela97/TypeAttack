using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WordSpawner : MonoBehaviour
{
    public GameObject word;
    private static float startTime;
    private TextMesh tm;
    private List<string> wordsHistory = new List<string>();
    private int position;
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        SpamWord();

        if (wordsHistory.Count > 0)
        {
            var givenWord = wordsHistory[0]; //first word in this history list
            
            var input = GetInput();
            
            if (input != null)
            {
                CheckWord(givenWord, input);
            }
        }
    }

    private void CheckWord(string givenWord, string input)
    {
        //check if position isn't bigger than word 
        var isCorrect = CheckInputAtPosition(input, givenWord, position);
                
        if (isCorrect)
        {
            Debug.Log("input: " + input + " was correct");
            position++;
            if (position == givenWord.Length)
            {
                Debug.Log("You typed word correctly");
                //destroy object spam a new one 
                position = 0; 
            }
        }
        else
        {
            position = 0;
            Debug.Log("input: " + input + " wasn't correct = GAME OVER" );
        }
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
        if (currentTime - startTime > 4f)
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
        
        tm = newWord.GetComponent<TextMesh>();
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
