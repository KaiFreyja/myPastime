using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;
using static BattleMain;

public class BattleMain : MonoBehaviour
{
    public enum BattleState
    {
        /// <summary>
        /// 等待
        /// </summary>
        WAIT,
        /// <summary>
        /// 進入Battle
        /// </summary>
        INIT_BATTLE,
        /// <summary>
        /// 進入關卡
        /// </summary>
        INTO_MISSION,
        /// <summary>
        /// 進入回合
        /// </summary>
        INTO_ROUND,
        /// <summary>
        /// 玩家行動等待
        /// </summary>
        PLAYER_ACTION_WAIT,
        /// <summary>
        /// 玩家行動
        /// </summary>
        PLAYER_ACTION,
        /// <summary>
        /// 敵方行動
        /// </summary>
        ENEMY_ACTION,
        /// <summary>
        /// 回合結束
        /// </summary>
        ROUND_FIN,
        /// <summary>
        /// 關卡結束
        /// </summary>
        MISSION_FIN,
        /// <summary>
        /// 戰鬥結束
        /// </summary>
        BATTLE_FIN,
        /// <summary>
        /// 結算
        /// </summary>
        RESULT,
    }

    public BattleState state
    {
        get
        {
            return battleState;
        }
    }
    public System.Action<BattleState,BattleState> OnChangeBattleState = null;
    public System.Action<BattleState> OnChangedBattleState = null;
    private BattleState battleState = BattleState.WAIT;
    BattleManager battleManager = new BattleManager();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (battleState)
        {
            case BattleState.WAIT:
                onChangedBatateState();
                break;

            case BattleState.INIT_BATTLE:
                battleManager = new BattleManager();
                battleManager.InitBattle();
                onChangedBatateState();
                changeBattleState(BattleState.INTO_MISSION);
                break;

            case BattleState.INTO_MISSION:
                battleManager.IntoMission();
                onChangedBatateState();
                changeBattleState(BattleState.INTO_ROUND);                    
                break;

            case BattleState.INTO_ROUND:
                battleManager.IntoRound();
                onChangedBatateState();
                changeBattleState(BattleState.PLAYER_ACTION_WAIT);
                break;

            case BattleState.PLAYER_ACTION_WAIT:
                //玩家行動等待中
                onChangedBatateState();
                break;

            case BattleState.PLAYER_ACTION:
                //玩家行為行動
                battleManager.playerAction();
                onChangedBatateState();

                if (battleManager.checkIsNextMission())
                {
                    changeBattleState(BattleState.ROUND_FIN);
                }
                else
                {
                    changeBattleState(BattleState.ENEMY_ACTION);
                }
                break;

            case BattleState.ENEMY_ACTION:
                battleManager.enemyAction();
                onChangedBatateState();

                //敵方行動
                changeBattleState(BattleState.ROUND_FIN);
                break;


            case BattleState.ROUND_FIN:
                battleManager.RoundFin();
                onChangedBatateState();

                if (battleManager.checkIsNextMission())
                {
                    if (battleManager.checkBattleFiin())
                    {
                        changeBattleState(BattleState.BATTLE_FIN);
                    }
                    else
                    {
                        changeBattleState(BattleState.INTO_MISSION);
                    }
                }
                else if (battleManager.checkBattleFiin())
                {
                    changeBattleState(BattleState.BATTLE_FIN);
                }
                else
                {
                    changeBattleState(BattleState.INTO_ROUND);
                }

                break;

            case BattleState.BATTLE_FIN:
                onChangedBatateState();
                changeBattleState(BattleState.RESULT);
                break;

            case BattleState.RESULT:
                onChangedBatateState();
                //changeBattleState(BattleState.WAIT);
                break;
        }
    }

    private BattleState preBattleState = BattleState.WAIT;
    private void onChangedBatateState()
    {
        if (preBattleState != this.battleState)
        {
            Debug.Log("onChangedBatateState : " + battleState);
            preBattleState = this.battleState;
            OnChangedBattleState?.Invoke(this.battleState);
        }
    }


    private void changeBattleState(BattleState battleState)
    {
        Debug.Log("changeBattleState : " + battleState);
        var temp = this.battleState;
        this.battleState = battleState;
        OnChangeBattleState?.Invoke(temp, this.battleState);
    }

    public void GameStart()
    {
        changeBattleState(BattleState.INIT_BATTLE);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="taregtEnemy"></param>
    /// <param name="cards"></param>
    public void PlayerCards(Card[] cards, Master taregtEnemy)
    {
        battleManager.playerCards(cards);
        battleManager.settingTaregt(taregtEnemy);
        changeBattleState(BattleState.PLAYER_ACTION);
    }

    public Master GetCardToMaster(Card card)
    {
        return battleManager.GetCardsToMaster(card);
    }

    public Master[] GetCurrentPlayerMaster()
    {
        return battleManager.GetPlayerTeams();
    }

    public Master[] GetCurrentEnemyMaster()
    {
        return battleManager.GetEnemyTeams();
    }

    public Card[] GetNowRoundCard()
    {
        return battleManager.GetCurrentsRoundCards();        
    }

    public int GetRoundNum()
    {
        return battleManager.GetRoundNum();
    }

    public int GetNowMissionsNum()
    {
        return battleManager.GetNowMissionNum();
    }

    public int GetMaxMissionsNum()
    {
        return battleManager.GetMaxMissionNum();
    }

    public bool GetIsWin()
    {
        return battleManager.checkIsWin();
    }
}

