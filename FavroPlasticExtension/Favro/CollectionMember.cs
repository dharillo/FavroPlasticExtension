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
