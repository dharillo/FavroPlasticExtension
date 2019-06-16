using System;
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
