using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain.Interfaces
{
    public interface IJeuDomain
    {
        Task<IEnumerable<JeuDTO>> GetAll();
        Task<List<JeuDTO>> GetRandomJeus();
        Task<JeuDTO> Create(JeuDTO JeuDTO);
        Task<JeuDTO> Update(JeuDTO JeuDTO);
        Task Delete(long id);

    }
}
