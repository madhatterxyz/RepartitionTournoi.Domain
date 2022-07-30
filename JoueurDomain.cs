using RepartitionTournoi.DAL.Entities;
using RepartitionTournoi.DAL.Interfaces;
using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain;

public class JoueurDomain : IJoueurDomain
{
    private readonly IJoueurDAL _joueurDAL;
    public JoueurDomain(IJoueurDAL joueurDAL)
    {
        _joueurDAL = joueurDAL;
    }

    public async Task<IEnumerable<JoueurDTO>> GetAll()
    {
        var joueurDTOs = await _joueurDAL.GetAll();
        return joueurDTOs.Select(x => new JoueurDTO(x.Id, x.Nom, x.Prénom, x.Telephone));
    }
    public async Task<JoueurDTO> GetById(long id)
    {
        var x = await _joueurDAL.GetById(id);
        return new JoueurDTO(x.Id, x.Nom, x.Prénom, x.Telephone);
    }
    public async Task<JoueurDTO> Create(JoueurDTO joueurDTO)
    {
        Joueur joueur = await _joueurDAL.Create(new Joueur()
        {
            Id = joueurDTO.Id,
            Nom = joueurDTO.Nom,
            Prénom = joueurDTO.Prénom,
            Telephone = joueurDTO.Téléphone
        });
        return new JoueurDTO(joueur.Id, joueur.Nom, joueur.Prénom, joueur.Telephone);
    }

    public async Task<List<JoueurDTO>> GetRandomJoueurs()
    {
        var all = await GetAll();
        List<JoueurDTO> resultList = all.ToList();
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

    public async Task<JoueurDTO> Update(JoueurDTO joueurDTO)
    {
        return _joueurDAL.Convert(await _joueurDAL.Update(new Joueur()
        {
            Id = joueurDTO.Id,
            Nom = joueurDTO.Nom,
            Prénom = joueurDTO.Prénom,
            Telephone = joueurDTO.Téléphone
        }));
    }

    public async Task Delete(long id)
    {
        await _joueurDAL.DeleteById(id);
    }

}

