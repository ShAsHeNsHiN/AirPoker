using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 起床實作重覆牌可以使用的部分，夫君加油呦~

public class TheMainGameVer2 : MonoBehaviour
{
    public static TheMainGameVer2 Instance{get ; private set;}

    public List<int> pokerTotalNumberList = new List<int>();
    private Dictionary<string , int> pokerDictionary = new Dictionary<string, int>();

    public List<string> pokerNameList = new();
    public List<int> pokerAmountList = new();

    public string enterNumberString;

    [Header("My")]
    private const string MY_NAME = "I";
    public List<int> myPokerTotalNumberList = new List<int>();
    public List<int> myPokerList = new List<int>();
    public List<int> myNoRepeatNumberList = new List<int>();
    private Dictionary<string , int> myPokerDictionary = new Dictionary<string, int>();
    public List<string> myPokerNameList = new();
    public List<int> myPokerAmountList = new();

    [Header("Opponent")]
    private const string OPPONENT_NAME = "Opponent";
    public List<int> opponentPokerTotalNumberList = new List<int>();
    public List<int> opponentPokerList = new List<int>();
    public List<int> opponentNoRepeatNumberList = new List<int>();
    private Dictionary<string , int> opponentPokerDictionary = new Dictionary<string, int>();

    private void Start(){
        Instance = this;
    }

    public void Initial(){
        for(int i = 15 ; i < 51 ; i++){
            pokerTotalNumberList.Add(i);
        }

        int pokerInitialAmount = 4;
        for(int i = 1 ; i < 14 ; i++){
            pokerDictionary.Add(i.ToString() , pokerInitialAmount);
        }

        // myPokerDictionary = pokerDictionary;
        // opponentPokerDictionary = pokerDictionary;
        // 上述兩行程式碼結論:三個字典會變成互通的(每對一個做變化都會回饋到其他的身上)

        myPokerDictionary = new Dictionary<string, int>(pokerDictionary);
        opponentPokerDictionary = new Dictionary<string, int>(pokerDictionary);
    }

    private void GiveBothsidesPokerTotalNumberOneTimes(List<int> plyaerOnePokerTotalNumberList , List<int> plyaerTwoPokerTotalNumberList , List<int> pokerTotalNumberList){
        // 適用於"空氣撲克主架構"的方法2、3
        // 這裡是隨機給予
        if(plyaerOnePokerTotalNumberList.Count == 0 && plyaerTwoPokerTotalNumberList.Count == 0){
            // 雙方為0，表示剛開始或一局的結束
            int x = pokerTotalNumberList[UnityEngine.Random.Range(0 , pokerTotalNumberList.Count)];
            plyaerOnePokerTotalNumberList.Add(x);
            pokerTotalNumberList.RemoveAt(pokerTotalNumberList.FindIndex(z => z == x));

            int y = pokerTotalNumberList[UnityEngine.Random.Range(0 , pokerTotalNumberList.Count)];
            plyaerTwoPokerTotalNumberList.Add(y);
            pokerTotalNumberList.RemoveAt(pokerTotalNumberList.FindIndex(z => z == y));
            
        }
        else{
            // 不能連續拿數字卡，一局一次 
        }
    }

    private bool ValidString(string enterString){
        int count = 0;
        switch(enterString.Length){
            case 2:
                // 給組牌用的
                if(enterString == enterNumberString && Convert.ToInt32(enterString) < 14){
                    foreach(var i in enterString){
                        if(char.IsNumber(i)){
                            count++;
                            if(enterString.Length == count){
                                // 此字串皆為數字
                                return true;
                            }
                        }
                    }
                }
                break;
            case 1:
                // 給組牌用的
                if(char.IsNumber(Convert.ToChar(enterString))){
                    // 此字串皆為數字(雖然人家就只有一個數字)
                    return true;
                }
                break;
        }

        return false;
    }

    private bool NumberCanUsed(string enterNumberString , Dictionary<string , int> targetPokerDictionary){
        if(targetPokerDictionary[enterNumberString] == 0){
            // 此牌為0張，不可再使用
            return false;
        }
        else{
            targetPokerDictionary[enterNumberString]--;
            return true;
        }
    }

    private void UseCardAgain(string enterNumberString , Dictionary<string , int> targetPokerDictionary){
        // int useAgainCardTimes = 0;
        if(targetPokerDictionary[enterNumberString] == 0){
            // 此牌為0張，不可再使用
        }
        else{
            targetPokerDictionary[enterNumberString]--;
        }
    }

    private void MakePoker(string enterNumberString , List<int> targetPokerList , List<int> targetNoReapeatNumberList , List<int> targetPokerTotalNumberList , Dictionary<string , int> targetPokerDictionary){
        // 我覺得遊玩可能是一張一張點擊，所以每次僅能輸入一個數字，總計5次
        if(targetPokerTotalNumberList.Count != 0){
            // 表示玩家有數字卡
            if(targetPokerList.Count < 5){
                // 確保清單僅有5張牌
                if(ValidString(enterNumberString) && NumberCanUsed(enterNumberString , targetPokerDictionary)){

                    int currentNumber = Convert.ToInt32(enterNumberString);

                    if(!targetPokerList.Contains(currentNumber)){
                        targetNoReapeatNumberList.Add(currentNumber);
                    }

                    targetPokerList.Add(currentNumber);
                }
            }
        }
    }

