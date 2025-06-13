using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleMainUnitCase : MonoBehaviour
{
    BattleMain main = null;

    Master[] players = null;
    Master[] enemys = null;
    Card[] cards = new Card[5];
    int maxMissionsNum = 0;
    int nowMissionsNum = 0;
    int nowRound = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        main = GetComponent<BattleMain>();
        main.OnChangedBattleState = (BattleMain.BattleState state) =>
        {
            switch (state)
            {
                case BattleMain.BattleState.INTO_ROUND:
                    maxMissionsNum = main.GetMaxMissionsNum();
                    nowMissionsNum = main.GetNowMissionsNum();
                    nowRound = main.GetRoundNum();
                    ///取得我方上場單位
                    players = main.GetCurrentPlayerMaster();
                    //取得敵方上場單位
                    enemys = main.GetCurrentEnemyMaster();
                    if (enemys[selectEnemy] == null)
                    {
                        for (int i = 0; i < enemys.Length; i++)
                        {
                            if (enemys[i] != null)
                            {
                                selectEnemy = i;
                                break;
                            }
                        }
                    }
                    //取得這回合的牌
                    cards = main.GetNowRoundCard();
                    break;

                case BattleMain.BattleState.PLAYER_ACTION_WAIT:
                    isSelectCard = false;
                    selectCards = new List<Card>();
                    break;

                case BattleMain.BattleState.PLAYER_ACTION_ANIMATION:
                    Debug.Log("播放玩家動畫");
                    isTiming = true;
                    actions = main.GetPlayerBattleAction();
                    break;

                case BattleMain.BattleState.ENEMY_ACTION_ANIMATION:
                    Debug.Log("撥放敵方動畫");
                    isTiming = true;
                    actions = main.GetEnemyBattleAction();
                    break;
            }
        };
    }

    BattleAction[] actions = null;

    bool isTiming = false;
    float dtTime = 0;
    // Update is called once per frame
    void Update()
    {
        if (isTiming)
        {
            dtTime += Time.deltaTime;
            if (dtTime > 2)
            {
                isTiming = false;
                dtTime = 0;
                switch (main.state)
                {
                    case BattleMain.BattleState.PLAYER_ACTION_ANIMATION:
                        main.PlayerAnimationFin();
                        break;

                    case BattleMain.BattleState.ENEMY_ACTION_ANIMATION:
                        main.EnemyAnimationFin();
                        break;
                }
            }
        }
    }

    private void OnGUI()
    {
        switch (main.state)
        {
            case BattleMain.BattleState.WAIT:
                uiWait();
                break;
            case BattleMain.BattleState.PLAYER_ACTION_WAIT:
                uiShowMissionData();
                uiShowMaster();
                if (isSelectCard)
                {
                    uiPlayerActionWaitSelectCard();
                }
                else
                {
                    uiPlayerActionWait();
                }
                break;

            case BattleMain.BattleState.PLAYER_ACTION_ANIMATION:
                uiShowMissionData();
                uiShowMaster();
                uiPlayerAnimation();
                break;

            case BattleMain.BattleState.ENEMY_ACTION_ANIMATION:
                uiShowMissionData();
                uiShowMaster();
                uiEnemyAnimation();
                break;

            case BattleMain.BattleState.RESULT:
                uiResult();
                break;
        }
    }

    private void uiWait()
    {
        if (GUI.Button(new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200), "開始"))
        {
            main.GameStart();
        }
    }
    bool isSelectCard = false;
    List<Card> selectCards = new List<Card>();
    private void uiPlayerActionWaitSelectCard()
    {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

        int w = 150;
        int h = 150;
        for (int i = 0; i < cards.Length; i++)
        {
            Card card = cards[i];
            bool isSelect = selectCards.Contains(card);
            Master master = main.GetCardToMaster(card);
            string text = master.name + "\n" + (isSelect ? "[" + card.color.ToString() + "]" + "\n" + selectCards.IndexOf(card) : card.color.ToString());
            if (GUI.Button(new Rect(Screen.width / (cards.Length + 1) * (i + 1) - w / 2, (Screen.height - h) / 2, w, h), text))
            {
                if (isSelect)
                {
                    selectCards.Remove(card);
                }
                else
                {
                    selectCards.Add(card);
                    if (selectCards.Count == 3)
                    {
                        main.PlayerCards(selectCards.ToArray(), enemys[selectEnemy]);
                        isSelectCard = false;
                    }
                }
            }
        }

        w = 100;
        h = 100;
        if (GUI.Button(new Rect(Screen.width - w, Screen.height - h, w, h), "取消"))
        {
            isSelectCard = false;
        }
    }

    private void uiPlayerActionWait()
    {
        int w = 100;
        int h = 100;

        if (GUI.Button(new Rect(Screen.width - w, Screen.height - h, w, h), "出牌"))
        {
            isSelectCard = true;
        }
    }

    int selectEnemy = 0;

    private void uiShowMaster()
    {
        int w = 100;
        int h = 200;

        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];

            if (player == null)
            {
                //該站位無單位
            }
            else
            {
                GUI.Box(new Rect(w * (i + 1) + 20 * i, Screen.height - h, w, h), player.name + "\n" + "hp " + player.hp + "/" + player.maxHp + "\n" + "np " + player.np + "/100");
            }
        }

        for (int i = 0; i < enemys.Length; i++)
        {
            var enemy = enemys[i];
            if (enemy == null)
            {
                //該站位無單位
            }
            else
            {
                string name = (selectEnemy == i) ? "[" + enemy.name + "]" : enemy.name;
                if (GUI.Button(new Rect(w * (i + 1) + 20 * i, 0, w, h), name + "\n" + enemy.hp + "/" + enemy.maxHp))
                {
                    selectEnemy = i;
                }
            }
        }
    }
    private void uiShowMissionData()
    {
        int w = 100;
        int h = 100;

        GUI.Box(new Rect(Screen.width - w, 0, w, h), "[" + nowMissionsNum + "/" + maxMissionsNum + "]" + "\n" + "Round : " + nowRound);
        
    }

    private void uiPlayerAnimation()
    {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "玩家動畫播放中");
    }
    private void uiEnemyAnimation()
    {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "敵方動畫播放中");
    }

    private void uiResult()
    {
        bool isWin = main.GetIsWin();
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), isWin?"Win":"Lose");

        int w = 100;
        int h = 100;
        if (GUI.Button(new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h), "重新開始"))
        {
            main.GameStart();
        }
    }
}
