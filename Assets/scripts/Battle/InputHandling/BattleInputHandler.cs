using UnityEngine;
using System.Collections;

public class BattleInputHandler : MonoBehaviour
{
    //базовый для всех классов обрабатывающие input в бою
    public BattleController BattleController;


    public SkillDecorator skillDecorator;

    [HideInInspector]
    public Vector3 mousePosition;

    [HideInInspector]
    public Vector3Int tileMousePosition;

    public Camera CurentCamera;

    protected MainBattleInputManager mainBattleInputManager;

    protected CharacterOutlineSetController outlineController;

    public virtual void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TryCancelInputHandler();
        }
    }

    public virtual void OnHandlerDisabled()
    {
    }

    public virtual void OnBegin()
    {

    }

    public void Awake()
    {
        BattleController = FindObjectOfType<BattleController>();

    }

    protected void Start()
    {
        mainBattleInputManager = MainBattleInputManager.MainInputManager;
        skillDecorator = SkillDecorator.skillDecorator;
        SetUp();
    }

    protected virtual void SetUp()
    {

    }

    public void Update()
    {
        if(mainBattleInputManager != null && mainBattleInputManager.curentInputhandler == this)
        {
            mousePosition = mainBattleInputManager.mousePosition;
            tileMousePosition = mainBattleInputManager.tileMousePosition;
        }



    }

    /// <summary>
    /// Cancel this inputHandler if it is permanent
    /// </summary>
    public virtual void TryCancelInputHandler()
    {

        if (mainBattleInputManager != null && !mainBattleInputManager.AutoSwithing)
        {
            OnCancel();
            mainBattleInputManager.SetToSwithching();
        }
    }

    protected virtual void OnCancel()
    {

    }

    public virtual void MouseSelectedCharacterChanged_Begin(BattleObject newObj)
    {

    }

    public virtual void MouseSelectedCharacterChanged_End(BattleObject oldObj)
    {

    }

}
