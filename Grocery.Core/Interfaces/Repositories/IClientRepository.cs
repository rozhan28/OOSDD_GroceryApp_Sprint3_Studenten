using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Client? Get(string email);
        Client? Get(int id);
        List<Client> GetAll();

        void Add(Client client); 
    }
}
