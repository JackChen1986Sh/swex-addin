﻿//**********************
//SwEx.AddIn - development tools for SOLIDWORKS add-ins
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestackdev/swex-addin/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/swex/add-in/
//**********************

using System;

namespace CodeStack.SwEx.AddIn.Attributes
{
    /// <summary>
    /// Provides the additional information about the command group
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class CommandGroupInfoAttribute : Attribute
    {
        internal int UserId { get; private set; }

        internal Type ParentGroupType { get; private set; }
        
        /// <summary>
        /// Constructor for specifying the additional information for group
        /// </summary>
        /// <param name="userId">User id for the command group. Must be unique per add-in</param>
        public CommandGroupInfoAttribute(int userId) : this(userId, null)
        {
        }

        /// <param name="parentGroupType">Type of the parent group enumeration</param>
        /// <inheritdoc cref="CommandGroupInfoAttribute(int)"/>
        /// <remarks>This group will be displayed as sub group in the menu and separated in the command tab box</remarks>
        public CommandGroupInfoAttribute(Type parentGroupType) : this(-1, parentGroupType)
        {
        }

        /// <inheritdoc cref="CommandGroupInfoAttribute(int)"/>
        /// <inheritdoc cref="CommandGroupInfoAttribute(Type)"/>
        public CommandGroupInfoAttribute(int userId, Type parentGroupType)
        {
            UserId = userId;
            
            if (parentGroupType != null && !parentGroupType.IsEnum)
            {
                throw new InvalidCastException(
                    $"Type '{parentGroupType.FullName}' specified as subgroup must be an enumeration");
            }

            ParentGroupType = parentGroupType;
        }
    }
}
