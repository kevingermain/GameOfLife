using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameOfLife.Enums;
using GameOfLife.Extensions;
using GameOfLife.Models;
using Microsoft.AspNet.SignalR;

namespace GameOfLife.Hubs
{
    public class GameOfLifeHub : Hub
    {
        private static int WidthCanvas = 640;
        private static int HeightCanvas = 480;
        private const int SizeCell = 10;
        public const string DeadCellColor = "#FAFAFA";

        private static List<Player> _players = new List<Player>();
        private static List<Cell> _newCells = new List<Cell>();
        private static List<Cell> _cells = new List<Cell>();
        private static List<Cell> _cellsNexGeneration = new List<Cell>();

        private static Thread Game;

        private static readonly object Locker = new object();
        private static readonly IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<GameOfLifeHub>();

        private static void Main()
        {
            while (true)
            {
                lock (Locker)
                {
                    _cells.AddAdjacentCellsForEachCell();

                    foreach (var cell in _cells)
                    {
                        var livingCells = cell.GetNumberOfLivingAdjacentCells(_cells);

                        // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
                        // Any live cell with more than three live neighbours dies, as if by overcrowding.
                        if (cell.IsAlive() && (livingCells < 2 || livingCells > 3))
                        {
                            _cellsNexGeneration.Add(new Cell(cell.X, cell.Y, DeadCellColor));
                        }

                        // Any live cell with two or three live neighbours lives on to the next generation.
                        else if (cell.IsAlive() && (livingCells == 2 || livingCells == 3))
                        {
                            _cellsNexGeneration.Add(new Cell(cell.X, cell.Y, cell.Color));
                        }

                        //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                        else if (!cell.IsAlive() && livingCells == 3)
                        {
                            _cellsNexGeneration.Add(new Cell(cell.X, cell.Y, Cell.MixColorsOfRebornCell(_cells, cell)));
                        }
                    }

                    // Update the map to everyone with updated cells
                    HubContext.Clients.All.setCells(_cellsNexGeneration.Where(c => c.Color != null));

                    // Keep only alive cells and inside the map for the next generation
                    _cells = _cellsNexGeneration.Where(c => c.IsAlive()
                                                    && c.X >= -2
                                                    && c.X <= (WidthCanvas / SizeCell) + 2
                                                    && c.Y >= -2 && c.Y <= (HeightCanvas / SizeCell) + 2)
                                                .ToList();

                    // Add the newCells from players for the next generation
                    _cells.AddRangeOfCells(_newCells);

                    _newCells.Clear();
                    _cellsNexGeneration.Clear();

                    Thread.Sleep(1000);
                }
            }
        }

        public override Task OnConnected()
        {
            var random = new Random();
            var newColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            var player = new Player(Context.ConnectionId, ColorTranslator.ToHtml(newColor));

            _players.Add(player);
            Clients.All.setListOfPlayers(_players);

            if (Game == null)
                Game = new Thread(Main);

            if (!Game.IsAlive)
                Game.Start();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stop)
        {
            var player = _players.Find(p => p.ConnectionId == Context.ConnectionId);
            _players.Remove(player);
            Clients.All.setListOfPlayers(_players);

            if (!_players.Any())
            {
                Game.Abort();
                Game = null;
                _newCells.Clear();
                _cells.Clear();
                _cellsNexGeneration.Clear();
            }

            return base.OnDisconnected(stop);
        }

        public void OnNewCell(string id, int x, int y)
        {
            var player = _players.Find(p => p.ConnectionId == id);

            var cellX = x / SizeCell;
            var cellY = y / SizeCell;

            _newCells.Add(new Cell(cellX, cellY, player.Color));
        }

        public void SetPattern(string id, PatternEnum pattern)
        {
            var player = _players.Find(p => p.ConnectionId == id);

            var r = new Random();

            var cellX = r.Next(WidthCanvas) / SizeCell;
            var cellY = r.Next(HeightCanvas) / SizeCell;

            _newCells.AddRange(Cell.AddCellsByPattern(pattern, player.Color, cellX, cellY));
        }
    }
}