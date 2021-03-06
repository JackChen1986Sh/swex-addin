﻿//**********************
//SwEx.AddIn - development tools for SOLIDWORKS add-ins
//Copyright(C) 2019 www.codestack.net
//License: https://github.com/codestackdev/swex-addin/blob/master/LICENSE
//Product URL: https://www.codestack.net/labs/solidworks/swex/add-in/
//**********************

using CodeStack.SwEx.AddIn.Core;
using CodeStack.SwEx.AddIn.Delegates;
using CodeStack.SwEx.Common.Attributes;
using CodeStack.SwEx.Common.Base;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Windows.Forms;

namespace CodeStack.SwEx.AddIn.Base
{
    /// <summary>
    /// Base interface for enabling the Framework for SOLIDWORKS add-ins
    /// </summary>
    [ModuleInfo("SwEx.AddIn")]
    public interface ISwAddInEx : IModule
    {
        /// <summary>
        /// Overload to initialize the data. This method is called
        /// from <see cref="SolidWorks.Interop.swpublished.ISwAddin.ConnectToSW(object, int)"/> method when the add-in loads
        /// </summary>
        /// <returns>True if initialization is successful. Return false to stop loading the add-in
        /// (in this case add-in will be unchecked in the add-ins manager dialog)</returns>
        /// <remarks>Use this method to load the command manager using the
        /// <see cref="AddCommandGroup{TCmdEnum}(Action{TCmdEnum}, EnableMethodDelegate{TCmdEnum})"/>
        /// or <see cref="AddContextMenu{TCmdEnum}(Action{TCmdEnum}, swSelectType_e, EnableMethodDelegate{TCmdEnum})"/> methods</remarks>
        /// Access the pointer to application via m_App field
        bool OnConnect();

        /// <summary>
        /// Called on disposing of the add-in (either when SOLIDWORKS application closes
        /// or when add-in is explicitly unloaded by the user from the AddIns Manager dialog)
        /// </summary>
        /// <returns>True to indicate that unloading is successful</returns>
        /// <remarks>Use this method to clear all temp files or close the opened streams</remarks>
        bool OnDisconnect();

        /// <summary>
        /// Add command group based on the defined commands enumerator
        /// </summary>
        /// <typeparam name="TCmdEnum">Enumerator with commands</typeparam>
        /// <param name="callback">Callback function for the commands</param>
        /// <param name="enable">Optional enable method for the commands.
        /// If this method is not used than command will be enabled according to the workspace
        /// defined in the <see cref="Attributes.CommandItemInfoAttribute.SupportedWorkspaces"/> for this command</param>
        /// <returns>Newly created command group</returns>
        CommandGroup AddCommandGroup<TCmdEnum>(Action<TCmdEnum> callback,
            EnableMethodDelegate<TCmdEnum> enable = null)
            where TCmdEnum : IComparable, IFormattable, IConvertible;

        /// <summary>
        /// Add context menu based on the defined commands enumerator
        /// </summary>
        /// <typeparam name="TCmdEnum">Enumerator with commands</typeparam>
        /// <param name="callback">Callback function for the commands</param>
        /// <param name="contextMenuSelectType">Selection type where the menu is enabled as defined in <see href="https://help.solidworks.com/2012/english/api/swconst/solidworks.interop.swconst~solidworks.interop.swconst.swselecttype_e.html">swSelectType_e</see></param>
        /// <param name="enable">Optional enable method for the commands.
        /// If this method is not used than command will be enabled according to the workspace
        /// defined in the <see cref="Attributes.CommandItemInfoAttribute.SupportedWorkspaces"/> for this command</param>
        /// <returns>Newly created <see href="https://help.solidworks.com/2012/english/api/sldworksapi/SolidWorks.Interop.sldworks~SolidWorks.Interop.sldworks.ICommandGroup.html">CommandGroup</see></returns>
        /// <remarks>It is only possible to specify single selection group for the context menu.
        /// If it is required to enable the menu for multiple selection elements
        /// use <see cref="swSelectType_e.swSelEVERYTHING"/> option and <paramref name="enable"/> parameter
        /// to specify the enable function</remarks>
        CommandGroup AddContextMenu<TCmdEnum>(Action<TCmdEnum> callback,
            swSelectType_e contextMenuSelectType = swSelectType_e.swSelEVERYTHING,
            EnableMethodDelegate<TCmdEnum> enable = null)
            where TCmdEnum : IComparable, IFormattable, IConvertible;

        /// <summary>
        /// Creates document life cycle hander model
        /// </summary>
        /// <typeparam name="TDocHandler">Document handler wrapper</typeparam>
        /// <returns>Documents handler instance</returns>
        IDocumentsHandler<TDocHandler> CreateDocumentsHandler<TDocHandler>()
            where TDocHandler : IDocumentHandler, new();

        /// <summary>
        /// Creates generic documents handler which exposes lifecycle events
        /// </summary>
        /// <returns>Instance of generic documents handler. Explore the available events of <see cref="DocumentHandler"/> type</returns>
        IDocumentsHandler<DocumentHandler> CreateDocumentsHandler();
        
        /// <inheritdoc cref="CreateTaskPane{TControl, TCmdEnum}(Action{TCmdEnum}, out TControl)"/>
        ITaskpaneView CreateTaskPane<TControl>(out TControl ctrl)
            where TControl : UserControl, new();

        /// <summary>
        /// Creates task pane control
        /// </summary>
        /// <typeparam name="TControl">Control type to host in task pane</typeparam>
        /// <typeparam name="TCmdEnum">Descriptor for commands in task pane. Use <see cref="Attributes.TaskPaneStandardButtonAttribute"/> to mark standard commands</typeparam>
        /// <param name="ctrl">Instance of created control</param>
        /// <returns>Task pane view</returns>
        /// <param name="cmdHandler">Handler of the commands</param>
        ITaskpaneView CreateTaskPane<TControl, TCmdEnum>(Action<TCmdEnum> cmdHandler, out TControl ctrl)
            where TControl : UserControl, new()
            where TCmdEnum : IComparable, IFormattable, IConvertible;
    }
}
