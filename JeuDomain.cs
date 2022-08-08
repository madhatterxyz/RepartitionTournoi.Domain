using RepartitionTournoi.DAL.Entities;
using RepartitionTournoi.DAL.Interfaces;
using RepartitionTournoi.Domain.Interfaces;
using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain;

public class JeuDomain : IJeuDomain
{
    private readonly IJeuDAL _jeuDAL;
    public JeuDomain(IJeuDAL jeuDAL)
    {
        _jeuDAL = jeuDAL;
    }

    public async Task<IEnumerable<JeuDTO>> GetAll()
    {
        var JeuDTOs = await _jeuDAL.GetAll();
        return JeuDTOs.Select(x => new JeuDTO(x.Id, x.Nom, x.NbJoueursMin, x.NbJoueursMax));
    }
    public async Task<JeuDTO> GetById(long id)
    {
        var x = await _jeuDAL.GetById(id);
        return new JeuDTO(x.Id, x.Nom, x.NbJoueursMin, x.NbJoueursMax);
    }
    public async Task<JeuDTO> Create(JeuDTO JeuDTO)
    {
        Jeu Jeu = await _jeuDAL.Create(new Jeu()
        {
            Id = JeuDTO.Id,
            Nom = JeuDTO.Nom,
            NbJoueursMin = JeuDTO.NbJoueursMin,
            NbJoueursMax = JeuDTO.NbJoueursMax
        });
        return new JeuDTO(Jeu.Id, Jeu.Nom, Jeu.NbJoueursMin, Jeu.NbJoueursMax);
    }

    public async Task<List<JeuDTO>> GetRandomJeus()
    {
        var all = await GetAll();
        List<JeuDTO> resultList = all.ToList();
        Shuffle(resultList);
        return resultList;
    }

    private void Shuffle<T>(IList<T> list)
    {
        Random rnd = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public async Task<JeuDTO> Update(JeuDTO JeuDTO)
    {
        return _jeuDAL.Convert(await _jeuDAL.Update(new Jeu()
        {
            Id = JeuDTO.Id,
            Nom = JeuDTO.Nom,
            NbJoueursMin = JeuDTO.NbJoueursMin,
            NbJoueursMax = JeuDTO.NbJoueursMax
        }));
    }

    public async Task Delete(long id)
    {
        await _jeuDAL.DeleteById(id);
    }

}

