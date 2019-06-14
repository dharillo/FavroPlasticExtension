using System;
namespace FavroPlasticExtension.Favro
{
    public class User
    {
        /// <summary>
        /// The ID of the user
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The main account email of the user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The role of the user in the organization.
        /// <para>
        /// Posible values: <see cref="OrganizationMember.ROLE_ADMINISTRATOR"/>, <see cref="OrganizationMember.ROLE_DISABLED" />,
        /// <see cref="OrganizationMember.ROLE_EXTERNAL_MEMBER"/>, <see cref="OrganizationMember.ROLE_FULL_MEMBER"/> or
        /// <see cref="ROLE_GUEST"/>
        /// </para>
        /// </summary>
        public string OrganizationRole { get; set; }
    }
}
