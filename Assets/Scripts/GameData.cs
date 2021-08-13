using System;
using SQLite4Unity3d;

namespace DatabaseData
{
    [Serializable]
    public class GameData
    {
        public GameData(
            string productName,
            string productCode,
            string serialCode,
            string developer,
            string publisher,
            string dateReleased,
            string genre,
            string region,
            string platform
        )
        {
            ProductName = productName;
            ProductCode = productCode;
            SerialCode = serialCode;
            Developer = developer;
            Publisher = publisher;
            DateReleased = dateReleased;
            Genre = genre;
            Region = region;
            Platform = platform;
        }

        public GameData()
        {
        }

        public string ProductName { get; set; }
        [PrimaryKey]
        public string ProductCode { get; set; }
        public string SerialCode { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string DateReleased { get; set; }
        public string Genre { get; set; }
        public string Region { get; set; }
        public string Platform { get; set; }
    }
}
