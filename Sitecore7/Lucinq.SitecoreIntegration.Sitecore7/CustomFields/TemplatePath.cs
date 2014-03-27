using System.Collections.Generic;

using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Lucinq.SitecoreIntegration.Sitecore7.CustomFields
{
    public class TemplatePath : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            return GetAllTemplates(indexable as SitecoreIndexableItem);
        }

        private object GetAllTemplates(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.IsNotNull(item.Template, "Item template not found.");
            List<string> list = new List<string> { IdHelper.NormalizeGuid(item.TemplateID) };
            AddTemplateToList(item.Template, ref list);
            return list;
        }

        private void AddTemplateToList(TemplateItem templateItem, ref List<string> list)
        {
            if (templateItem.BaseTemplates == null)
            {
                return;
            }

            foreach (var baseTemplateItem in templateItem.BaseTemplates)
            {
                list.Add(IdHelper.NormalizeGuid(baseTemplateItem.ID));
                AddTemplateToList(baseTemplateItem, ref list);
            }
        }
    }
}
