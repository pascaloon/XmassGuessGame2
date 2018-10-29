using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XmassGuessGame2.Models
{
    public class Player
    {
        public string Name { get; }
        public string HashedName { get; }
        public string Pick { get; }

        public Player(string name, string hashedName, string pick)
        {
            Name = name;
            HashedName = hashedName;
            Pick = pick;
        }
    }


    public class GameViewModel
    {
        public List<SelectListItem> Names { get; }

        public string Person { get; set; }
        public string Pick { get; set; }

        public bool? Answer { get; set; }
        
        public GameViewModel(List<string> names)
        {
            Names = names.Select(name => new SelectListItem(name, name)).ToList();
            Person = null;
            Pick = null;
            Answer = null;
        }
        
        public GameViewModel()
        {
            Names = new List<SelectListItem>();
            Person = null;
            Pick = null;
            Answer = null;
        }
    }
}