    private void RemoveCard(string enterNumberString , List<int> targetPokerList , List<int> targetNoReapeatNumberList){
        if(ValidString(enterNumberString)){
            int currentNumber = Convert.ToInt32(enterNumberString); 
            if(targetPokerList.Contains(currentNumber)){
                // 移除指定數字，若此數字在這牌裡的話
                targetPokerList.RemoveAt(targetPokerList.FindIndex(x => x == currentNumber));
                if(!targetPokerList.Contains(currentNumber)){
                    // 若牌裡不再有此數字，則將不重覆數字清單中的該數字刪除
                    targetNoReapeatNumberList.RemoveAt(targetNoReapeatNumberList.FindIndex(x => x == currentNumber));
                }
            }
        }
    }

    private bool ValidPoker(List<int> targetPokerList , List<int> targetNoReapeatNumberList , List<int> targetPokerTotalNumberList){
        if(targetPokerList.Count == 5){
            // 當牌組成5張時再判斷總和數字是否等於第1戰的數字卡
            if(targetPokerList.Sum() == targetPokerTotalNumberList[0] && targetNoReapeatNumberList.Count != 1){
                // 想組的牌上數字總和與抽到的數字卡相符
                // 不重覆數字若是1表示有數字選了5次
                targetPokerList.Sort();
                targetNoReapeatNumberList.Sort();

                targetPokerTotalNumberList.RemoveAt(0);

                return true;
            }

            else{
                print("請再選擇一次，牌上數字總和與所選數字不符 或 某個數字有5張牌");
                // return false;
            }
        }

        return false;
    }

    public void UpdatePokerDictionary(Dictionary<string , int> usedPokerDictionary){
        // 更新撲克數量
        // 每局結束時會使用這個函數
        foreach(var (key , value) in usedPokerDictionary.ToList()){
            if(pokerDictionary.ContainsKey(key)){
                // 使用過的牌在撲克牌中
                if(pokerDictionary[key] >= value){
                    // 牌的數量不小於用過牌的數量才可扣除(先不計重覆使用這一部分)
                    pokerDictionary[key] -= value;
                }
            }
        }

        // 每局結束就要更新兩者的字典
        myPokerDictionary = new Dictionary<string, int>(pokerDictionary);
        opponentPokerDictionary = new Dictionary<string, int>(pokerDictionary);
    }

    public void LookPokerDictionary(){
        pokerNameList = pokerDictionary.Keys.ToList();
        pokerAmountList = pokerDictionary.Values.ToList();

        myPokerNameList = myPokerDictionary.Keys.ToList();
        myPokerAmountList = myPokerDictionary.Values.ToList();
    }

    private void Ready(List<int> targetPokerList , List<int> targetNoReapeatNumberList , string whoReady){
        // JudgePoker.Instance.GetPoker(targetPokerList , targetNoReapeatNumberList , whoReady);
    }

    // 這裡以下都是方便我自行測試的函式

    public void Reset(){
        // 快速通關用
        pokerTotalNumberList.Clear();
        // pokerDictionary.Clear();

        pokerNameList.Clear();
        pokerAmountList.Clear();

        enterNumberString = "";

        myPokerList.Clear();
        myPokerTotalNumberList.Clear();
        myNoRepeatNumberList.Clear();
        myPokerNameList.Clear();
        myPokerAmountList.Clear();

        opponentPokerList.Clear();
        opponentPokerTotalNumberList.Clear();
        opponentNoRepeatNumberList.Clear();
    }

    public void IMakePoker(){
        MakePoker(enterNumberString , myPokerList , myNoRepeatNumberList , myPokerTotalNumberList , myPokerDictionary);
    }

    public void IRemoveCard(){
        RemoveCard(enterNumberString , myPokerList , myNoRepeatNumberList);
    }

    public void IReady(){
        if(ValidPoker(myPokerList , myNoRepeatNumberList , myPokerTotalNumberList)){
            Ready(myPokerList , myNoRepeatNumberList , MY_NAME);
        }
    }

    public void OpponentMakePoker(){
        MakePoker(enterNumberString , opponentPokerList , opponentNoRepeatNumberList , opponentPokerTotalNumberList , opponentPokerDictionary);
    }

    public void OpponentReady(){
        if(ValidPoker(opponentPokerList , opponentNoRepeatNumberList , opponentPokerTotalNumberList)){
            Ready(opponentPokerList , opponentNoRepeatNumberList , OPPONENT_NAME);
        }
    }

    public void GiveBothsidesNumber(){
        GiveBothsidesPokerTotalNumberOneTimes(myPokerTotalNumberList , opponentPokerTotalNumberList , pokerTotalNumberList);
    }
}
