namespace FavroPlasticExtension.Favro
{
    public class CardBoardTime
    {
        /// <summary>
        /// Total time card has been on current board
        /// </summary>
        public long Time { get; set; }
        /// <summary>
        /// Indicates either card was archived or reached the final column on the
        /// current board.
        /// </summary>
        public bool IsStopped { get; set; }
    }
}
