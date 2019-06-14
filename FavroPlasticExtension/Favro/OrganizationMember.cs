using System;
namespace FavroPlasticExtension.Favro
{
    public class OrganizationMember
    {
        #region Organization roles
        /// <summary>
        /// Administrator/Owner of the organization. Has full control over
        /// the organization
        /// </summary>
        public const string ROLE_ADMINISTRATOR = "administrator";
        /// <summary>
        /// Full member of the organization
        /// </summary>
        public const string ROLE_FULL_MEMBER = "fullMember";
        /// <summary>
        /// External member of the organization. Not shared organization collections
        /// by default
        /// </summary>
        public const string ROLE_EXTERNAL_MEMBER = "externalMember";
        /// <summary>
        /// Guest user in the organization. A restricted user account
        /// </summary>
        public const string ROLE_GUEST = "guest";
        /// <summary>
        /// Disabled user in the organization
        /// </summary>
        public const string ROLE_DISABLED = "disabled";
        #endregion

        /// <summary>
        /// User ID of the user in the organization
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// The role of the user in the organization.
        /// <para>
        /// Posible values: <see cref="ROLE_ADMINISTRATOR"/>, <see cref="ROLE_DISABLED" />, <see cref="ROLE_EXTERNAL_MEMBER"/>, <see cref="ROLE_FULL_MEMBER"/> or
        /// <see cref="ROLE_GUEST"/>
        /// </para>
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// The date that this user was added to the organization
        /// </summary>
        public DateTime JoinDate { get; set; }
    }
}