class BattleManager
{
    /// <summary>
    /// 子關卡
    /// </summary>
    Queue<Mission> missions = new Queue<Mission>();

    /// <summary>
    /// 玩家隊伍
    /// </summary>
    List<Master> playerTeam = new List<Master>();

    /// <summary>
    /// 關卡敵方單位
    /// </summary>
    List<Master> missionEnemyTeam = new List<Master>();

    /// <summary>
    /// 最大關卡數
    /// </summary>
    private int maxMissionNum = 0;
    /// <summary>
    /// 現在關卡數
    /// </summary>
    private int nowMissionNum = 0;
    /// <summary>
    /// 現在回合數
    /// </summary>
    private int nowRoundNum = 0;

    public void InitBattle()
    {
        ///取得玩家隊伍
        initPlayerTeam();
        //取得關卡資訊
        initMission();
        maxMissionNum = missions.Count;
    }

    /// <summary>
    /// 取得玩家隊伍
    /// </summary>
    private void initPlayerTeam()
    {
        playerTeam = new List<Master>(BattleUnitTestCase.GetPlayerTeam());
    }

    /// <summary>
    /// 取得關卡資訊
    /// </summary>
    private void initMission()
    {
        missions = new Queue<Mission>(BattleUnitTestCase.GetMission());
    }

    public Mission currentMission = null;
    public Master[] currentPlayerTeam = new Master[3];
    public Master[] currentEnemyTeam = new Master[3];
    /// <summary>
    /// 進入關卡
    /// </summary>
    public void IntoMission()
    {
        nowMissionNum++;

        initCurrentPlayer();
        initCurrentEnemys();
    }

    /// <summary>
    /// 初始化我方單位
    /// </summary>
    private void initCurrentPlayer()
    {
        ///初始話當前出場單位
        if (nowMissionNum == 1)
        {
            currentPlayerTeam = new Master[3];
            for (int i = 0; i < currentPlayerTeam.Length; i++)
            {
                if (playerTeam.Count > i)
                {
                    currentPlayerTeam[i] = playerTeam[i];
                }
            }
        }
    }

    /// <summary>
    /// 初始化敵方單位
    /// </summary>
    private void initCurrentEnemys()
    {
        ///取得現在關卡
        currentMission = missions.Dequeue();

        missionEnemyTeam = new List<Master>(currentMission.enemys);

        currentEnemyTeam = new Master[3];
        for (int i = 0; i < currentEnemyTeam.Length; i++)
        {
            if (missionEnemyTeam.Count > i)
            {
                currentEnemyTeam[i] = missionEnemyTeam[i];
            }
        }
    }

    /// <summary>
    /// 目前牌組
    /// </summary>
    Queue<Card> playersCards = new Queue<Card>();
    /// <summary>
    /// 該回合的卡牌
    /// </summary>
    List<Card> currentsRoundCards = new List<Card>();
    /// <summary>
    /// 進入回合
    /// </summary>
    public void IntoRound()
    {
        nowRoundNum++;
        //發牌
        dealCards();
    }

