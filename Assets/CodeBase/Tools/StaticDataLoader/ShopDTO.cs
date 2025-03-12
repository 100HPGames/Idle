using System;
using System.Collections.Generic;
using _Tools;

namespace CodeBase.Tools.StaticDataLoader
{
    [Serializable]
    public class ShopDTO
    {
        public List<CurrencyShopDTO> CurrencyShopDTO;
        public List<LootBoxDTO> LootBoxesDTOs;
    }
}