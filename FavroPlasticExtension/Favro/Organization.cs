using System.Collections.Generic;

namespace FavroPlasticExtension.Favro
{
    public class Organization
    {
        /// <summary>
        /// The ID of the organization
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// The name of the organization
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The users that are members of the organization
        /// </summary>
        public List<OrganizationMember> SharedToUsers { get; set; }
    }
}
