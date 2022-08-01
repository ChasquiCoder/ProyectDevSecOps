using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ABB.Catalogo.Entidades.Core;
using ABB.Catalogo.LogicaNegocio.Core;
using System.Configuration;
using System.Net.Http;
using System.Data.SqlClient;

namespace ABB.Catalogo.ClienteWeb.Controllers
{
    public class UsuariosController : Controller
    {
        string RutaApi = "https://localhost:44370/api/"; //define la ruta del web api
        string jsonMediaType = "application/json"; // define el tipo de dat

        // GET: Usuarios
        public ActionResult Index()
        {
            string controladora = "";
            string metodo = "";
            TokenResponse tokenrsp = new TokenResponse();
            //llamada al web Api de Autorizacion.
            tokenrsp = Respuest();
            //llamada al Web Api para listar Usuarios.
            controladora = "Usuarios";
            metodo = "Get";
            List<Usuarios> listausuarios = new List<Usuarios>();
            using (WebClient usuario = new WebClient())
            {
                usuario.Headers.Clear();//borra datos anteriores
                //establece el token de autorizacion en la cabecera
                usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                //establece el tipo de dato de tranferencia
                usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                usuario.Encoding = UTF8Encoding.UTF8;
                string rutacompleta = RutaApi + controladora;
                //ejecuta la busqueda en la web api usando metodo GET
                var data = usuario.DownloadString(new Uri(rutacompleta));
                // convierte los datos traidos por la api a tipo lista de usuarios
                listausuarios = JsonConvert.DeserializeObject<List<Usuarios>>(data);
            }


            return View(listausuarios);
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            string controladora = "Usuarios";
            string metodo = "GetUserId";
            Usuarios users = new Usuarios();
            TokenResponse tokenrsp = new TokenResponse();
            tokenrsp = Respuest();
            using (WebClient usuario = new WebClient())
            {
                usuario.Headers.Clear();//borra datos anteriores
                usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                //establece el tipo de dato de tranferencia
                usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                usuario.Encoding = UTF8Encoding.UTF8;
                string rutacompleta = RutaApi + controladora + "?IdUsuario=" + id;
                //ejecuta la busqueda en la web api usando metodo GET
                var data = usuario.DownloadString(new Uri(rutacompleta));

                // convierte los datos traidos por la api a tipo lista de usuarios
                users = JsonConvert.DeserializeObject<Usuarios>(data);
            }

            return View(users);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            Usuarios usuario = new Usuarios();// se crea uns instancia de la clase usuario

            List<Rol> listarol = new List<Rol>();
            listarol = new RolLN().ListaRol();
            listarol.Add(new Rol() { IdRol = 0, DesRol = "[Seleccione Rol...]" });
            ViewBag.listaRoles = listarol;

            return View(usuario);
        }

