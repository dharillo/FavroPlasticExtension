//  Favro Plastic Extension
//  Copyright(C) 2019  David Harillo Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published
//  by the Free Software Foundation, either version 3 of the License, or
//  any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details in the project root.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program. If not, see<https://www.gnu.org/licenses/>

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
