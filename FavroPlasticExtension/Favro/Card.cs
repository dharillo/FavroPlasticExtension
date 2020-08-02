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

using System;
using System.Collections.Generic;

namespace FavroPlasticExtension.Favro.API
{
    public class Card
    {
        /// <summary>
        /// The ID of the card
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// The Id of the organization that this card exists in
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// The shared ID of the widget that this card exists on. Only returned if
        /// the card does not exist in a todo list.
        /// </summary>
        public string WidgetCommonId { get; set; }
        /// <summary>
        /// The user ID of the user of the todo list that this card exists in. Only returned
        /// if the card exists in a todo list. Otherwise, widgetCommonId will be returned.
        /// Not available when using channels
        /// </summary>
        public string TodoListUserId { get; set; }
        /// <summary>
        /// Returns <c>true</c> if the card exists in a todo list and has been completed by
        /// that user. Not available when using channels
        /// </summary>
        public bool TodoListCompleted { get; set; }
        /// <summary>
        /// The ID of the column that this card exists in. Only returned if the card exists
        /// on a board
        /// </summary>
        public string ColumnId { get; set; }
        /// <summary>
        /// The id of the lane that this card exists in. Only returned if the card exists on
        /// a board and the board has the Lanes app enabled. Not available when using channels.
        /// </summary>
        public string LaneId { get; set; }
        /// <summary>
        /// The id of the parent card. Only returned if the card exists in a backlog and is the
        /// child of another card.
        /// </summary>
        public string ParentCardId { get; set; }
        /// <summary>
        /// <c>true</c> if the card is a lane. Not available when using channels.
        /// </summary>
        public bool IsLane { get; set; }
        /// <summary>
        /// <c>true</c> if the card is archived.
        /// </summary>
        public bool Archived { get; set; }
        /// <summary>
        /// The position of the card in the column or backlog in relation to other cards.
        /// </summary>
        public double Position { get; set; }
        /// <summary>
        /// A shared id for all instances of this card in the organization.
        /// </summary>
        public string CardCommonId { get; set; }
        /// <summary>
        /// The name of the card.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The detailed description of the card.
        /// </summary>
        public string DetailedDescription { get; set; }
        /// <summary>
        /// The tags IDs that are set on the card. Only available on boards when using channels.
        /// </summary>
        public List<string> Tags { get; set; }
        /// <summary>
        /// The sequentialId of the card. Useful for creating human readable links.
        /// </summary>
        public int SequentialId { get; set; }
        /// <summary>
        /// The start date of the card. Not available when using channels.
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// The due date of the card. Not available when using channels.
        /// </summary>
        public DateTime DueTime { get; set; }
        /// <summary>
        /// The users assigned to the card and whether or not they have completed the card
        /// </summary>
        public List<CardAssignment> Assignments { get; set; }
        /// <summary>
        /// The number of comments posted on the card.
        /// </summary>
        public int NumComments { get; set; }
        /// <summary>
        /// The number of tasks on the card.
        /// </summary>
        public int TasksTotal { get; set; }
        /// <summary>
        /// The number of tasks completed on the card
        /// </summary>
        public int TasksDone { get; set; }
        /// <summary>
        /// The file attachments on the card.
        /// </summary>
        public List<CardAttachment> Attachments { get; set; }
        /// <summary>
        /// The custom fields that are set on the card and enabled in the organization
        /// </summary>
        public List<CardCustomField> CustomFields { get; set; }
        /// <summary>
        /// The amount of time card has been on current board
        /// </summary>
        public CardBoardTime TimeOnBoard { get; set; }
        /// <summary>
        /// The detailed summary of time card has been on each column of the current board.
        /// The object key represents the columnId of the column, and the value is the amount
        /// of time card has been on that column.
        /// </summary>
        public object TimeOnColumns { get; set; }
        /// <summary>
        /// The Favro attachments on the card
        /// </summary>
        public List<FavroAttachment> FavroAttachments { get; set; }
    }
}
