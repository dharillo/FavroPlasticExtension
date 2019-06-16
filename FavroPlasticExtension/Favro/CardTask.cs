using System;
namespace FavroPlasticExtension.Favro
{
    public class CardTask
    {
        /// <summary>
        /// The name of the task
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Indicates whether the task is completed or not. <c>false</c> by default.
        /// </summary>
        public bool Completed { get; set; }
    }
}
