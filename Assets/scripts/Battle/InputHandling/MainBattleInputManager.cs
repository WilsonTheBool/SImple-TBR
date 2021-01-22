using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using Assets.scripts.UI.BattleCharacterSheet;

public class MainBattleInputManager : MonoBehaviour
{
    public static MainBattleInputManager MainInputManager { get; private set; }

    private BattleController BattleController;

    [SerializeField]
    private CharacterSheetController characterSheetController;

    private PathFinder PathFinder;

    [HideInInspector]
    public Vector3 mousePosition;

    [HideInInspector]
    public Vector3Int tileMousePosition;

    public Camera CurentCamera;

    public CursorController CursorController;
    
    public BattleInputHandler curentInputhandler;

    public BattleMeleeAttackInputHandfler meleeInput;

    public BattleRangeAttackInputHandler rangedInput;

    public BattleMovementController movementInput;

    public BattleGetInfoHandler infoInput;

    public event EventHandler<SelectedChangedArgs> MouseSelectedObjChanged;
    //Choosing Hero Input;

    public bool AutoSwithing = true;

    private void Awake()
    {
        if (MainInputManager != null)
        {
            Destroy(this);
        }
        else
        {
            MainInputManager = this;
        }
    }

    void Start()
    {
        

        BattleController = GetComponentInParent<BattleController>();

        PathFinder = BattleController.PathFinder;
        outlineController = CharacterOutlineSetController.outlineSetController;
        CursorController = CursorController.cursorController; 

        if(characterSheetController == null)
        {
            FindObjectOfType<CharacterSheetController>();
        }

        MouseSelectedObjChanged += MainBattleInputManager_MouseSelectedObjChanged;
    }

    protected CharacterOutlineSetController outlineController;
    protected BattleObject lastSelected;

    /// <summary>
    /// событие выбора мышью нового персонажа
    /// </summary>
    /// <param name="sender"></param> 
    /// <param name="e"></param>
    private void MainBattleInputManager_MouseSelectedObjChanged(object sender, SelectedChangedArgs e)
    {
        HandleSelectedChangedGeneral(e.oldObj as Character, e.newObj as Character);
        lastHandler?.MouseSelectedCharacterChanged_End(e.oldObj);
            curentInputhandler?.MouseSelectedCharacterChanged_Begin(e.newObj);

    }

    /// <summary>
    /// Обрабатывает событие изменения выбранного (мышью) персонажа;
    /// включает общие действия для всех InputHandler-ов
    /// </summary>
    private void HandleSelectedChangedGeneral(Character oldCh, Character newCh)
    {
        if(newCh != null)
            ShowSelectedStats(newCh);

        if (oldCh != null)
            HideSelectedStats(oldCh);

    }

    private void ShowSelectedStats(Character ch)
    {
        ch.stats.ShowAllStats();
    }

    private void HideSelectedStats(Character ch)
    {
        ch.stats.HideToShort();
    }

    public void SetInputPermanent(BattleInputHandler input)
    {
        curentInputhandler = input;
        
        AutoSwithing = false;

        input.OnBegin();
    }

    /// <summary>
    /// Set AutoSwitching to false and curentInput to null
    /// </summary>
    public void SetToSwithching()
    {
        curentInputhandler = null;
        AutoSwithing = true;
    }

    void Update()
    {
       // if(BattleController != null)
        ClearTileMaps();
        UpdateInputData();
        if (AutoSwithing)
        {
            UpdateInputHandler();
        }

        if(curentInputhandler != null)
        {
            curentInputhandler.HandleInput();
        }


    }
    void ClearTileMaps()
    {
        BattleController.attackTileMap.ClearAllTiles();

        BattleController.pathTileMap.ClearAllTiles();
    }

