using System.Collections.Generic;

namespace Shoelace.Bejeweld
{
    public interface IMatchFinder
    {
        Match[] LookForMatches();
        List<Match> FindHorizontalMatches();
        List<Match> FindVerticalMatches();
    }   
}