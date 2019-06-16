namespace FavroPlasticExtension.Favro
{
    public class CardAssignment
    {
        /// <summary>
        /// The user assigned to the card
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Indicates if the user completed the card. Not available when using channels.
        /// </summary>
        public bool Completed { get; set; }
    }
}
