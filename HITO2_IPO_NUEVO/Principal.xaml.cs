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
        Usuario user;
        Boolean visible = true;

        public Principal(Usuario u)
        {
            user = u;
            InitializeComponent();

            listadoRutas = CargarContenidoRutasXML();
            foreach (Ruta ruta in listadoRutas)
            {
                ListBoxRutas.Items.Add(ruta.Nombre);
            }
            
            
            PrintUserData();
        }

        private List<Ruta> CargarContenidoRutasXML()
        {
            List<Ruta> listado = new List<Ruta>();
            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/rutas.xml", UriKind.Relative)); 
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
               //Ruta(string nombre, string origen, string destino, string provincia, DateTime fecha, string dificultad, int plazasDisponibles, string material, int numRealizaciones)
                var nuevaRuta = new Ruta("", "","", "",DateTime.Today, "", 0,"", 0);
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
                nuevaRuta.URL_INTERES = new Uri(node.Attributes["URL_INTERES"].Value, UriKind.Absolute);
                listado.Add(nuevaRuta);
            }
            return listado;
        }


        void PrintUserData()
        {
            lbNombreUsuario.Content = user.Name.ToString(); ;
            lbApellidosUsuario.Content = user.LastName.ToString();
            lbEmailUsuario.Content=user.Email.ToString();  
      
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

        private void PrintText(object sender, SelectionChangedEventArgs e)
        {
            int index = ListBoxRutas.SelectedIndex;
            var rutaAux = listadoRutas[index];

            lb_nombre.Content = rutaAux.Nombre.ToString();
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

            var fullFilePath2 = rutaAux.URL_INTERES.ToString();
            BitmapImage bitmap2 = new BitmapImage();
            bitmap2.BeginInit();
            bitmap2.UriSource = new Uri(fullFilePath2, UriKind.Absolute);
            bitmap2.EndInit();

            img_interesRuta.Source = bitmap2;
        }

    }
    
}
