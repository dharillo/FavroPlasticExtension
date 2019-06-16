using System;
using System.Collections.Generic;

namespace FavroPlasticExtension.Favro
{
    public class CardTasklist
    {
        /// <summary>
        /// The name of the tasklist
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The list of tasks (look at <see cref="CardTask"/>) or tasknames
        /// </summary>
        public List<CardTask> Tasks { get; set; }
    }
}
