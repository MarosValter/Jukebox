using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jukebox.Player.Search
{
    public interface ISearchEngine
    {
        Task<IList<SearchResultInfo>> Search(string q, int maxResults = 5);
    }
}
