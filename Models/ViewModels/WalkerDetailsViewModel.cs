using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerDetailsViewModel
    {
        public Walker walker { get; set; }
        public List<Walk> Walks{ get; set; }

        public List<Walker> Walkers { get; set; }
        public List<Dog> Dogs { get; set; }

        public List<Owner> Owners { get; set; }
        
    }
}
