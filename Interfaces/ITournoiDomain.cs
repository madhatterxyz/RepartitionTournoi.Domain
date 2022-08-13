using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain.Interfaces
{
    public interface ITournoiDomain
    {
        Task<TournoiDTO> Create(string nom);
        IEnumerable<MatchDTO> Chunk(IEnumerable<JoueurDTO> joueurs, int chunk);
        int GetGroupSize(IEnumerable<JoueurDTO> joueurs);
        Task<TournoiDTO> GetById(long id);
        Task<List<TournoiDTO>> All();

    }
}
