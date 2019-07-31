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
