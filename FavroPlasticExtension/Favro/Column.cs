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
    public class Column
    {
        /// <summary>
        /// The column ID
        /// </summary>
        public string ColumnId { get; set; }
        /// <summary>
        /// The ID of the organization that this column exists in
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        ///  The common ID of the widget that this column exists on
        /// </summary>
        public string WidgetCommonId { get; set; }
        /// <summary>
        /// The name of the column
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The position of the column on the widget
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Total number of cards on the column
        /// </summary>
        public int CardCount { get; set; }
        /// <summary>
        /// Summary time spent of the cards on the column in milisecond. Not available
        /// when using channels
        /// </summary>
        public long TimeSum { get; set; }
        /// <summary>
        /// Summary estimation of the cards on the column. Not available when using channels.
        /// </summary>
        public long EstimationSum { get; set; }
    }
}
