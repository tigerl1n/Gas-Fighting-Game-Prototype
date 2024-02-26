using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character : MonoBehaviour
{
    RectTransform rect;
    Vector2 pos;

    public int health = 50;     int mHealth;
    public int special = 20;    int mSpecial;
    public int atk = 4;
    public int defence;
    public int speed;

    public Slider hBar;
    public Slider sBar;
    public Text hlt;
    public Text sp;

    public GameObject outline;
    public Texture selected;
    public Texture invalid;
    Texture nSelected;

    int wait = 10;

    void Start(){
        rect = GetComponent<RectTransform>();
        pos = new Vector2(0, -250);
        nSelected = outline.GetComponent<RawImage>().texture;

        mHealth = health;
        hBar.maxValue = mHealth;
        mSpecial = special;
        sBar.maxValue = mSpecial;
    }

    void Update(){
        if (health > mHealth){ health = mHealth; }
        if (special > mSpecial){ special = mSpecial; }
        wait++;
        rect.anchoredPosition = pos;

        hBar.value = health;
        sBar.value = special;
        string healths = health.ToString();
        string mhealths = mHealth.ToString();
        hlt.text = healths + "/" + mhealths;
        string specials = special.ToString();
        string mspecials = mSpecial.ToString();
        sp.text = specials + "/" + mspecials;
    }


    public void attack(GameObject enemy){
        var damage = Random.Range(atk, atk*2);
        enemy.GetComponent<enemyController>().health -= damage;

        if (special < mSpecial - 1){
            special += 2;
        }
        else{
            special = mSpecial;
        }
    }
    public void specialAttack(GameObject enemy){
        if (special > 4){
            var damage = Random.Range(4, 16);
            enemy.GetComponent<enemyController>().health -= damage;
            special -= 5;
        }
        else{
            outline.GetComponent<RawImage>().texture = invalid;
            wait = 0;
        }
    }


    public void startTurn(){
        if (wait > 10){
            pos = new Vector2(pos.x, 109);
            outline.GetComponent<RawImage>().texture = selected;
        }
    }
    public void endTurn(){
        pos = new Vector2(pos.x, -575);
        outline.GetComponent<RawImage>().texture = nSelected;
    }
    public void gmode(){
        health = 999;
        special = 999;
        mHealth = 999;
        mSpecial = 999;
    }
}