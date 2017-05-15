using System.Collections.Generic;
using System.Linq;
using GameOfLife.Hubs;
using GameOfLife.Models;

namespace GameOfLife.Extensions
{
    public static class CellsExtension
    {
        private static Cell GetCell(this IEnumerable<Cell> cells, int x, int y)
        {
            return cells.SingleOrDefault(c => c.X == x && c.Y == y);
        }

        private static IEnumerable<Cell> GetAllAdjacentCells(this IEnumerable<Cell> cells, int x, int y)
        {
            yield return cells.GetCell(x - 1, y);
            yield return cells.GetCell(x - 1, y - 1);
            yield return cells.GetCell(x, y - 1);
            yield return cells.GetCell(x + 1, y - 1);
            yield return cells.GetCell(x + 1, y);
            yield return cells.GetCell(x + 1, y + 1);
            yield return cells.GetCell(x, y + 1);
            yield return cells.GetCell(x - 1, y + 1);
        }

        public static void AddRangeOfCells(this List<Cell> cells, IEnumerable<Cell> newCells)
        {
            foreach (var newCell in newCells.ToList())
            {
                var cell = cells.GetCell(newCell.X, newCell.Y);
                if (cell != null)
                    cell.Color = newCell.Color;
                else
                    cells.Add(new Cell(newCell.X, newCell.Y, newCell.Color));
            }
        }

        public static int GetNumberOfLivingAdjacentCells(this Cell cell, IEnumerable<Cell> cells)
        {
            return cells.GetAllAdjacentCells(cell.X, cell.Y).Count(c => c.IsAlive());
        }

        public static void AddAdjacentCellsForEachCell(this List<Cell> cells)
        {
            foreach (var cell in cells.ToList())
            {
                AddAdjacentCells(cells, cell.X, cell.Y);
            }
        }

        private static void AddAdjacentCells(this List<Cell> cells, int x, int y)
        {
            AddEmptyCell(cells, x - 1, y);
            AddEmptyCell(cells, x - 1, y - 1);
            AddEmptyCell(cells, x, y - 1);
            AddEmptyCell(cells, x + 1, y - 1);
            AddEmptyCell(cells, x + 1, y);
            AddEmptyCell(cells, x + 1, y + 1);
            AddEmptyCell(cells, x, y + 1);
            AddEmptyCell(cells, x - 1, y + 1);
        }

        private static void AddEmptyCell(this List<Cell> cells, int x, int y)
        {
            if (!cells.Any(c => c.X == x && c.Y == y))
            {
                cells.Add(new Cell(x, y, null));
            }
        }

        public static IEnumerable<string> Get3ALiveNeighboursColors(this IEnumerable<Cell> cells, Cell cell)
        {
            return cells.GetAllAdjacentCells(cell.X, cell.Y)
                        .Where(c => c.IsAlive())
                        .Select(c => c.Color);
        }

        public static bool IsAlive(this Cell cell)
        {
            return cell?.Color != null && cell.Color != GameOfLifeHub.DeadCellColor;
        }
    }
}