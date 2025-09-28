using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;
using static BattleMain;
using Newtonsoft.Json.Linq;
using UnityEngine.Timeline;

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
        /// 玩家行為動畫
        /// </summary>
        PLAYER_ACTION_ANIMATION,
        /// <summary>
        /// 玩家行為動畫結束
        /// </summary>
        PLAYER_ACTION_ANIMATION_FIN,
        /// <summary>
        /// 敵方行動
        /// </summary>
        ENEMY_ACTION,
        /// <summary>
        /// 敵方行為動畫
        /// </summary>
        ENEMY_ACTION_ANIMATION,
        /// <summary>
        /// 敵方行為動畫結束
        /// </summary>
        ENEMY_ACTION_ANIMATION_FIN,
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
                onChangeBattleState(BattleState.INTO_MISSION);
                break;

            case BattleState.INTO_MISSION:
                battleManager.IntoMission();
                onChangedBatateState();
                onChangeBattleState(BattleState.INTO_ROUND);                    
                break;

            case BattleState.INTO_ROUND:
                battleManager.IntoRound();
                onChangedBatateState();
                onChangeBattleState(BattleState.PLAYER_ACTION_WAIT);
                break;

            case BattleState.PLAYER_ACTION_WAIT:
                //玩家行動等待中
                onChangedBatateState();
                break;

            case BattleState.PLAYER_ACTION:
                //玩家行為行動
                battleManager.playerAction();
                onChangedBatateState();
                onChangeBattleState(BattleState.PLAYER_ACTION_ANIMATION);
                break;
            case BattleState.PLAYER_ACTION_ANIMATION:
                //玩家動畫播放中
                onChangedBatateState();
                break;
            case BattleState.PLAYER_ACTION_ANIMATION_FIN:
                onChangedBatateState();
                if (battleManager.checkIsNextMission())
                {
                    onChangeBattleState(BattleState.ROUND_FIN);
                }
                else
                {
                    onChangeBattleState(BattleState.ENEMY_ACTION);
                }
                break;

            case BattleState.ENEMY_ACTION:
                //敵方行動
                battleManager.enemyAction();
                onChangedBatateState();
                onChangeBattleState(BattleState.ENEMY_ACTION_ANIMATION);
                break;

            case BattleState.ENEMY_ACTION_ANIMATION:
                //敵方動畫撥放中
                onChangedBatateState();
                break;

            case BattleState.ENEMY_ACTION_ANIMATION_FIN:
                onChangedBatateState();
                onChangeBattleState(BattleState.ROUND_FIN);
                break;

            case BattleState.ROUND_FIN:
                battleManager.RoundFin();
                onChangedBatateState();

                if (battleManager.checkIsNextMission())
                {
                    if (battleManager.checkBattleFin())
                    {
                        onChangeBattleState(BattleState.BATTLE_FIN);
                    }
                    else
                    {
                        onChangeBattleState(BattleState.INTO_MISSION);
                    }
                }
                else if (battleManager.checkBattleFin())
                {
                    onChangeBattleState(BattleState.BATTLE_FIN);
                }
                else
                {
                    onChangeBattleState(BattleState.INTO_ROUND);
                }

                break;

            case BattleState.BATTLE_FIN:
                onChangedBatateState();
                onChangeBattleState(BattleState.RESULT);
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


    private void onChangeBattleState(BattleState battleState)
    {
        Debug.Log("changeBattleState : " + battleState);
        var temp = this.battleState;
        this.battleState = battleState;
        OnChangeBattleState?.Invoke(temp, this.battleState);
    }

    /// <summary>
    /// 遊戲開始
    /// </summary>
    public void GameStart()
    {
        onChangeBattleState(BattleState.INIT_BATTLE);
    }

    /// <summary>
    /// 玩家動畫撥放結束
    /// </summary>
    public void PlayerAnimationFin()
    {
        onChangeBattleState(BattleState.PLAYER_ACTION_ANIMATION_FIN);
    }

    /// <summary>
    /// 敵方動畫撥放結束
    /// </summary>
    public void EnemyAnimationFin()
    {
        onChangeBattleState(BattleState.ENEMY_ACTION_ANIMATION_FIN);
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
        onChangeBattleState(BattleState.PLAYER_ACTION);
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

    public BattleAction[] GetPlayerBattleAction()
    {
        return battleManager.GetPlayerBattleAction();
    }

    public BattleAction[] GetEnemyBattleAction()
    {
        return battleManager.GetEnemyBattleAction();
    }

    public bool GetIsWin()
    {
        return battleManager.checkIsWin();
    }
}

class BattleManager : BattleCardProvider
{

    /// <summary>
    /// 卡片系統
    /// </summary>
    BattleCardManager battleCardManager = new BattleCardManager();

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

