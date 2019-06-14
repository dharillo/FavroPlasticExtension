namespace FavroPlasticExtension.Favro
{
    public class CollectionMember
    {
        /// <summary>
        /// User ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Role of the user in the collection.
        /// <para>
        /// Possible values: <see cref="Collection.ROLE_ADMIN" />, <see cref="Collection.ROLE_EDIT" />, <see cref="Collection.ROLE_GUEST" /> or <see cref="Collection.ROLE_VIEW" />
        /// </para>
        /// </summary>
        public string Role { get; set; }
    }
}
