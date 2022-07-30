using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain
{
    public class TournoiDomain : ITournoiDomain
    {
        private readonly int _nbJoueurParEquipeMin;
        private readonly int _nbJoueurIdealParEquipe;
        private readonly int _nbJoueurParEquipeMax;
        public TournoiDomain()
        {
            _nbJoueurParEquipeMin = 2;
            _nbJoueurIdealParEquipe = 3;
            _nbJoueurParEquipeMax = 4;
        }

        public IEnumerable<MatchDTO> Chunk(IEnumerable<JoueurDTO> joueurs, int chunk)
        {

            int nbGroups = decimal.ToInt32(Math.Ceiling(Decimal.Divide(joueurs.Count(), chunk)));

            List<MatchDTO> matchs = new List<MatchDTO>();
            for (int i = 0; i < nbGroups; i++)
            {
                MatchDTO match = new MatchDTO() { Id = i + 1 };
                match.Scores.AddRange(joueurs.Skip(i * chunk).Take(chunk).Select(x=> new ScoreDTO() { Joueur = x, Points = 0}));
                matchs.Add(match);
            }
            if (matchs.Any(x => x.Scores.Count() == 1))
            {
                JoueurDTO joueur = matchs.First(x => x.Scores.Count() == 1).Scores.First().Joueur;
                matchs.Skip(matchs.Count() - 2).Take(1).Single().Scores.Add( new ScoreDTO() { Joueur = joueur, Points = 0 });
                matchs.RemoveAt(matchs.Count() - 1);
            }
            return matchs;
        }
        public int GetGroupSize(IEnumerable<JoueurDTO> joueurs)
        {
            int chunkSize = _nbJoueurParEquipeMin;
            if (joueurs.Count() % 3 == 0 || joueurs.Count() % 4 == 1)
            {
                chunkSize = _nbJoueurIdealParEquipe;
            }
            else if (joueurs.Count() >= (_nbJoueurParEquipeMax + (_nbJoueurParEquipeMax / 2)))
            {
                chunkSize = _nbJoueurParEquipeMax;
            }
            else if (joueurs.Count() % 2 != 0)
            {
                chunkSize = _nbJoueurIdealParEquipe;
            }

            return chunkSize;
        }

    }
}