    /// <summary>
    /// 發牌
    /// </summary>
    private void dealCards()
    {
        if (playersCards.Count < 5)
        {
            initPlayerCard();
        }

        currentsRoundCards = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            currentsRoundCards.Add(playersCards.Dequeue());
        }
    }

    /// <summary>
    /// 初始化牌組順序
    /// </summary>
    private void initPlayerCard()
    {
        playersCards = new Queue<Card>();
        //初始牌型
        var cards = new List<Card>();
        for (int i = 0; i < currentPlayerTeam.Length; i++)
        {
            Master master = currentPlayerTeam[i];
            if (master == null)
            {
                continue;
            }
            for (int j = 0; j < master.cards.Count; j++)
            {
                cards.Add(master.cards[j]);
            }
        }
        //洗牌
        for (int i = 0; i < cards.Count; i++)
        {
            int random = UnityEngine.Random.Range(0, cards.Count - 1);
            var temp = cards[i];
            cards[i] = cards[random];
            cards[random] = temp;
        }

        foreach (var a in cards)
        {
            playersCards.Enqueue(a);
        }
    }

    /// <summary>
    /// 玩家出的牌
    /// </summary>
    Card[] currentPlayerCards;
    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name=""></param>
    public void playerCards(Card[] cards)
    {
        currentPlayerCards = cards;
        Card.CardColor fristColor = cards[0].color;
        if (fristColor == cards[1].color && fristColor  == cards[2].color)
        {
            switch (fristColor)
            {
                case Card.CardColor.RED:
                    break;
                case Card.CardColor.GREEN:
                    break;
                case Card.CardColor.BULE:
                    foreach (var a in currentPlayerTeam)
                    {
                        a.np += 20;
                    }
                    break;
                default:

                    break;
            }
        }
    }

    Master currentTargetMaster = null;

    /// <summary>
    /// 設定攻擊目標
    /// </summary>
    /// <param name="flag"></param>
    public void settingTaregt(Master master)
    {
        currentTargetMaster = master;
    }

    /// <summary>
    /// 我方行動
    /// </summary>
    public void playerAction()
    {
        for (int i = 0;i < currentPlayerCards.Length;i++)
        {
            var card = currentPlayerCards[i];
            Master master = getCardsToMaster(card);
            float colorAtkRate = 1;
            switch (card.color)
            {
                case Card.CardColor.RED:
                    colorAtkRate = 1.3f;
                    break;
                case Card.CardColor.BULE:
                    colorAtkRate = 1f;
                    break;
                case Card.CardColor.GREEN:
                    colorAtkRate = 0.8f;
                    break;
            }
            currentTargetMaster.hp -= (int)(master.atk * colorAtkRate);

            if (!currentTargetMaster.isAlive)
            {
                var flag = Array.IndexOf(currentEnemyTeam, currentTargetMaster);
                //currentEnemyTeam.Remove(currentTargetMaster);
                if (flag >= 0)
                {
                    currentEnemyTeam[flag] = null;
                    
                    //選擇下一個目標
                    for (int j = 0; j < currentEnemyTeam.Length; j++)
                    {
                        if (currentEnemyTeam[j] != null)
                        {
                            currentTargetMaster = currentEnemyTeam[j];
                            break;
                        }
                        if (j == currentEnemyTeam.Length - 1)
                        {
                            Debug.Log("敵方目前單位全滅");
                        }
                    }
                }
            }

        }
    }

    /// <summary>
    /// 用卡色反找單位
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private Master getCardsToMaster(Card card)
    {
        foreach (var a in playerTeam)
        {
            if (a.cards.Contains(card))
            {
                return a;
            }
        }
        return null;
    }

    /// <summary>
    /// 敵方行動
    /// </summary>
    public void enemyAction()
    {
        List<Master> aliveEnemyTeam = new List<Master>();
        for (int j = 0; j < currentEnemyTeam.Length; j++)
        {
            if (currentPlayerTeam[j] != null && currentPlayerTeam[j].isAlive)
            {
                aliveEnemyTeam.Add(currentPlayerTeam[j]);
            }
        }

        //bool isUpdatePlayerCard = false;
        for (int i = 0; i < aliveEnemyTeam.Count; i++)
        {
            List<Master> alivePlayerTeaam = new List<Master>();
            for (int j = 0; j < currentPlayerTeam.Length; j++)
            {
                if (currentPlayerTeam[j] != null && currentPlayerTeam[j].isAlive)
                {
                    alivePlayerTeaam.Add(currentPlayerTeam[j]);
                }
            }
            if (alivePlayerTeaam.Count == 0)
            {
                Debug.LogError("我方現在單位全滅");
                return;
            }


            int flag = UnityEngine.Random.Range(0, aliveEnemyTeam.Count - 1);
            Master enemyMaster = aliveEnemyTeam[flag];

            int taget = UnityEngine.Random.Range(0, alivePlayerTeaam.Count - 1);
            Master playerMaster = alivePlayerTeaam[taget];

            playerMaster.hp -= enemyMaster.atk;
            if (!playerMaster.isAlive)
            {
                int index = Array.IndexOf(currentPlayerTeam,playerMaster);
                currentPlayerTeam[index] = null;
                isUpdatePlayerCard = true;
            }
        }
    }

    /// <summary>
    /// 是否要更新發牌組
    /// </summary>
    bool isUpdatePlayerCard = false;
    public void RoundFin()
    {
        //補齊我方單位
        for (int i = 0; i < currentPlayerTeam.Length; i++)
        {
            if (currentPlayerTeam[i] == null)
            {
                for (int j = 0; j < playerTeam.Count; j++)
                {
                    var playerMaster = playerTeam[j];
                    if (!playerMaster.isAlive)
                    {
                        continue;
                    }
                    int index = Array.IndexOf(currentPlayerTeam, playerMaster);
                    //目前不在隊伍裡
                    if (index < 0)
                    {
                        isUpdatePlayerCard = true;
                        //補上單位
                        currentPlayerTeam[i] = playerMaster;
                        break;
                    }
                }
            }
        }

        //補齊敵方單位
        for (int i = 0; i < currentEnemyTeam.Length; i++)
        {
            if (currentEnemyTeam[i] == null)
            {
                for (int j = 0; j < missionEnemyTeam.Count; j++)
                {
                    var enemyMaster = missionEnemyTeam[j];
                    if (!enemyMaster.isAlive)
                    {
                        continue;
                    }

                    //目前不在隊伍內
                    int index = Array.IndexOf(currentEnemyTeam,enemyMaster);
                    if (index < 0)
                    {
                        currentEnemyTeam[i] = enemyMaster;
                        break;
                    }
                }
            }
        }

        if (isUpdatePlayerCard)
        {
            isUpdatePlayerCard = false;
            ///重新發牌
            initPlayerCard();
        }
    }

    /// <summary>
    /// 檢查是否進入下一關卡
    /// </summary>
    /// <returns></returns>
    public bool checkIsNextMission()
    {
        var enemy = currentMission.enemys;
        foreach (var a in enemy)
        {
            if (a.isAlive)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 是否獲勝
    /// </summary>
    /// <returns></returns>
    public bool checkIsWin()
    {
        foreach (var a in playerTeam)
        {
            if (a.isAlive)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 是否戰鬥結束
    /// </summary>
    /// <returns></returns>
    public bool checkBattleFiin()
    {
        bool isEnenyDied = true;
        bool isPlayerDied = true;
        if (missions.Count <= 0)
        {
            for (int i = 0; i < missionEnemyTeam.Count; i++)
            {
                if (missionEnemyTeam[i].isAlive)
                {
                    isEnenyDied = false;
                    break;
                }
            }
        }
        else
        {
            isEnenyDied = false;
        }
        foreach (var a in playerTeam)
        {
            if (a.isAlive)
            {
                isPlayerDied = false;
                break;
            }
        }

        return isEnenyDied || isPlayerDied;
    }

    /// <summary>
    /// 取得回合發牌
    /// </summary>
    public Card[] GetCurrentsRoundCards()
    {
        return currentsRoundCards.ToArray();
    }

    public Master GetCardsToMaster(Card card)
    {
        return getCardsToMaster(card);
    }

    /// <summary>
    /// 取得現在單位
    /// </summary>
    /// <returns></returns>
    public Master[] GetPlayerTeams()
    {
        return currentPlayerTeam;
    }

    /// <summary>
    /// 取得敵方現在單位
    /// </summary>
    /// <returns></returns>
    public Master[] GetEnemyTeams()
    {
        return currentEnemyTeam;
    }

    /// <summary>
    /// 取得關卡數
    /// </summary>
    /// <returns></returns>
    public int GetMaxMissionNum()
    {
        return maxMissionNum;
    }

    /// <summary>
    /// 取得現在關卡
    /// </summary>
    /// <returns></returns>
    public int GetNowMissionNum()
    {
        return nowMissionNum;
    }

    /// <summary>
    /// 取得現在回合
    /// </summary>
    /// <returns></returns>
    public int GetRoundNum()
    {
        return nowRoundNum;
    }
}

/// <summary>
/// 關卡資訊
/// </summary>
public class Mission
{
    public List<Master> enemys = new List<Master>();
}

public class Master
{
    public string id = "";
    public string name;
    public int maxHp = 1000;
    public int hp = 1000;
    public int atk = 100;
    public int np = 0;

    public List<Card> cards = new List<Card>();

    public bool isAlive
    {
        get
        {
            return hp > 0;
        }
    }

}

public class Card
{
    public enum CardColor
    { 
        RED,
        BULE,
        GREEN,
    }
    public string id = "";
    public CardColor color;
}

public class BattleUnitTestCase
{
    public static Master[] GetPlayerTeam()
    {
        List<Master> playerTeam = new List<Master>();
        Master master = new Master() { id = "1", name = "單位1" };
        master.cards = new List<Card>() { new Card { id = "1", color = Card.CardColor.RED }, new Card { id = "2", color = Card.CardColor.RED }, new Card { id = "3", color = Card.CardColor.BULE }, new Card { id = "4", color = Card.CardColor.BULE }, new Card { id = "5", color = Card.CardColor.GREEN } };
        playerTeam.Add(master);
        master = new Master() { id = "2", name = "單位2" };
        master.cards = new List<Card>() { new Card { id = "11", color = Card.CardColor.RED }, new Card { id = "12", color = Card.CardColor.RED }, new Card { id = "13", color = Card.CardColor.BULE }, new Card { id = "14", color = Card.CardColor.BULE }, new Card { id = "15", color = Card.CardColor.GREEN } };
        playerTeam.Add(master);
        master = new Master() { id = "3", name = "單位3" };
        master.cards = new List<Card>() { new Card { id = "21", color = Card.CardColor.RED }, new Card { id = "22", color = Card.CardColor.RED }, new Card { id = "23", color = Card.CardColor.BULE }, new Card { id = "24", color = Card.CardColor.BULE }, new Card { id = "25", color = Card.CardColor.GREEN } };
        playerTeam.Add(master);

        master = new Master() { id = "4", name = "單位4" };
        master.cards = new List<Card>() { new Card { id = "31", color = Card.CardColor.RED }, new Card { id = "32", color = Card.CardColor.RED }, new Card { id = "33", color = Card.CardColor.BULE }, new Card { id = "34", color = Card.CardColor.BULE }, new Card { id = "35", color = Card.CardColor.GREEN } };
        playerTeam.Add(master);

        master = new Master() { id = "5", name = "單位5" };
        master.cards = new List<Card>() { new Card { id = "41", color = Card.CardColor.RED }, new Card { id = "42", color = Card.CardColor.RED }, new Card { id = "43", color = Card.CardColor.BULE }, new Card { id = "44", color = Card.CardColor.BULE }, new Card { id = "45", color = Card.CardColor.GREEN } };
        playerTeam.Add(master);
        master = new Master() { id = "6", name = "單位6" };
        master.cards = new List<Card>() { new Card { id = "51", color = Card.CardColor.RED }, new Card { id = "52", color = Card.CardColor.RED }, new Card { id = "53", color = Card.CardColor.BULE }, new Card { id = "54", color = Card.CardColor.BULE }, new Card { id = "55", color = Card.CardColor.GREEN } };
        playerTeam.Add(master);

        return playerTeam.ToArray();
    }

    public static Mission[] GetMission()
    {
        List<Mission> missions = new List<Mission>();
        missions.Add(new Mission { enemys = new List<Master>() { new Master { id = "11", name = "敵方單位11" }, new Master { id = "12", name = "敵方單位12" }, new Master { id = "13", name = "敵方單位13" }, new Master { id = "13", name = "敵方單位14" } } });
        missions.Add(new Mission { enemys = new List<Master>() { new Master { id = "21", name = "敵方單位21" }, new Master { id = "22", name = "敵方單位22" }, new Master { id = "23", name = "敵方單位23" } } });
        missions.Add(new Mission { enemys = new List<Master>() { new Master { id = "31", name = "敵方單位31" }, new Master { id = "32", name = "敵方單位32" }, new Master { id = "33", name = "敵方單位33" } } });
        return missions.ToArray();
    }
}
