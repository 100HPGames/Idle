using System;
using System.Collections.Generic;
using _Tools;
using CodeBase.UI.LootBoxes;

namespace CodeBase.Tools.StaticDataLoader
{
    [Serializable]
    public class ShopDTO
    {
        public List<CurrencyShopDTO> CurrencyShopDTO;
        public List<LootBoxDTO> LootBoxesDTOs;
    }
}