    void UpdateInputData()
    {
        mousePosition = CurentCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        tileMousePosition = BattleController.floorTilemap.WorldToCell(mousePosition);
    }
    void UpdateInputHandler()
    {
        
        //if(BattleController == null)
        //{
        //    BattleController = GetComponentInParent<BattleController>();
        //    PathFinder = BattleController.PathFinder;
        //}

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //UI Input Controller (nothing)
        }
        else
        {
            if (BattleController.SomeoneActing)
            {

                curentInputhandler = null;
                CursorController.SetCursor(CursorController.CursorType.wait);
            }
            else
            {
                BattleObject obj = BattleController.GetObjectOnTile(tileMousePosition);


                curentInputhandler = null;

                if (BattleController.selectedCharacter != null)
                {
                    

                    bool isPointerTileInRange = PathFinder.SavedNodeContainsPosition(tileMousePosition);
                    bool isPointerTileEmpty = obj == null;
                    bool isPointerOverEnemy = false;
                    bool isPointerOverEnemyInRange = false;
                    bool isPoinerOverCharacter = false;
                    bool isSelectedMelee = BattleController.selectedCharacter is IMeleeAttacker;
                    bool isSelectedRange = BattleController.selectedCharacter is IRangeAttacker;
                    bool isSelectedRangeAndCanRangeAttack = isSelectedRange && (BattleController.selectedCharacter as RangedCharacter).CanRangeAttack();






                    if (!isPointerTileEmpty)
                    {
                        

                        isPoinerOverCharacter = obj is Character;
                        isPointerOverEnemy = obj.team == BattleObject.Team.enemy;
                        isPointerOverEnemyInRange = obj != null && isPointerOverEnemy && HasClosestTileInRange(tileMousePosition);
                    }

                   

                    //если клетка пустая и на нее можно сходить
                    if (isPointerTileInRange && isPointerTileEmpty)
                    {
                        curentInputhandler = movementInput;
                        CursorController.SetCursor(CursorController.CursorType.boots);
                    }

                    if (isSelectedRangeAndCanRangeAttack)
                    {
                        if (!isPointerTileEmpty && isPointerOverEnemy)
                        {
                            curentInputhandler = rangedInput;
                            CursorController.SetCursor(CursorController.CursorType.crossbow);
                        }

                    }
                    else
                    {
                        if (!isPointerTileEmpty && isPointerOverEnemyInRange && isSelectedMelee)
                        {
                            curentInputhandler = meleeInput;
                            CursorController.SetCursor(CursorController.CursorType.sword);
                        }

                        if (!isPointerTileEmpty && !isPointerOverEnemyInRange)
                        {
                            curentInputhandler = null;
                            CursorController.SetCursor(CursorController.CursorType.def);
                        }


                    }

                    //if input didnt change this frame
                    if(curentInputhandler == null)
                    {
                        //pointed over an object to get info
                        if ((!isPointerTileEmpty && !isPointerOverEnemy && isPoinerOverCharacter) || (isPointerOverEnemy && !isPointerOverEnemyInRange))
                        {
                            curentInputhandler = infoInput;
                            CursorController.cursorController.SetCursor(CursorController.CursorType.info);
                        }

                        if (isPointerTileEmpty && !isPointerTileInRange)
                        {
                            curentInputhandler = null;
                            CursorController.cursorController.SetCursor(CursorController.CursorType.def);
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        //вызвать CarecterSheet
                        if (isPoinerOverCharacter)
                        {
                            characterSheetController.UpdateUI(BattleController.GetObjectOnTile<Character>(tileMousePosition));
                        }
                        else
                        {
                            characterSheetController.CloseInfoWindow();
                        }
                    }

                }
                else
                {
                    curentInputhandler = null;
                    CursorController.cursorController.SetCursor(CursorController.CursorType.def);
                }


                if (obj != lastSelected)
                {
                    MouseSelectedObjChanged?.Invoke(this, new SelectedChangedArgs(obj, lastSelected));
                    lastSelected = obj;
                }
            }
           

        }

       
        if(curentInputhandler != lastHandler)
        {
            lastHandler?.OnHandlerDisabled();
        }

        lastHandler = curentInputhandler;
    }

    private BattleInputHandler lastHandler;
    private bool HasClosestTileInRange(Vector3Int Tile)
    {


        Vector3Int neighbour;

        neighbour = Tile + new Vector3Int(1, 0, 0);
        if (BattleController.PathFinder.SavedNodeContainsPosition(neighbour))
        {
            return true;
        }

        neighbour = Tile + new Vector3Int(-1, 0, 0);
        if (BattleController.PathFinder.SavedNodeContainsPosition(neighbour))
        {
            return true;
        }

        neighbour = Tile + new Vector3Int(0, 1, 0);
        if (BattleController.PathFinder.SavedNodeContainsPosition(neighbour))
        {
            return true;
        }

        neighbour = Tile + new Vector3Int(0, -1, 0);
        if (BattleController.PathFinder.SavedNodeContainsPosition(neighbour))
        {
            return true;
        }

        return false;
    }

    public class SelectedChangedArgs : EventArgs
    {
        public BattleObject newObj;

        public BattleObject oldObj;

        public SelectedChangedArgs(BattleObject newObj, BattleObject oldObj)
        {
            this.newObj = newObj;
            this.oldObj = oldObj;
        }
    }
}
