using System;

namespace FavroPlasticExtension.Favro.API
{
    internal class CardComment
    {
        public string CommentId { get; set; }
        public string OrganizationId { get; set; }
        public string CardCommonId { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}