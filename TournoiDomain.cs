using RepartitionTournoi.DAL.Entities;
using RepartitionTournoi.DAL.Interfaces;
using RepartitionTournoi.Domain.Interfaces;
using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain
{
    public class TournoiDomain : ITournoiDomain
    {
        private readonly int _nbJoueurParEquipeMin;
        private readonly int _nbJoueurIdealParEquipe;
        private readonly int _nbJoueurParEquipeMax;
        private readonly IJeuDAL _jeuDAL;
        private readonly IJoueurDomain _joueurDomain;
        private readonly ITournoiDAL _tournoiDAL;
        private readonly IMatchDAL _matchDAL;
        private readonly ICompositionDAL _compositionDAL;
        private readonly IScoreDAL _scoreDAL;
        public TournoiDomain(IJoueurDomain joueurDomain, IJeuDAL jeuDAL, ITournoiDAL tournoiDAL, IMatchDAL matchDAL, ICompositionDAL compositionDAL, IScoreDAL scoreDAL)
        {
            _jeuDAL = jeuDAL;
            _joueurDomain = joueurDomain;
            _tournoiDAL = tournoiDAL;
            _matchDAL = matchDAL;
            _compositionDAL = compositionDAL;
            _scoreDAL = scoreDAL;
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
                match.Scores.AddRange(joueurs.Skip(i * chunk).Take(chunk).Select(x => new ScoreDTO() { Joueur = x, Points = 0 }));
                matchs.Add(match);
            }
            if (matchs.Any(x => x.Scores.Count() == 1))
            {
                JoueurDTO joueur = matchs.First(x => x.Scores.Count() == 1).Scores.First().Joueur;
                matchs.Skip(matchs.Count() - 2).Take(1).Single().Scores.Add(new ScoreDTO() { Joueur = joueur, Points = 0 });
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
        /// <summary>
        /// Pour chaque jeu, je veux une composition des groupes différente d'une existante
        /// </summary>
        /// <returns></returns>
        private async Task<List<CompositionDTO>> InitCompositions()
        {
            List<CompositionDTO> compositions = new List<CompositionDTO>();
            IEnumerable<JoueurDTO> joueurs = await _joueurDomain.GetRandomJoueurs();
            int chunkSize = GetGroupSize(joueurs);
            var comparer = new ObjectsComparer.Comparer<IEnumerable<MatchDTO>>();
            foreach (var jeu in await _jeuDAL.GetAll())
            {
                CompositionDTO composition = new CompositionDTO() { Jeu = new JeuDTO(jeu.Id, jeu.Nom, jeu.NbJoueursMin, jeu.NbJoueursMax) };
                IEnumerable<MatchDTO> groupes = Chunk(joueurs, chunkSize);
                while (compositions.Any(x => comparer.Compare(x.Matchs, groupes)))
                {
                    groupes = Chunk(await _joueurDomain.GetRandomJoueurs(), chunkSize);
                }
                if (!compositions.Any(x => x.Matchs == groupes))
                    composition.Matchs = groupes.ToList();
                compositions.Add(composition);
            }
            return compositions;
        }
        public async Task<TournoiDTO> Create(string nom)
        {
            Tournoi tournoi = new Tournoi() { Nom = nom };
            await _tournoiDAL.Create(tournoi);
            var compositions = await InitCompositions();
            foreach (var compoDTO in compositions)
            {
                for (int i=0;i< compoDTO.Matchs.Count(); i++)
                {
                    Match match = new Match() { Nom = $"Match {i + 1}" };
                    await _matchDAL.Create(match);
                    foreach (var scoreDTO in compoDTO.Matchs[i].Scores)
                    {
                        Score score = new Score() { JoueurId = scoreDTO.Joueur.Id, MatchId = match.Id, Points = 0 };
                        await _scoreDAL.Create(score);
                    }
                    Composition composition = new Composition() { JeuId = compoDTO.Jeu.Id, MatchId = match.Id, TournoiId = tournoi.Id };
                    await _compositionDAL.Create(composition);
                }
            }
            return _tournoiDAL.Convert(tournoi);
        }
        public async Task<TournoiDTO> GetById(long id)
        {
            return _tournoiDAL.Convert(await _tournoiDAL.GetById(id));
        }
        public async Task<List<TournoiDTO>> All()
        {
            var tournoiDTOs = new List<TournoiDTO>();
            foreach(var tournoi in await _tournoiDAL.GetAll())
            {
                tournoiDTOs.Add(_tournoiDAL.Convert(tournoi));
            }
            return tournoiDTOs; 
        }

    }
}
