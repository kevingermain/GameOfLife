namespace GameOfLife.Models
{
    public class Player
    {
        public string ConnectionId { get; }
        public string Color { get; }

        public Player(string connectionId, string color)
        {
            ConnectionId = connectionId;
            Color = color;
        }
    }
}