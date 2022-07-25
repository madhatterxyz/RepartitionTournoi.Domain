using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain
{
    public interface ITournoiDomain
    {
        IEnumerable<Match> Chunk(IEnumerable<Joueur> joueurs, int chunk);
        int GetGroupSize(IEnumerable<Joueur> joueurs);

    }
}
