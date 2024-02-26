using static System.Random;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // variables
    int enumb;
    int currentTurn = 1;
    int wait = 10;
    int j = 0;

    string action = "";
    string difficulty = "";

    bool created;
    bool pTurn = true;
    bool selected = false;
    bool battle = false;
    bool mf = false, mb = false, ml = false, mr = false;

    int c1mood = 0;
    int c2mood = 0;
    int c3mood = 0;
    int c4mood = 0;


    // game objects 
    public GameObject upper;
    GameObject c1;
    GameObject c2;
    GameObject c3;
    GameObject c4;

    GameObject iMenu;
    GameObject lowerhud;
    GameObject pmove;

    GameObject eselect;
    GameObject pselect;

    GameObject current;
    GameObject currentEnemy;
    GameObject currentPlayer;
    GameObject targ;

    GameObject room;
    GameObject end;


    //Audio
    AudioSource sound;
    public AudioClip attack;
    public AudioClip special;
    public AudioClip boss;
    public AudioClip enemyAttack;
    public AudioClip enemyDeath;
    public AudioClip footsteps;
    public AudioClip change;
    public AudioClip select;

    public AudioClip smusic;
    public AudioClip bmusic;
    public AudioClip emusic;


    // other
    public Texture invalid;
    Texture normal;

    public Text itemL1;
    public Text itemL2;

    Camera above;
    Camera norm;

    Vector3 mforward=new Vector3(0, 0, 0), mback=new Vector3(0, 0, 0), mleft=new Vector3(0, 0, 0), mright=new Vector3(0, 0, 0);


    // lists
    GameObject[] enemies = new GameObject[4];
    GameObject[] healthbs = new GameObject[4];

    IDictionary<string, int> items = new Dictionary<string, int>();
    IDictionary<Vector3, bool> cleared = new Dictionary<Vector3, bool>(21);
    IDictionary<Vector3, string> diff = new Dictionary<Vector3, string>(21);
    IDictionary<Vector3, Light> lights = new Dictionary<Vector3, Light>(21);


    void Start(){
        // camera
        lowerhud = GameObject.Find("HUD lower");
        norm = GameObject.Find("Main Camera").GetComponent<Camera>();
        above = GameObject.Find("Camera").GetComponent<Camera>();
        above.enabled = false;


        // game objects
        c1 = GameObject.Find("C1 menu");
        c2 = GameObject.Find("C2 menu");
        c3 = GameObject.Find("C3 menu");
        c4 = GameObject.Find("C4 menu");
        c1.GetComponent<character>().atk += c1mood;
        c2.GetComponent<character>().atk += c2mood;
        c3.GetComponent<character>().atk += c3mood;
        c4.GetComponent<character>().atk += c4mood;
        iMenu = GameObject.Find("ItemMenu");
        eselect = GameObject.Find("Enemy select");
        pselect = GameObject.Find("Player select");
        room = GameObject.Find("room select");
        end = GameObject.Find("end");
        current = c1;

        sound = GameObject.Find("Main Camera").GetComponent<AudioSource>();

    
        // other
        enumb = Random.Range(2, 4);
        normal = eselect.GetComponent<RawImage>().texture;
        sound.clip = smusic;
        sound.Play(0);
        sound.loop = true;


        // items
        items.Add("Potions", 6);
        items.Add("Elixer", 3);


        // Filling cleared 
        cleared.Add(new Vector3(10, 4.5f, 0), false);       diff.Add(new Vector3(10, 4.5f, 0), "Easy");    
        cleared.Add(new Vector3(-10, 4.5f, 0), false);      diff.Add(new Vector3(-10, 4.5f, 0), "Easy");
        cleared.Add(new Vector3(-30, 4.5f, 0), false);      diff.Add(new Vector3(-30, 4.5f, 0), "Easy");
        cleared.Add(new Vector3(-50, 4.5f, 0), false);      diff.Add(new Vector3(-50, 4.5f, 0), "Easy");
        cleared.Add(new Vector3(-10, 4.5f, -20), false);    diff.Add(new Vector3(-10, 4.5f, -20), "Easy");
        cleared.Add(new Vector3(-30, 4.5f, -20), false);    diff.Add(new Vector3(-30, 4.5f, -20), "Easy"); 
        cleared.Add(new Vector3(-50, 4.5f, -20), false);    diff.Add(new Vector3(-50, 4.5f, -20), "Easy");

        cleared.Add(new Vector3(-70, 4.5f, -20), false);    diff.Add(new Vector3(-70, 4.5f, -20), "Med");
        cleared.Add(new Vector3(-10, 4.5f, -40), false);    diff.Add(new Vector3(-10, 4.5f, -40), "Med");
        cleared.Add(new Vector3(-30, 4.5f, -40), false);    diff.Add(new Vector3(-30, 4.5f, -40), "Med");
        cleared.Add(new Vector3(-50, 4.5f, -40), false);    diff.Add(new Vector3(-50, 4.5f, -40), "Med");
        cleared.Add(new Vector3(-70, 4.5f, -40), false);    diff.Add(new Vector3(-70, 4.5f, -40), "Med");
        cleared.Add(new Vector3(-90, 4.5f, -40), false);    diff.Add(new Vector3(-90, 4.5f, -40), "Med");

        cleared.Add(new Vector3(-30, 4.5f, -60), false);    diff.Add(new Vector3(-30, 4.5f, -60), "Hard");
        cleared.Add(new Vector3(-50, 4.5f, -60), false);    diff.Add(new Vector3(-50, 4.5f, -60), "Hard");
        cleared.Add(new Vector3(-70, 4.5f, -60), false);    diff.Add(new Vector3(-70, 4.5f, -60), "Hard");
        cleared.Add(new Vector3(-90, 4.5f, -60), false);    diff.Add(new Vector3(-90, 4.5f, -60), "Hard");    
        cleared.Add(new Vector3(-50, 4.5f, -80), false);    diff.Add(new Vector3(-50, 4.5f, -80), "Hard");
        cleared.Add(new Vector3(-70, 4.5f, -80), false);    diff.Add(new Vector3(-70, 4.5f, -80), "Hard");    
        cleared.Add(new Vector3(-90, 4.5f, -80), false);    diff.Add(new Vector3(-90, 4.5f, -80), "Hard");
        cleared.Add(new Vector3(-70, 4.5f, -100), false);   diff.Add(new Vector3(-70, 4.5f, -100), "Boss");


        lights.Add(new Vector3(10, 4.5f, 0), GameObject.Find("base").GetComponent<Light>());
        lights.Add(new Vector3(-10, 4.5f, 0), GameObject.Find("easy1").GetComponent<Light>());
        lights.Add(new Vector3(-30, 4.5f, 0), GameObject.Find("easy2").GetComponent<Light>());
        lights.Add(new Vector3(-50, 4.5f, 0), GameObject.Find("easy3").GetComponent<Light>());
        lights.Add(new Vector3(-10, 4.5f, -20), GameObject.Find("easy4").GetComponent<Light>());
        lights.Add(new Vector3(-30, 4.5f, -20), GameObject.Find("easy5").GetComponent<Light>());
        lights.Add(new Vector3(-50, 4.5f, -20), GameObject.Find("easy6").GetComponent<Light>());

        lights.Add(new Vector3(-70, 4.5f, -20), GameObject.Find("med1").GetComponent<Light>());
        lights.Add(new Vector3(-10, 4.5f, -40), GameObject.Find("med2").GetComponent<Light>());
        lights.Add(new Vector3(-30, 4.5f, -40), GameObject.Find("med3").GetComponent<Light>());
        lights.Add(new Vector3(-50, 4.5f, -40), GameObject.Find("med4").GetComponent<Light>());
        lights.Add(new Vector3(-70, 4.5f, -40), GameObject.Find("med5").GetComponent<Light>());
        lights.Add(new Vector3(-90, 4.5f, -40), GameObject.Find("med6").GetComponent<Light>());

        lights.Add(new Vector3(-30, 4.5f, -60), GameObject.Find("hard1").GetComponent<Light>());
        lights.Add(new Vector3(-50, 4.5f, -60), GameObject.Find("hard2").GetComponent<Light>());
        lights.Add(new Vector3(-70, 4.5f, -60), GameObject.Find("hard3").GetComponent<Light>());
        lights.Add(new Vector3(-90, 4.5f, -60), GameObject.Find("hard4").GetComponent<Light>());
        lights.Add(new Vector3(-50, 4.5f, -80), GameObject.Find("hard5").GetComponent<Light>());
        lights.Add(new Vector3(-70, 4.5f, -80), GameObject.Find("hard6").GetComponent<Light>());
        lights.Add(new Vector3(-90, 4.5f, -80), GameObject.Find("hard7").GetComponent<Light>());
        lights.Add(new Vector3(-70, 4.5f, -100), GameObject.Find("boss").GetComponent<Light>());
    }

    void FixedUpdate(){

        // starts battle
        if (!cleared[norm.GetComponent<RectTransform>().position]){
            created = false;
            battle = true;
            cleared[norm.GetComponent<RectTransform>().position] = true;
            difficulty = diff[norm.GetComponent<RectTransform>().position];
        }

        if (created == false){
            enumb = Random.Range(2, 5);
            enemies = new GameObject[4];
            if (difficulty == "Boss") { enumb = 1; }
            for (int i = 0; i < enumb; i++){
                enemies[i] = Instantiate(Resources.Load("Enemy")) as GameObject;
                enemies[i].GetComponent<enemyController>().diff = difficulty;
                healthbs[i] = Instantiate(Resources.Load("healthbar")) as GameObject;
                enemies[i].GetComponent<enemyController>().setHealthBar(healthbs[i]);
                enemies[i].transform.SetParent(norm.GetComponent<RectTransform>());
                healthbs[i].transform.SetParent(upper.transform);
            }
            currentEnemy = enemies[0];
            if (enumb == 1){
                healthbs[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-440, -950);
                healthbs[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f);
                healthbs[0].transform.localScale = new Vector2(1, 2);
                enemies[0].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 10);
                if (difficulty == "Boss") {
                    enemies[0].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f); 
                    enemies[0].GetComponent<RectTransform>().localPosition = new Vector3(0, 3, 15);
                    }
            }

            if (enumb == 2){
                healthbs[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-580, -950);
                healthbs[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[0].transform.localScale = new Vector2(0.8f, 2);
                enemies[0].GetComponent<RectTransform>().localPosition = new Vector3(-2, 0, 10);

                healthbs[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-120, -950);
                healthbs[1].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[1].transform.localScale = new Vector2(0.8f, 2);
                enemies[1].GetComponent<RectTransform>().localPosition = new Vector3(2, 0, 10);
            }

            if (enumb == 3){
                healthbs[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-540, -950);
                healthbs[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[0].transform.localScale = new Vector2(0.5f, 2);
                enemies[0].GetComponent<RectTransform>().localPosition = new Vector3(-4, 0, 10);

                healthbs[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-220, -950);
                healthbs[1].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[1].transform.localScale = new Vector2(0.5f, 2);
                enemies[1].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 10);

                healthbs[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -950);
                healthbs[2].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[2].transform.localScale = new Vector2(0.5f, 2);
                enemies[2].GetComponent<RectTransform>().localPosition = new Vector3(4, 0, 10);
            }

            if (enumb == 4){
                healthbs[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-520, -950);
                healthbs[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[0].transform.localScale = new Vector2(0.4f, 2);
                enemies[0].GetComponent<RectTransform>().localPosition = new Vector3(-4.5f, 0, 10);

                healthbs[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, -950);
                healthbs[1].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[1].transform.localScale = new Vector2(0.4f, 2);
                enemies[1].GetComponent<RectTransform>().localPosition = new Vector3(-1.5f, 0, 10);

                healthbs[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(-80, -950);
                healthbs[2].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[2].transform.localScale = new Vector2(0.4f, 2);
                enemies[2].GetComponent<RectTransform>().localPosition = new Vector3(1.5f, 0, 10);

                healthbs[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(140, -950);
                healthbs[3].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
                healthbs[3].transform.localScale = new Vector2(0.4f, 2);
                enemies[3].GetComponent<RectTransform>().localPosition = new Vector3(4.5f, 0, 10);            
            }
            created = true;
            battle = true;
        }

        wait++;
        back();


        j = 0;
        for (int i = 0; i < enemies.Length; i++){
            if (enemies[i] != null){
                if (enemies[i].GetComponent<enemyController>().health > 0){
                    j++;
                }
            }
        }
        if (j == 0){
            if (difficulty == "Boss") { end.GetComponent<RectTransform>().localPosition = new Vector3(0, 60, 0); }
            battle = false;
            for (int i = 0; i < 4; i++){
                if (enemies[i] != null){
                    Destroy(enemies[i]);
                }
            }
            enemies = new GameObject[4];
        }

        foreach (KeyValuePair<Vector3, bool> kvp in cleared){
            if (kvp.Value){
                lights[kvp.Key].color = UnityEngine.Color.green;
            }
        }

        // handles player actions 
        if (battle){
            if (selected){
                eselect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -850);
            }

            if (pTurn){
                if (currentTurn == 1){
                    if (c1.GetComponent<character>().health <= 0){ currentTurn++; }
                    else{ c1.GetComponent<character>().startTurn(); current = c1; }
                }
                if (currentTurn == 2){
                    if (c2.GetComponent<character>().health <= 0){ currentTurn++; }
                    else{ c2.GetComponent<character>().startTurn(); current = c2; }
                    c1.GetComponent<character>().endTurn();
                }
                if (currentTurn == 3){
                    if (c3.GetComponent<character>().health <= 0){ currentTurn++; }
                    else{ c3.GetComponent<character>().startTurn(); current = c3; }
                    c2.GetComponent<character>().endTurn();
                }
                if (currentTurn == 4){
                    if (c4.GetComponent<character>().health <= 0){ currentTurn++; }
                    else{ c4.GetComponent<character>().startTurn(); current = c4; }
                    c3.GetComponent<character>().endTurn();
                }
                if (currentTurn == 5){
                    c4.GetComponent<character>().endTurn();
                    currentTurn = 1;
                    pTurn = false;
                }
            }


            // enemy turn
            else{
                targ = c1;
                for (int i = 0; i < enemies.Length; i++){
                        var target = Random.Range(1, 5);
                        if (target == 1){targ = c1;}
                        if (target == 2){targ = c2;}
                        if (target == 3){targ = c3;}
                        if (target == 4){targ = c4;}
                        if (targ.GetComponent<character>().health > 0 && enemies[i].GetComponent<enemyController>().health > 0){
                            enemies[i].GetComponent<enemyController>().attack(targ);
                            if (difficulty != "Boss"){ sound.clip = enemyAttack; }
                            else { sound.clip = boss;}
                            sound.Play(20000);
                            pTurn = true;
                        }
                }
            }
            room.GetComponent<RectTransform>().position = new Vector3(0, -800, 0);
        }
        else{
            room.GetComponent<RectTransform>().localPosition = new Vector3(0, -24, 0);
            mforward = norm.GetComponent<RectTransform>().position + new Vector3(-20, 0, 0);
            mback = norm.GetComponent<RectTransform>().position + new Vector3(20, 0, 0);
            mleft = norm.GetComponent<RectTransform>().position + new Vector3(0, 0, -20);
            mright = norm.GetComponent<RectTransform>().position + new Vector3(0, 0, 20);
            if (cleared.ContainsKey(mforward))  { mf = true;}   else {mf = false;}
            if (cleared.ContainsKey(mback))  { mb = true;}      else {mb = false;}
            if (cleared.ContainsKey(mright))  { mr = true;}     else {mr = false;}
            if (cleared.ContainsKey(mleft))  { ml = true;}      else {ml = false;}
        }
    }
    

    // player action functions 
    public void pAttack(){
        iMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 1000);
        selectEnemy();
        action  ="attack";
    }

    public void pSpecial(){
        iMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 1000);
        if (current.GetComponent<character>().special > 4){ selectEnemy(); }
        else{ current.GetComponent<character>().specialAttack(currentEnemy);  }
        action = "special";
    }

    public void pItems(){
        sound.PlayOneShot(select, 0.7f);
        foreach(KeyValuePair<string, int> i in items){
            itemL1.text = "Potions: " + items["Potions"].ToString();
            itemL2.text = "Elixer: " + items["Elixer"].ToString();
        }
        iMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 50);
    }

    public void potion(){
        sound.PlayOneShot(select, 0.7f);
        if (items["Potions"] > 0){
            action = "potion";
            pselect.GetComponent<RectTransform>().localPosition = new Vector3(0, 109, 0);
            items["Potions"] -= 1;
        }
    }
    public void elixer(){
        sound.PlayOneShot(select, 0.7f);
        if (items["Elixer"] > 0){
            action = "elixer";
            pselect.GetComponent<RectTransform>().anchoredPosition = new Vector2(3, -250);
            items["Elixer"] -= 1;
        }
    }
    public void flee(){
        sound.PlayOneShot(select, 1);
            for (int i = 0; i < 4; i++){
                if (enemies[i] != null){
                    enemies[i].GetComponent<enemyController>().health = -1;
                }
            }
            mforward = norm.GetComponent<RectTransform>().position + new Vector3(-20, 0, 0);
            mback = norm.GetComponent<RectTransform>().position + new Vector3(20, 0, 0);
            mleft = norm.GetComponent<RectTransform>().position + new Vector3(0, 0, -20);
            mright = norm.GetComponent<RectTransform>().position + new Vector3(0, 0, 20);
            if (cleared.ContainsKey(mforward))  { mf = true;}   else {mf = false;}
            if (cleared.ContainsKey(mback))  { mb = true;}      else {mb = false;}
            if (cleared.ContainsKey(mright))  { mr = true;}     else {mr = false;}
            if (cleared.ContainsKey(mleft))  { ml = true;}      else {ml = false;}
            cleared[norm.GetComponent<RectTransform>().position] = false;

            if (cleared[mforward] == true) { norm.GetComponent<RectTransform>().position += new Vector3(-20, 0, 0); }
            else if (cleared[mback] == true) { norm.GetComponent<RectTransform>().position += new Vector3(20, 0, 0); }
            else if (cleared[mleft] == true) { norm.GetComponent<RectTransform>().position += new Vector3(0, 0, -20); }
            else if (cleared[mright] == true) { norm.GetComponent<RectTransform>().position += new Vector3(0, 0, 20); }
    }
    public void godmode(){
        sound.PlayOneShot(change, 1);
        c1.GetComponent<character>().gmode();
        c2.GetComponent<character>().gmode();
        c3.GetComponent<character>().gmode();
        c4.GetComponent<character>().gmode();
    }


    // selecting enemies 
    public void e1(){
        if (enemies[0].GetComponent<enemyController>().health > 0){
            currentEnemy = enemies[0];
            selected = true;
            control();
        }
        else{wait = 0;}
    }
    public void e2(){
        if (enumb > 1 && enemies[1].GetComponent<enemyController>().health > 0){
            currentEnemy = enemies[1];
            selected = true;
            control();
        }
        else{wait = 0;}
    }
    public void e3(){
        if (enumb > 2 && enemies[2].GetComponent<enemyController>().health > 0){
            currentEnemy = enemies[2];
            selected = true;
            control();
        }
        else{wait = 0;}
    }
    public void e4(){
        if (enumb == 4 && enemies[3].GetComponent<enemyController>().health > 0){
            currentEnemy = enemies[3];
            selected = true;
            control();
        }
        else{wait = 0;}
    }
    void selectEnemy(){
        sound.PlayOneShot(select, 1);
        if (enumb > 1){
            eselect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 109);
            selected = false;
        }
        else{
            currentEnemy = enemies[0];
            selected = true;
            control();
        }
    }


    // selecting players 
    public void p1(){
        if (action == "potion"){c1.GetComponent<character>().health += 20;}   
        if (action == "elixer"){c1.GetComponent<character>().special += 10;}   
        control();}
    public void p2(){
        if (action == "potion"){c2.GetComponent<character>().health += 20;}   
        if (action == "elixer"){c2.GetComponent<character>().special += 10;}   
        control();}
    public void p3(){
        if (action == "potion"){c3.GetComponent<character>().health += 20;}   
        if (action == "elixer"){c3.GetComponent<character>().special += 10;}   
        control();}
    public void p4(){
        if (action == "potion"){c4.GetComponent<character>().health += 20;}   
        if (action == "elixer"){c4.GetComponent<character>().special += 10;}   
        control();}


    // general functions 
    void control(){
        if (action == "attack"){
            current.GetComponent<character>().attack(currentEnemy);
            sound.PlayOneShot(attack, 1);
            currentTurn += 1;
        }
        if (action == "special"){
             if (current.GetComponent<character>().special > 4){
                sound.PlayOneShot(special, 1);
                currentTurn += 1;
            }
            current.GetComponent<character>().specialAttack(currentEnemy);
        }
        if (action == "potion" || action == "elixer"){
            sound.PlayOneShot(select, 0.7f);
            pselect.GetComponent<RectTransform>().localPosition = new Vector3(0, 1000, 0);
            iMenu.GetComponent<RectTransform>().localPosition = new Vector3(0, 1000, 0);
            currentTurn += 1;
        }
    }

    void back(){
        if (wait < 10){eselect.GetComponent<RawImage>().texture = invalid;}
        else{eselect.GetComponent<RawImage>().texture = normal;}
    }

    public void switchCamera(){
        sound.PlayOneShot(change, 1);
        if (norm.enabled == true){
            norm.enabled = false;
            above.enabled = true;
            lowerhud.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -900);
        }
        else if (above.enabled == true){
            norm.enabled = true;
            above.enabled = false;
            lowerhud.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -432);
        }
    }

    public void End(){
        Application.Quit();
    }


    // player move
    public void moveforward(){
        if (mf){
            norm.GetComponent<RectTransform>().position += new Vector3(-20, 0, 0);
            sound.PlayOneShot(footsteps, 1);
        }
    }
    public void moveright(){
        if (mr){
            norm.GetComponent<RectTransform>().position += new Vector3(0, 0, 20);
            sound.PlayOneShot(footsteps, 1);
        }
    }
    public void moveleft(){
        if (ml){
            norm.GetComponent<RectTransform>().position += new Vector3(0, 0, -20);
            sound.PlayOneShot(footsteps, 1);
        }
    }
    public void moveback(){
        if (mb){
            norm.GetComponent<RectTransform>().position += new Vector3(20, 0, 0);
            sound.PlayOneShot(footsteps, 1);
        }
    }

    void OnEnable()
    {
        c1mood = PlayerPrefs.GetInt("AAttack");
        c2mood = PlayerPrefs.GetInt("BAttack");
        c3mood = PlayerPrefs.GetInt("CAttack");
        c4mood = PlayerPrefs.GetInt("DAttack");
    }
}