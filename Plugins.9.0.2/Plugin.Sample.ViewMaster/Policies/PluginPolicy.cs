﻿
namespace Plugin.Sample.ViewMaster
{
    using Sitecore.Commerce.Core;

    /// <summary>
    /// Used to house any high level policies directly related to this Plugin
    /// </summary>
    public class PluginPolicy : Policy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PluginPolicy()
        {
            Icon = "emoticon_cool";
            IsDisabled = true;
        }

        /// <summary>
        /// Icon to use for this Plugin in the Focused Commerce Experience
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Plugin is Disabled (true)
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}