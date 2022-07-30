using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain
{
    public interface ICompositionDomain
    {
        Task<List<CompositionDTO>> InitCompositions();
    }
}
