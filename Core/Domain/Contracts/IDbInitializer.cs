using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDbInitializer
    {
        /*
         * If There Is No DataBase --> Create It.
         * If There Are Any Pending Migrations --> Apply Them. 
         * If The DataBase Is Empty --> Seed Data (DataSeeding)
         */


        // Always Deal With The DataBase with --> Async
        Task InitializeAsync();
        Task InitializeIdentityAsync();

    }
}
