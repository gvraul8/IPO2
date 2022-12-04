using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HITO2_IPO_NUEVO
{
    internal class Usuario
    {
        public string  User { set; get; }
        public String Pass { set; get; }

        public Usuario(string user, string pass)
        {
            User = user;
            Pass = pass;
        }
    }
}
