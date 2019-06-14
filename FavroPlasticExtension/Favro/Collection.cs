using System;
using System.Collections.Generic;

namespace FavroPlasticExtension.Favro
{
    public class Collection
    {
        #region Collection background
        public const string BACKGROUND_PURPLE       = "purple";
        public const string BACKGROUND_GREEN        = "green";
        public const string BACKGROUND_GRAPE        = "grape";
        public const string BACKGROUND_RED          = "red";
        public const string BACKGROUND_PINK         = "pink";
        public const string BACKGROUND_BLUE         = "blue";
        public const string BACKGROUND_SOLID_PURPLE = "solidPurple";
        public const string BACKGROUND_SOLID_GREEN  = "solidGreen";
        public const string BACKGROUND_SOLID_GRAPE  = "solidGrape";
        public const string BACKGROUND_SOLID_RED    = "solidRed";
        public const string BACKGROUND_SOLID_PINK   = "solidPink";
        public const string BACKGROUND_SOLID_GRAY   = "solidGray";
        #endregion
        #region Collection roles
        /// <summary>
        /// Guest user
        /// </summary>
        public const string ROLE_GUEST  = "guest";
        /// <summary>
        /// User with view rights
        /// </summary>
        public const string ROLE_VIEW   = "view";
        /// <summary>
        /// User with edit rights
        /// </summary>
        public const string ROLE_EDIT   = "edit";
        /// <summary>
        /// User with admin rights
        /// </summary>
        public const string ROLE_ADMIN  = "admin";
        #endregion
        #region Public sharing state
        /// <summary>
        /// Specific users only
        /// </summary>
        public const string SHARING_USERS = "users";
        /// <summary>
        /// All members of the organization
        /// </summary>
        public const string SHARING_ORGANIZATION = "organization";
        /// <summary>
        /// Everyone on the Internet
        /// </summary>
        public const string SHARING_PUBLIC = "public";
        #endregion
        /// <summary>
        /// The ID of the collection
        /// </summary>
        public string CollectionId { get; set; }
        /// <summary>
        /// The ID of the organization that this collection exists in
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// The name of the collection
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The array of <see cref="CollectionMember"/> that the collection is shared to
        /// </summary>
        public List<CollectionMember> SharedToUsers { get; set; }
        /// <summary>
        /// The sharing level of this collection.
        /// <para>
        /// Possible values: <see cref="SHARING_ORGANIZATION"/>, <see cref="SHARING_PUBLIC"/> or <see cref="SHARING_USERS"/>.
        /// </para>
        /// </summary>
        public string PublicSharing { get; set; }
        /// <summary>
        /// The collection background color. Not available when using channels. See the background colors available in this
        /// class.
        /// </summary>
        public string Background { get; set; }
        /// <summary>
        /// Whether or not the collection is archived
        /// </summary>
        public bool Archived { get; set; }
        /// <summary>
        /// Whether or not full members of this collection can create new widgets. Not
        /// available when using channels.
        /// </summary>
        public bool FullMembersCanAddWidgets { get; set; }
        /// <summary>
        /// The shared ID of the widget in this collection. Only available when using channels.
        /// </summary>
        public string WidgetCommonId { get; set; }
    }
}
