using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyController : MonoBehaviour
{
    public int health;
    int mHealth;
    public int atk = 5;
    public int defence;
    public int speed;
    public string diff;

    Slider hBar;
    Text hlt;
    public GameObject h;

    public Sprite e1;
    public Sprite e2;
    public Sprite e3;
    private SpriteRenderer sp;

    bool created = false;

    void Start()
    {
        mHealth = health;
        sp = GetComponent<SpriteRenderer>();
        if (Random.Range(1, 3) == 1){
            sp.sprite = e1;
        }
        else{
            sp.sprite = e2;
        }
        if (diff == "Easy"){
            health = 20;
            mHealth = 20;
            atk = 5;
        }
        else if (diff == "Med"){
            health = 30;
            mHealth = 30;
            atk = 7;
        }
        else if (diff == "Hard"){
            health = 40;
            mHealth = 40;
            atk = 10;
        }
        else if (diff == "Boss"){
            sp.sprite = e3;
            health = 100;
            mHealth = 100;
            atk = 20;
        }
    }

    void Update()
    {
        if (health >= 0){
            if (created){
                string hlts = health.ToString();
                string mhlts = mHealth.ToString();
                hlt.text = hlts + "/" + mhlts;

                hBar.value = health;
                hBar.maxValue = mHealth;
            }
        }
        else{
            h.GetComponent<RectTransform>().anchoredPosition = new Vector2(1000, 1000);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, 2000);
        }
    }
    
    public void attack(GameObject enemy){
        enemy.GetComponent<character>().health -= atk;
    }

    public void setHealthBar(GameObject healthbar){
        hBar = healthbar.GetComponentInChildren(typeof(Slider)) as Slider;
        hlt = healthbar.GetComponentInChildren(typeof(Text)) as Text;
        h = healthbar;
        created = true;
    }
}