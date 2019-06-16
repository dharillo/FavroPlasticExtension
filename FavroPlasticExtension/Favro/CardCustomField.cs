namespace FavroPlasticExtension.Favro
{
    public class CardCustomField
    {
        /// <summary>
        /// The ID of the custom field
        /// </summary>
        public string CustomFieldId { get; set; }
        /// <summary>
        /// The value of the custom field. The type of this field depends on the type
        /// of the custom field. Refer to the custom field types.
        /// </summary>
        public object CustomValue { get; set; }
    }
}
