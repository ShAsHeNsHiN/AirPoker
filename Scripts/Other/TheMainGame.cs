using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TheMainGame : MonoBehaviour
{
    public static TheMainGame Instance{get ; private set;}

    public List<int> pokerTotalNumberList = new List<int>();
    private Dictionary<string , int> pokerDictionary = new Dictionary<string, int>();

    private string enterPokerTotalNumber;
    public string enterNumberString;

    [Header("My")]
    public List<int> myPokerTotalNumberList = new List<int>();
    public List<int> myPokerList = new List<int>();
    public List<int> myNoRepeatNumberList = new List<int>();
    private Dictionary<string , int> myPokerDictionary = new Dictionary<string, int>();
    private const string MY_NAME = "I";

    [Header("Opponent")]
    public List<int> opponentPokerTotalNumberList = new List<int>();
    public List<int> opponentPokerList = new List<int>();
    public List<int> opponentNoRepeatNumberList = new List<int>();
    private Dictionary<string , int> opponentPokerDictionary = new Dictionary<string, int>();
    private const string OPPONENT_NAME = "Opponent";

    // [Serializable]
    // public struct PlayerPoker{
    //     public List<int> pokerTotalNumberList;
    //     public List<int> pokerList;
    //     public List<int> noRepeatNumberList;
    // }

    public enum Race{
        FiftyCardAndNoOrderNumber ,
        FiftyCardAndGetNumberInEveryPoint ,
        MinusCardAndGetNumberInEveryPoint ,
        MinusCardAndRevealNumberPerTwice
    }

    //  第1局開始
    // (1-1)從4~13中挑出3個數字並扣除1張牌(重點在於讓大數字不要輕鬆碾小數字)
    // (1-2)雙方從各自抽出的4張數字卡中拿出1張來應戰
    // (1-3)雙方根據自己的數字開始從檯面上49張撲克牌組牌
    // (1-4)雙方對決
    // (1-5)結果(勝負及扣血)

    public void Initial(){
        // for(int i = 15 ; i < 51 ; i++){
        //     pokerTotalNumberList.Add(i);
        // }

        int pokerInitialAmount = 4;
        for(int pokerCard = 1 ; pokerCard < 14 ; pokerCard++){
            pokerDictionary.Add(pokerCard.ToString() , pokerInitialAmount);
        }

        List<int> numberCanPickedList = new();
        for(int numberCanPicked = 4 ; numberCanPicked < 14 ; numberCanPicked++){
            numberCanPickedList.Add(numberCanPicked);
        }

        for(int count = 0 ; count < 3 ; count++){
            int pickedNumber = numberCanPickedList[UnityEngine.Random.Range(0 , numberCanPickedList.Count)];
            numberCanPickedList.RemoveAt(numberCanPickedList.FindIndex(z => z == pickedNumber));
            pokerDictionary[pickedNumber.ToString()]--;
        }

        // foreach(var kvp in pokerDictionary){
        //     print(kvp);
        // }

        // pokerNameList = pokerDictionary.Keys.ToList();
        // pokerAmountList = pokerDictionary.Values.ToList();

        // foreach(var (key , value) in pokerDictionary){
        //     print($"{key} is {value}");
        // }

        // foreach(var (key , value) in pokerDictionary){
        //     print(key + value);
        // }

        // foreach(var kvp in pokerDictionary){
        //     print(kvp);
        // }
        // Result:
        // {1 , 4}
        // {2 , 4}
        // {3 , 4}
        // {4 , 4}
        // {5 , 4}
        // ………

        // int i = 0;
        // while(i < 5){
        //     i++;
        //     print(i);
        // }
    }

    private void Start(){
        // Initial();
        for(int i = 15 ; i < 51 ; i++){
            pokerTotalNumberList.Add(i);
        }

        Instance = this;

        // print(pokerDictionary.Count);

        // GetPokerNumberTotal("" , myPokerTotalNumberList);

        // print(enterNumberString[0] - '0' + enterNumberString[1] - '0');

        // print("33 3 255".Length);
        // print(string.IsNullOrWhiteSpace("33 3 255"[2].ToString()));

        // print(string.IsNullOrEmpty(" "));
        // print(string.IsNullOrWhiteSpace(" "));
        // 上者為false , 下者為true

        // print(char.IsNumber("33 3 255"[2]));
        // print(char.IsNumber("33 3 255"[1]));

        // print(string.IsInterned("1 "));

        // foreach(var i in "33333"){
        //     myPokerList.Add(i - '0');
        // }
    }

    private void GetPokerNumberTotal(string enterPokerTotalNumber , List<int> targetPokerTotalNumberList){
        int maxRound = 4;

        if(targetPokerTotalNumberList.Count == maxRound){
            // 比賽只比4回合
        }
        else{
            if(ValidString(enterPokerTotalNumber)){
                int x = Convert.ToInt32(enterPokerTotalNumber);
                if(pokerTotalNumberList.Contains(x)){
                    targetPokerTotalNumberList.Add(x);
                    pokerTotalNumberList.RemoveAt(pokerTotalNumberList.FindIndex(z => z == x));

                    CannotAddPokerTotalNumber();
                }
                else{
                    // 要嘛輸入的值不在PokerTotalNumberList裡頭，要嘛人家的清單已經清空了
                    print("Wrong Value , the value isn't in the pokerTotalNumberList or List is already empty");
                }
            }
            else{
                print("Wrong Value , the string Can't Identify");

                // this.enterPokerTotalNumber = "";
            }
        }

        // this.enterPokerTotalNumber = "";
    }

    private bool ValidString(string enterString){
        int count = 0;
        switch(enterString.Length){
            case 2:
                // 給抽總和數字卡用的(15~50)
                if(enterString == enterPokerTotalNumber){
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
                // 給組牌用的(1~13)
                else if(enterString == enterNumberString && Convert.ToInt32(enterString) < 14){
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

    private void TakePokerTotalNumber(string enterPokerTotalNumber , List<int> targetPokerTotalNumberList){
        // 選擇數字卡
        // 適用於"空氣撲克主架構"的方法1、4
        int currentNumber = Convert.ToInt32(enterPokerTotalNumber);
        int originFirstValueInList = targetPokerTotalNumberList[0];
        if(targetPokerTotalNumberList.Contains(currentNumber)){
            int currentNumberIndex = targetPokerTotalNumberList.FindIndex(x => x == currentNumber);
            if(currentNumberIndex == 0){
                // 此數字本來就在第一個，不用對調
            }

            else{
                // 將此數字與第一項的數字對調
                targetPokerTotalNumberList[currentNumberIndex] = originFirstValueInList;
                targetPokerTotalNumberList[0] = currentNumber;
            }
        }
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

    private void GiveBothsidesPokerTotalNumberFourTimes(List<int> plyaerOnePokerTotalNumberList , List<int> plyaerTwoPokerTotalNumberList , List<int> pokerTotalNumberList){
        // 適用於"空氣撲克主架構"的方法1、4，當玩家不想選擇時即可使用
        if(plyaerOnePokerTotalNumberList.Count == 0 && plyaerTwoPokerTotalNumberList.Count == 0){
            // 雙方為0，表示剛開始
            for(int i = 0 ; i < 4 ; i++){
                int x = pokerTotalNumberList[UnityEngine.Random.Range(0 , pokerTotalNumberList.Count)];
                plyaerOnePokerTotalNumberList.Add(x);
                pokerTotalNumberList.RemoveAt(pokerTotalNumberList.FindIndex(z => z == x));

                int y = pokerTotalNumberList[UnityEngine.Random.Range(0 , pokerTotalNumberList.Count)];
                plyaerTwoPokerTotalNumberList.Add(y);
                pokerTotalNumberList.RemoveAt(pokerTotalNumberList.FindIndex(z => z == y));
            }
        }
        else{
            // 不能連續拿數字卡，一局一次 
        }
    }

    private void GiveComputerTotalNumberFourTimes(List<int> computerPokerTotalNumberList , List<int> pokerTotalNumberList){
        if(computerPokerTotalNumberList.Count == 0){
            // 剛開始
            for(int i = 0 ; i < 4 ; i++){
                int x = pokerTotalNumberList[UnityEngine.Random.Range(0 , pokerTotalNumberList.Count)];
                computerPokerTotalNumberList.Add(x);
                pokerTotalNumberList.RemoveAt(pokerTotalNumberList.FindIndex(z => z == x));
            }
        }
    }

    private void MakePoker(string enterNumberString , List<int> targetPokerList , List<int> targetNoReapeatNumberList , List<int> targetPokerTotalNumberList){
        // 我覺得遊玩可能是一張一張點擊，所以每次僅能輸入一個數字，總計5次
        if(targetPokerTotalNumberList.Count != 0){
            if(targetPokerList.Count < 5){
                // 確保清單僅有5張牌
                if(ValidString(enterNumberString)){

                    int currentNumber = Convert.ToInt32(enterNumberString);

                    if(!targetPokerList.Contains(currentNumber)){
                        targetNoReapeatNumberList.Add(currentNumber);
                    }

                    targetPokerList.Add(currentNumber);
                }
            }
        }

        else{
            // 比賽結束
        }

        // this.enterNumberString = "";
    }

    private int CardOverUsed(List<int> targetPokerList , List<int> targetNoReapeatNumberList , Dictionary<string , int> targetPokerDictionary){
        if(targetNoReapeatNumberList.Count >= 3){
            // 1個數字最多持有3張，也就是絕對不會超過的意思
            return 0;
        }
        else{
            int overUsedTimes = 0;
            foreach(var i in targetNoReapeatNumberList){
                // 唯獨鐵支會自我扣血
                if(targetPokerDictionary[i.ToString()] < targetPokerList.FindAll(x => x == i).Count){
                    // 左邊最高為4，最低為3；右邊最高為4
                    overUsedTimes++;
                    break;
                }
            }

            return overUsedTimes;
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

    private void Ready(List<int> targetPokerList , List<int> targetNoReapeatNumberList , string whoReady){
        // JudgePoker.Instance.GetPoker(targetPokerList , targetNoReapeatNumberList , whoReady , CardOverUsed(targetPokerList , targetNoReapeatNumberList , pokerDictionary));
    }

    private void CannotAddPokerTotalNumber(){
        if(myPokerTotalNumberList.Count == 4 && opponentPokerTotalNumberList.Count == 4){
            pokerTotalNumberList.Clear();
        }
    }

    public void LookPokerDictionary(){
        foreach(var kvp in pokerDictionary){
            print(kvp);
        }
    }

    // 這裡以下都是方便我自行測試的函式

    public void Reset(){
        // 快速通關用
        pokerTotalNumberList.Clear();

        pokerDictionary.Clear();

        myPokerList.Clear();
        // myPokerTotalNumberList.Clear();
        myNoRepeatNumberList.Clear();

        opponentPokerList.Clear();
        // opponentPokerTotalNumberList.Clear();
        opponentNoRepeatNumberList.Clear();

        enterNumberString = "";
        enterPokerTotalNumber = "";
    }

    public void IGetPokerTotalNumber(){
        GetPokerNumberTotal(enterPokerTotalNumber , myPokerTotalNumberList);
    }

    public void ITakePokerTotalNumber(){
        TakePokerTotalNumber(enterPokerTotalNumber , myPokerTotalNumberList);
    }

    public void IMakePoker(){
        MakePoker(enterNumberString , myPokerList , myNoRepeatNumberList , myPokerTotalNumberList);
    }

    public void IRemoveCard(){
        RemoveCard(enterNumberString , myPokerList , myNoRepeatNumberList);
    }

    public void IReady(){
        if(ValidPoker(myPokerList , myNoRepeatNumberList , myPokerTotalNumberList)){
            Ready(myPokerList , myNoRepeatNumberList , MY_NAME);
        }
    }

    public void OpponentGetPokerTotalNumber(){
        GetPokerNumberTotal(enterPokerTotalNumber , opponentPokerTotalNumberList);
    }

    public void OpponentMakePoker(){
        MakePoker(enterNumberString , opponentPokerList , opponentNoRepeatNumberList , opponentPokerTotalNumberList);
    }

    public void OpponentReady(){
        if(ValidPoker(opponentPokerList , opponentNoRepeatNumberList , opponentPokerTotalNumberList)){
            Ready(opponentPokerList , opponentNoRepeatNumberList , OPPONENT_NAME);
        }
    }

    public void GiveBothsidesNumber4Times(){
        // GiveBothsidesPokerTotalNumberOneTimes(myPokerTotalNumberList , opponentPokerTotalNumberList , pokerTotalNumberList);
        GiveBothsidesPokerTotalNumberFourTimes(myPokerTotalNumberList , opponentPokerTotalNumberList , pokerTotalNumberList);
    }
}
