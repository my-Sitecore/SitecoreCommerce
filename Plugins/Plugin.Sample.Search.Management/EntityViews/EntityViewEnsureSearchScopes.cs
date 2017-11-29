﻿
using System.Threading.Tasks;
using System.Linq;

namespace Plugin.Sample.Search.Management
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using Sitecore.Commerce.Plugin.Search;

    /// <summary>
    /// Defines a block which EntityViewEnsureSearchScopes.
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Sitecore.Commerce.EntityViews.EntityView,
    ///         Sitecore.Commerce.EntityViews.EntityView, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("EntityViewEnsureSearchScopes")]
    public class EntityViewEnsureSearchScopes : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {

        private readonly CommerceCommander _commerceCommander;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewEnsureSearchScopes"/> class.
        /// </summary>
        /// <param name="commerceCommander">The <see cref="CommerceCommander"/> is a gateway object to resolving and executing other Commerce Commands and other control points.</param>
        public EntityViewEnsureSearchScopes(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        /// <summary>The execute.</summary>
        /// <param name="entityView">The argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="EntityView"/>.</returns>
        public override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            if (entityView.Name != "Search-Scopes")
            {
                return Task.FromResult(entityView);
            }

            if (!string.IsNullOrEmpty(entityView.Action))
            {
                return Task.FromResult(entityView);
            }
            var entityViewArgument = this._commerceCommander.Command<ViewCommander>()
                .CurrentEntityViewArgument(context.CommerceContext);

            var pluginPolicy = context.GetPolicy<PluginPolicy>();

            entityView.DisplayName = "Search Scopes";

            var searchScopePolicies = context.CommerceContext.Environment.GetPolicies<SearchScopePolicy>();

            entityView.UiHint = "Table";

            foreach (var searchScopePolicy in searchScopePolicies)
            {
                var newEntityView = new EntityView
                {
                    Name = "",
                    DisplayName = "",
                    Icon = pluginPolicy.Icon,
                    ItemId = searchScopePolicy.Name
                };
                newEntityView.Properties.Add(
                    new ViewProperty
                    {
                        Name = "Name",
                        IsHidden = false,
                        RawValue = searchScopePolicy.Name
                    });
                newEntityView.Properties.Add(
                    new ViewProperty
                    {
                        Name = "DeletedListName",
                        IsHidden = false,
                        RawValue = searchScopePolicy.DeletedListName
                    });
                newEntityView.Properties.Add(
                    new ViewProperty
                    {
                        Name = "EntityTypeName",
                        IsHidden = false,
                        RawValue = searchScopePolicy.EntityTypeNames.First()
                    });
                newEntityView.Properties.Add(
                    new ViewProperty
                    {
                        Name = "FullListName",
                        IsHidden = false,
                        RawValue = searchScopePolicy.FullListName
                    });
                newEntityView.Properties.Add(
                    new ViewProperty
                    {
                        Name = "IncrementalListName",
                        IsHidden = false,
                        RawValue = searchScopePolicy.IncrementalListName
                    });

                entityView.ChildViews.Add(newEntityView);
            }

            return Task.FromResult(entityView);
        }
    }
}
