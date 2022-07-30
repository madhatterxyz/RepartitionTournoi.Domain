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
        public async Task<List<CompositionDTO>> InitCompositions()
        {
            List<CompositionDTO> compositions = new List<CompositionDTO>();
            IEnumerable<JoueurDTO> joueurs = await _joueurDomain.GetRandomJoueurs();
            int chunkSize = _tournoiDomain.GetGroupSize(joueurs);
            var comparer = new ObjectsComparer.Comparer<IEnumerable<MatchDTO>>();
            foreach (var jeu in await _jeuDAL.GetAll())
            {
                CompositionDTO composition = new CompositionDTO() { Jeu = new JeuDTO(jeu.Id, jeu.Nom, jeu.NbJoueursMin, jeu.NbJoueursMax) };
                IEnumerable<MatchDTO> groupes = _tournoiDomain.Chunk(joueurs, chunkSize);
                while (compositions.Any(x => comparer.Compare(x.Matchs, groupes)))
                {
                    groupes = _tournoiDomain.Chunk(await _joueurDomain.GetRandomJoueurs(), chunkSize);
                }
                if (!compositions.Any(x => x.Matchs == groupes))
                    composition.Matchs = groupes;
                compositions.Add(composition);
            }
            return compositions;
        }

    }
}
