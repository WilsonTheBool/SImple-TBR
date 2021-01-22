using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;


public class BattleMessagesController : MonoBehaviour
{
    public static BattleMessagesController messagesController;

    public Canvas battleCanvas;

    BattleController battleController;

    public Text TextPrefab;
    public Text fastmessgae_TextPrefab;

    public Text smallTextStaticPrefab;

    public float maxDelyay;

    private float delay;

    private Queue<TextDraw_Data> drawQueue;

    public Color redColor;
    public Color defaultColor;
    
    private void FixedUpdate()
    {
        if(delay > 0)
        {
            delay -= Time.fixedDeltaTime;
        }
        else
        {
            if(drawQueue.Count > 0)
            {
                DrawFromQueue();
                delay = maxDelyay;
            }
        }
    }

    public Vector3 smallTextOffset;

    public Text DrawStaticTextSmall(Vector3 pos, string message, Color color)
    {
        Text result = Instantiate<Text>(smallTextStaticPrefab, pos, new Quaternion(), battleCanvas.transform);
        result.text = message;
        //result.color = color;

        return result;
    }
    public void Awake()
    {

        if(messagesController == null)
        {
            messagesController = this;
        }
        else
        {
            Destroy(this);
        }

        drawQueue = new Queue<TextDraw_Data>(); 
        battleController = FindObjectOfType<BattleController>();
    }

    public int defultFontSize;

    public void Start()
    {
        //battleController.SomeTakeDamage += BattleController_SomeTakeDamage;
        //battleController.SomeExpGained += BattleController_SomeExpGained;

        //battleController.SomeLevelUp += BattleController_SomeLevelUp;
    }

    private void BattleController_SomeLevelUp(object sender, EventArgs e)
    {
        BattleObject ch = sender as BattleObject;
        if (ch != null)
        {
            ShowText(ch, defaultColor, "Level up!", defultFontSize + 1);
        }
    }

    private void BattleController_SomeExpGained(object sender, Character.ExpGainArgs e)
    {
        BattleObject ch = sender as BattleObject;
        if (ch != null)
        {
            ShowText(ch, defaultColor, $"+ {e.expGainAmmount} Exp", defultFontSize);
        }
    }

    private void DrawFromQueue()
    {
        DrawText(drawQueue.Dequeue());
    }

    public struct TextDraw_Data
    {
        public bool isFast;
        public BattleObject obj;
        public Color color;
        public string message;
        public int size;

        public TextDraw_Data(BattleObject obj, Color color, string message, int size)
        {
            this.obj = obj;
            this.color = color;
            this.message = message;
            this.size = size;
            this.isFast = true;
        }

        public TextDraw_Data(BattleObject obj, Color color, string message, int size, bool isFast)
        {
            this.obj = obj;
            this.color = color;
            this.message = message;
            this.size = size;
            this.isFast = isFast;
        }
    }

    public Vector3 textOffSet;
    private void BattleController_SomeTakeDamage(object sender, Character.TakeDamageEventArgs e)
    {
        Character ch = sender as Character;
        if (ch != null)
        {
            ShowText(ch,redColor, $"-{e.attackData.damage}", defultFontSize);
        }
       
    }

    public void ShowText(BattleObject target, Color color, string message, int size)
    {
        drawQueue.Enqueue(new TextDraw_Data(target, color, message, size));
    }

    public void DrawText(TextDraw_Data data)
    {
        
        Vector3 pos = data.obj.transform.position + textOffSet;
        Text text;
        if (data.isFast)
        {
            text = Instantiate<Text>(fastmessgae_TextPrefab, pos, new Quaternion(), battleCanvas.transform);
        }
        else
        {
            text = Instantiate<Text>(TextPrefab, pos, new Quaternion(), battleCanvas.transform);
        }

        text.text = data.message;
        text.color = data.color;
        text.fontSize = data.size;
    }




}
