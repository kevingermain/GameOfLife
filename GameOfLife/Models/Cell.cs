using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameOfLife.Enums;
using GameOfLife.Extensions;

namespace GameOfLife.Models
{
    public class Cell
    {
        public Cell(int x, int y, string color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        public int X { get; }
        public int Y { get; }
        public string Color { get; set; }

        public static string MixColorsOfRebornCell(IEnumerable<Cell> cells, Cell cell)
        {
            var colors = cells.Get3ALiveNeighboursColors(cell).ToList();

            var color1 = ColorTranslator.FromHtml(colors[0]);
            var color2 = ColorTranslator.FromHtml(colors[1]);
            var color3 = ColorTranslator.FromHtml(colors[2]);

            if (color1 == color2 && color2 == color3)
                return ColorTranslator.ToHtml(color1);

            var r = (byte)(color1.R * 0.33 + color2.R * 0.33 + color3.R * 0.34);
            var g = (byte)(color1.G * 0.33 + color2.G * 0.33 + color3.G * 0.34);
            var b = (byte)(color1.B * 0.33 + color2.B * 0.33 + color3.B * 0.34);

            return ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(r, g, b));
        }

        public static IEnumerable<Cell> AddCellsByPattern(PatternEnum pattern, string color, int x, int y)
        {
            switch (pattern)
            {
                case PatternEnum.Block:
                    yield return new Cell(x, y, color);
                    yield return new Cell(x + 1, y, color);
                    yield return new Cell(x, y + 1, color);
                    yield return new Cell(x + 1, y + 1, color);
                    break;
                case PatternEnum.Blinker:
                    yield return new Cell(x, y, color);
                    yield return new Cell(x + 1, y, color);
                    yield return new Cell(x + 2, y, color);
                    break;
                case PatternEnum.Glider:
                    yield return new Cell(x + 1, y, color);
                    yield return new Cell(x + 2, y + 1, color);
                    yield return new Cell(x, y + 2, color);
                    yield return new Cell(x + 1, y + 2, color);
                    yield return new Cell(x + 2, y + 2, color);
                    break;
                case PatternEnum.Pentadecathlon:
                    yield return new Cell(x + 1, y, color);
                    yield return new Cell(x + 1, y + 1, color);
                    yield return new Cell(x, y + 2, color);
                    yield return new Cell(x + 2, y + 2, color);
                    yield return new Cell(x + 1, y + 3, color);
                    yield return new Cell(x + 1, y + 4, color);
                    yield return new Cell(x + 1, y + 5, color);
                    yield return new Cell(x + 1, y + 6, color);
                    yield return new Cell(x, y + 7, color);
                    yield return new Cell(x + 2, y + 7, color);
                    yield return new Cell(x + 1, y + 8, color);
                    yield return new Cell(x + 1, y + 9, color);
                    break;
            }
        }
    }
}