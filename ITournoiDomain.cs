using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain
{
    public interface ITournoiDomain
    {
        IEnumerable<MatchDTO> Chunk(IEnumerable<JoueurDTO> joueurs, int chunk);
        int GetGroupSize(IEnumerable<JoueurDTO> joueurs);

    }
}
