using Bluscream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PCGWMetaData.Classes
{
    public class Cache
    {
        internal List<Game> games;
        internal DirectoryInfo cacheDir;
        internal PCGWMetaDataPlugin plugin;

        public Cache(PCGWMetaDataPlugin plugin)
        {
            this.plugin = plugin;
            cacheDir = new DirectoryInfo(plugin.GetPluginUserDataPath()).Combine("cache");
            cacheDir.Create();
            games = new List<Game>();
            refresh();
        }

        public List<Game> refresh()
        {
            games.Clear();
            foreach (var file in cacheDir.EnumerateFiles("*.json", SearchOption.TopDirectoryOnly))
            {
                games.Add(new Game(file.FileNameWithoutExtension(), this));
            }
            // games = .Select(f => f.FileNameWithoutExtension()).ToList();
            return games;
        }

        private Game addGame(string name)
        {
            var game = new Game(name, this);
            //plugin.api.Dialogs.ShowMessage(name + " " + game.Name + " " + game.EncodedUri());
            var json = plugin.webClient.DownloadString(string.Format(PCGWMetaDataPlugin.url_base, game.EncodedUri()));
            game.File().WriteAllText(json);
            games.Add(game);
            return game;
        }

        private Game _getGame(string name)
        {
            return games.FirstOrDefault(g => g.Name == name);
        }

        public Game getGame(string name)
        {
            var game = _getGame(name);
            if (game is null) 
                game = addGame(name);
            return game;
        }

        public void Purge()
        {
            foreach (var file in cacheDir.EnumerateFiles())
                file.Delete();
            refresh();
        }

        public void PurgeOutdated()
        {
            foreach (var game in games)
            {
                if (game.isOutdated())
                    game.File().Delete();
            }
            refresh();
        }
    }

    public class Game
    {
        public string Name;
        private Cache _cache;

        public Game(string Name, Cache _cache)
        {
            this.Name = Name; this._cache = _cache;
        }

        public FileInfo File()
        {
            return _cache.cacheDir.CombineFile(string.Format("{0}.json", this.EncodedName()));
        }

        public ApiResult Data()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResult>(File().ReadAllText());
        }

        public string EncodedUri() => Uri.EscapeUriString(this.Name);
        public string EncodedName() => HttpUtility.UrlEncode(this.Name.ToLowerInvariant());
        //public string DecodedName() => HttpUtility.HtmlDecode(this.Name);

        public bool isOutdated()
        {
            return DateTime.Now - System.IO.File.GetLastWriteTime(File().FullName) > TimeSpan.FromDays(30); // option
        }
    }
}