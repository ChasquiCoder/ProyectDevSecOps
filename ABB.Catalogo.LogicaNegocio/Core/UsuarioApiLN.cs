using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Catalogo.Entidades.Base;
using ABB.Catalogo.Entidades.Core;
using ABB.Catalogo.AccesoDatos.Core;

namespace ABB.Catalogo.LogicaNegocio.Core
{
    public class UsuarioApiLN: BaseLN
    {
        public UsuariosAPI BuscaUsuarioApi(UsuariosAPI PamUsuarioApi)
        {
            return new UsuarioApiAD().BuscaUsuarioApi(PamUsuarioApi);
        }

    }
}
