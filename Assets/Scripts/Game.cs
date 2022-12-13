using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public List<Balls> ballPrefabs;
    public Transform ballPosition;
    Balls balls;
    List<Balls> ball = new List<Balls>();
    int ballId;
    bool isGameOver;
    public GameObject escape;
    bool isEscape=true;
    public Text scoreText;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
       balls = SetNextBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver||isEscape==false)
        {
            Return();
            return;
        }
        if(Input.GetMouseButtonUp(0))
        {
            var mousePos = Input.mousePosition;
            var wolrdPos = Camera.main.ScreenToWorldPoint(mousePos);
            var ballPos = new Vector2(wolrdPos.x, ballPosition.position.y);
            balls.gameObject.transform.position = ballPos;
            balls.SetSimulated(true);
            balls = SetNextBall();
        }       
        Return();
    }

    Balls SetNextBall()
    {
        var rand = Random.Range(0,ballPrefabs.Count);
        var prefab = ballPrefabs[rand].gameObject;
        var pos = ballPosition.position;

        return SetBalls(prefab,pos);
    }
    Balls SetBalls(GameObject prefab, Vector3 pos)
    {       
        
        var ballObject = Instantiate(prefab,pos,Quaternion.identity);
        var fBall = ballObject.GetComponent<Balls>();
        fBall.SetSimulated(false);
        fBall.id = ballId++;

        fBall.OnLevilUp = (a,b) =>
        {
            if(IsBallsExist(a)&&IsBallsExist(b))
            {
                var pos1 = a.gameObject.transform.position;
                var pos2 = b.gameObject.transform.position;
                var pos = (pos1+pos2)/2;
                RemoveBall(a);
                RemoveBall(b);
                AddScore(a.score);
                var ba = SetBalls(a.ballsNextPrefab, pos);
                ba.SetSimulated(true);
            }
        };

        fBall.OnGameOver = () =>
        {
            isGameOver = true;
        };

        ball.Add(fBall);      
        return fBall;
    }
    void RemoveBall(Balls b)
    {
        for (int i = 0; i<ball.Count; i++)
        {
            if(ball[i].id==b.id)
            {
                ball.Remove(b);
                Destroy(b.gameObject);
                return;
            }
        }
    }

    bool IsBallsExist(Balls b)
    {
        for (int i = 0; i<ball.Count; i++)
        {
            if(ball[i].id==b.id)
            {
                return true;
            }
        }
        return false;
    }

    void Return()
    {
        if(isGameOver)
        {
            escape.SetActive(true);
        }
        else 
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                escape.SetActive(isEscape);
                isEscape=!isEscape;
                if(isEscape==false)
                {
                    Time.timeScale=0;
                }
                else
                {
                    Time.timeScale=1;
                }
            }
        }      
    }

    public void Resart()
    {
        Time.timeScale=1;
        SceneManager.LoadScene(0);  
    }

    public void Quit()
    {
        Application.Quit();
    }

    void AddScore(int score)
    {
        this.score+=score;
        scoreText.text = "Score:"+this.score.ToString();
    }
}
