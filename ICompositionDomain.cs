using RepartitionTournoi.Models;

namespace RepartitionTournoi.Domain
{
    public interface ICompositionDomain
    {
        List<Composition> GetCompositions();
    }
}
