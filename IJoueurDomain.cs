using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain;

public interface IJoueurDomain
{
    Task<IEnumerable<JoueurDTO>> GetAll();
    Task<List<JoueurDTO>> GetRandomJoueurs();
    Task<JoueurDTO> Create(JoueurDTO joueurDTO);
    Task<JoueurDTO> Update(JoueurDTO joueurDTO);
    Task Delete(long id);

}

