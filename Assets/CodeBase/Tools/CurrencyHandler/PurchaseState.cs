﻿namespace MoneyHandler
{
    public enum PurchaseState
    {
        Unavailable = 0,
        CanBePurchased = 1,
        DoNoEnoughMoney = 10,
        ReachedMaxLevel = 20,
    }
}