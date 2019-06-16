namespace FavroPlasticExtension.Favro
{
    public class FavroAttachment
    {
        public const string TYPE_CARD = "card";
        public const string TYPE_BACKLOG = "backlog";
        public const string TYPE_BOARD = "board";
        /// <summary>
        /// The cardCommonId of the card or widgetCommonId of the widget that is
        /// linked to the card
        /// </summary>
        public string ItemCommonId { get; set; }
        /// <summary>
        /// Type of item that is linked to the card.
        /// <para>
        /// Possible values: <see cref="TYPE_BACKLOG"/>, <see cref="TYPE_BOARD"/> or <see cref="TYPE_CARD"/>
        /// </para>
        /// </summary>
        public string Type { get; set; }
    }
}
