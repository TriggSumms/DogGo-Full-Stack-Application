using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetAll();

        //List<Walk> GetWalksByTheWalker(int id);

        //List<Walk> GetWalksByTheDog(int id);

        List<Walk> GetALLWalksandIds(int id);
    }
}