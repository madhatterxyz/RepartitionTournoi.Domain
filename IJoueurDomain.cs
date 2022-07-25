using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain;

public interface IJoueurDomain
{
    IEnumerable<Joueur> All();
    List<Joueur> GetRandomJoueurs();

}

