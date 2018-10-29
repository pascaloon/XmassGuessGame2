using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using XmassGuessGame2.Models;

namespace XmassGuessGame2.Services
{
    public class Game
    {
        private string _configFileName = "config.json";
        private string _picksFileName = "picks.json";
        
        private GameConfig _gameConfig;
        private PicksConfig _picksConfig;

        public Game()
        {
            
        }

        public GameConfig GameConfig
        {
            get { return _gameConfig; }
        }

        public PicksConfig PicksConfig
        {
            get { return _picksConfig; }
        }

        public bool GuessPick(string person, string pick) =>
            _picksConfig.Picks[MD5Hash(person)].Equals(MD5Hash(pick));

        public string GetPickNameForHashedName(string hashedName)
        {
            if (PicksConfig == null)
            {
                return null;
            }

            string hashedPick = _picksConfig.Picks[hashedName];
            return GetNameFromHash(hashedPick);
        }

        public string GetNameFromHash(string hashedName) => 
            _gameConfig.Names.FirstOrDefault(name => hashedName.Equals(MD5Hash(name)));

        public string GetHashFromName(string name) => MD5Hash(name);

        public void LoadConfigurations()
        {
            DataContractJsonSerializer gameConfigSerializer = new DataContractJsonSerializer(typeof(GameConfig));
            using (StreamReader streamReader = new StreamReader(_configFileName))
            {
                _gameConfig = (GameConfig)gameConfigSerializer.ReadObject(streamReader.BaseStream);
            }

            if (File.Exists(_picksFileName))
            {
                DataContractJsonSerializer picksConfigSerializer = new DataContractJsonSerializer(typeof(PicksConfig));
                using (StreamReader streamReader = new StreamReader(_picksFileName))
                {
                    _picksConfig = (PicksConfig)picksConfigSerializer.ReadObject(streamReader.BaseStream);
                }
            }
            
        }

        public void RegeneratePicksConfig()
        {
            Random rand = new Random();
            bool isGenerationValid = true;
            _picksConfig = new PicksConfig();
            
            do
            {
                _picksConfig.Picks.Clear();            
                List<string> availablePicks = GameConfig.Names.ToList();

                foreach (string name in GameConfig.Names)
                {
                    List<string> otherNames = availablePicks.Where(n => !name.Equals(n)).ToList();
                    if (otherNames.Count == 0)
                    {
                        isGenerationValid = false;
                        break;
                    }

                    string pick = otherNames[rand.Next(0, otherNames.Count)];
                    availablePicks.Remove(pick);
                    _picksConfig.Picks[MD5Hash(name)] = MD5Hash(pick);
                }
            } while (!isGenerationValid);
            
            // Validation
            HashSet<string> uniquePicks = new HashSet<string>(_gameConfig.Names.Count);
            foreach (var pick in _picksConfig.Picks)
            {
                if (uniquePicks.Contains(pick.Value))
                {
                    throw new InvalidDataException("DUPLICATE PICKS!!!");
                }

                uniquePicks.Add(pick.Value);
            }
            
            
            DataContractJsonSerializer picksConfigSerializer = new DataContractJsonSerializer(typeof(PicksConfig));

            using (StreamWriter streamWriter = new StreamWriter(_picksFileName))
            {
                picksConfigSerializer.WriteObject(streamWriter.BaseStream, _picksConfig);
            }
        }

        private string MD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}