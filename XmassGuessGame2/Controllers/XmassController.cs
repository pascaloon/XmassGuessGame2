using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using XmassGuessGame2.Models;
using XmassGuessGame2.Services;

namespace XmassGuessGame2.Controllers
{
    [Route("xmass")]
    public class XmassController : Controller
    {
        private readonly Game _game;

        public XmassController(Game game)
        {
            _game = game;
        }
        // GET
        [HttpGet("config")]
        public IActionResult Config()
        {
            List<Player> players = _game.GameConfig.Names
                .Select(name => new Player(name, _game.GetHashFromName(name), null))
                .ToList();
            return View(players);
        }
        
        [HttpPost("regeneratepicks")]
        public IActionResult RegeneratePicks()
        {
            _game.RegeneratePicksConfig();
            return Redirect("/Xmass/Config");
        }

        [HttpGet("picks/{id}")]
        public IActionResult Player(string id)
        {
            if (String.IsNullOrEmpty(id) || _game.PicksConfig == null)
            {
                return Redirect("/");
            }

            Player player = new Player(_game.GetNameFromHash(id), id, _game.GetPickNameForHashedName(id));
            return View(player);
        }

        [HttpGet("game")]
        public IActionResult Game()
        {
            GameViewModel gvm = new GameViewModel(_game.GameConfig.Names);
            return View(gvm);
        }

        [HttpPost("game")]
        public IActionResult Game(GameViewModel input)
        {
            input = new GameViewModel(_game.GameConfig.Names)
            {
                Answer = _game.GuessPick(input.Person, input.Pick)
            };
            return View(input);
        }
    }
}