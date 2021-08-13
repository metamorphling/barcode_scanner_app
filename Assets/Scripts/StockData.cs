using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

namespace DatabaseData
{
    [Serializable]
    public class StockData
    {
        public StockData(
            string itemId,
            uint boughtPrice,
            uint soldPrice,
            DateTime boughtDate,
            DateTime soldDate,
            string description
        )
        {
            ItemId = itemId;
            BoughtPrice = boughtPrice;
            SoldPrice = soldPrice;
            BoughtDate = boughtDate;
            SoldDate = soldDate;
            Description = description;
        }

        public StockData()
        {
        }

        public uint StockId { get; set; }
        public string ItemId { get; set; }
        public uint BoughtPrice { get; set; }
        public uint SoldPrice { get; set; }
        public DateTime BoughtDate { get; set; }
        public DateTime SoldDate { get; set; }
        public string Description { get; set; }
    }
}
