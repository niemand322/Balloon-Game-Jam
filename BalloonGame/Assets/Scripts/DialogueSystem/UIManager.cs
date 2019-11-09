using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject container_NPC; // Container to activate/deactivate everything for the NPC or the player at once.
    public GameObject container_Player;
    public TextMeshProUGUI text_NPC;
    public TextMeshProUGUI label_NPC;
    public TextMeshProUGUI[] text_Choices; // The text on the buttons for the player's decisions
    [SerializeField]
    private bool[] displayChoice; // Which possible decisions are available to the player.
    public Image NPCSprite;
    public Image playerSprite;

    public int defaulFontSize = 14;
    public Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
        NPCSprite.gameObject.SetActive(false);
        playerSprite.gameObject.SetActive(false);

        displayChoice = new bool[text_Choices.Length];
        for (int i = 0; i < displayChoice.Length; i++)
        {
            displayChoice[i] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!VD.isActive)
            {
                Begin(); // At the moment the dialog is triggered by pressing the Enter key, this must be replaced in the finished game by something else.
            }
            else
            {
                // If the player must decide, then Enter will simply take the first selection 
                // One could also check if it's the player's turn, but then one could no longer confirm with Enter.
                VD.Next(); 
            }
        }
    }
    void Begin()
    {
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += End;
        // This loads the VIDE_Assign into the VD. 
        //If you want to play other dialogs, you would have to select a different VIDE_Assign here.
        VD.BeginDialogue(GetComponent<VIDE_Assign>());
    }

    void UpdateUI(VD.NodeData data)
    {

        container_NPC.SetActive(false); // This statement should be removed if you want to continue displaying the question during answer selection..
        container_Player.SetActive(false);

        //NPCSprite.gameObject.SetActive(false); // Not necessary, as the container takes care of this.
        //playerSprite.gameObject.SetActive(false);
        playerSprite.sprite = null;
        NPCSprite.sprite = null;

        //Look for dynamic text change in extraData
        PostConditions(data);

        if (data.isPlayer)
        {
            playerSprite.gameObject.SetActive(true);

            // Set a specific player sprite if there is one, otherwise set the default player sprite.
            if (data.sprite != null)
            {
                playerSprite.sprite = data.sprite;
            } else if(VD.assigned.defaultPlayerSprite != null)
            {
                playerSprite.sprite = VD.assigned.defaultPlayerSprite;
            }

            // The buttons are written here.
            container_Player.SetActive(true);
            for (int i = 0; i < text_Choices.Length; i++)
            {
                if (i < data.comments.Length)
                {
                    text_Choices[i].transform.parent.gameObject.SetActive(displayChoice[i]); // The buttons are only called if displayChoice allows it.
                    text_Choices[i].text = data.comments[i];

                    // One could play an audio clip here, but then one would still have to work with the fact that the dialog system changes immediately to the next text after the selection.
                    // Probably it would be easier to repeat the text in the next NPC node and then play the sound there.
                }
                else
                {
                    text_Choices[i].transform.parent.gameObject.SetActive(false); // If there is no text for the buttons, the buttons will not be activated.
                }

            }
            // One should probably change this so that not the first button is selected, but the first button that is active.
            text_Choices[0].transform.parent.GetComponent<Button>().Select(); 
        }
        else
        {

            NPCSprite.gameObject.SetActive(true);

            // The sprite of the NPCs is chosen here.
            if (data.sprite != null)
            {
                // With the keyword sprite you can give a line a special sprite.
                // Probably one should not pass an int, but a string, so that one can specify several lines at once.
                if (data.extraVars.ContainsKey("sprite"))
                {
                    if (data.commentIndex == (int)data.extraVars["sprite"])
                        NPCSprite.sprite = data.sprite;
                    else if (VD.assigned.defaultNPCSprite != null)
                        NPCSprite.sprite = VD.assigned.defaultNPCSprite;
                }
                else
                {
                    NPCSprite.sprite = data.sprite;
                }
            }
            else if(VD.assigned.defaultNPCSprite != null)
            {
                NPCSprite.sprite = VD.assigned.defaultNPCSprite;
            }

            // Writes the text of the NPC.
            container_NPC.SetActive(true);
            text_NPC.text = data.comments[data.commentIndex];

            // Appropriate spot to play an audio clip of an NPC.

            // If there's a tag, it's used to label the speaker. Otherwise it is the default label.
            if (data.tag.Length > 0)
                label_NPC.text = data.tag;
            else
                label_NPC.text = VD.assigned.alias;
        }

    }
    void End(VD.NodeData data)
    {
        // Resets everything after the conversation is finished.
        container_NPC.SetActive(false);
        container_Player.SetActive(false);

        NPCSprite.sprite = null;
        playerSprite.sprite = null;
        NPCSprite.gameObject.SetActive(false);
        playerSprite.gameObject.SetActive(false);
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
        VD.EndDialogue();
    }

    private void OnDisable()
    {
        // This is to clean up the dialog if the conversation is ended unexpectedly. The program is closed/ you exit play mode.

        if (container_NPC != null) // This method seems suspicious to me. But it's from the VIDE tutorial. So I won't argue about it.
        {
            End(null);
        }
    }

    public void SetPlayerChoice(int choice)
    {
        // This function is called by a button and passes the player's choice.
        VD.nodeData.commentIndex = choice;
        if (Input.GetMouseButtonUp(0))
            VD.Next();
    }

    public void FeedBools(string bools)
    {
        // This function can be used by an action node to enable or disable certain specified response options.
        string[] boolList = bools.Split(',');
        for (int i = 0; i < boolList.Length; i++)
        {
            if (boolList[i] != "true" && boolList[i] != "false") continue;
            bool var;
            bool.TryParse(boolList[i], out var);
            displayChoice[i] = var;
        }
    }

    public void FeedBools((int, bool) result)
    {
        // This function works like the above one, but the data does not have to be converted into a string first.
        displayChoice[result.Item1] = result.Item2;
    }


    void PostConditions(VD.NodeData data)
    {
        // If you want to replace individual words in the text, that would be a good place for it.
        // ReplaceWord(data); //

        if (!data.isPlayer)
        {
            // The instructions for the appearance of the text must be entered in the nodes in the form "fs,40,bold,italic,color,255,0,0".
            // The order of the individual commands is unimportant.
            string[] extra = data.extraData[data.commentIndex].Split(',');


            // Here, the text size is read from the ExtraData.
            if (data.extraData[data.commentIndex].Contains("fs"))
            {
                int fSize = defaulFontSize;
                int.TryParse(extra[System.Array.IndexOf(extra, "fs") + 1], out fSize);
                text_NPC.fontSize = fSize;
                text_NPC.fontStyle = TMPro.FontStyles.Bold;

            }
            else
            {
                text_NPC.fontSize = defaulFontSize;
            }

            // Here it is checked whether the text should be bold or italic.
            bool bold = data.extraData[data.commentIndex].Contains("bold");
            bool italic = data.extraData[data.commentIndex].Contains("italic");


            text_NPC.fontStyle = TMPro.FontStyles.Normal;

            if (bold)
            {
                text_NPC.fontStyle |= TMPro.FontStyles.Bold;
            }
            if (italic)
            {
                text_NPC.fontStyle |= TMPro.FontStyles.Italic;
            }

            // Here the color of the text is checked.
            if (data.extraData[data.commentIndex].Contains("color"))
            {
                int.TryParse(extra[System.Array.IndexOf(extra, "color") + 1], out int colorR);
                int.TryParse(extra[System.Array.IndexOf(extra, "color") + 2], out int colorG);
                int.TryParse(extra[System.Array.IndexOf(extra, "color") + 3], out int colorB);

                Color textColor = new Color(colorR,colorG,colorB);
                text_NPC.color = textColor;

            }
            else
            {
                text_NPC.color = defaultColor;
            }

        }
        else
        {
            // If you want to allow the change of the text also for the answer choices of the player, you would have to add it here.
        }
    }
}
