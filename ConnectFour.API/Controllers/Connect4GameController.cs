using ConnectFour.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ConnectFour.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Connect4GameController : ControllerBase
    {

        private readonly ILogger<Connect4GameController> _logger;
        private readonly Connect4Game _game;

        public Connect4GameController(ILogger<Connect4GameController> logger, Connect4Game game)
        {
            _game = game;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<int[][]> Get()
        {
            if (this._game.board == null)
            {
                return BadRequest("Game not started");
            }
            var board = this._game.board.GetBoard();
            var newBoard = new int[6][];
            for (byte row = 0; row < 6; row++)
            {
                newBoard[row] = new int[7];
            }
            for (byte row = 0; row < 6; row++)
            {
                for (byte column = 0; column < 7; column++)
                {
                    newBoard[6 - 1 - row][column] = (int)board[column, row];
                }
            }
            return Ok(newBoard);
        }

        [HttpPost("{column}")]
        public IActionResult Post(byte column)
        {
            if (this._game.board == null)
            {
                return BadRequest("Game not started");
            }
            try
            {
                return Ok((int)this._game.board.SetStone(column));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put()
        {
            this._game.board = new GameBoard();
            return NoContent();
        }
    }
}
