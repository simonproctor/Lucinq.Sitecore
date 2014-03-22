using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lucene.Net.Documents;
using Lucinq.SitecoreIntegration.Indexing;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Search;
using Sitecore.Search.Crawlers;
using Sitecore.Xml;
using Field = Sitecore.Data.Fields.Field;

namespace Lucinq.SitecoreIntegration.Sitecore66.Indexing
{
    public class StandardCrawler : DatabaseCrawler
    {
        #region [ Fields ]

        private static IndexingHelper indexingHelper;

        #endregion

        #region [ Properties ]

        public static IndexingHelper IndexingHelper
        {
            get
            {
                return indexingHelper ?? (indexingHelper = new IndexingHelper());
            }
        }

        #endregion

        #region [ Methods ]

        public virtual void AddFieldTypes(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            var fieldName = XmlUtil.GetAttribute("name", configNode);
            var fieldType = XmlUtil.GetAttribute("type", configNode);
            IndexingHelper.FieldTypes.Add(fieldName.ToLowerInvariant(), fieldType.ToLower());
        }

        public virtual void AddFieldConfigurations(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            var fieldName = XmlUtil.GetAttribute("name", configNode);
            var idString = XmlUtil.GetAttribute("id", configNode);
            ID fieldId;
            if (!ID.TryParse(idString, out fieldId))
            {
                return;
            }
            FieldConfiguration fieldConfiguration = new FieldConfiguration();
            bool store;
            fieldConfiguration.Store = bool.TryParse(XmlUtil.GetAttribute("store", configNode), out store) && store;
            bool analyze;
            fieldConfiguration.Analyze = bool.TryParse(XmlUtil.GetAttribute("analyze", configNode), out analyze) && analyze;

            IndexingHelper.FieldConfigurations.Add(fieldId, fieldConfiguration);
        }

        protected override void IndexVersion(Item item, Item latestVersion, IndexUpdateContext context)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(latestVersion, "latestVersion");
            Assert.ArgumentNotNull(context, "context");

            if (item.Template == null)
            {
                Log.Warn(string.Format("Cannot update item version. Reason: Template is NULL in item '{0}'.", item.Paths.FullPath), this);
                return;
            }

            Document document = new Document();
            AddVersionIdentifiers(item, latestVersion, document);
            AddAllFields(document, item, true);
            AddSpecialFields(document, item);
            AdjustBoost(document, item);
            context.AddDocument(document);
        }

        protected override void AddAllFields(Document document, Item item, bool versionSpecific)
        {
            ProcessFields(document, item);
            // base.AddAllFields(document, item, versionSpecific);
        }

        protected List<Field> GetFields(Item item)
        {
            return item.Fields.Where(IncludeField).ToList();
        }

        protected virtual void ProcessFields(Document document, Item item)
        {
            item.Fields.ReadAll();
            IndexingHelper.AddLucinqFields(document, item);
            IndexingHelper.AddCustomFields(document, item);

            List<Field> fields = GetFields(item);
            fields.ForEach(field => IndexingHelper.AddField(document, field));
        }


        protected virtual Lucene.Net.Documents.Field ProcessField(Field field)
        {
            Lucene.Net.Documents.Field luceneField = new Lucene.Net.Documents.Field(field.Name.ToLower(), field.Value.ToLower(), Lucene.Net.Documents.Field.Store.YES,
                                                      Lucene.Net.Documents.Field.Index.ANALYZED);
            return luceneField;
        }

        protected virtual bool IncludeField(Field field)
        {
            // todo: Allow Ommision of Fields
            return true;
        }

        #endregion
    }
}
