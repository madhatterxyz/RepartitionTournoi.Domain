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

    public IEnumerable<Joueur> All()
    {
        return _joueurDAL.All();
    }
    public Joueur GetById(int id)
    {
        return _joueurDAL.GetById(id);
    }
    private Random rnd = new Random();

    public List<Joueur> GetRandomJoueurs()
    {
        List<Joueur> resultList = All().ToList();
        Shuffle(resultList);
        return resultList;
    }

    private void Shuffle<T>(IList<T> list)
    {
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

}

