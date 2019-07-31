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
