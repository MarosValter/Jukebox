namespace Jukebox.Player.Manager
{
    public interface IPlayerManager
    {
        IPlayer GetPlayer(PlayerType type);
    }
}
