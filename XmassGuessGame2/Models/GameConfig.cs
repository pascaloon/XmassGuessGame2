using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XmassGuessGame2.Models
{
    [DataContract] 
    public class GameConfig
    {
        [DataMember]
        public List<string> Names { get; set; }

        public GameConfig()
        {
            Names = new List<string>();
        }
    }

    [DataContract] 
    public class PicksConfig
    {
        [DataMember]
        public Dictionary<string, string> Picks { get; set; }

        public PicksConfig()
        {
            Picks = new Dictionary<string, string>();
        }
        
    }
}