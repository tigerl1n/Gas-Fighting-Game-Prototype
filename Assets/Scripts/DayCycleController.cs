using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayCycleController : MonoBehaviour
{
    //Values that carry over
    //Party stats - ATK, DEF, CHR, SPD
    static int[,] partyStats = { { 4, 4, 4, 4 }, { 4, 4, 4, 4 }, { 4, 4, 4, 4 }, { 4, 4, 4, 4 } };

    //Values
    string currentLocation;
    int currentPartyMember;
    string[] partyLocations;
    int[] statChanges;
    List<string> visitedLocations = new List<string>();
    int visitedPartyMembers = 0;

    //Dialogue choices
    string bDialogue = "‘Sup dude! Hey, wanna go to the gym in a bit? Gotta keep up my training after all, I can finally lift 100Kg!",
    bDialogueChoice1 = "Nice going “dude”!",
    bDialogueChoice2 = "Really? You’re quite scrawny.",
    bResponse1 = "Hehe, you flatter me dude, I bet you can be lifting the same real soon!",
    bResponse2 = "Ah, maybe I did exaggerate it, just a little bit.",
    
    cDialogue = "Oh hey, it’s you. Have you seen the posters around the town? Apparently a cat’s missing.",
    cDialogueChoice1 = "Shall we go look for it?",
    cDialogueChoice2 = "It’s probably dead.",
    cResponse1 = "Only if you have time, I’m sure its owner would appreciate it.",
    cResponse2 = "Oh…",
    
    dDialogue = "Hiya bestie! So I’m totally struggling with the current assignment and was wondering whether you got it?",
    dDialogueChoice1 = "Not really, but I’m sure if we work together, we can work it out!",
    dDialogueChoice2 = "You should be able to work it out on your own.",
    dResponse1 = "Heck yeah we can, I’ll go grab some snacks!",
    dResponse2 = "Ah, maybe if I try again for a few more hours I might finally get it!\r\n… Or not.\r\n";

    //Sprites
    public Sprite a1Sprite, a2Sprite, a3Sprite, b1Sprite, b2Sprite, c1Sprite, c2Sprite, d1Sprite, d2Sprite;

    //Backgrounds
    Image meetupCharacterSprite;

    //Animators
    Animator meetupBackgroundAnimator, statsChangesBackgroundAnimator, noMeetupBackgroundAnimator, alreadyVisitedBackgroundAnimator, visitAllBackgroundAnimator, shopBackgroundAnimator;

    //Text
    TextMeshProUGUI meetupDialogueText, meetupDialogueChoice1Text, meetupDialogueChoice2Text, statsChangesText;

    // Start is called before the first frame update
    void Start()
    {
        GameObject meetupBackground = GameObject.Find("MeetupBackground");
        GameObject meetupCharacter = GameObject.Find("CharacterSprite");
        GameObject statsChangeBackground = GameObject.Find("StatsChangeBackground");
        GameObject meetupDialogue = GameObject.Find("MeetupDialogue");
        GameObject dialogueChoice1Button = GameObject.Find("DialogueChoice1");
        GameObject dialogueChoice2Button = GameObject.Find("DialogueChoice2");
        GameObject statsChanges = GameObject.Find("StatsChanges");
        GameObject noMeetupBackground = GameObject.Find("NoMeetupBackground");
        GameObject alreadyVisitedBackground = GameObject.Find("AlreadyVisitedBackground");
        GameObject visitAllBackground = GameObject.Find("VisitAllBackground");
        GameObject shopBackground = GameObject.Find("ShopBackground");

        //Sprites
        meetupCharacterSprite = meetupCharacter.GetComponent<Image>();

        //Animators
        meetupBackgroundAnimator = meetupBackground.GetComponent<Animator>();
        statsChangesBackgroundAnimator = statsChangeBackground.GetComponent<Animator>();
        noMeetupBackgroundAnimator = noMeetupBackground.GetComponent<Animator>();
        alreadyVisitedBackgroundAnimator = alreadyVisitedBackground.GetComponent<Animator>();
        visitAllBackgroundAnimator = visitAllBackground.GetComponent<Animator>();
        shopBackgroundAnimator = shopBackground.GetComponent<Animator>();

        //Text
        meetupDialogueText = meetupDialogue.GetComponent<TextMeshProUGUI>();
        meetupDialogueChoice1Text = dialogueChoice1Button.GetComponentInChildren<TextMeshProUGUI>();
        meetupDialogueChoice2Text = dialogueChoice2Button.GetComponentInChildren<TextMeshProUGUI>();
        statsChangesText = statsChanges.GetComponent<TextMeshProUGUI>();

        //Location randomizer
        partyLocations = new string[3];
        List<string> locations = new List<string>() { "Dorms", "Park", "Cafe", "Train station", "Campus" };
        int randomIndex;
        for (int i = 0; i < 3; i++)
        {
            randomIndex = Random.Range(0, locations.Count);
            partyLocations[i] = locations[randomIndex];
            locations.RemoveAt(randomIndex);
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("AAttack", partyStats[0, 0]);
        PlayerPrefs.SetInt("BAttack", partyStats[1, 0]);
        PlayerPrefs.SetInt("CAttack", partyStats[2, 0]);
        PlayerPrefs.SetInt("DAttack", partyStats[3, 0]);
    }

    void OnLocationClick()
    {
        currentPartyMember = LocationChecker(currentLocation);
        if (currentPartyMember > 0)
        {
            visitedPartyMembers += 1;
            MoveToMeetup();
        }
        else
        {
            MoveToNoMeetup();
        }
    }

    int LocationChecker(string location)
    {
        for (int i = 0; i < partyLocations.Length; i++)
        {
            if (partyLocations[i] == location)
            {
                return i + 1;
            }
        }
        return 0;
    }

    void MoveToMeetup()
    {
        switch (currentPartyMember)
        {
            case 1:
                meetupCharacterSprite.sprite = b1Sprite;
                meetupDialogueText.text = bDialogue;
                meetupDialogueChoice1Text.text = bDialogueChoice1;
                meetupDialogueChoice2Text.text = bDialogueChoice2;
                break;
            case 2:
                meetupCharacterSprite.sprite = c1Sprite;
                meetupDialogueText.text = cDialogue;
                meetupDialogueChoice1Text.text = cDialogueChoice1;
                meetupDialogueChoice2Text.text = cDialogueChoice2;
                break;
            case 3:
                meetupCharacterSprite.sprite = d1Sprite;
                meetupDialogueText.text = dDialogue;
                meetupDialogueChoice1Text.text = dDialogueChoice1;
                meetupDialogueChoice2Text.text = dDialogueChoice2;
                break;
        }
        meetupBackgroundAnimator.SetTrigger("MoveIn");
    }

    string StatChanges(bool increase)
    {
        string text = "";

        if (increase)
        {
            text += "ATK: " + partyStats[currentPartyMember, 0] + " + " + statChanges[0] + " = " + (partyStats[currentPartyMember, 0] + statChanges[0]) + "\n";
            text += "DEF: " + partyStats[currentPartyMember, 1] + " + " + statChanges[1] + " = " + (partyStats[currentPartyMember, 1] + statChanges[1]) + "\n";
            text += "CHR: " + partyStats[currentPartyMember, 2] + " + " + statChanges[2] + " = " + (partyStats[currentPartyMember, 2] + statChanges[2]) + "\n";
            text += "SPD: " + partyStats[currentPartyMember, 3] + " + " + statChanges[3] + " = " + (partyStats[currentPartyMember, 3] + statChanges[3]);
            for (int i = 0; i < 4; i++)
            {
                partyStats[currentPartyMember, i] += statChanges[i];
            }
        }
        else
        {
            text += "ATK: " + partyStats[currentPartyMember, 0] + " - " + statChanges[0] + " = " + (partyStats[currentPartyMember, 0] - statChanges[0]) + "\n";
            text += "DEF: " + partyStats[currentPartyMember, 1] + " - " + statChanges[1] + " = " + (partyStats[currentPartyMember, 1] - statChanges[1]) + "\n";
            text += "CHR: " + partyStats[currentPartyMember, 2] + " - " + statChanges[2] + " = " + (partyStats[currentPartyMember, 2] - statChanges[2]) + "\n";
            text += "SPD: " + partyStats[currentPartyMember, 3] + " - " + statChanges[3] + " = " + (partyStats[currentPartyMember, 3] - statChanges[3]);
            for (int i = 0; i < 4; i++)
            {
                partyStats[currentPartyMember, i] -= statChanges[i];
            }
        }

        return text;
    }

    void MoveToNoMeetup()
    {
        noMeetupBackgroundAnimator.SetTrigger("MoveIn");
    }

    void MoveToAlreadyVisited()
    {
        alreadyVisitedBackgroundAnimator.SetTrigger("MoveIn");
    }

    void MoveToVisitAll()
    {
        visitAllBackgroundAnimator.SetTrigger("MoveIn");
    }

    void MoveToShop()
    {
        shopBackgroundAnimator.SetTrigger("MoveIn");
    }

    // *********
    //  Buttons
    // *********

    public void NightclubOnClick()
    {
        if (visitedPartyMembers < 3)
        {
            MoveToVisitAll();
        }
        else
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            SceneManager.LoadScene("gasfighting dungoen map", LoadSceneMode.Additive);
        }
    }

    public void DormsOnClick()
    {
        if (visitedLocations.Contains("Dorms"))
        {
            MoveToAlreadyVisited();
        }
        else
        {
            currentLocation = "Dorms";
            visitedLocations.Add(currentLocation);
            OnLocationClick();
        }
    }
    public void ParkOnClick()
    {
        if (visitedLocations.Contains("Park"))
        {
            MoveToAlreadyVisited();
        }
        else
        {
            currentLocation = "Park";
            visitedLocations.Add(currentLocation);
            OnLocationClick();
        }
    }
    public void CafeOnClick()
    {
        if (visitedLocations.Contains("Cafe"))
        {
            MoveToAlreadyVisited();
        }
        else
        {
            currentLocation = "Cafe";
            visitedLocations.Add(currentLocation);
            OnLocationClick();
        }
    }
    public void TrainStationOnClick()
    {
        if (visitedLocations.Contains("Train station"))
        {
            MoveToAlreadyVisited();
        }
        else
        {
            currentLocation = "Train station";
            visitedLocations.Add(currentLocation);
            OnLocationClick();
        }
    }
    public void CampusOnClick()
    {
        if (visitedLocations.Contains("Campus"))
        {
            MoveToAlreadyVisited();
        }
        else
        {
            currentLocation = "Campus";
            visitedLocations.Add(currentLocation);
            OnLocationClick();
        }
    }
    public void ShopOnClick()
    {
        MoveToShop();
    }

    public void DialogueChoice1OnClick()
    {
        switch (currentPartyMember)
        {
            case 1:
                meetupDialogueText.text = bResponse1;
                statChanges = new int[4] {2, 2, 2, 2};
                break;
            case 2:
                meetupDialogueText.text = cResponse1;
                statChanges = new int[4] {2, 2, 2, 2};
                break;
            case 3:
                meetupDialogueText.text = dResponse1;
                statChanges = new int[4] {2, 2, 2, 2};
                break;
        }
        statsChangesText.text = StatChanges(false);
        Invoke("StatsChangeMoveIn", 3.0f);
    }
    public void DialogueChoice2OnClick()
    {
        switch (currentPartyMember)
        {
            case 1:
                meetupDialogueText.text = bResponse2;
                statChanges = new int[4] {2, 2, 2, 2};
                break;
            case 2:
                meetupDialogueText.text = cResponse2;
                statChanges = new int[4] {2, 2, 2, 2};
                break;
            case 3:
                meetupDialogueText.text = dResponse2;
                statChanges = new int[4] {2, 2, 2, 2};
                break;
        }
        statsChangesText.text = StatChanges(true);
        Invoke("StatsChangeMoveIn", 3.0f);
    }

    void StatsChangeMoveIn()
    {
        meetupDialogueText.text = "";
        statsChangesBackgroundAnimator.SetTrigger("MoveIn");
    }

    public void StatsChangeConfirmationOnClick()
    {
        statsChangesBackgroundAnimator.SetTrigger("MoveOut");
        meetupBackgroundAnimator.SetTrigger("MoveOut");
    }

    public void NoMeetupConfirmationOnClick()
    {
        noMeetupBackgroundAnimator.SetTrigger("MoveOut");
    }

    public void AlreadyVisitedConfirmationOnClick()
    {
        alreadyVisitedBackgroundAnimator.SetTrigger("MoveOut");
    }

    public void VisitAllConfirmationOnClick()
    {
        visitAllBackgroundAnimator.SetTrigger("MoveOut");
    }

    public void ShopConfirmationOnClick()
    {
        shopBackgroundAnimator.SetTrigger("MoveOut");
    }
}