        battleCardManager.cardProvider = this;
    }

    public Master[] GetCurrentPlayerTeam()
    {
        return currentPlayerTeam;
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
    /// 進入回合
    /// </summary>
    public void IntoRound()
    {
        nowRoundNum++;
        //發牌
        battleCardManager.dealCards();
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

    List<BattleAction> playerBattleActions = new List<BattleAction>();
    List<BattleAction> enemyBattleActions = new List<BattleAction>();
    /// <summary>
    /// 我方行動
    /// </summary>
    public void playerAction()
    {
        playerBattleActions = new List<BattleAction>();
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

            int damage = (int)(master.atk * colorAtkRate);
            currentTargetMaster.hp -= damage;

            playerBattleActions.Add(new BattleAction
            {
                attacker = master,
                target = currentTargetMaster,
                color = card.color,
                damage = damage,
                isTargetDead = !currentTargetMaster.isAlive
            });



            if (!currentTargetMaster.isAlive)
            {
                var flag = Array.IndexOf(currentEnemyTeam, currentTargetMaster);
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
        enemyBattleActions = new List<BattleAction>();

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

            int damage = enemyMaster.atk;
            playerMaster.hp -= damage;

            enemyBattleActions.Add(new BattleAction 
            {
                attacker = enemyMaster,
                target = playerMaster,
                color = Card.CardColor.RED,
                damage = damage,
                isTargetDead = playerMaster.isAlive
            });

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
            battleCardManager.initPlayerCard();
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
    public bool checkBattleFin()
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
        return battleCardManager.currentsRoundCards.ToArray();
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

    /// <summary>
    /// 取得玩家戰鬥動畫
    /// </summary>
    /// <returns></returns>
    public BattleAction[] GetPlayerBattleAction()
    {
        return playerBattleActions.ToArray();
    }

    /// <summary>
    /// 取得敵方戰鬥動畫
    /// </summary>
    /// <returns></returns>
    public BattleAction[] GetEnemyBattleAction()
    {
        return enemyBattleActions.ToArray();
    }
}

/// <summary>
/// 關卡資訊
/// </summary>
public class Mission
{
    public List<Master> enemys = new List<Master>();
}

/// <summary>
/// 單位
/// </summary>
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

    public JObject ToJson()
    {
        JObject json = new JObject();
        json.Add("id", id);
        json.Add("name",name);
        json.Add("max_hp",maxHp);
        json.Add("hp", hp);
        json.Add("atk",atk);
        json.Add("np", np);
        return json;
    }

}

/// <summary>
/// 卡色
/// </summary>
public class Card
{
    public enum CardColor
    { 
        RED,
        BULE,
        GREEN,
    }

    /// <summary>
    /// 卡片id
    /// </summary>
    public string id = "";
    /// <summary>
    /// 單位id
    /// </summary>
    public string mid = "";
    /// <summary>
    /// 卡色
    /// </summary>
    public CardColor color;

    public JObject ToJson()
    {
        JObject json = new JObject();
        json.Add("id",id);
        json.Add("mid",mid);
        json.Add("color", color.ToString());
        return json;
    }
}

/// <summary>
/// 戰鬥動畫
/// </summary>
public class BattleAction
{
    /// <summary>
    /// 攻擊者
    /// </summary>
    public Master attacker;
    /// <summary>
    /// 對象
    /// </summary>
    public Master target;
    /// <summary>
    /// 傷害
    /// </summary>
    public int damage;
    /// <summary>
    /// 卡色
    /// </summary>
    public Card.CardColor color;
    /// <summary>
    /// 對象是否死亡
    /// </summary>
    public bool isTargetDead;
}

public class BattleUnitTestCase
{
    public static Master[] GetPlayerTeam()
    {
        List<Master> playerTeam = new List<Master>();
        Master master = new Master() { id = "1", name = "單位1" };
        master.cards = new List<Card>() { new Card { id = "1",mid = master.id, color = Card.CardColor.RED }, new Card { id = "2", mid = master.id, color = Card.CardColor.RED }, new Card { id = "3", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "4", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "5", mid = master.id, color = Card.CardColor.GREEN } };
        playerTeam.Add(master);
        master = new Master() { id = "2", name = "單位2" };
        master.cards = new List<Card>() { new Card { id = "11", mid = master.id, color = Card.CardColor.RED }, new Card { id = "12", mid = master.id, color = Card.CardColor.RED }, new Card { id = "13", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "14", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "15", mid = master.id, color = Card.CardColor.GREEN } };
        playerTeam.Add(master);
        master = new Master() { id = "3", name = "單位3" };
        master.cards = new List<Card>() { new Card { id = "21", mid = master.id, color = Card.CardColor.RED }, new Card { id = "22", mid = master.id, color = Card.CardColor.RED }, new Card { id = "23", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "24", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "25", mid = master.id, color = Card.CardColor.GREEN } };
        playerTeam.Add(master);

        master = new Master() { id = "4", name = "單位4" };
        master.cards = new List<Card>() { new Card { id = "31", mid = master.id, color = Card.CardColor.RED }, new Card { id = "32", mid = master.id, color = Card.CardColor.RED }, new Card { id = "33", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "34", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "35", mid = master.id, color = Card.CardColor.GREEN } };
        playerTeam.Add(master);
        master = new Master() { id = "5", name = "單位5" };
        master.cards = new List<Card>() { new Card { id = "41", mid = master.id, color = Card.CardColor.RED }, new Card { id = "42", mid = master.id, color = Card.CardColor.RED }, new Card { id = "43", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "44", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "45", mid = master.id, color = Card.CardColor.GREEN } };
        playerTeam.Add(master);
        master = new Master() { id = "6", name = "單位6" };
        master.cards = new List<Card>() { new Card { id = "51", mid = master.id, color = Card.CardColor.RED }, new Card { id = "52", mid = master.id, color = Card.CardColor.RED }, new Card { id = "53", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "54", mid = master.id, color = Card.CardColor.BULE }, new Card { id = "55", mid = master.id, color = Card.CardColor.GREEN } };
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
