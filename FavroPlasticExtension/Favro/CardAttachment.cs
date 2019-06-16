namespace FavroPlasticExtension.Favro
{
    public class CardAttachment
    {
        /// <summary>
        /// The name of the file
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The URL of the file
        /// </summary>
        public string FileURL { get; set; }
        /// <summary>
        /// Optional thumbnail URL of the file
        /// </summary>
        public string ThumbnailURL { get; set; }
    }
}
