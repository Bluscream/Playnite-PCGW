using Newtonsoft.Json;
using Playnite.SDK;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace PCGWMetaData
{
    public class PCGWMetaDataPlugin : MetadataPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        public override string Name { get; } = "PCGamingWiki";
        public override Guid Id { get; } = Guid.Parse("111001DB-DBD1-46C6-B5D0-B1BA559D10E4");
        public override List<MetadataField> SupportedFields { get; } = new List<MetadataField> { MetadataField.Tags };
        public IPlayniteAPI api;

        public PCGWMetaDataPlugin(IPlayniteAPI playniteAPI) : base(playniteAPI)
        {
            api = playniteAPI;
        }

        public override OnDemandMetadataProvider GetMetadataProvider(MetadataRequestOptions options)
        {
            return new PCGWMetadataProvider(options, api);
        }
    }

    public class PCGWMetadataProvider : OnDemandMetadataProvider
    {
        private WebClient webClient = new WebClient();
        private const string url_base = "https://www.pcgamingwiki.com/w/api.php?action=browsebysubject&format=json&subject=";
        private readonly MetadataRequestOptions options;
        private List<MetadataField> availableFields;
        public IPlayniteAPI api;

        public PCGWMetadataProvider(MetadataRequestOptions options, IPlayniteAPI api)
        {
            this.options = options;
            this.api = api;
        }

        public override List<MetadataField> AvailableFields
        {
            get
            {
                if (availableFields == null)
                {
                    availableFields = GetAvailableFields();
                }

                return availableFields;
            }
        }

        private List<MetadataField> GetAvailableFields()
        {
            return new List<MetadataField> { };
        }

        public override List<string> GetTags()
        {
            var tags = new List<string>();
            var l_ = api.Database.Games.FirstOrDefault(g => g.Id == options.GameData.Id);
            if (l_ != null)
            {
                var l__ = l_.Tags;
                if (l__ != null)
                {
                    tags = l__.Select(t => t.Name).ToList();
                }
            }
            var url = url_base + HttpUtility.HtmlEncode(options.GameData.Name);
            var json = webClient.DownloadString(url);
            var result = JsonConvert.DeserializeObject<Classes.ApiResult>(json);
            // api.Dialogs.ShowMessage(JsonConvert.SerializeObject(result));
            if (result is null || result.Query is null || result.Query.Data is null) return null;
            var engine = result.Query.Data.Where(i => i.Property == "Uses_engine").FirstOrDefault()?.Dataitem.FirstOrDefault().Item;
            if (engine != null)
                tags.Add("engine:" + engine.Replace("#404#", ""));
            api.Dialogs.ShowMessage(JsonConvert.SerializeObject(tags));
            return tags;
        }
    }
}