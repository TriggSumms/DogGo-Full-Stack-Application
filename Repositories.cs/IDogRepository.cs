using DogGo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
       void AddDog(Dog dog);
        void DeleteDog(int dogId);
        Dog GetDogByBreed(string breed);
        List<Dog> GetAllDogs();
        Dog GetDogById(int id);
        void UpdateDog(Dog dog);
        List<Dog> GetDogsByOwnerId(int id);

        List<Dog> GetDogsByWalkerId(int id);
    }
}