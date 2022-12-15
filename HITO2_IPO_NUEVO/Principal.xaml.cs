using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml;

namespace HITO2_IPO_NUEVO
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        private MainWindow login;
        List<Ruta> listadoRutas = new List<Ruta>();
        List<PuntoInteres> listadoPuntosInteres = new List<PuntoInteres>();
        List<Excursionista> listadoExcursionistas = new List<Excursionista>();
        List<Guia> listadoGuias = new List<Guia>();
        List<Oferta> listadoOfertas = new List<Oferta>();
        Usuario user;

        public Principal(Usuario u)
        {
            user = u;
            InitializeComponent();
            inicializaHerramientas();

            listadoRutas = CargarContenidoRutasXML();
            imprimirNombreRutas();
            
            PrintUserData();

            //----------- A PARTIR DE AQUÍ EL CÓDIGO ES PARA PRUEBAS ----------------

            listadoPuntosInteres = CargarContenidoPuntosInteresXML();
            imprimirNombrePuntosInteres();

            listadoExcursionistas = CargarContenidoExcursionistasXML();
            imprimirNombreExcursionistas();

            listadoGuias = CargarContenidoGuiasXML();
            imprimirNombreGuias();

            listadoOfertas = CargarContenidoOfertasXML();
            imprimirNombreOfertas();

        }

        void inicializaHerramientas()
        {
            cb_rutasExcursionistas.IsEnabled = false;
        }

        void PrintUserData()
        {
            lbNombreUsuario.Content = user.Name.ToString(); ;
            lbApellidosUsuario.Content = user.LastName.ToString();
            lbEmailUsuario.Content=user.Email.ToString();  
           // lbUltimoAccesoUsuario.Content = user.LastLogin.ToString();

            var fullFilePath = user.ImgUrl.ToString();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            imgUsuario.Source = bitmap;
        }

        private void btCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            login= new MainWindow();
            login.Show();
            this.Close();
        }

        private void btn_Ayuda_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/gvraul8/IPO/wiki/AYUDA");
        }

        private List<Ruta> CargarContenidoRutasXML()
        {
            List<Ruta> listado = new List<Ruta>();
            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            
            var fichero = Application.GetResourceStream(new Uri("Datos/rutas.xml", UriKind.Relative));
            //var fichero = Application.GetResourceStream(new Uri("bin/Debug/rutas.xml", UriKind.Relative));
            
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                //Ruta(string nombre, string origen, string destino, string provincia, DateTime fecha, string dificultad, int plazasDisponibles, string material, int numRealizaciones)
                var nuevaRuta = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                nuevaRuta.Nombre = node.Attributes["Nombre"].Value;
                nuevaRuta.Origen = node.Attributes["Origen"].Value;
                nuevaRuta.Destino = node.Attributes["Destino"].Value;
                nuevaRuta.Provincia = node.Attributes["Provincia"].Value;
                nuevaRuta.Fecha = Convert.ToDateTime(node.Attributes["Fecha"].Value);
                nuevaRuta.Dificultad = node.Attributes["Dificultad"].Value;
                nuevaRuta.PlazasDisponibles = Convert.ToInt32(node.Attributes["PlazasDisponibles"].Value);
                nuevaRuta.MaterialNecesario = node.Attributes["MaterialNecesario"].Value;
                nuevaRuta.NumeroDeRealizaciones = Convert.ToInt32(node.Attributes["NumeroDeRealizaciones"].Value);
                nuevaRuta.URL_RUTA = new Uri(node.Attributes["URL_RUTA"].Value, UriKind.Absolute);
                //nuevaRuta.URL_INTERES = new Uri(node.Attributes["URL_INTERES"].Value, UriKind.Absolute);
                listado.Add(nuevaRuta);
            }
            return listado;
        }

        private List<PuntoInteres> CargarContenidoPuntosInteresXML()
        {

            listadoRutas = CargarContenidoRutasXML();

            List<PuntoInteres> listado = new List<PuntoInteres>();
            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/puntosInteres.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                //Ruta(string nombre, string origen, string destino, string provincia, DateTime fecha, string dificultad, int plazasDisponibles, string material, int numRealizaciones)
                var rutaAux = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                              
                var nuevoPuntoInteres = new PuntoInteres("","", rutaAux);
                nuevoPuntoInteres.Nombre = node.Attributes["Nombre"].Value;
                nuevoPuntoInteres.Descripcion = node.Attributes["Nombre"].Value;
                for (int i = 0; i < listadoRutas.Count; i++)
                {
                    if (listadoRutas[i].Nombre == node.Attributes["NombreRuta"].Value)
                    {
                        nuevoPuntoInteres.Ruta = listadoRutas[i];
                    }
                }
                nuevoPuntoInteres.URL_IMAGEN = new Uri(node.Attributes["Imagen"].Value, UriKind.Absolute);

                listado.Add(nuevoPuntoInteres);

            }
            return listado;
        }

        private List<Excursionista> CargarContenidoExcursionistasXML()
        {
            listadoRutas = CargarContenidoRutasXML();
            List<Excursionista> listado = new List<Excursionista>();

            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/excursionistas.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var rutaAux = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                var nuevoExcursionista = new Excursionista("","","","","", DateTime.Today,"", 1, true, listadoRutas, listadoRutas);
                nuevoExcursionista.User = node.Attributes["User"].Value;
                nuevoExcursionista.Pass = node.Attributes["Pass"].Value;
                nuevoExcursionista.Name = node.Attributes["Name"].Value;
                nuevoExcursionista.LastName = node.Attributes["LastName"].Value;
                nuevoExcursionista.Email = node.Attributes["Email"].Value;
                nuevoExcursionista.LastLogin = Convert.ToDateTime(node.Attributes["LastLogin"].Value);
                nuevoExcursionista.Telefono = node.Attributes["Phone"].Value;
                nuevoExcursionista.ImgUrl = new Uri(node.Attributes["ImgUrl"].Value);
                nuevoExcursionista.Edad = int.Parse(node.Attributes["Edad"].Value);
                nuevoExcursionista.Notificaciones = Convert.ToBoolean(node.Attributes["Notificaciones"].Value);
                
                string[] rutasRealizadas = node.Attributes["RutasRealizadas"].Value.Split(',');
                List<Ruta> listadoRutasRealizadas = new List<Ruta>();
                foreach (string nombreRutaRealizadas in rutasRealizadas)
                {
                    for (int i = 0; i < listadoRutas.Count; i++)
                    {
                        if (listadoRutas[i].Nombre == nombreRutaRealizadas)
                        {
                            listadoRutasRealizadas.Add(listadoRutas[i]);
                        }
                    }
                }
                nuevoExcursionista.RutasRealizadas = listadoRutasRealizadas;

                string[] RutasFuturas = node.Attributes["RutasFuturas"].Value.Split(',');
                List<Ruta> listadoRutasFuturas = new List<Ruta>();
                foreach (string nombreRutaFuturas in RutasFuturas)
                {
                    for (int i = 0; i < listadoRutas.Count; i++)
                    {
                        if (listadoRutas[i].Nombre == nombreRutaFuturas)
                        {
                            listadoRutasFuturas.Add(listadoRutas[i]);
                        }
                    }
                }
                nuevoExcursionista.RutasFuturas = listadoRutasFuturas;

                listado.Add(nuevoExcursionista);
            }
            return listado;
        }

        private List<Guia> CargarContenidoGuiasXML()
        {
            listadoRutas = CargarContenidoRutasXML();
            List<Guia> listado = new List<Guia>();

            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/guias.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var rutaAux = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                var nuevoGuia = new Guia("", "", "", "", "", DateTime.Today, "","", listadoRutas, listadoRutas);
                nuevoGuia.User = node.Attributes["User"].Value;
                nuevoGuia.Pass = node.Attributes["Pass"].Value;
                nuevoGuia.Name = node.Attributes["Name"].Value;
                nuevoGuia.LastName = node.Attributes["LastName"].Value;
                nuevoGuia.Email = node.Attributes["Email"].Value;
                nuevoGuia.LastLogin = Convert.ToDateTime(node.Attributes["LastLogin"].Value);
                nuevoGuia.Telefono = node.Attributes["Phone"].Value;
                nuevoGuia.ImgUrl = new Uri(node.Attributes["ImgUrl"].Value);
                nuevoGuia.Disponibilidad = node.Attributes["Disponibilidad"].Value;

                string[] rutasRealizadas = node.Attributes["RutasRealizadas"].Value.Split(',');
                List<Ruta> listadoRutasRealizadas = new List<Ruta>();
                foreach (string nombreRutaRealizadas in rutasRealizadas)
                {
                    for (int i = 0; i < listadoRutas.Count; i++)
                    {
                        if (listadoRutas[i].Nombre == nombreRutaRealizadas)
                        {
                            listadoRutasRealizadas.Add(listadoRutas[i]);
                        }
                    }
                }
                nuevoGuia.RutasRealizadas = listadoRutasRealizadas;

                string[] RutasFuturas = node.Attributes["RutasFuturas"].Value.Split(',');
                List<Ruta> listadoRutasFuturas = new List<Ruta>();
                foreach (string nombreRutaFuturas in RutasFuturas)
                {
                    for (int i = 0; i < listadoRutas.Count; i++)
                    {
                        if (listadoRutas[i].Nombre == nombreRutaFuturas)
                        {
                            listadoRutasFuturas.Add(listadoRutas[i]);
                        }
                    }
                }
                nuevoGuia.RutasFuturas = listadoRutasFuturas;

                listado.Add(nuevoGuia);
            }
            return listado;
        }

        private List<Oferta> CargarContenidoOfertasXML()
        {
            listadoRutas = CargarContenidoRutasXML();
            List<Oferta> listado = new List<Oferta>();

            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/ofertas.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var rutaAux = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                var nuevaOferta = new Oferta(0,rutaAux,"");
                nuevaOferta.Id = int.Parse(node.Attributes["Id"].Value);
                nuevaOferta.Descripcion = node.Attributes["Descripcion"].Value;
                nuevaOferta.IMG_OFERTA = new Uri(node.Attributes["IMG_OFERTA"].Value);
                for (int i = 0; i < listadoRutas.Count; i++)
                {
                    if (listadoRutas[i].Nombre == node.Attributes["Ruta"].Value)
                    {
                        nuevaOferta.Ruta = listadoRutas[i];
                    }
                }
                listado.Add(nuevaOferta);
            }
            return listado;
        }

        private void InsertarRutaEnXML(List<Ruta> listadoAux)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode cabecera = doc.CreateElement("Rutas");
            doc.AppendChild(cabecera);
            doc.InsertBefore(doc.CreateXmlDeclaration("1.0","UTF-8",null), cabecera);
           
            XmlNode rutaInsertar = doc.CreateElement("Ruta");

            foreach (Ruta rutaAux in listadoAux)
            {
                rutaInsertar = doc.CreateElement("Ruta");

                XmlAttribute nombre = doc.CreateAttribute("Nombre");
                nombre.Value = rutaAux.Nombre;
                rutaInsertar.Attributes.Append(nombre);

                XmlAttribute origen = doc.CreateAttribute("Origen");
                origen.Value = rutaAux.Origen;
                rutaInsertar.Attributes.Append(origen);

                XmlAttribute destino = doc.CreateAttribute("Destino");
                destino.Value = rutaAux.Destino;
                rutaInsertar.Attributes.Append(destino);

                XmlAttribute provincia = doc.CreateAttribute("Provincia");
                provincia.Value = rutaAux.Provincia;
                rutaInsertar.Attributes.Append(provincia);

                XmlAttribute fecha = doc.CreateAttribute("Fecha");
                fecha.Value = rutaAux.Fecha.ToString();
                rutaInsertar.Attributes.Append(fecha);

                XmlAttribute dificultad = doc.CreateAttribute("Dificultad");
                dificultad.Value = rutaAux.Dificultad;
                rutaInsertar.Attributes.Append(dificultad);

                XmlAttribute plazasDisponibles = doc.CreateAttribute("PlazasDisponibles");
                plazasDisponibles.Value = rutaAux.PlazasDisponibles.ToString();
                rutaInsertar.Attributes.Append(plazasDisponibles);

                XmlAttribute materialNecesario = doc.CreateAttribute("MaterialNecesario");
                materialNecesario.Value = rutaAux.MaterialNecesario;
                rutaInsertar.Attributes.Append(materialNecesario);

                XmlAttribute numeroRealizaciones = doc.CreateAttribute("NumeroDeRealizaciones");
                numeroRealizaciones.Value = rutaAux.NumeroDeRealizaciones.ToString();
                rutaInsertar.Attributes.Append(numeroRealizaciones);

                XmlAttribute url_Ruta = doc.CreateAttribute("URL_RUTA");
                url_Ruta.Value = rutaAux.URL_RUTA.ToString();
                rutaInsertar.Attributes.Append(url_Ruta);

                cabecera.AppendChild(rutaInsertar);
            }
            
            doc.Save("rutas2.xml");
            //doc.Save("Datos/rutas.xml");
        }
   
        private void imprimirNombreRutas()
        {
            foreach (Ruta ruta in listadoRutas)
            {
                ListBoxRutas.Items.Add(ruta.Nombre);
            }
        }

        private void imprimirNombreGuias()
        {
            foreach (Guia guia in listadoGuias)
            {
                ListBoxGuias.Items.Add(guia.Name); 
            }
            
        }

        private void imprimirNombreExcursionistas()
        {
            foreach (Excursionista excursionista in listadoExcursionistas)
            {
                ListBoxExcursionistas.Items.Add(excursionista.Name);
            }

        }

        private void imprimirNombreOfertas()
        {
            foreach (Oferta oferta in listadoOfertas)
            {
                ListBoxOfertas.Items.Add(oferta.Id); 
            }
        }

        private void rellenaCasillasRuta(object sender, SelectionChangedEventArgs e)
        {
            int index = ListBoxRutas.SelectedIndex;
            var rutaAux = listadoRutas[index];

            tb_nombre.Text = rutaAux.Nombre.ToString();
            tb_origen.Text = rutaAux.Origen.ToString();
            tb_destino.Text = rutaAux.Origen.ToString();
            tb_provincia.Text = rutaAux.Provincia.ToString();
            tb_dificultad.Text = rutaAux.Dificultad.ToString();
            tb_plazas.Text = rutaAux.PlazasDisponibles.ToString();
            //tb_material.Text = rutaAux.MaterialNecesario.ToString();
            //tb_realizaciones.Text = rutaAux.NumeroDeRealizaciones.ToString();
            dp_fecha.Text = Convert.ToDateTime(rutaAux.Fecha.ToString()).ToString();

            // https://stackoverflow.com/questions/18435829/showing-image-in-wpf-using-the-url-link-from-database
            var fullFilePath = rutaAux.URL_RUTA.ToString();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            img_ruta.Source = bitmap;

            //var fullFilePath2 = rutaAux.URL_INTERES.ToString();
            //BitmapImage bitmap2 = new BitmapImage();
            //bitmap2.BeginInit();
            //bitmap2.UriSource = new Uri(fullFilePath2, UriKind.Absolute);
            //bitmap2.EndInit();

            //img_interesRuta.Source = bitmap2;

            bt_anadir.IsEnabled = false;
            bt_editar.IsEnabled = true;
            bt_eliminar.IsEnabled = true;
        }

        //RELLENAR LUEGO CON LA NUEVA PESTAÑA
        private void imprimirNombrePuntosInteres()
        {

        }

        private void rellenaCasillasExcursionista(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxExcursionistas.SelectedItem != null)
            {
                lb_rutasrealplaExcursionista.Items.Clear();
                cb_rutasExcursionistas.IsEnabled = true;

                int index = ListBoxExcursionistas.SelectedIndex;
                var excursionistaAux = listadoExcursionistas[index];

                lb_nombre_excursionista.Content = excursionistaAux.Name;
                tb_edad.Text = excursionistaAux.Edad.ToString();
                tb_telefonoexcursionista.Text = excursionistaAux.Telefono;
                tb_correoexcursionista.Text = excursionistaAux.Email;
                if (excursionistaAux.Notificaciones)
                {
                    //cb_ofertas.Checked = true;
                }

                bt_anadirExcursionista.IsEnabled = false;
                bt_editarExcursionista.IsEnabled = true;
                bt_eliminarExcursionista.IsEnabled = true;
            }
            else if (ListBoxExcursionistas.SelectedIndex == -1)
            {
                cb_rutasExcursionistas.IsEnabled = false;
            }
        }

        private void ComboBoxRutasExcursionistas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = cb_rutasExcursionistas.SelectedValue.ToString();

            if (selectedItem.Equals("System.Windows.Controls.ComboBoxItem: Rutas Realizadas"))
            {
                imprimeRutasRealizarExcursionista();
            }
            else if (selectedItem.Equals("System.Windows.Controls.ComboBoxItem: Rutas Planificadas"))
            {
                imprimeRutasFuturasExcursionista();
            }
        }

        private void imprimeRutasRealizarExcursionista()
        {
            lb_rutasrealplaExcursionista.Items.Clear();
            int index = ListBoxExcursionistas.SelectedIndex;
            var excursionistaAux = listadoExcursionistas[index];

            foreach (Ruta ruta in excursionistaAux.RutasRealizadas)
            {
                lb_rutasrealplaExcursionista.Items.Add(ruta.Nombre + " (" + ruta.Fecha + ")");
            }
        }

        private void imprimeRutasFuturasExcursionista()
        {
            lb_rutasrealplaExcursionista.Items.Clear();
            int index = ListBoxExcursionistas.SelectedIndex;
            var excursionistaAux = listadoExcursionistas[index];

            foreach (Ruta ruta in excursionistaAux.RutasFuturas)
            {
                lb_rutasrealplaExcursionista.Items.Add(ruta.Nombre + " (" + ruta.Fecha + ")");
            }
        }

        private void rellenaCasillasOferta(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cb_ofertas_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void bt_editars_Click(object sender, RoutedEventArgs e)
        {
            tcPestanas.SelectedIndex = 1;
        }

        private void edicionTextBox(Boolean bloqueado)
        {
            tb_nombre.IsReadOnly = bloqueado;
            tb_origen.IsReadOnly = bloqueado;
            tb_destino.IsReadOnly = bloqueado;
            tb_provincia.IsReadOnly = bloqueado;
            //dp_fecha.IsReadOnly = activado;
            tb_dificultad.IsReadOnly = bloqueado;
            tb_plazas.IsReadOnly = bloqueado;

            if (bloqueado == false)
            {
                tb_nombre.Text = "Escriba aqui el nombre de la ruta";
                bt_editars.Content = "Seleccionar guia";
                bt_consultarPDis.IsEnabled = false;
                tb_nombre.Focus();
            }

        }

        private void bt_editar_Click(object sender, RoutedEventArgs e)
        {
            edicionTextBox(false);
        }

     
        private void bt_consultarPDis_Click(object sender, RoutedEventArgs e)
        {
            bt_consultarPDis.IsEnabled = true;


            tcPestanas.SelectedIndex = 4;
        }

    }
}
    

