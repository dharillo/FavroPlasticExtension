using System;
using System.Collections.Generic;

namespace FavroPlasticExtension.Favro
{
    public class Widget
    {
        #region Widget types
        public const string TYPE_BACKLOG = "backlog";
        public const string TYPE_BOARD = "board";
        #endregion
        #region Widget roles
        public const string ROLE_OWNERS = "owners";
        public const string ROLE_FULL_MEMBERS = "fullMembers";
        public const string ROLE_GUEST = "guests";
        #endregion
        #region Widget colors
        public const string COLOR_BLUE = "blue";
        public const string COLOR_LIGHTGREEN = "lightgreen";
        public const string COLOR_BROWN = "brown";
        public const string COLOR_PURPLE = "purple";
        public const string COLOR_ORANGE = "orange";
        public const string COLOR_YELLOW = "yellow";
        public const string COLOR_GRAY = "gray";
        public const string COLOR_RED = "red";
        public const string COLOR_CYAN = "cyan";
        public const string COLOR_GREEN = "green";
        #endregion
        /// <summary>
        /// The shared ID of the widget
        /// </summary>
        public string WidgetCommonId { get; set; }
        /// <summary>
        /// The ID of the organization that this widget exists in
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// The IDs of the collections that this widget exists in. This array
        /// will only contain collections that the user has access to.
        /// </summary>
        public List<string> CollectionIds { get; set; }
        /// <summary>
        /// The name of the widget
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The type of the widget.
        /// <para>
        /// Possible values: <see cref="TYPE_BACKLOG"/> or <see cref="TYPE_BOARD"/>
        /// </para>
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// If set, this means that this widget is a breakdown of a card. Not available
        /// when using channels
        /// </summary>
        public string BreakdownCardCommonId { get; set; }
        /// <summary>
        /// The color of the widget icon. Not available when using channels
        /// <para>
        /// Possible values: <see cref="COLOR_BLUE"/>, <see cref="COLOR_BROWN"/>, <see cref="COLOR_CYAN"/>,
        /// <see cref="COLOR_GRAY"/>, <see cref="COLOR_GREEN"/>, <see cref="COLOR_LIGHTGREEN"/>,
        /// <see cref="COLOR_ORANGE"/>, <see cref="COLOR_PURPLE"/>, <see cref="COLOR_RED"/> or
        /// <see cref="COLOR_YELLOW"/>
        /// </para>
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// The user roles that have ownership of the widget.
        /// <para>Possible values: <see cref="ROLE_FULL_MEMBERS"/>, <see cref="ROLE_GUEST"/> or <see cref="ROLE_OWNERS"/></para>
        /// </summary>
        public string OwnerRole { get; set; }
        /// <summary>
        /// The user that can add, edit and move cards on the widget.
        /// <para>Possible values: <see cref="ROLE_FULL_MEMBERS"/>, <see cref="ROLE_GUEST"/> or <see cref="ROLE_OWNERS"/></para>
        /// </summary>
        public string EditRole { get; set; }
    }
}
