using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPreventa
{
    public class controlarEncriptacion
    {
        public int tipoEncriptacion(string pass)
        {
            //1: md5:32ch
            //2: sha1:40ch
            //3: sha256:64ch
            if (pass.Length == 32) 
            {
                return 1;
            }else if (pass.Length == 40)
            {
                return 2;
            }else if (pass.Length == 64)
            {
                return 3;
            }
            else
            {
                return 0;
            }            
        }
    }

}
