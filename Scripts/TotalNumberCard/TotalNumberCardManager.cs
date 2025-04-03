using System;

public static class TotalNumberCardManager
{
    public static event EventHandler<TotalNumberCardInformation> OnPlayerGetNumberInChoosePokerTotalNumberScene;

    public static event EventHandler<TotalNumberCardInformation> OnPlayerGetNumberInMakePokerScene;

    public class TotalNumberCardInformation : EventArgs
    {
        public static readonly new TotalNumberCardInformation Empty = new();

        public int TotalNumberCardNumber;
    }

    public static void PlayerGetNumberInChoosePokerTotalNumberScene(int totalNumber)
    {
        OnPlayerGetNumberInChoosePokerTotalNumberScene?.Invoke(null , new TotalNumberCardInformation
        {
            TotalNumberCardNumber = totalNumber
        });
    }

    public static void PlayerGetNumberInMakePokerScene(int totalNumber)
    {
        OnPlayerGetNumberInMakePokerScene?.Invoke(null , new TotalNumberCardInformation
        {
            TotalNumberCardNumber = totalNumber
        });
    }

    public static void ResetStaticData()
    {
        OnPlayerGetNumberInChoosePokerTotalNumberScene = null;
        OnPlayerGetNumberInMakePokerScene = null;
    }
}
