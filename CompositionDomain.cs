using RepartitionTournoi.DAL.Interfaces;
using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain
{
    public class CompositionDomain : ICompositionDomain
    {
        private readonly ITournoiDomain _tournoiDomain;
        private readonly IJeuDAL _jeuDAL;
        private readonly IJoueurDomain _joueurDomain;
        public CompositionDomain(ITournoiDomain tournoiDomain, IJeuDAL jeuDAL, IJoueurDomain joueurDomain)
        {
            _tournoiDomain = tournoiDomain;
            _jeuDAL = jeuDAL;
            _joueurDomain = joueurDomain;
        }
        /// <summary>
        /// Pour chaque jeu, je veux une composition des groupes différente d'une existante
        /// </summary>
        /// <returns></returns>
        public List<Composition> GetCompositions()
        {
            List<Composition> compositions = new List<Composition>();
            IEnumerable<Joueur> joueurs = _joueurDomain.GetRandomJoueurs();
            int chunkSize = _tournoiDomain.GetGroupSize(joueurs);
            var comparer = new ObjectsComparer.Comparer<IEnumerable<Match>>();
            foreach (Jeu jeu in _jeuDAL.All())
            {
                Composition composition = new Composition() { Jeu = jeu };
                IEnumerable<Match> groupes = _tournoiDomain.Chunk(joueurs, chunkSize);
                while (compositions.Any(x => comparer.Compare(x.Matchs , groupes)))
                {
                    groupes = _tournoiDomain.Chunk(_joueurDomain.GetRandomJoueurs(), chunkSize);
                }
                if (!compositions.Any(x => x.Matchs == groupes))
                    composition.Matchs = groupes;
                compositions.Add(composition);
            }
            return compositions;
        }

    }
}