        // POST: Usuarios/Create
        [HttpPost]
        public ActionResult Create(Usuarios collection)
        {

            string controladora = "Usuarios";
            try
            {
                TokenResponse tokenrsp = new TokenResponse();
                //llamada al web Api de Autorizacion.
                tokenrsp = Respuest();
                // TODO: Add insert logic here
                using (WebClient usuario = new WebClient())
                {
                    usuario.Headers.Clear();//borra datos anteriores
                    //establece el token de autorizacion en la cabecera
                    usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                    //establece el tipo de dato de tranferencia
                    usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    //typo de decodificador reconocimiento carecteres especiales
                    usuario.Encoding = UTF8Encoding.UTF8;
                    //convierte el objeto de tipo Usuarios a una trama Json
                    var usuarioJson = JsonConvert.SerializeObject(collection);
                    string rutacompleta = RutaApi + controladora;
                    var resultado = usuario.UploadString(new Uri(rutacompleta), usuarioJson);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {

            string controladora = "Usuarios";
            string metodo = "GetUserId";
            TokenResponse tokenrsp = new TokenResponse();
            //llamada al web Api de Autorizacion.
            tokenrsp = Respuest();

            Usuarios users = new Usuarios();
            using (WebClient usuario = new WebClient())
            {
                usuario.Headers.Clear();//borra datos anteriores
                //establece el token de autorizacion en la cabecera
                usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                //establece el tipo de dato de tranferencia
                usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                usuario.Encoding = UTF8Encoding.UTF8;
                string rutacompleta = RutaApi + controladora + "?IdUsuario=" + id;
                //ejecuta la busqueda en la web api usando metodo GET
                var data = usuario.DownloadString(new Uri(rutacompleta));

                // convierte los datos traidos por la api a tipo lista de usuarios
                users = JsonConvert.DeserializeObject<Usuarios>(data);
            }

            List<Rol> listarol = new List<Rol>();
            listarol = new RolLN().ListaRol();
            listarol.Add(new Rol() { IdRol = 0, DesRol = "[Seleccione Rol...]" });
            ViewBag.listaRoles = listarol;

            return View(users);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        // [HttpPut]
        public ActionResult Edit(int Id, Usuarios collection)
        {
            string controladora = "Usuarios";
            collection.IdUsuario = Id;
            collection.ClaveTxt = "";
            collection.Clave = null;
            collection.DesRol = "";

            string metodo = "Put";
            TokenResponse tokenrsp = new TokenResponse();
            try
            {
                //llamada al web Api de Autorizacion.
                tokenrsp = Respuest();
                // TODO: Add update logic here

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    //client.DefaultRequestHeaders.Add("token", "Bearer " + tokenrsp.Token);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenrsp.Token);
                    client.BaseAddress = new Uri(RutaApi);
                    string rutacompleta = RutaApi + controladora + "/" + Id;
                    // var putTask = client.PutAsJsonAsync($"Usuarios/{collection.IdUsuario}", collection);
                    var putTask = client.PutAsJsonAsync(rutacompleta, collection);
                    putTask.Wait();
                    var result = putTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }

                //    using (WebClient usuario = new WebClient())
                //{
                //    usuario.Headers.Clear();//borra datos anteriores
                //   //establece el token de autorizacion en la cabecera
                //    usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                //    //establece el tipo de dato de tranferencia
                //    usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                //usuario.QueryString = "PUT";
                //    usuario.Encoding = UTF8Encoding.UTF8; 
                //convierte el objeto de tipo Usuarios a una trama Json
                //    var usuarioJson = JsonConvert.SerializeObject(collection);
                //    string rutacompleta = RutaApi + controladora + "?IdUsuario=" + Id;
                // string rutacompleta = RutaApi + controladora + "?=" + Id;
                //    var resultado = usuario.UploadString((rutacompleta),metodo, usuarioJson);

                // }

                // return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return RedirectToAction("Index");
                // return View();
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            string controladora = "Usuarios";
            string metodo = "GetUserId";
            Usuarios users = new Usuarios();
            TokenResponse tokenrsp = new TokenResponse();
            tokenrsp = Respuest();
            using (WebClient usuario = new WebClient())
            {
                usuario.Headers.Clear();//borra datos anteriores
                                        // usuario.DefaultRequestHeaders.Add("token", "Bearer " + tokenrsp.Token)
                usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                //establece el tipo de dato de tranferencia
                usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                usuario.Encoding = UTF8Encoding.UTF8;
                string rutacompleta = RutaApi + controladora + "?IdUsuario=" + id;
                //ejecuta la busqueda en la web api usando metodo GET
                var data = usuario.DownloadString(new Uri(rutacompleta));

                // convierte los datos traidos por la api a tipo lista de usuarios
                users = JsonConvert.DeserializeObject<Usuarios>(data);
            }

            return View(users);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string controladora = "Usuarios/";
           
            try
            {
                TokenResponse tokenrsp = new TokenResponse();
                tokenrsp = Respuest();
                // TODO: Add insert logic here
                using (WebClient usuario = new WebClient())
                {

                    Usuarios usuarios = new Usuarios();
                    usuario.Headers.Clear();//borra datos anteriores
                    usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                    //establece el tipo de dato de tranferencia
                    usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    //typo de decodificador reconocimiento carecteres especiales
                    usuario.Encoding = UTF8Encoding.UTF8;

                    //convierte el objeto de tipo Usuarios a una trama Json
                   // var usuarioJson = JsonConvert.SerializeObject(collection);
                    string dataString2 = "";
                    byte[] dataBytes2 = Encoding.UTF8.GetBytes(dataString2);


                    string rutacompleta = RutaApi + controladora + id;
                    //var resultado = usuario.UploadString(new Uri(rutacompleta), "DELETE", usuarioJson);
                     var resultado = usuario.UploadData(new Uri(rutacompleta), "DELETE", dataBytes2);//INTENTA EJECUTAR ETA LINEA Y SE PIERDE
                    string responseString2 = Encoding.UTF8.GetString(resultado);

                }

                return RedirectToAction("Index");
            }
            catch
            {

                return View();
            }
        }

        private TokenResponse Respuest()
        {
            TokenResponse respuesta = new TokenResponse();
            string controladora = "Auth";
            string metodo = "Post";
            var resultado = "";
            UsuariosAPI usuapi = new UsuariosAPI();
            usuapi.Codigo = Convert.ToInt32(ConfigurationManager.AppSettings["UsuApiCodigo"]);
            usuapi.UserName = ConfigurationManager.AppSettings["UsuApiUserName"];
            usuapi.Clave = ConfigurationManager.AppSettings["UsuApiClave"];
            usuapi.Nombre = ConfigurationManager.AppSettings["UsuApiNombre"];
            usuapi.Rol = ConfigurationManager.AppSettings["UsuApiRol"];
            using (WebClient usuarioapi = new WebClient())
            {
                usuarioapi.Headers.Clear();//borra datos anteriores
                //establece el tipo de dato de tranferencia
                usuarioapi.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                usuarioapi.Encoding = UTF8Encoding.UTF8;
                //convierte el objeto de tipo Usuarios a una trama Json
                var usuarioJson = JsonConvert.SerializeObject(usuapi);
                string rutacompleta = RutaApi + controladora;
                resultado = usuarioapi.UploadString(new Uri(rutacompleta), usuarioJson);
                respuesta = JsonConvert.DeserializeObject<TokenResponse>(resultado);
            }
            return respuesta;
        }


    }
}
