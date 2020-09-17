namespace Jukebox.Player.Search
{
    public interface ISearchEngineProvider
    {
        ISearchEngine GetSearchEngine(PlayerType type);
    }
}
