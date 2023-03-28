using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Tarea2_PrograAv {
    public partial class Formulario : Form {

        static Metodos metodos = new Metodos();//Instanciamos la clase que contiene los metodos que usaremos para agilizar el programa

        //Declaracion de arreglos de 20 posiciones de [0 a 19]
        public static Cliente[] clientes = new Cliente[19];
        public static Aeropuerto[] aeropuertos = new Aeropuerto[19];
        public static Aerolinea[] aerolineas = new Aerolinea[19];
        public static Vuelo[] vuelos = new Vuelo[19];
        public static Boleto[] boletos = new Boleto[19];

        //Declaracion de variables de Registro de Clientes
        public static string id, nombre, primerApellido, segundoApellido, codigo, nombreAeropuerto, pais, ciudad, telefono, nombreAerolinea, telefonoAerolinea, duracionStr, nombreAerolineaVuelo;
        public static string codigoOrigen, codigoDestino, estadoAeropuertoGridView, clienteBoleto;
        public static DateTime fechaNacimiento, fechaCompra, duracion;
        public static char genero;
        public static int vueloBoleto, numeroBoleto, asientos, numeroVuelo, idAerolinea, capacidadDePasajeros, contadorClientes = 0, contadorAeropuertos = 0, contadorAerolineas = 0, contadorVuelos = 0, contadorBoletos = 0;
        public static bool estadoAeropuerto, estadoAerolinea;

        //Declaracion de los objetos de Registro de Vuelos
        public static Aerolinea Aerolinea;
        public static Aeropuerto Origen;
        public static Aeropuerto Destino;

        //Declaracion de los objetos de la compra de Boletos
        public static Vuelo Vuelo;
        public static Cliente Cliente;

        public Formulario() {
            InitializeComponent();

            //Ubicamos el formulario en el centro de la pantalla
            this.StartPosition = FormStartPosition.CenterScreen;

            //Aca llamamos a los metodos que se encargan de ocultar los paneles
            metodos.PanelesRegistro(registrarClientesPanel, registrarAeropuertosPanel, registrarAerolineasPanel, registrarVuelosPanel, comprarBoletosPanel);
            metodos.PanelesMostrar(mostrarClientesPanel, mostrarAeropuertosPanel, mostrarAerolineasPanel, mostrarVuelosPanel, mostrarBoletosPanel);

            ///Combos box en orden de todos los registros, aca le añadimos los elementos a los combo box y los inicializamos en la primera posicion
            comboBoxGeneros.Items.Add("Femenino"     );
            comboBoxGeneros.Items.Add("Masculino"    );
            comboBoxGeneros.Items.Add("No especifica");
            comboBoxGeneros.SelectedIndex = 0;

            comboBoxAeropuertos.Items.Add("Activo"   );
            comboBoxAeropuertos.Items.Add("Inactivo" );
            comboBoxAeropuertos.SelectedIndex = 0;

            comboBoxAerolineas.Items.Add("Activo"    );
            comboBoxAerolineas.Items.Add("Inactivo"  );
            comboBoxAerolineas.SelectedIndex = 0;

            //Aca convertimos el datetimepicker en un formato de horas, especificamente de 24 horas ya que hay vuelos que duran hasta 18 horas
            duracionDateTime.Format = DateTimePickerFormat.Custom;
            duracionDateTime.CustomFormat = "HH:mm";
            duracionDateTime.ShowUpDown = true; //Aca le habilitamos la opciion de porder incrementar las horas
        }

        ///Eventos para impedir el ingreso de caracteres en los campos que guardan datos de numero enteros
        private void idAerolineaTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void numeroVueloTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void capacidadTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void numeroBoletoTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void vueloTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void asientosTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        ///Botones de envio de registros
        private void enviarRegistroClientes_Click(object sender, EventArgs e) {
            //Inicializamos la variables que vamos autilizar
            id = idTextBox.Text;
            nombre = nombreTextBox.Text;
            primerApellido = primerATextBox.Text;
            segundoApellido = segundoATextBox.Text;
            fechaNacimiento = dateTimeCalendar.Value;
            if (comboBoxGeneros.SelectedIndex == 0) {
                genero = 'F';
            } else if (comboBoxGeneros.SelectedIndex == 1) {
                genero = 'M';
            } else if (comboBoxGeneros.SelectedIndex == 2) {
                genero = 'N';
            }

            //Aca validamos si hay un id igual al que el nuevo usuario ingreso
            if (ValidarId(id)) {
                metodos.ClienteYaRegistrado();
            }

            //Aca validamos si hay cadenas vacias o si el textbox nunca se lleno, osea si esta vacio
            if (metodos.EspaciosClientes(idTextBox, nombreTextBox, primerATextBox, segundoATextBox)) {
                metodos.LLenarEspacios();
            }
            else if (!metodos.EspaciosClientes(idTextBox, nombreTextBox, primerATextBox, segundoATextBox)) {
                //Si el nuevo id ingresado es unico se procede a guardar el registro de en arreglo
                if (!ValidarId(id)) {
                    if (contadorClientes < 20) {
                        Cliente cliente = new Cliente(id, nombre, primerApellido, segundoApellido, fechaNacimiento, genero);
                        clientes[contadorClientes] = cliente;
                        metodos.RegistroExitoso();
                        //Por ultimo vaciamos los textbox para un nuevo ingreso desde 0
                        metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
                        contadorClientes++;
                    } else if (contadorClientes > 19) {
                        metodos.EspacioInsuficiente();
                    }
                }
            }
        }

        private void enviarRegistroAeropuertos_Click(object sender, EventArgs e) {
            //Inicializamos la variables que vamos autilizar
            codigo = codigoTextBox.Text;
            nombreAeropuerto = nombreAeropuertoTextBox.Text;
            pais = paisTextBox.Text;
            ciudad = ciudadTextBox.Text;
            telefono = telefonoTextBox.Text;
            if (comboBoxAeropuertos.SelectedIndex == 0) {
                estadoAeropuerto = true;
            } else if (comboBoxAeropuertos.SelectedIndex == 1) {
                estadoAeropuerto = false;
            }

            //Aca validamos si hay un codigo de aeropuerto igual al que el nuevo usuario ingreso
            if (ValidarCodigo(codigo)) {
                metodos.AeropuertoYaRegistrado();
            }

            //Aca validamos si hay cadenas vacias o si el textbox nunca se lleno, osea si esta vacio
            if (metodos.EspaciosAeropuertos(codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox)) {
                metodos.LLenarEspacios();
            } 
            else if (!metodos.EspaciosAeropuertos(codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox)) {
                //Si el nuevo codigo de aeropuerto ingresado es unico se procede a guardar el registro de en arreglo
                if (!ValidarCodigo(codigo)) {
                   if (contadorAeropuertos < 20) {
                       Aeropuerto aeropuerto = new Aeropuerto(codigo, nombreAeropuerto, pais, ciudad, telefono, estadoAeropuerto);
                       aeropuertos[contadorAeropuertos] = aeropuerto;
                       metodos.RegistroExitoso();
                        //Por ultimo vaciamos los textbox para un nuevo ingreso desde 0
                        metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
                        contadorAeropuertos++;
                   } else if (contadorAeropuertos > 19) {
                       metodos.EspacioInsuficiente();
                   }
                }
            }
        }

        private void enviarRegistroAerolineas_Click(object sender, EventArgs e) {
            //Inicializamos la variables que vamos autilizar, las variables que almacenas numero enteros manejamos sus excepciones con un try catch
            string numStr;
            try {
                numStr = idAerolineaTextBox.Text;
                idAerolinea = int.Parse(numStr);
            }
            catch (FormatException ex) {
                throw new Exception("Error de formato en el ID de la aerolinea", ex);
            }
            catch (Exception ex) {
                MessageBox.Show("Se produjo un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            nombreAerolinea = nombreAerolineaTextBox.Text;
            telefonoAerolinea = telefonoAerolineaTextBox.Text;
            if (comboBoxAerolineas.SelectedIndex == 0) {
                estadoAerolinea = true;
            } else if (comboBoxAerolineas.SelectedIndex == 1) {
                estadoAerolinea = false;
            }

            //Aca validamos si hay un id de aerolinea igual al que el nuevo usuario ingreso
            if (ValidarIdAerolinea(idAerolinea)) {
                metodos.AerolineaYaRegistrada();
            }

            //Aca validamos si hay cadenas vacias o si el textbox nunca se lleno, osea si esta vacio
            if (metodos.EspaciosAerolineas(idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox)) {
                metodos.LLenarEspacios();
            }
            else if (!metodos.EspaciosAerolineas(idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox)) {
                //Si el nuevo id de aerolinea ingresado es unico se procede a guardar el registro de en arreglo
                if (!ValidarIdAerolinea(idAerolinea)) {
                    if (contadorAerolineas < 20) {
                        Aerolinea aerolinea = new Aerolinea(idAerolinea, nombreAerolinea, telefonoAerolinea, estadoAerolinea);
                        aerolineas[contadorAerolineas] = aerolinea;
                        metodos.RegistroExitoso();
                        //Por ultimo vaciamos los textbox para un nuevo ingreso desde 0
                        metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
                        contadorAerolineas++;
                    } else if (contadorAerolineas > 19) {
                        metodos.EspacioInsuficiente();
                    }
                }
            }
        }

        private void enviarRegistroVuelos_Click(object sender, EventArgs e) {
            //Inicializamos la variables que vamos autilizar, las variables que almacenas numero enteros manejamos sus excepciones con un try catch
            string numStr, capacidadStr, duracionStr;

            try {
                numStr = numeroVueloTextBox.Text;
                numeroVuelo = int.Parse(numStr);
            }
            catch (FormatException ex) {
                throw new Exception("Error de formato en el número de vuelo", ex);
            }
            catch (Exception ex) {
                MessageBox.Show("Se produjo un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            nombreAerolineaVuelo = nombreAerolineaVueloTextBox.Text;
            codigoOrigen = origenTextBox.Text;
            codigoDestino = destinoTextBox.Text;
            duracion = duracionDateTime.Value;
            duracionStr = duracion.ToString("HH:mm"); //Aca le asiganamos el valor en horas 24:00 a la variable duracion, ya que al ser un datetimepicker guarda el formato de fecha

            try {
                capacidadStr = capacidadTextBox.Text;
                capacidadDePasajeros = int.Parse(capacidadStr);
            }
            catch (FormatException ex) {
                throw new Exception("Error de formato en la capacidad de pasajeros", ex);
            }
            catch (Exception ex) {
                MessageBox.Show("Se produjo un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Aca validamos si hay un numero de vuelo igual al que el nuevo usuario ingreso
            if (ValidarNumeroVuelo(numeroVuelo)) {
                metodos.VueloYaRegistrado();
            }

            //Aca validamos si hay cadenas vacias o si el textbox nunca se lleno, osea si esta vacio
            if (metodos.EspaciosVuelos(numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox)) {
                metodos.LLenarEspacios();
            } else if (!metodos.EspaciosVuelos(numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox)) {
                if (!ValidarNumeroVuelo(numeroVuelo)) {
                    //Aca llamamos la funcion que se encarga de buscar si existe un objeto Aerolinea con el nombre pasado por parametro
                    Aerolinea = BuscarAerolinea(nombreAerolinea, aerolineas);
                    if (Aerolinea == null) {
                        metodos.TextoInvalidoNombreAerolinea();
                    }

                    //Aca llamamos la funcion que se encarga de buscar si existe un objeto Origen con el id del aeropuerto pasado por parametro
                    Origen = BuscarAeropuerto(codigoOrigen, aeropuertos);
                    if (Origen == null) {
                        metodos.TextoInvalidoOrigen();
                    }

                    //Aca llamamos la funcion que se encarga de buscar si existe un objeto Destino con el id del aeropuerto pasado por parametro
                    Destino = BuscarAeropuerto(codigoDestino, aeropuertos);
                    if (Destino == null) {
                        metodos.TextoInvalidoDestino();
                    }

                    //Aca validamos que el aeropuerto de Origen y el de Destino no sean iguales
                    if (Origen == Destino) {
                        metodos.AeropuertosIguales();
                    }

                    //Si las funciones encuentran un objeto bajo esa propiedad se procede a continuar con el registro
                    if (Aerolinea != null && Origen != null && Destino != null) {
                        //Y si lo aeropuertos de Origen y Destino son diferentes se procede con el registro del vuelo
                        if (Origen != Destino) {
                            if (contadorVuelos < 20) {
                                Vuelo vuelo = new Vuelo(numeroVuelo, Aerolinea, Origen, Destino, duracionStr, capacidadDePasajeros);
                                vuelos[contadorVuelos] = vuelo;
                                metodos.RegistroExitoso();
                                //Por ultimo vaciamos los textbox para un nuevo ingreso desde 0
                                metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
                                contadorVuelos++;
                            } else if (contadorVuelos > 19) {
                                metodos.EspacioInsuficiente();
                            }
                        }
                    }
                }
            }
        }

        private void enviarRegistroBoletos_Click(object sender, EventArgs e) {
            //Inicializamos la variables que vamos autilizar, las variables que almacenas numero enteros manejamos sus excepciones con un try catch
            string numBStr, asientStr, vueloStr;

            try {
                numBStr = numeroBoletoTextBox.Text;
                numeroBoleto = int.Parse(numBStr);
            }
            catch (FormatException ex) {
                throw new Exception("Error de formato en el número de boleto", ex);
            }
            catch (Exception ex) {
                MessageBox.Show("Se produjo un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try {
                vueloStr = vueloTextBox.Text;
                vueloBoleto = int.Parse(vueloStr);
            }
            catch (FormatException ex) {
                throw new Exception("Error de formato en el número de vuelo", ex);
            }
            catch (Exception ex) {
                MessageBox.Show("Se produjo un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            clienteBoleto = clienteTextBox.Text;
            fechaCompra = dateTimePickerBoleto.Value;

            try {
                asientStr = asientosTextBox.Text;
                asientos = int.Parse(asientStr);
            }
            catch (FormatException ex) {
                throw new Exception("Error de formato en el número de asientos", ex);
            }
            catch (Exception ex) {
                MessageBox.Show("Se produjo un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Aca validamos si hay un numero de boleto igual al que el nuevo usuario ingreso
            if (ValidarBoleto(numeroBoleto)) {
                metodos.BoletoYaRegistrado();
            }

            //Aca validamos si hay cadenas vacias o si el textbox nunca se lleno, osea si esta vacio
            if (metodos.EspaciosBoletos(numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox)) {
                metodos.LLenarEspacios();
            } else if (!metodos.EspaciosBoletos(numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox)) {
                if (!ValidarBoleto(numeroVuelo)) {
                    //Aca llamamos la funcion que se encarga de buscar si existe un objeto Vuelo con el numero de vuelo pasado por parametro
                    Vuelo = BuscarVuelo(numeroVuelo, vuelos);
                    if (Vuelo == null) {
                        metodos.TextoInvalidoVuelo();
                    }

                    //Aca llamamos la funcion que se encarga de buscar si existe un objeto Cliente con el id del cliente pasado por parametro
                    Cliente = BuscarCliente(clienteBoleto, clientes);
                    if (Cliente == null) {
                        metodos.TextoInvalidoCliente();
                    }

                    /*
                       Aca llamamos la funcion que se encarga de buscar si existe un objeto Vuelo bajo el numero de vuelo ingresado
                       por el usuario y si es asi comparamos la propiedad capacidad de pasajeros de ese objeto con la cantidad de asientos
                     */
                    if (!ValidarAsientos(vueloBoleto, asientos, vuelos)) {
                        metodos.AsientosMenor_1();
                    }

                    //Si las funciones encuentran un objeto bajo esa propiedad se procede a continuar con el registro
                    if (Vuelo != null && Cliente != null) {
                        if (ValidarAsientos(vueloBoleto, asientos, vuelos)) {
                            if (contadorBoletos < 20) {
                                Boleto boleto = new Boleto(numeroBoleto, Vuelo, Cliente, fechaCompra, asientos);
                                boletos[contadorBoletos] = boleto;
                                metodos.RegistroExitoso();
                                //Por ultimo vaciamos los textbox para un nuevo ingreso desde 0
                                metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
                                contadorBoletos++;
                            } else if (contadorBoletos > 19) {
                                metodos.EspacioInsuficiente();
                            }
                        }
                    }
                }
            }
        }

        ///Botones de cancelación de registros, lo que hacen es vaciar los textbox y ocultar el panel para ya no ser visto y proceder a registrar otro registro
        private void cancelarRegistroClientes_Click(object sender, EventArgs e) {
            metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
            metodos.PanelCancelar(menuInicio, registrarClientesPanel);
        }

        private void cancelarRegistroAeropuertos_Click(object sender, EventArgs e) {
            metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
            metodos.PanelCancelar(menuInicio, registrarAeropuertosPanel);
        }

        private void cancelarRegistroAerolineas_Click(object sender, EventArgs e) {
            metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
            metodos.PanelCancelar(menuInicio, registrarAerolineasPanel);
        }

        private void cancelarRegistroVuelos_Click(object sender, EventArgs e) {
            metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
            metodos.PanelCancelar(menuInicio, registrarVuelosPanel);
        }

        private void cancelarRegistroBoletos_Click(object sender, EventArgs e) {
            metodos.VaciarTexto(idTextBox, nombreTextBox, primerATextBox, segundoATextBox, codigoTextBox, nombreAeropuertoTextBox, paisTextBox, ciudadTextBox, telefonoTextBox, idAerolineaTextBox, nombreAerolineaTextBox, telefonoAerolineaTextBox, numeroVueloTextBox, nombreAerolineaVueloTextBox, origenTextBox, destinoTextBox, capacidadTextBox, numeroBoletoTextBox, vueloTextBox, clienteTextBox, asientosTextBox);
            metodos.PanelCancelar(menuInicio, comprarBoletosPanel);
        }

        private void volverMenuMostrarClientes_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(menuInicio, mostrarClientesPanel);
        }

        private void volverMenuMostrarAeropuertos_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(menuInicio, mostrarAeropuertosPanel);
        }

        private void volverMenuMostrarAerolineas_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(menuInicio, mostrarAerolineasPanel);
        }

        private void volverMenuMostrarVuelos_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(menuInicio, mostrarVuelosPanel);
        }

        private void volverMenuMostrarBoletos_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(menuInicio, mostrarBoletosPanel);
        }

        ///Botones de apertura de los paneles de los registros, lo que hacen es mostrar los paneles con su contenido cada vez que usuario selleciona una opcion del menu
        private void registrarClientesButton_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(registrarClientesPanel, menuInicio);
        }

        private void registrarAeropuertosButton_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(registrarAeropuertosPanel, menuInicio);
        }

        private void registrarAerolineasButton_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(registrarAerolineasPanel, menuInicio);
        }

        private void registrarVuelosButton_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(registrarVuelosPanel, menuInicio);
        }

        private void registrarBoletosButton_Click(object sender, EventArgs e) {
            metodos.PanelCancelar(comprarBoletosPanel, menuInicio);
        }

        ///Botones para mostrar los registros, lo que hacen es mostrar los paneles correspondientes que se encargan de mostrar los datos, pero no se puede acceder a ellos hasta que no haya un registro previo
        private void mostrarClientesButton_Click(object sender, EventArgs e) {
            if (contadorClientes == 0) {
                metodos.RegistroAnticipado("un cliente registrado");
            } else {
                metodos.PanelCancelar(mostrarClientesPanel, menuInicio);
            }
        }

        private void mostrarAeropuertosButton_Click(object sender, EventArgs e) {
            if (contadorAeropuertos == 0) {
                metodos.RegistroAnticipado("un aeropuerto registrado");
            } else if (contadorAeropuertos > 0) {
                metodos.PanelCancelar(mostrarAeropuertosPanel, menuInicio);
            }
        }

        private void mostrarAerolineasButton_Click(object sender, EventArgs e) {
            if (contadorAerolineas == 0) {
                metodos.RegistroAnticipado("una aerolinea registrada");
            } else if (contadorAerolineas > 0) {
                metodos.PanelCancelar(mostrarAerolineasPanel, menuInicio);
            }
        }

        private void mostrarVuelosButton_Click(object sender, EventArgs e) {
            if (contadorVuelos == 0) {
                metodos.RegistroAnticipado("un vuelo registrado");
            } else if (contadorVuelos > 0) {
                metodos.PanelCancelar(mostrarVuelosPanel, menuInicio);
            }
        }

        private void mostrarBoletosButton_Click(object sender, EventArgs e) {
            if (contadorBoletos == 0) {
                metodos.RegistroAnticipado("un boleto comprado");
            } else if (contadorBoletos > 0) {
                metodos.PanelCancelar(mostrarBoletosPanel, menuInicio);
            }
        }

        ///Botones para mostrar los registros en los gridView
        /////Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
        private void mostrarGridViewClientes_Click(object sender, EventArgs e) {
            //Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
            DataTable dt = new DataTable();
            dt.Columns.Add("Identificación", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Primer Apellido", typeof(string));
            dt.Columns.Add("Segundo Apellido", typeof(string));
            dt.Columns.Add("Fecha de Nacimiento", typeof(DateTime));
            dt.Columns.Add("Género", typeof(string));
            //Luego Aca creamos una fila que añadiremos a la tabla, se añadiran conforme a las columnas de la tabla

            foreach (Cliente cliente in clientes) {
                if (cliente != null) {
                    DataRow dr = dt.NewRow();
                    dr["Identificación"] = cliente.Id;
                    dr["Nombre"] = cliente.Nombre;
                    dr["Primer Apellido"] = cliente.PrimerApellido;
                    dr["Segundo Apellido"] = cliente.SegundoApellido;
                    dr["Fecha de Nacimiento"] = cliente.FechaNacimiento;
                    dr["Género"] = cliente.Genero;
                    dt.Rows.Add(dr);
                }
            }
            /*
               Por ultimo le añadimos esta tabla al grid view, al momento de precionar el boton se creara una nueva tabla, asi que se cada vez que desea ver
               un nuevo registro que antes no existia debera de precionar el boton nuevamente ya que no se actualiza de manera automatica
             */
            dataGridViewClientes.DataSource = dt;
        }

        //Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
        private void mostrarGridViewAeropuertos_Click(object sender, EventArgs e) {
            //Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
            DataTable dt = new DataTable();
            dt.Columns.Add("Código del aeropuerto", typeof(string));
            dt.Columns.Add("Nombre del aeropuerto", typeof(string));
            dt.Columns.Add("País", typeof(string));
            dt.Columns.Add("Ciudad", typeof(string));
            dt.Columns.Add("Teléfono", typeof(string));
            dt.Columns.Add("Estado", typeof(string));
            //Luego Aca creamos una fila que añadiremos a la tabla, se añadiran conforme a las columnas de la tabla

            foreach (Aeropuerto aeropuerto in aeropuertos) {
                if (aeropuerto != null) {
                    DataRow dr = dt.NewRow();
                    dr["Código del aeropuerto"] = aeropuerto.Codigo;
                    dr["Nombre del aeropuerto"] = aeropuerto.NombreAeropuerto;
                    dr["País"] = aeropuerto.Pais;
                    dr["Ciudad"] = aeropuerto.Ciudad;
                    dr["Teléfono"] = aeropuerto.Telefono;
                    dr["Estado"] = aeropuerto.Estado;
                    dt.Rows.Add(dr);
                }
            }
            /*
               Por ultimo le añadimos esta tabla al grid view, al momento de precionar el boton se creara una nueva tabla, asi que se cada vez que desea ver
               un nuevo registro que antes no existia debera de precionar el boton nuevamente ya que no se actualiza de manera automatica
             */
            dataGridViewAeropuertos.DataSource = dt;
        }

        //Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
        private void mostrarGridViewAerolineas_Click(object sender, EventArgs e) {
            //Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
            DataTable dt = new DataTable();
            dt.Columns.Add("ID de la aerolinea", typeof(int));
            dt.Columns.Add("Nombre de le aerolinea", typeof(string));
            dt.Columns.Add("Teléfono", typeof(string));
            dt.Columns.Add("Estado", typeof(string));
            //Luego Aca creamos una fila que añadiremos a la tabla, se añadiran conforme a las columnas de la tabla

            foreach (Aerolinea aerolinea in aerolineas) {
                if (aerolinea != null) {
                    DataRow dr = dt.NewRow();
                    dr["ID de la aerolinea"] = aerolinea.IdAerolinea;
                    dr["Nombre de le aerolinea"] = aerolinea.NombreAerolinea;
                    dr["Teléfono"] = aerolinea.TelefonoAerolinea;
                    dr["Estado"] = aerolinea.EstadoAerolinea;
                    dt.Rows.Add(dr);
                }
            }
            /*
               Por ultimo le añadimos esta tabla al grid view, al momento de precionar el boton se creara una nueva tabla, asi que se cada vez que desea ver
               un nuevo registro que antes no existia debera de precionar el boton nuevamente ya que no se actualiza de manera automatica
             */
            dataGridViewAerolineas.DataSource = dt;
        }

        //Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
        private void mostrarGridViewVuelos_Click(object sender, EventArgs e) {
            //Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
            DataTable dt = new DataTable();
            dt.Columns.Add("Número de vuelo", typeof(int));
            dt.Columns.Add("Nombre de la aerolinea", typeof(string));
            dt.Columns.Add("País de origen", typeof(string));
            dt.Columns.Add("Ciudad de origen", typeof(string));
            dt.Columns.Add("País de destino", typeof(string));
            dt.Columns.Add("Ciudad de destino", typeof(string));
            dt.Columns.Add("Duración", typeof(string));
            dt.Columns.Add("Capacidad", typeof(int));

            //Luego Aca creamos una fila que añadiremos a la tabla, se añadiran conforme a las columnas de la tabla
            foreach (Vuelo vuelo in vuelos) {
                if (vuelo != null) {
                    DataRow dr = dt.NewRow();
                    dr["Número de vuelo"] = vuelo.NumeroVuelo;
                    dr["Nombre de la aerolinea"] = vuelo.Aerolinea.NombreAerolinea;
                    dr["País de origen"] = vuelo.Origen.Pais;
                    dr["Ciudad de origen"] = vuelo.Origen.Ciudad;
                    dr["País de destino"] = vuelo.Destino.Pais;
                    dr["Ciudad de destino"] = vuelo.Destino.Ciudad;
                    dr["Duración"] = vuelo.DuracionStr;
                    dr["Capacidad"] = vuelo.Capacidad;
                    dt.Rows.Add(dr);
                }
            }
            /*
               Por ultimo le añadimos esta tabla al grid view, al momento de precionar el boton se creara una nueva tabla, asi que se cada vez que desea ver
               un nuevo registro que antes no existia debera de precionar el boton nuevamente ya que no se actualiza de manera automatica
             */
            dataGridViewVuelos.DataSource = dt;
        }

        //Evento click del boton que muestra la informacion en pantalla de acuerdo a la siguiente lista
        private void mostrarGridViewBoletos_Click(object sender, EventArgs e) {
            //Aca creamos la tabla, con la siguiente cantidad de columnas
            DataTable dt = new DataTable();
            dt.Columns.Add("Número de boleto", typeof(int));
            dt.Columns.Add("Número de vuelo", typeof(int));
            dt.Columns.Add("Nombre del cliente", typeof(string));
            dt.Columns.Add("Fecha de la compra", typeof(DateTime));
            dt.Columns.Add("Cantidad de asientos", typeof(int));

            //Luego Aca creamos una fila que añadiremos a la tabla, se añadiran conforme a las columnas de la tabla
            foreach (Boleto boleto in boletos) {
                if (boleto != null) {
                    DataRow dr = dt.NewRow();
                    dr["Número de boleto"] = boleto.NumeroBoleto;
                    dr["Número de vuelo"] = boleto.Vuelo.NumeroVuelo;
                    dr["Nombre del cliente"] = boleto.Cliente.Nombre;
                    dr["Fecha de la compra"] = boleto.FechaCompra;
                    dr["Cantidad de asientos"] = boleto.Asientos;
                    dt.Rows.Add(dr);
                }
            }
            /*
               Por ultimo le añadimos esta tabla al grid view, al momento de precionar el boton se creara una nueva tabla, asi que se cada vez que desea ver
               un nuevo registro que antes no existia debera de precionar el boton nuevamente ya que no se actualiza de manera automatica
             */
            dataGridViewBoletos.DataSource = dt;
        }

        ///Boton de salir del sistema que pregunta si de verdad quiere salir del sistema
        private void salirDelSistema_Click(object sender, EventArgs e) {
            DialogResult result;
            result = metodos.Salir();
            if (result == DialogResult.Yes) {
                Application.Exit();
            }
        }

        ///Metodos para comprobar que los id registrados no sean los mismos
        /////Funcion que busca sobre el arreglo clientes un objeto con una propiedad en especifico y si coincide con la que el usuario ingreso se le mada un mensaje que esta ya hay un registro bajo ese nombre o identificacion
        static bool ValidarId(string id) {
            for (int i = 0; i < contadorClientes; i++) {
                if (clientes[i].Id == id) {
                    return true;
                }
            }
            return false;
        }
        //Funcion que busca sobre el arreglo aeropuertos un objeto con una propiedad en especifico y si coincide con la que el usuario ingreso se le mada un mensaje que esta ya hay un registro bajo ese nombre o identificacion
        static bool ValidarCodigo(string codigo) {
            for (int i = 0; i < contadorAeropuertos; i++) {
                if (aeropuertos[i].Codigo == codigo) {
                    return true;
                }
            }
            return false;
        }
        //Funcion que busca sobre el arreglo aerolineas un objeto con una propiedad en especifico y si coincide con la que el usuario ingreso se le mada un mensaje que esta ya hay un registro bajo ese nombre o identificacion
        static bool ValidarIdAerolinea(int idaerolinea) {
            for (int i = 0; i < contadorAerolineas; i++) {
                if (aerolineas[i].IdAerolinea == idaerolinea) {
                    return true;
                }
            }
            return false;
        }
        //Funcion que busca sobre el arreglo vuelos un objeto con una propiedad en especifico y si coincide con la que el usuario ingreso se le mada un mensaje que esta ya hay un registro bajo ese nombre o identificacion
        static bool ValidarNumeroVuelo(int numero) {
            for (int i = 0; i < contadorVuelos; i++) {
                if (vuelos[i].NumeroVuelo == numero) {
                    return true;
                }
            }
            return false;
        }
        //Funcion que busca sobre el arreglo boletos un objeto con una propiedad en especifico y si coincide con la que el usuario ingreso se le mada un mensaje que esta ya hay un registro bajo ese nombre o identificacion
        static bool ValidarBoleto(int numeroBoleto) {
            for (int i = 0; i < contadorBoletos; i++) {
                if (boletos[i].NumeroBoleto == numeroBoleto) {
                    return true;
                }
            }
            return false;
        }
        /*
           Funcion que valida que los asientos ingresados por el usuario sean mayor a 1 y que sean menor a la capacidad ingresada por el usuario en el registro de ese vuelo
           al momento de registrar un vuelo la capacidad cambia, entonces para no hacerlo global y tener errores en esta validacion buscamos el vuelo que ya se registro para
           poder acceder a la propiedad de ese nuevo objeto y asi poder comparar el numero de asientos con la capacidad de pasajeros
         */
        public bool ValidarAsientos(int numeroVuelo, int numeroAsientos, Vuelo[] vuelos) {
            Vuelo vuelo = BuscarVuelo(numeroVuelo, vuelos);
            if (vuelo == null) {
                return false; //Si no se encuentra el vuelo, retorna falso
            }
            if (numeroAsientos > 1 && numeroAsientos < vuelo.Capacidad) {
                return true;
            }
            return false;
        }
        //Funcion que busca sobre el arreglo aerolineas un objeto, en la propiedad nombre que conicida con la ingresada por el usuario
        public Aerolinea BuscarAerolinea(string nombreAerolinea, Aerolinea[] aerolineas) {
            foreach (Aerolinea aerolinea in aerolineas) {
                if (aerolinea != null && aerolinea.NombreAerolinea == nombreAerolinea) {
                    return aerolinea;
                }
            }
            return null;
        }

        //Funcion que busca directamente sobre el arreglo aeropuerto y sus objetos un codigo de Origen o Destino que consida con el ingresado por el usuario
        public Aeropuerto BuscarAeropuerto(string codigo, Aeropuerto[] aeropuertos) {
            foreach (Aeropuerto aeropuerto in aeropuertos) {
                if (aeropuerto != null && aeropuerto.Codigo == codigo) {
                    return aeropuerto;
                }
            }
            return null;
        }
        //Funcion que busca sobre el arreglo vuelos un objeto, en la propiedad nombre que conicida con la ingresada por el usuario
        public Vuelo BuscarVuelo(int numeroVuelo, Vuelo[] vuelos) {
            foreach (Vuelo vuelo in vuelos) {
                if (vuelo != null && vuelo.NumeroVuelo == numeroVuelo) {
                    return vuelo;
                }
            }
            return null;
        }
        //Funcion que busca sobre el arreglo clientes un objeto, en la propiedad identificacion que conicida con la ingresada por el usuario
        public Cliente BuscarCliente(string id, Cliente[] clientes) {
            foreach (Cliente cliente in clientes) {
                if (cliente != null && cliente.Id == id) {
                    return cliente;
                }
            }
            return null;
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuInicio = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.mostrarBoletosButton = new System.Windows.Forms.Button();
            this.mostrarVuelosButton = new System.Windows.Forms.Button();
            this.mostrarAerolineasButton = new System.Windows.Forms.Button();
            this.mostrarAeropuertosButton = new System.Windows.Forms.Button();
            this.mostrarClientesButton = new System.Windows.Forms.Button();
            this.registrarBoletosButton = new System.Windows.Forms.Button();
            this.registrarVuelosButton = new System.Windows.Forms.Button();
            this.registrarAerolineasButton = new System.Windows.Forms.Button();
            this.registrarAeropuertosButton = new System.Windows.Forms.Button();
            this.registrarClientesButton = new System.Windows.Forms.Button();
            this.salirDelSistema = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.registrarClientesPanel = new System.Windows.Forms.Panel();
            this.button12 = new System.Windows.Forms.Button();
            this.tituloClientes = new System.Windows.Forms.Label();
            this.comboBoxGeneros = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dateTimeCalendar = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.segundoATextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.primerATextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nombreTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.enviarRegistroClientes = new System.Windows.Forms.Button();
            this.cancelarRegistroClientes = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.registrarAeropuertosPanel = new System.Windows.Forms.Panel();
            this.codigoTextBox = new System.Windows.Forms.TextBox();
            this.button17 = new System.Windows.Forms.Button();
            this.comboBoxAeropuertos = new System.Windows.Forms.ComboBox();
            this.telefonoTextBox = new System.Windows.Forms.TextBox();
            this.ciudadTextBox = new System.Windows.Forms.TextBox();
            this.paisTextBox = new System.Windows.Forms.TextBox();
            this.nombreAeropuertoTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tituloAeropuerto = new System.Windows.Forms.Label();
            this.button19 = new System.Windows.Forms.Button();
            this.cancelarRegistroAeropuertos = new System.Windows.Forms.Button();
            this.enviarRegistroAeropuertos = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.registrarAerolineasPanel = new System.Windows.Forms.Panel();
            this.button21 = new System.Windows.Forms.Button();
            this.comboBoxAerolineas = new System.Windows.Forms.ComboBox();
            this.telefonoAerolineaTextBox = new System.Windows.Forms.TextBox();
            this.nombreAerolineaTextBox = new System.Windows.Forms.TextBox();
            this.idAerolineaTextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.button23 = new System.Windows.Forms.Button();
            this.cancelarRegistroAerolineas = new System.Windows.Forms.Button();
            this.enviarRegistroAerolineas = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.registrarVuelosPanel = new System.Windows.Forms.Panel();
            this.numeroVueloTextBox = new System.Windows.Forms.TextBox();
            this.button26 = new System.Windows.Forms.Button();
            this.capacidadTextBox = new System.Windows.Forms.TextBox();
            this.duracionDateTime = new System.Windows.Forms.DateTimePicker();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.destinoTextBox = new System.Windows.Forms.TextBox();
            this.origenTextBox = new System.Windows.Forms.TextBox();
            this.nombreAerolineaVueloTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.button27 = new System.Windows.Forms.Button();
            this.cancelarRegistroVuelos = new System.Windows.Forms.Button();
            this.enviarRegistroVuelos = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.mostrarClientesPanel = new System.Windows.Forms.Panel();
            this.dataGridViewClientes = new System.Windows.Forms.DataGridView();
            this.button8 = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.button25 = new System.Windows.Forms.Button();
            this.mostrarGridViewClientes = new System.Windows.Forms.Button();
            this.volverMenuMostrarClientes = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.mostrarAeropuertosPanel = new System.Windows.Forms.Panel();
            this.dataGridViewAeropuertos = new System.Windows.Forms.DataGridView();
            this.button29 = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.button32 = new System.Windows.Forms.Button();
            this.mostrarGridViewAeropuertos = new System.Windows.Forms.Button();
            this.volverMenuMostrarAeropuertos = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.mostrarAerolineasPanel = new System.Windows.Forms.Panel();
            this.dataGridViewAerolineas = new System.Windows.Forms.DataGridView();
            this.button14 = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.button33 = new System.Windows.Forms.Button();
            this.mostrarGridViewAerolineas = new System.Windows.Forms.Button();
            this.volverMenuMostrarAerolineas = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.mostrarVuelosPanel = new System.Windows.Forms.Panel();
            this.dataGridViewVuelos = new System.Windows.Forms.DataGridView();
            this.button30 = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.button37 = new System.Windows.Forms.Button();
            this.mostrarGridViewVuelos = new System.Windows.Forms.Button();
            this.volverMenuMostrarVuelos = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.comprarBoletosPanel = new System.Windows.Forms.Panel();
            this.asientosTextBox = new System.Windows.Forms.TextBox();
            this.button18 = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.dateTimePickerBoleto = new System.Windows.Forms.DateTimePicker();
            this.label32 = new System.Windows.Forms.Label();
            this.clienteTextBox = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.vueloTextBox = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.numeroBoletoTextBox = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.button20 = new System.Windows.Forms.Button();
            this.enviarRegistroBoletos = new System.Windows.Forms.Button();
            this.cancelarRegistroBoletos = new System.Windows.Forms.Button();
            this.button28 = new System.Windows.Forms.Button();
            this.mostrarBoletosPanel = new System.Windows.Forms.Panel();
            this.dataGridViewBoletos = new System.Windows.Forms.DataGridView();
            this.button22 = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.button24 = new System.Windows.Forms.Button();
            this.mostrarGridViewBoletos = new System.Windows.Forms.Button();
            this.volverMenuMostrarBoletos = new System.Windows.Forms.Button();
            this.button35 = new System.Windows.Forms.Button();
            this.menuInicio.SuspendLayout();
            this.registrarClientesPanel.SuspendLayout();
            this.registrarAeropuertosPanel.SuspendLayout();
            this.registrarAerolineasPanel.SuspendLayout();
            this.registrarVuelosPanel.SuspendLayout();
            this.mostrarClientesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClientes)).BeginInit();
            this.mostrarAeropuertosPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAeropuertos)).BeginInit();
            this.mostrarAerolineasPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAerolineas)).BeginInit();
            this.mostrarVuelosPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVuelos)).BeginInit();
            this.comprarBoletosPanel.SuspendLayout();
            this.mostrarBoletosPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBoletos)).BeginInit();
            this.SuspendLayout();
            // 
            // menuInicio
            // 
            this.menuInicio.BackColor = System.Drawing.Color.SeaGreen;
            this.menuInicio.Controls.Add(this.label1);
            this.menuInicio.Controls.Add(this.mostrarBoletosButton);
            this.menuInicio.Controls.Add(this.mostrarVuelosButton);
            this.menuInicio.Controls.Add(this.mostrarAerolineasButton);
            this.menuInicio.Controls.Add(this.mostrarAeropuertosButton);
            this.menuInicio.Controls.Add(this.mostrarClientesButton);
            this.menuInicio.Controls.Add(this.registrarBoletosButton);
            this.menuInicio.Controls.Add(this.registrarVuelosButton);
            this.menuInicio.Controls.Add(this.registrarAerolineasButton);
            this.menuInicio.Controls.Add(this.registrarAeropuertosButton);
            this.menuInicio.Controls.Add(this.registrarClientesButton);
            this.menuInicio.Controls.Add(this.salirDelSistema);
            this.menuInicio.Controls.Add(this.button1);
            this.menuInicio.Controls.Add(this.button2);
            this.menuInicio.Controls.Add(this.button4);
            this.menuInicio.Controls.Add(this.button5);
            this.menuInicio.Location = new System.Drawing.Point(0, 0);
            this.menuInicio.Name = "menuInicio";
            this.menuInicio.Size = new System.Drawing.Size(961, 649);
            this.menuInicio.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(270, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(441, 39);
            this.label1.TabIndex = 11;
            this.label1.Text = "◉Bienvenido a AEROUNED◉";
            // 
            // mostrarBoletosButton
            // 
            this.mostrarBoletosButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.mostrarBoletosButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarBoletosButton.FlatAppearance.BorderSize = 0;
            this.mostrarBoletosButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarBoletosButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarBoletosButton.Location = new System.Drawing.Point(487, 471);
            this.mostrarBoletosButton.Name = "mostrarBoletosButton";
            this.mostrarBoletosButton.Size = new System.Drawing.Size(350, 55);
            this.mostrarBoletosButton.TabIndex = 10;
            this.mostrarBoletosButton.TabStop = false;
            this.mostrarBoletosButton.Text = "Mostrar Boletos                >";
            this.mostrarBoletosButton.UseVisualStyleBackColor = false;
            this.mostrarBoletosButton.Click += new System.EventHandler(this.mostrarBoletosButton_Click);
            // 
            // mostrarVuelosButton
            // 
            this.mostrarVuelosButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.mostrarVuelosButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarVuelosButton.FlatAppearance.BorderSize = 0;
            this.mostrarVuelosButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarVuelosButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarVuelosButton.Location = new System.Drawing.Point(487, 400);
            this.mostrarVuelosButton.Name = "mostrarVuelosButton";
            this.mostrarVuelosButton.Size = new System.Drawing.Size(350, 55);
            this.mostrarVuelosButton.TabIndex = 9;
            this.mostrarVuelosButton.TabStop = false;
            this.mostrarVuelosButton.Text = "Mostrar Vuelos                 >";
            this.mostrarVuelosButton.UseVisualStyleBackColor = false;
            this.mostrarVuelosButton.Click += new System.EventHandler(this.mostrarVuelosButton_Click);
            // 
            // mostrarAerolineasButton
            // 
            this.mostrarAerolineasButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.mostrarAerolineasButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarAerolineasButton.FlatAppearance.BorderSize = 0;
            this.mostrarAerolineasButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarAerolineasButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarAerolineasButton.Location = new System.Drawing.Point(487, 328);
            this.mostrarAerolineasButton.Name = "mostrarAerolineasButton";
            this.mostrarAerolineasButton.Size = new System.Drawing.Size(350, 55);
            this.mostrarAerolineasButton.TabIndex = 8;
            this.mostrarAerolineasButton.TabStop = false;
            this.mostrarAerolineasButton.Text = "Mostrar Aerolineas          >";
            this.mostrarAerolineasButton.UseVisualStyleBackColor = false;
            this.mostrarAerolineasButton.Click += new System.EventHandler(this.mostrarAerolineasButton_Click);
            // 
            // mostrarAeropuertosButton
            // 
            this.mostrarAeropuertosButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.mostrarAeropuertosButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarAeropuertosButton.FlatAppearance.BorderSize = 0;
            this.mostrarAeropuertosButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarAeropuertosButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarAeropuertosButton.Location = new System.Drawing.Point(487, 252);
            this.mostrarAeropuertosButton.Name = "mostrarAeropuertosButton";
            this.mostrarAeropuertosButton.Size = new System.Drawing.Size(350, 55);
            this.mostrarAeropuertosButton.TabIndex = 7;
            this.mostrarAeropuertosButton.TabStop = false;
            this.mostrarAeropuertosButton.Text = "Mostrar Aeropuertos        >";
            this.mostrarAeropuertosButton.UseVisualStyleBackColor = false;
            this.mostrarAeropuertosButton.Click += new System.EventHandler(this.mostrarAeropuertosButton_Click);
            // 
            // mostrarClientesButton
            // 
            this.mostrarClientesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.mostrarClientesButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarClientesButton.FlatAppearance.BorderSize = 0;
            this.mostrarClientesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarClientesButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarClientesButton.Location = new System.Drawing.Point(487, 176);
            this.mostrarClientesButton.Name = "mostrarClientesButton";
            this.mostrarClientesButton.Size = new System.Drawing.Size(350, 55);
            this.mostrarClientesButton.TabIndex = 6;
            this.mostrarClientesButton.TabStop = false;
            this.mostrarClientesButton.Text = "Mostrar Clientes              >";
            this.mostrarClientesButton.UseVisualStyleBackColor = false;
            this.mostrarClientesButton.Click += new System.EventHandler(this.mostrarClientesButton_Click);
            // 
            // registrarBoletosButton
            // 
            this.registrarBoletosButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.registrarBoletosButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.registrarBoletosButton.FlatAppearance.BorderSize = 0;
            this.registrarBoletosButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.registrarBoletosButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrarBoletosButton.Location = new System.Drawing.Point(130, 471);
            this.registrarBoletosButton.Name = "registrarBoletosButton";
            this.registrarBoletosButton.Size = new System.Drawing.Size(350, 55);
            this.registrarBoletosButton.TabIndex = 5;
            this.registrarBoletosButton.TabStop = false;
            this.registrarBoletosButton.Text = "Comprar Boletos             >";
            this.registrarBoletosButton.UseVisualStyleBackColor = false;
            this.registrarBoletosButton.Click += new System.EventHandler(this.registrarBoletosButton_Click);
            // 
            // registrarVuelosButton
            // 
            this.registrarVuelosButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.registrarVuelosButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.registrarVuelosButton.FlatAppearance.BorderSize = 0;
            this.registrarVuelosButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.registrarVuelosButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrarVuelosButton.Location = new System.Drawing.Point(131, 400);
            this.registrarVuelosButton.Name = "registrarVuelosButton";
            this.registrarVuelosButton.Size = new System.Drawing.Size(350, 55);
            this.registrarVuelosButton.TabIndex = 4;
            this.registrarVuelosButton.TabStop = false;
            this.registrarVuelosButton.Text = "Registrar Vuelos              >";
            this.registrarVuelosButton.UseVisualStyleBackColor = false;
            this.registrarVuelosButton.Click += new System.EventHandler(this.registrarVuelosButton_Click);
            // 
            // registrarAerolineasButton
            // 
            this.registrarAerolineasButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.registrarAerolineasButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.registrarAerolineasButton.FlatAppearance.BorderSize = 0;
            this.registrarAerolineasButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.registrarAerolineasButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrarAerolineasButton.Location = new System.Drawing.Point(130, 328);
            this.registrarAerolineasButton.Name = "registrarAerolineasButton";
            this.registrarAerolineasButton.Size = new System.Drawing.Size(350, 55);
            this.registrarAerolineasButton.TabIndex = 3;
            this.registrarAerolineasButton.TabStop = false;
            this.registrarAerolineasButton.Text = "Registrar Aerolineas       >";
            this.registrarAerolineasButton.UseVisualStyleBackColor = false;
            this.registrarAerolineasButton.Click += new System.EventHandler(this.registrarAerolineasButton_Click);
            // 
            // registrarAeropuertosButton
            // 
            this.registrarAeropuertosButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.registrarAeropuertosButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.registrarAeropuertosButton.FlatAppearance.BorderSize = 0;
            this.registrarAeropuertosButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.registrarAeropuertosButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrarAeropuertosButton.Location = new System.Drawing.Point(130, 252);
            this.registrarAeropuertosButton.Name = "registrarAeropuertosButton";
            this.registrarAeropuertosButton.Size = new System.Drawing.Size(350, 55);
            this.registrarAeropuertosButton.TabIndex = 2;
            this.registrarAeropuertosButton.TabStop = false;
            this.registrarAeropuertosButton.Text = "Registrar Aeropuertos     >";
            this.registrarAeropuertosButton.UseVisualStyleBackColor = false;
            this.registrarAeropuertosButton.Click += new System.EventHandler(this.registrarAeropuertosButton_Click);
            // 
            // registrarClientesButton
            // 
            this.registrarClientesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.registrarClientesButton.CausesValidation = false;
            this.registrarClientesButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.registrarClientesButton.FlatAppearance.BorderSize = 0;
            this.registrarClientesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.registrarClientesButton.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrarClientesButton.Location = new System.Drawing.Point(131, 176);
            this.registrarClientesButton.Name = "registrarClientesButton";
            this.registrarClientesButton.Size = new System.Drawing.Size(350, 55);
            this.registrarClientesButton.TabIndex = 1;
            this.registrarClientesButton.TabStop = false;
            this.registrarClientesButton.Text = "Registrar Clientes           >";
            this.registrarClientesButton.UseVisualStyleBackColor = false;
            this.registrarClientesButton.Click += new System.EventHandler(this.registrarClientesButton_Click);
            // 
            // salirDelSistema
            // 
            this.salirDelSistema.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.salirDelSistema.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.salirDelSistema.FlatAppearance.BorderSize = 0;
            this.salirDelSistema.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.salirDelSistema.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirDelSistema.Location = new System.Drawing.Point(351, 542);
            this.salirDelSistema.Name = "salirDelSistema";
            this.salirDelSistema.Size = new System.Drawing.Size(270, 64);
            this.salirDelSistema.TabIndex = 13;
            this.salirDelSistema.TabStop = false;
            this.salirDelSistema.Text = "Salir";
            this.salirDelSistema.UseVisualStyleBackColor = false;
            this.salirDelSistema.Click += new System.EventHandler(this.salirDelSistema_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button1.Enabled = false;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(36, 542);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(309, 64);
            this.button1.TabIndex = 14;
            this.button1.TabStop = false;
            this.button1.Text = "◉◉";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button2.Enabled = false;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(627, 542);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(295, 64);
            this.button2.TabIndex = 15;
            this.button2.TabStop = false;
            this.button2.Text = "◉◉";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button4.Enabled = false;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(36, 43);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(886, 117);
            this.button4.TabIndex = 25;
            this.button4.TabStop = false;
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button5.Enabled = false;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button5.FlatAppearance.BorderSize = 5;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(36, 123);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(886, 483);
            this.button5.TabIndex = 26;
            this.button5.TabStop = false;
            this.button5.UseVisualStyleBackColor = false;
            // 
            // registrarClientesPanel
            // 
            this.registrarClientesPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.registrarClientesPanel.Controls.Add(this.button12);
            this.registrarClientesPanel.Controls.Add(this.tituloClientes);
            this.registrarClientesPanel.Controls.Add(this.comboBoxGeneros);
            this.registrarClientesPanel.Controls.Add(this.label7);
            this.registrarClientesPanel.Controls.Add(this.dateTimeCalendar);
            this.registrarClientesPanel.Controls.Add(this.label6);
            this.registrarClientesPanel.Controls.Add(this.segundoATextBox);
            this.registrarClientesPanel.Controls.Add(this.label5);
            this.registrarClientesPanel.Controls.Add(this.primerATextBox);
            this.registrarClientesPanel.Controls.Add(this.label4);
            this.registrarClientesPanel.Controls.Add(this.nombreTextBox);
            this.registrarClientesPanel.Controls.Add(this.label3);
            this.registrarClientesPanel.Controls.Add(this.idTextBox);
            this.registrarClientesPanel.Controls.Add(this.label2);
            this.registrarClientesPanel.Controls.Add(this.button9);
            this.registrarClientesPanel.Controls.Add(this.enviarRegistroClientes);
            this.registrarClientesPanel.Controls.Add(this.cancelarRegistroClientes);
            this.registrarClientesPanel.Controls.Add(this.button6);
            this.registrarClientesPanel.Location = new System.Drawing.Point(0, 0);
            this.registrarClientesPanel.Name = "registrarClientesPanel";
            this.registrarClientesPanel.Size = new System.Drawing.Size(961, 649);
            this.registrarClientesPanel.TabIndex = 12;
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button12.Enabled = false;
            this.button12.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button12.FlatAppearance.BorderSize = 0;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.Location = new System.Drawing.Point(372, 542);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(212, 64);
            this.button12.TabIndex = 27;
            this.button12.TabStop = false;
            this.button12.Text = "◉◉";
            this.button12.UseVisualStyleBackColor = false;
            // 
            // tituloClientes
            // 
            this.tituloClientes.AutoSize = true;
            this.tituloClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tituloClientes.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tituloClientes.Location = new System.Drawing.Point(303, 88);
            this.tituloClientes.Name = "tituloClientes";
            this.tituloClientes.Size = new System.Drawing.Size(338, 39);
            this.tituloClientes.TabIndex = 12;
            this.tituloClientes.Text = "◉Registrar Clientes◉";
            // 
            // comboBoxGeneros
            // 
            this.comboBoxGeneros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxGeneros.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxGeneros.FormattingEnabled = true;
            this.comboBoxGeneros.Location = new System.Drawing.Point(487, 491);
            this.comboBoxGeneros.Name = "comboBoxGeneros";
            this.comboBoxGeneros.Size = new System.Drawing.Size(273, 29);
            this.comboBoxGeneros.TabIndex = 11;
            this.comboBoxGeneros.TabStop = false;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label7.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(189, 489);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(273, 30);
            this.label7.TabIndex = 10;
            this.label7.Text = "◉ Género";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimeCalendar
            // 
            this.dateTimeCalendar.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeCalendar.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeCalendar.Location = new System.Drawing.Point(487, 427);
            this.dateTimeCalendar.Name = "dateTimeCalendar";
            this.dateTimeCalendar.Size = new System.Drawing.Size(273, 35);
            this.dateTimeCalendar.TabIndex = 9;
            this.dateTimeCalendar.TabStop = false;
            this.dateTimeCalendar.Value = new System.DateTime(2023, 3, 16, 0, 0, 0, 0);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label6.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(189, 427);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(273, 30);
            this.label6.TabIndex = 8;
            this.label6.Text = "◉ Fecha de nacimiento";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // segundoATextBox
            // 
            this.segundoATextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.segundoATextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.segundoATextBox.Location = new System.Drawing.Point(487, 367);
            this.segundoATextBox.MaxLength = 20;
            this.segundoATextBox.Multiline = true;
            this.segundoATextBox.Name = "segundoATextBox";
            this.segundoATextBox.Size = new System.Drawing.Size(273, 31);
            this.segundoATextBox.TabIndex = 7;
            this.segundoATextBox.TabStop = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label5.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(189, 367);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(273, 30);
            this.label5.TabIndex = 6;
            this.label5.Text = "◉ Segundo Apellido";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // primerATextBox
            // 
            this.primerATextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.primerATextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.primerATextBox.Location = new System.Drawing.Point(487, 308);
            this.primerATextBox.MaxLength = 20;
            this.primerATextBox.Multiline = true;
            this.primerATextBox.Name = "primerATextBox";
            this.primerATextBox.Size = new System.Drawing.Size(273, 31);
            this.primerATextBox.TabIndex = 5;
            this.primerATextBox.TabStop = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label4.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(189, 308);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(273, 30);
            this.label4.TabIndex = 4;
            this.label4.Text = "◉ Primer Apellido";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nombreTextBox
            // 
            this.nombreTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nombreTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombreTextBox.Location = new System.Drawing.Point(487, 246);
            this.nombreTextBox.MaxLength = 20;
            this.nombreTextBox.Multiline = true;
            this.nombreTextBox.Name = "nombreTextBox";
            this.nombreTextBox.Size = new System.Drawing.Size(273, 31);
            this.nombreTextBox.TabIndex = 3;
            this.nombreTextBox.TabStop = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label3.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(189, 246);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(273, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "◉ Nombre";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // idTextBox
            // 
            this.idTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.idTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idTextBox.Location = new System.Drawing.Point(487, 186);
            this.idTextBox.MaxLength = 20;
            this.idTextBox.Multiline = true;
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(273, 31);
            this.idTextBox.TabIndex = 1;
            this.idTextBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label2.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(189, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(273, 31);
            this.label2.TabIndex = 0;
            this.label2.Text = "◉ Identificación";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button9.Enabled = false;
            this.button9.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(36, 43);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(886, 117);
            this.button9.TabIndex = 24;
            this.button9.TabStop = false;
            this.button9.UseVisualStyleBackColor = false;
            // 
            // enviarRegistroClientes
            // 
            this.enviarRegistroClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.enviarRegistroClientes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enviarRegistroClientes.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.enviarRegistroClientes.FlatAppearance.BorderSize = 0;
            this.enviarRegistroClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.enviarRegistroClientes.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enviarRegistroClientes.Location = new System.Drawing.Point(36, 542);
            this.enviarRegistroClientes.Name = "enviarRegistroClientes";
            this.enviarRegistroClientes.Size = new System.Drawing.Size(331, 64);
            this.enviarRegistroClientes.TabIndex = 13;
            this.enviarRegistroClientes.TabStop = false;
            this.enviarRegistroClientes.Text = "Aceptar";
            this.enviarRegistroClientes.UseVisualStyleBackColor = false;
            this.enviarRegistroClientes.Click += new System.EventHandler(this.enviarRegistroClientes_Click);
            // 
            // cancelarRegistroClientes
            // 
            this.cancelarRegistroClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cancelarRegistroClientes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelarRegistroClientes.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.cancelarRegistroClientes.FlatAppearance.BorderSize = 0;
            this.cancelarRegistroClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelarRegistroClientes.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelarRegistroClientes.Location = new System.Drawing.Point(590, 542);
            this.cancelarRegistroClientes.Name = "cancelarRegistroClientes";
            this.cancelarRegistroClientes.Size = new System.Drawing.Size(332, 64);
            this.cancelarRegistroClientes.TabIndex = 14;
            this.cancelarRegistroClientes.TabStop = false;
            this.cancelarRegistroClientes.Text = "Cancelar";
            this.cancelarRegistroClientes.UseVisualStyleBackColor = false;
            this.cancelarRegistroClientes.Click += new System.EventHandler(this.cancelarRegistroClientes_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button6.Enabled = false;
            this.button6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button6.FlatAppearance.BorderSize = 5;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(36, 83);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(886, 523);
            this.button6.TabIndex = 28;
            this.button6.TabStop = false;
            this.button6.UseVisualStyleBackColor = false;
            // 
            // registrarAeropuertosPanel
            // 
            this.registrarAeropuertosPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.registrarAeropuertosPanel.Controls.Add(this.codigoTextBox);
            this.registrarAeropuertosPanel.Controls.Add(this.button17);
            this.registrarAeropuertosPanel.Controls.Add(this.comboBoxAeropuertos);
            this.registrarAeropuertosPanel.Controls.Add(this.telefonoTextBox);
            this.registrarAeropuertosPanel.Controls.Add(this.ciudadTextBox);
            this.registrarAeropuertosPanel.Controls.Add(this.paisTextBox);
            this.registrarAeropuertosPanel.Controls.Add(this.nombreAeropuertoTextBox);
            this.registrarAeropuertosPanel.Controls.Add(this.label13);
            this.registrarAeropuertosPanel.Controls.Add(this.label12);
            this.registrarAeropuertosPanel.Controls.Add(this.label11);
            this.registrarAeropuertosPanel.Controls.Add(this.label10);
            this.registrarAeropuertosPanel.Controls.Add(this.label9);
            this.registrarAeropuertosPanel.Controls.Add(this.label8);
            this.registrarAeropuertosPanel.Controls.Add(this.tituloAeropuerto);
            this.registrarAeropuertosPanel.Controls.Add(this.button19);
            this.registrarAeropuertosPanel.Controls.Add(this.cancelarRegistroAeropuertos);
            this.registrarAeropuertosPanel.Controls.Add(this.enviarRegistroAeropuertos);
            this.registrarAeropuertosPanel.Controls.Add(this.button10);
            this.registrarAeropuertosPanel.Location = new System.Drawing.Point(0, 0);
            this.registrarAeropuertosPanel.Name = "registrarAeropuertosPanel";
            this.registrarAeropuertosPanel.Size = new System.Drawing.Size(961, 649);
            this.registrarAeropuertosPanel.TabIndex = 19;
            // 
            // codigoTextBox
            // 
            this.codigoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.codigoTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codigoTextBox.Location = new System.Drawing.Point(498, 191);
            this.codigoTextBox.MaxLength = 4;
            this.codigoTextBox.Multiline = true;
            this.codigoTextBox.Name = "codigoTextBox";
            this.codigoTextBox.Size = new System.Drawing.Size(273, 31);
            this.codigoTextBox.TabIndex = 33;
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button17.Enabled = false;
            this.button17.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button17.FlatAppearance.BorderSize = 0;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button17.Location = new System.Drawing.Point(369, 542);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(230, 64);
            this.button17.TabIndex = 29;
            this.button17.TabStop = false;
            this.button17.Text = "◉◉";
            this.button17.UseVisualStyleBackColor = false;
            // 
            // comboBoxAeropuertos
            // 
            this.comboBoxAeropuertos.AutoCompleteCustomSource.AddRange(new string[] {
            "Activo",
            "Inactivo"});
            this.comboBoxAeropuertos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxAeropuertos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxAeropuertos.FormattingEnabled = true;
            this.comboBoxAeropuertos.Location = new System.Drawing.Point(498, 489);
            this.comboBoxAeropuertos.Name = "comboBoxAeropuertos";
            this.comboBoxAeropuertos.Size = new System.Drawing.Size(273, 29);
            this.comboBoxAeropuertos.TabIndex = 28;
            this.comboBoxAeropuertos.TabStop = false;
            // 
            // telefonoTextBox
            // 
            this.telefonoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.telefonoTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.telefonoTextBox.Location = new System.Drawing.Point(498, 429);
            this.telefonoTextBox.MaxLength = 20;
            this.telefonoTextBox.Multiline = true;
            this.telefonoTextBox.Name = "telefonoTextBox";
            this.telefonoTextBox.Size = new System.Drawing.Size(273, 31);
            this.telefonoTextBox.TabIndex = 24;
            // 
            // ciudadTextBox
            // 
            this.ciudadTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ciudadTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ciudadTextBox.Location = new System.Drawing.Point(498, 372);
            this.ciudadTextBox.MaxLength = 20;
            this.ciudadTextBox.Multiline = true;
            this.ciudadTextBox.Name = "ciudadTextBox";
            this.ciudadTextBox.Size = new System.Drawing.Size(273, 31);
            this.ciudadTextBox.TabIndex = 23;
            // 
            // paisTextBox
            // 
            this.paisTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.paisTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paisTextBox.Location = new System.Drawing.Point(498, 313);
            this.paisTextBox.MaxLength = 20;
            this.paisTextBox.Multiline = true;
            this.paisTextBox.Name = "paisTextBox";
            this.paisTextBox.Size = new System.Drawing.Size(273, 31);
            this.paisTextBox.TabIndex = 22;
            // 
            // nombreAeropuertoTextBox
            // 
            this.nombreAeropuertoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nombreAeropuertoTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombreAeropuertoTextBox.Location = new System.Drawing.Point(498, 251);
            this.nombreAeropuertoTextBox.MaxLength = 20;
            this.nombreAeropuertoTextBox.Multiline = true;
            this.nombreAeropuertoTextBox.Name = "nombreAeropuertoTextBox";
            this.nombreAeropuertoTextBox.Size = new System.Drawing.Size(273, 31);
            this.nombreAeropuertoTextBox.TabIndex = 21;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label13.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(195, 487);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(273, 31);
            this.label13.TabIndex = 19;
            this.label13.Text = "◉ Estado";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label12.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(195, 429);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(273, 31);
            this.label12.TabIndex = 18;
            this.label12.Text = "◉ Teléfono";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label11.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(195, 372);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(273, 31);
            this.label11.TabIndex = 17;
            this.label11.Text = "◉ Ciudad";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label10.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(195, 313);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(273, 31);
            this.label10.TabIndex = 16;
            this.label10.Text = "◉ País";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label9.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(195, 251);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(273, 31);
            this.label9.TabIndex = 15;
            this.label9.Text = "◉ Nombre del Aeropuerto";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label8.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(195, 191);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(278, 31);
            this.label8.TabIndex = 14;
            this.label8.Text = "◉ Codigo del Aeropuerto";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tituloAeropuerto
            // 
            this.tituloAeropuerto.AutoSize = true;
            this.tituloAeropuerto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tituloAeropuerto.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tituloAeropuerto.Location = new System.Drawing.Point(303, 86);
            this.tituloAeropuerto.Name = "tituloAeropuerto";
            this.tituloAeropuerto.Size = new System.Drawing.Size(395, 39);
            this.tituloAeropuerto.TabIndex = 13;
            this.tituloAeropuerto.Text = "◉Registrar Aeropuertos◉";
            // 
            // button19
            // 
            this.button19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button19.Enabled = false;
            this.button19.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button19.FlatAppearance.BorderSize = 0;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button19.Location = new System.Drawing.Point(36, 43);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(886, 117);
            this.button19.TabIndex = 31;
            this.button19.TabStop = false;
            this.button19.UseVisualStyleBackColor = false;
            // 
            // cancelarRegistroAeropuertos
            // 
            this.cancelarRegistroAeropuertos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cancelarRegistroAeropuertos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelarRegistroAeropuertos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.cancelarRegistroAeropuertos.FlatAppearance.BorderSize = 0;
            this.cancelarRegistroAeropuertos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelarRegistroAeropuertos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelarRegistroAeropuertos.Location = new System.Drawing.Point(605, 542);
            this.cancelarRegistroAeropuertos.Name = "cancelarRegistroAeropuertos";
            this.cancelarRegistroAeropuertos.Size = new System.Drawing.Size(317, 64);
            this.cancelarRegistroAeropuertos.TabIndex = 27;
            this.cancelarRegistroAeropuertos.TabStop = false;
            this.cancelarRegistroAeropuertos.Text = "Cancelar";
            this.cancelarRegistroAeropuertos.UseVisualStyleBackColor = false;
            this.cancelarRegistroAeropuertos.Click += new System.EventHandler(this.cancelarRegistroAeropuertos_Click);
            // 
            // enviarRegistroAeropuertos
            // 
            this.enviarRegistroAeropuertos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.enviarRegistroAeropuertos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enviarRegistroAeropuertos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.enviarRegistroAeropuertos.FlatAppearance.BorderSize = 0;
            this.enviarRegistroAeropuertos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.enviarRegistroAeropuertos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enviarRegistroAeropuertos.Location = new System.Drawing.Point(36, 542);
            this.enviarRegistroAeropuertos.Name = "enviarRegistroAeropuertos";
            this.enviarRegistroAeropuertos.Size = new System.Drawing.Size(327, 64);
            this.enviarRegistroAeropuertos.TabIndex = 26;
            this.enviarRegistroAeropuertos.TabStop = false;
            this.enviarRegistroAeropuertos.Text = "Aceptar";
            this.enviarRegistroAeropuertos.UseVisualStyleBackColor = false;
            this.enviarRegistroAeropuertos.Click += new System.EventHandler(this.enviarRegistroAeropuertos_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button10.Enabled = false;
            this.button10.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button10.FlatAppearance.BorderSize = 5;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(36, 123);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(886, 483);
            this.button10.TabIndex = 32;
            this.button10.TabStop = false;
            this.button10.UseVisualStyleBackColor = false;
            // 
            // registrarAerolineasPanel
            // 
            this.registrarAerolineasPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.registrarAerolineasPanel.Controls.Add(this.button21);
            this.registrarAerolineasPanel.Controls.Add(this.comboBoxAerolineas);
            this.registrarAerolineasPanel.Controls.Add(this.telefonoAerolineaTextBox);
            this.registrarAerolineasPanel.Controls.Add(this.nombreAerolineaTextBox);
            this.registrarAerolineasPanel.Controls.Add(this.idAerolineaTextBox);
            this.registrarAerolineasPanel.Controls.Add(this.label16);
            this.registrarAerolineasPanel.Controls.Add(this.label17);
            this.registrarAerolineasPanel.Controls.Add(this.label20);
            this.registrarAerolineasPanel.Controls.Add(this.label21);
            this.registrarAerolineasPanel.Controls.Add(this.label22);
            this.registrarAerolineasPanel.Controls.Add(this.button23);
            this.registrarAerolineasPanel.Controls.Add(this.cancelarRegistroAerolineas);
            this.registrarAerolineasPanel.Controls.Add(this.enviarRegistroAerolineas);
            this.registrarAerolineasPanel.Controls.Add(this.button11);
            this.registrarAerolineasPanel.Location = new System.Drawing.Point(0, 0);
            this.registrarAerolineasPanel.Name = "registrarAerolineasPanel";
            this.registrarAerolineasPanel.Size = new System.Drawing.Size(961, 649);
            this.registrarAerolineasPanel.TabIndex = 21;
            // 
            // button21
            // 
            this.button21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button21.Enabled = false;
            this.button21.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button21.FlatAppearance.BorderSize = 0;
            this.button21.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button21.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button21.Location = new System.Drawing.Point(369, 542);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(251, 64);
            this.button21.TabIndex = 29;
            this.button21.TabStop = false;
            this.button21.Text = "◉◉";
            this.button21.UseVisualStyleBackColor = false;
            // 
            // comboBoxAerolineas
            // 
            this.comboBoxAerolineas.AutoCompleteCustomSource.AddRange(new string[] {
            "Activo",
            "Inactivo"});
            this.comboBoxAerolineas.BackColor = System.Drawing.Color.White;
            this.comboBoxAerolineas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxAerolineas.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxAerolineas.FormattingEnabled = true;
            this.comboBoxAerolineas.Location = new System.Drawing.Point(512, 462);
            this.comboBoxAerolineas.Name = "comboBoxAerolineas";
            this.comboBoxAerolineas.Size = new System.Drawing.Size(273, 29);
            this.comboBoxAerolineas.TabIndex = 28;
            this.comboBoxAerolineas.TabStop = false;
            // 
            // telefonoAerolineaTextBox
            // 
            this.telefonoAerolineaTextBox.BackColor = System.Drawing.Color.White;
            this.telefonoAerolineaTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.telefonoAerolineaTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.telefonoAerolineaTextBox.Location = new System.Drawing.Point(512, 380);
            this.telefonoAerolineaTextBox.MaxLength = 20;
            this.telefonoAerolineaTextBox.Multiline = true;
            this.telefonoAerolineaTextBox.Name = "telefonoAerolineaTextBox";
            this.telefonoAerolineaTextBox.Size = new System.Drawing.Size(273, 31);
            this.telefonoAerolineaTextBox.TabIndex = 24;
            // 
            // nombreAerolineaTextBox
            // 
            this.nombreAerolineaTextBox.BackColor = System.Drawing.Color.White;
            this.nombreAerolineaTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nombreAerolineaTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombreAerolineaTextBox.Location = new System.Drawing.Point(512, 289);
            this.nombreAerolineaTextBox.MaxLength = 20;
            this.nombreAerolineaTextBox.Multiline = true;
            this.nombreAerolineaTextBox.Name = "nombreAerolineaTextBox";
            this.nombreAerolineaTextBox.Size = new System.Drawing.Size(273, 31);
            this.nombreAerolineaTextBox.TabIndex = 21;
            // 
            // idAerolineaTextBox
            // 
            this.idAerolineaTextBox.BackColor = System.Drawing.Color.White;
            this.idAerolineaTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.idAerolineaTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idAerolineaTextBox.Location = new System.Drawing.Point(512, 204);
            this.idAerolineaTextBox.MaxLength = 20;
            this.idAerolineaTextBox.Multiline = true;
            this.idAerolineaTextBox.Name = "idAerolineaTextBox";
            this.idAerolineaTextBox.Size = new System.Drawing.Size(273, 31);
            this.idAerolineaTextBox.TabIndex = 20;
            this.idAerolineaTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.idAerolineaTextBox_KeyPress);
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label16.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(209, 460);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(273, 31);
            this.label16.TabIndex = 19;
            this.label16.Text = "◉ Estado";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label17.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(209, 380);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(273, 31);
            this.label17.TabIndex = 18;
            this.label17.Text = "◉ Teléfono";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label20.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(209, 289);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(273, 31);
            this.label20.TabIndex = 15;
            this.label20.Text = "◉ Nombre de la Aerolinea";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label21.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(209, 204);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(273, 31);
            this.label21.TabIndex = 14;
            this.label21.Text = "◉ ID de la Aerolinea";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label22.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(303, 86);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(373, 39);
            this.label22.TabIndex = 13;
            this.label22.Text = "◉Registrar Aerolineas◉";
            // 
            // button23
            // 
            this.button23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button23.Enabled = false;
            this.button23.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button23.FlatAppearance.BorderSize = 0;
            this.button23.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button23.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button23.Location = new System.Drawing.Point(36, 43);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(886, 117);
            this.button23.TabIndex = 31;
            this.button23.TabStop = false;
            this.button23.UseVisualStyleBackColor = false;
            // 
            // cancelarRegistroAerolineas
            // 
            this.cancelarRegistroAerolineas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cancelarRegistroAerolineas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelarRegistroAerolineas.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.cancelarRegistroAerolineas.FlatAppearance.BorderSize = 0;
            this.cancelarRegistroAerolineas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelarRegistroAerolineas.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelarRegistroAerolineas.Location = new System.Drawing.Point(626, 542);
            this.cancelarRegistroAerolineas.Name = "cancelarRegistroAerolineas";
            this.cancelarRegistroAerolineas.Size = new System.Drawing.Size(296, 64);
            this.cancelarRegistroAerolineas.TabIndex = 27;
            this.cancelarRegistroAerolineas.TabStop = false;
            this.cancelarRegistroAerolineas.Text = "Cancelar";
            this.cancelarRegistroAerolineas.UseVisualStyleBackColor = false;
            this.cancelarRegistroAerolineas.Click += new System.EventHandler(this.cancelarRegistroAerolineas_Click);
            // 
            // enviarRegistroAerolineas
            // 
            this.enviarRegistroAerolineas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.enviarRegistroAerolineas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enviarRegistroAerolineas.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.enviarRegistroAerolineas.FlatAppearance.BorderSize = 0;
            this.enviarRegistroAerolineas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.enviarRegistroAerolineas.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enviarRegistroAerolineas.Location = new System.Drawing.Point(36, 542);
            this.enviarRegistroAerolineas.Name = "enviarRegistroAerolineas";
            this.enviarRegistroAerolineas.Size = new System.Drawing.Size(327, 64);
            this.enviarRegistroAerolineas.TabIndex = 26;
            this.enviarRegistroAerolineas.TabStop = false;
            this.enviarRegistroAerolineas.Text = "Aceptar";
            this.enviarRegistroAerolineas.UseVisualStyleBackColor = false;
            this.enviarRegistroAerolineas.Click += new System.EventHandler(this.enviarRegistroAerolineas_Click);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button11.Enabled = false;
            this.button11.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button11.FlatAppearance.BorderSize = 5;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(36, 123);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(886, 483);
            this.button11.TabIndex = 32;
            this.button11.TabStop = false;
            this.button11.UseVisualStyleBackColor = false;
            // 
            // registrarVuelosPanel
            // 
            this.registrarVuelosPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.registrarVuelosPanel.Controls.Add(this.numeroVueloTextBox);
            this.registrarVuelosPanel.Controls.Add(this.button26);
            this.registrarVuelosPanel.Controls.Add(this.capacidadTextBox);
            this.registrarVuelosPanel.Controls.Add(this.duracionDateTime);
            this.registrarVuelosPanel.Controls.Add(this.label25);
            this.registrarVuelosPanel.Controls.Add(this.label24);
            this.registrarVuelosPanel.Controls.Add(this.destinoTextBox);
            this.registrarVuelosPanel.Controls.Add(this.origenTextBox);
            this.registrarVuelosPanel.Controls.Add(this.nombreAerolineaVueloTextBox);
            this.registrarVuelosPanel.Controls.Add(this.label14);
            this.registrarVuelosPanel.Controls.Add(this.label15);
            this.registrarVuelosPanel.Controls.Add(this.label18);
            this.registrarVuelosPanel.Controls.Add(this.label19);
            this.registrarVuelosPanel.Controls.Add(this.label23);
            this.registrarVuelosPanel.Controls.Add(this.button27);
            this.registrarVuelosPanel.Controls.Add(this.cancelarRegistroVuelos);
            this.registrarVuelosPanel.Controls.Add(this.enviarRegistroVuelos);
            this.registrarVuelosPanel.Controls.Add(this.button13);
            this.registrarVuelosPanel.Location = new System.Drawing.Point(0, 0);
            this.registrarVuelosPanel.Name = "registrarVuelosPanel";
            this.registrarVuelosPanel.Size = new System.Drawing.Size(961, 649);
            this.registrarVuelosPanel.TabIndex = 22;
            // 
            // numeroVueloTextBox
            // 
            this.numeroVueloTextBox.BackColor = System.Drawing.Color.White;
            this.numeroVueloTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numeroVueloTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeroVueloTextBox.Location = new System.Drawing.Point(502, 191);
            this.numeroVueloTextBox.MaxLength = 20;
            this.numeroVueloTextBox.Multiline = true;
            this.numeroVueloTextBox.Name = "numeroVueloTextBox";
            this.numeroVueloTextBox.Size = new System.Drawing.Size(273, 31);
            this.numeroVueloTextBox.TabIndex = 33;
            this.numeroVueloTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numeroVueloTextBox_KeyPress);
            // 
            // button26
            // 
            this.button26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button26.Enabled = false;
            this.button26.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button26.FlatAppearance.BorderSize = 0;
            this.button26.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button26.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button26.Location = new System.Drawing.Point(373, 542);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(226, 64);
            this.button26.TabIndex = 35;
            this.button26.TabStop = false;
            this.button26.Text = "◉◉";
            this.button26.UseVisualStyleBackColor = false;
            // 
            // capacidadTextBox
            // 
            this.capacidadTextBox.BackColor = System.Drawing.Color.White;
            this.capacidadTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.capacidadTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.capacidadTextBox.Location = new System.Drawing.Point(502, 486);
            this.capacidadTextBox.MaxLength = 4;
            this.capacidadTextBox.Multiline = true;
            this.capacidadTextBox.Name = "capacidadTextBox";
            this.capacidadTextBox.Size = new System.Drawing.Size(273, 31);
            this.capacidadTextBox.TabIndex = 32;
            this.capacidadTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.capacidadTextBox_KeyPress);
            // 
            // duracionDateTime
            // 
            this.duracionDateTime.CalendarForeColor = System.Drawing.Color.Transparent;
            this.duracionDateTime.CalendarMonthBackground = System.Drawing.Color.Transparent;
            this.duracionDateTime.CalendarTitleBackColor = System.Drawing.Color.Transparent;
            this.duracionDateTime.CalendarTitleForeColor = System.Drawing.Color.Transparent;
            this.duracionDateTime.CalendarTrailingForeColor = System.Drawing.Color.Transparent;
            this.duracionDateTime.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.duracionDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.duracionDateTime.Location = new System.Drawing.Point(502, 425);
            this.duracionDateTime.Name = "duracionDateTime";
            this.duracionDateTime.Size = new System.Drawing.Size(273, 35);
            this.duracionDateTime.TabIndex = 31;
            this.duracionDateTime.TabStop = false;
            this.duracionDateTime.Value = new System.DateTime(2023, 3, 18, 16, 21, 0, 0);
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label25.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(200, 486);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(273, 31);
            this.label25.TabIndex = 30;
            this.label25.Text = "◉ Capacidad";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label24.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(199, 427);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(273, 31);
            this.label24.TabIndex = 29;
            this.label24.Text = "◉ Duración";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // destinoTextBox
            // 
            this.destinoTextBox.BackColor = System.Drawing.Color.White;
            this.destinoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.destinoTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.destinoTextBox.Location = new System.Drawing.Point(502, 369);
            this.destinoTextBox.MaxLength = 20;
            this.destinoTextBox.Multiline = true;
            this.destinoTextBox.Name = "destinoTextBox";
            this.destinoTextBox.Size = new System.Drawing.Size(273, 31);
            this.destinoTextBox.TabIndex = 28;
            // 
            // origenTextBox
            // 
            this.origenTextBox.BackColor = System.Drawing.Color.White;
            this.origenTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.origenTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.origenTextBox.Location = new System.Drawing.Point(502, 310);
            this.origenTextBox.MaxLength = 20;
            this.origenTextBox.Multiline = true;
            this.origenTextBox.Name = "origenTextBox";
            this.origenTextBox.Size = new System.Drawing.Size(273, 31);
            this.origenTextBox.TabIndex = 24;
            // 
            // nombreAerolineaVueloTextBox
            // 
            this.nombreAerolineaVueloTextBox.BackColor = System.Drawing.Color.White;
            this.nombreAerolineaVueloTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nombreAerolineaVueloTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombreAerolineaVueloTextBox.Location = new System.Drawing.Point(502, 250);
            this.nombreAerolineaVueloTextBox.MaxLength = 20;
            this.nombreAerolineaVueloTextBox.Multiline = true;
            this.nombreAerolineaVueloTextBox.Name = "nombreAerolineaVueloTextBox";
            this.nombreAerolineaVueloTextBox.Size = new System.Drawing.Size(273, 31);
            this.nombreAerolineaVueloTextBox.TabIndex = 21;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label14.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(199, 369);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(273, 31);
            this.label14.TabIndex = 19;
            this.label14.Text = "◉ Aeropuerto de Destino";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label15.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(199, 310);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(273, 31);
            this.label15.TabIndex = 18;
            this.label15.Text = "◉ Aeropuerto de Origen";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label18.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(199, 250);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(273, 31);
            this.label18.TabIndex = 15;
            this.label18.Text = "◉ Nombre de la Aerolinea";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label19.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(199, 191);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(273, 31);
            this.label19.TabIndex = 14;
            this.label19.Text = "◉ Número del Vuelo";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label23.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(312, 87);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(309, 39);
            this.label23.TabIndex = 13;
            this.label23.Text = "◉Registrar Vuelos◉";
            // 
            // button27
            // 
            this.button27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button27.Enabled = false;
            this.button27.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button27.FlatAppearance.BorderSize = 0;
            this.button27.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button27.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button27.Location = new System.Drawing.Point(36, 43);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(886, 117);
            this.button27.TabIndex = 36;
            this.button27.TabStop = false;
            this.button27.UseVisualStyleBackColor = false;
            // 
            // cancelarRegistroVuelos
            // 
            this.cancelarRegistroVuelos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cancelarRegistroVuelos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelarRegistroVuelos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.cancelarRegistroVuelos.FlatAppearance.BorderSize = 0;
            this.cancelarRegistroVuelos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelarRegistroVuelos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelarRegistroVuelos.Location = new System.Drawing.Point(605, 542);
            this.cancelarRegistroVuelos.Name = "cancelarRegistroVuelos";
            this.cancelarRegistroVuelos.Size = new System.Drawing.Size(317, 64);
            this.cancelarRegistroVuelos.TabIndex = 27;
            this.cancelarRegistroVuelos.TabStop = false;
            this.cancelarRegistroVuelos.Text = "Cancelar";
            this.cancelarRegistroVuelos.UseVisualStyleBackColor = false;
            this.cancelarRegistroVuelos.Click += new System.EventHandler(this.cancelarRegistroVuelos_Click);
            // 
            // enviarRegistroVuelos
            // 
            this.enviarRegistroVuelos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.enviarRegistroVuelos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enviarRegistroVuelos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.enviarRegistroVuelos.FlatAppearance.BorderSize = 0;
            this.enviarRegistroVuelos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.enviarRegistroVuelos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enviarRegistroVuelos.Location = new System.Drawing.Point(36, 542);
            this.enviarRegistroVuelos.Name = "enviarRegistroVuelos";
            this.enviarRegistroVuelos.Size = new System.Drawing.Size(330, 64);
            this.enviarRegistroVuelos.TabIndex = 26;
            this.enviarRegistroVuelos.TabStop = false;
            this.enviarRegistroVuelos.Text = "Aceptar";
            this.enviarRegistroVuelos.UseVisualStyleBackColor = false;
            this.enviarRegistroVuelos.Click += new System.EventHandler(this.enviarRegistroVuelos_Click);
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button13.Enabled = false;
            this.button13.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button13.FlatAppearance.BorderSize = 5;
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.Location = new System.Drawing.Point(36, 123);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(886, 483);
            this.button13.TabIndex = 37;
            this.button13.TabStop = false;
            this.button13.UseVisualStyleBackColor = false;
            // 
            // mostrarClientesPanel
            // 
            this.mostrarClientesPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.mostrarClientesPanel.Controls.Add(this.dataGridViewClientes);
            this.mostrarClientesPanel.Controls.Add(this.button8);
            this.mostrarClientesPanel.Controls.Add(this.label26);
            this.mostrarClientesPanel.Controls.Add(this.button25);
            this.mostrarClientesPanel.Controls.Add(this.mostrarGridViewClientes);
            this.mostrarClientesPanel.Controls.Add(this.volverMenuMostrarClientes);
            this.mostrarClientesPanel.Controls.Add(this.button3);
            this.mostrarClientesPanel.Location = new System.Drawing.Point(0, 0);
            this.mostrarClientesPanel.Name = "mostrarClientesPanel";
            this.mostrarClientesPanel.Size = new System.Drawing.Size(961, 649);
            this.mostrarClientesPanel.TabIndex = 16;
            // 
            // dataGridViewClientes
            // 
            this.dataGridViewClientes.AllowUserToAddRows = false;
            this.dataGridViewClientes.AllowUserToDeleteRows = false;
            this.dataGridViewClientes.AllowUserToResizeColumns = false;
            this.dataGridViewClientes.AllowUserToResizeRows = false;
            this.dataGridViewClientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewClientes.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewClientes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewClientes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewClientes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClientes.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewClientes.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewClientes.EnableHeadersVisualStyles = false;
            this.dataGridViewClientes.GridColor = System.Drawing.Color.White;
            this.dataGridViewClientes.Location = new System.Drawing.Point(48, 167);
            this.dataGridViewClientes.MultiSelect = false;
            this.dataGridViewClientes.Name = "dataGridViewClientes";
            this.dataGridViewClientes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClientes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewClientes.RowHeadersWidth = 20;
            this.dataGridViewClientes.Size = new System.Drawing.Size(860, 368);
            this.dataGridViewClientes.TabIndex = 32;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button8.Enabled = false;
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(372, 542);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(211, 64);
            this.button8.TabIndex = 27;
            this.button8.TabStop = false;
            this.button8.Text = "◉◉";
            this.button8.UseVisualStyleBackColor = false;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label26.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(311, 88);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(316, 39);
            this.label26.TabIndex = 11;
            this.label26.Text = "◉Mostrar Clientes◉";
            // 
            // button25
            // 
            this.button25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button25.Enabled = false;
            this.button25.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button25.FlatAppearance.BorderSize = 0;
            this.button25.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button25.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button25.Location = new System.Drawing.Point(36, 43);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(886, 117);
            this.button25.TabIndex = 29;
            this.button25.TabStop = false;
            this.button25.UseVisualStyleBackColor = false;
            // 
            // mostrarGridViewClientes
            // 
            this.mostrarGridViewClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.mostrarGridViewClientes.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarGridViewClientes.FlatAppearance.BorderSize = 0;
            this.mostrarGridViewClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarGridViewClientes.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarGridViewClientes.Location = new System.Drawing.Point(36, 542);
            this.mostrarGridViewClientes.Name = "mostrarGridViewClientes";
            this.mostrarGridViewClientes.Size = new System.Drawing.Size(328, 64);
            this.mostrarGridViewClientes.TabIndex = 14;
            this.mostrarGridViewClientes.TabStop = false;
            this.mostrarGridViewClientes.Text = "Mostrar";
            this.mostrarGridViewClientes.UseVisualStyleBackColor = false;
            this.mostrarGridViewClientes.Click += new System.EventHandler(this.mostrarGridViewClientes_Click);
            // 
            // volverMenuMostrarClientes
            // 
            this.volverMenuMostrarClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.volverMenuMostrarClientes.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.volverMenuMostrarClientes.FlatAppearance.BorderSize = 0;
            this.volverMenuMostrarClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.volverMenuMostrarClientes.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volverMenuMostrarClientes.Location = new System.Drawing.Point(594, 542);
            this.volverMenuMostrarClientes.Name = "volverMenuMostrarClientes";
            this.volverMenuMostrarClientes.Size = new System.Drawing.Size(328, 64);
            this.volverMenuMostrarClientes.TabIndex = 13;
            this.volverMenuMostrarClientes.TabStop = false;
            this.volverMenuMostrarClientes.Text = "Salir";
            this.volverMenuMostrarClientes.UseVisualStyleBackColor = false;
            this.volverMenuMostrarClientes.Click += new System.EventHandler(this.volverMenuMostrarClientes_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button3.Enabled = false;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button3.FlatAppearance.BorderSize = 5;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(36, 123);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(886, 483);
            this.button3.TabIndex = 33;
            this.button3.TabStop = false;
            this.button3.UseVisualStyleBackColor = false;
            // 
            // mostrarAeropuertosPanel
            // 
            this.mostrarAeropuertosPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.mostrarAeropuertosPanel.Controls.Add(this.dataGridViewAeropuertos);
            this.mostrarAeropuertosPanel.Controls.Add(this.button29);
            this.mostrarAeropuertosPanel.Controls.Add(this.label27);
            this.mostrarAeropuertosPanel.Controls.Add(this.button32);
            this.mostrarAeropuertosPanel.Controls.Add(this.mostrarGridViewAeropuertos);
            this.mostrarAeropuertosPanel.Controls.Add(this.volverMenuMostrarAeropuertos);
            this.mostrarAeropuertosPanel.Controls.Add(this.button7);
            this.mostrarAeropuertosPanel.Location = new System.Drawing.Point(0, 0);
            this.mostrarAeropuertosPanel.Name = "mostrarAeropuertosPanel";
            this.mostrarAeropuertosPanel.Size = new System.Drawing.Size(961, 649);
            this.mostrarAeropuertosPanel.TabIndex = 23;
            // 
            // dataGridViewAeropuertos
            // 
            this.dataGridViewAeropuertos.AllowUserToAddRows = false;
            this.dataGridViewAeropuertos.AllowUserToDeleteRows = false;
            this.dataGridViewAeropuertos.AllowUserToResizeColumns = false;
            this.dataGridViewAeropuertos.AllowUserToResizeRows = false;
            this.dataGridViewAeropuertos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewAeropuertos.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewAeropuertos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewAeropuertos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewAeropuertos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAeropuertos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewAeropuertos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAeropuertos.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAeropuertos.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewAeropuertos.EnableHeadersVisualStyles = false;
            this.dataGridViewAeropuertos.GridColor = System.Drawing.Color.White;
            this.dataGridViewAeropuertos.Location = new System.Drawing.Point(48, 167);
            this.dataGridViewAeropuertos.MultiSelect = false;
            this.dataGridViewAeropuertos.Name = "dataGridViewAeropuertos";
            this.dataGridViewAeropuertos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAeropuertos.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewAeropuertos.RowHeadersWidth = 20;
            this.dataGridViewAeropuertos.Size = new System.Drawing.Size(860, 368);
            this.dataGridViewAeropuertos.TabIndex = 33;
            // 
            // button29
            // 
            this.button29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button29.Enabled = false;
            this.button29.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button29.FlatAppearance.BorderSize = 0;
            this.button29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button29.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button29.Location = new System.Drawing.Point(372, 542);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(211, 64);
            this.button29.TabIndex = 27;
            this.button29.TabStop = false;
            this.button29.Text = "◉◉";
            this.button29.UseVisualStyleBackColor = false;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label27.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(285, 85);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(373, 39);
            this.label27.TabIndex = 11;
            this.label27.Text = "◉Mostrar Aeropuertos◉";
            // 
            // button32
            // 
            this.button32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button32.Enabled = false;
            this.button32.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button32.FlatAppearance.BorderSize = 0;
            this.button32.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button32.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button32.Location = new System.Drawing.Point(36, 43);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(886, 117);
            this.button32.TabIndex = 29;
            this.button32.TabStop = false;
            this.button32.UseVisualStyleBackColor = false;
            // 
            // mostrarGridViewAeropuertos
            // 
            this.mostrarGridViewAeropuertos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.mostrarGridViewAeropuertos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarGridViewAeropuertos.FlatAppearance.BorderSize = 0;
            this.mostrarGridViewAeropuertos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarGridViewAeropuertos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarGridViewAeropuertos.Location = new System.Drawing.Point(36, 542);
            this.mostrarGridViewAeropuertos.Name = "mostrarGridViewAeropuertos";
            this.mostrarGridViewAeropuertos.Size = new System.Drawing.Size(328, 64);
            this.mostrarGridViewAeropuertos.TabIndex = 14;
            this.mostrarGridViewAeropuertos.TabStop = false;
            this.mostrarGridViewAeropuertos.Text = "Mostrar";
            this.mostrarGridViewAeropuertos.UseVisualStyleBackColor = false;
            this.mostrarGridViewAeropuertos.Click += new System.EventHandler(this.mostrarGridViewAeropuertos_Click);
            // 
            // volverMenuMostrarAeropuertos
            // 
            this.volverMenuMostrarAeropuertos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.volverMenuMostrarAeropuertos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.volverMenuMostrarAeropuertos.FlatAppearance.BorderSize = 0;
            this.volverMenuMostrarAeropuertos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.volverMenuMostrarAeropuertos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volverMenuMostrarAeropuertos.Location = new System.Drawing.Point(594, 542);
            this.volverMenuMostrarAeropuertos.Name = "volverMenuMostrarAeropuertos";
            this.volverMenuMostrarAeropuertos.Size = new System.Drawing.Size(328, 64);
            this.volverMenuMostrarAeropuertos.TabIndex = 13;
            this.volverMenuMostrarAeropuertos.TabStop = false;
            this.volverMenuMostrarAeropuertos.Text = "Salir";
            this.volverMenuMostrarAeropuertos.UseVisualStyleBackColor = false;
            this.volverMenuMostrarAeropuertos.Click += new System.EventHandler(this.volverMenuMostrarAeropuertos_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button7.Enabled = false;
            this.button7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button7.FlatAppearance.BorderSize = 5;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(36, 123);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(886, 483);
            this.button7.TabIndex = 34;
            this.button7.TabStop = false;
            this.button7.UseVisualStyleBackColor = false;
            // 
            // mostrarAerolineasPanel
            // 
            this.mostrarAerolineasPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.mostrarAerolineasPanel.Controls.Add(this.dataGridViewAerolineas);
            this.mostrarAerolineasPanel.Controls.Add(this.button14);
            this.mostrarAerolineasPanel.Controls.Add(this.label28);
            this.mostrarAerolineasPanel.Controls.Add(this.button33);
            this.mostrarAerolineasPanel.Controls.Add(this.mostrarGridViewAerolineas);
            this.mostrarAerolineasPanel.Controls.Add(this.volverMenuMostrarAerolineas);
            this.mostrarAerolineasPanel.Controls.Add(this.button15);
            this.mostrarAerolineasPanel.Location = new System.Drawing.Point(0, 0);
            this.mostrarAerolineasPanel.Name = "mostrarAerolineasPanel";
            this.mostrarAerolineasPanel.Size = new System.Drawing.Size(961, 649);
            this.mostrarAerolineasPanel.TabIndex = 24;
            // 
            // dataGridViewAerolineas
            // 
            this.dataGridViewAerolineas.AllowUserToAddRows = false;
            this.dataGridViewAerolineas.AllowUserToDeleteRows = false;
            this.dataGridViewAerolineas.AllowUserToResizeColumns = false;
            this.dataGridViewAerolineas.AllowUserToResizeRows = false;
            this.dataGridViewAerolineas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewAerolineas.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewAerolineas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewAerolineas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewAerolineas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAerolineas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewAerolineas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAerolineas.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAerolineas.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewAerolineas.EnableHeadersVisualStyles = false;
            this.dataGridViewAerolineas.GridColor = System.Drawing.Color.White;
            this.dataGridViewAerolineas.Location = new System.Drawing.Point(48, 167);
            this.dataGridViewAerolineas.MultiSelect = false;
            this.dataGridViewAerolineas.Name = "dataGridViewAerolineas";
            this.dataGridViewAerolineas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAerolineas.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewAerolineas.RowHeadersWidth = 20;
            this.dataGridViewAerolineas.Size = new System.Drawing.Size(860, 368);
            this.dataGridViewAerolineas.TabIndex = 33;
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button14.Enabled = false;
            this.button14.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button14.FlatAppearance.BorderSize = 0;
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.Location = new System.Drawing.Point(372, 542);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(211, 64);
            this.button14.TabIndex = 27;
            this.button14.TabStop = false;
            this.button14.Text = "◉◉";
            this.button14.UseVisualStyleBackColor = false;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label28.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(285, 85);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(351, 39);
            this.label28.TabIndex = 11;
            this.label28.Text = "◉Mostrar Aerolineas◉";
            // 
            // button33
            // 
            this.button33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button33.Enabled = false;
            this.button33.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button33.FlatAppearance.BorderSize = 0;
            this.button33.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button33.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button33.Location = new System.Drawing.Point(36, 43);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(886, 117);
            this.button33.TabIndex = 29;
            this.button33.TabStop = false;
            this.button33.UseVisualStyleBackColor = false;
            // 
            // mostrarGridViewAerolineas
            // 
            this.mostrarGridViewAerolineas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.mostrarGridViewAerolineas.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarGridViewAerolineas.FlatAppearance.BorderSize = 0;
            this.mostrarGridViewAerolineas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarGridViewAerolineas.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarGridViewAerolineas.Location = new System.Drawing.Point(36, 542);
            this.mostrarGridViewAerolineas.Name = "mostrarGridViewAerolineas";
            this.mostrarGridViewAerolineas.Size = new System.Drawing.Size(328, 64);
            this.mostrarGridViewAerolineas.TabIndex = 14;
            this.mostrarGridViewAerolineas.TabStop = false;
            this.mostrarGridViewAerolineas.Text = "Mostrar";
            this.mostrarGridViewAerolineas.UseVisualStyleBackColor = false;
            this.mostrarGridViewAerolineas.Click += new System.EventHandler(this.mostrarGridViewAerolineas_Click);
            // 
            // volverMenuMostrarAerolineas
            // 
            this.volverMenuMostrarAerolineas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.volverMenuMostrarAerolineas.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.volverMenuMostrarAerolineas.FlatAppearance.BorderSize = 0;
            this.volverMenuMostrarAerolineas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.volverMenuMostrarAerolineas.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volverMenuMostrarAerolineas.Location = new System.Drawing.Point(594, 542);
            this.volverMenuMostrarAerolineas.Name = "volverMenuMostrarAerolineas";
            this.volverMenuMostrarAerolineas.Size = new System.Drawing.Size(328, 64);
            this.volverMenuMostrarAerolineas.TabIndex = 13;
            this.volverMenuMostrarAerolineas.TabStop = false;
            this.volverMenuMostrarAerolineas.Text = "Salir";
            this.volverMenuMostrarAerolineas.UseVisualStyleBackColor = false;
            this.volverMenuMostrarAerolineas.Click += new System.EventHandler(this.volverMenuMostrarAerolineas_Click);
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button15.Enabled = false;
            this.button15.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button15.FlatAppearance.BorderSize = 5;
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.Location = new System.Drawing.Point(36, 123);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(886, 483);
            this.button15.TabIndex = 34;
            this.button15.TabStop = false;
            this.button15.UseVisualStyleBackColor = false;
            // 
            // mostrarVuelosPanel
            // 
            this.mostrarVuelosPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.mostrarVuelosPanel.Controls.Add(this.dataGridViewVuelos);
            this.mostrarVuelosPanel.Controls.Add(this.button30);
            this.mostrarVuelosPanel.Controls.Add(this.label29);
            this.mostrarVuelosPanel.Controls.Add(this.button37);
            this.mostrarVuelosPanel.Controls.Add(this.mostrarGridViewVuelos);
            this.mostrarVuelosPanel.Controls.Add(this.volverMenuMostrarVuelos);
            this.mostrarVuelosPanel.Controls.Add(this.button16);
            this.mostrarVuelosPanel.Location = new System.Drawing.Point(0, 0);
            this.mostrarVuelosPanel.Name = "mostrarVuelosPanel";
            this.mostrarVuelosPanel.Size = new System.Drawing.Size(961, 649);
            this.mostrarVuelosPanel.TabIndex = 25;
            // 
            // dataGridViewVuelos
            // 
            this.dataGridViewVuelos.AllowUserToAddRows = false;
            this.dataGridViewVuelos.AllowUserToDeleteRows = false;
            this.dataGridViewVuelos.AllowUserToResizeColumns = false;
            this.dataGridViewVuelos.AllowUserToResizeRows = false;
            this.dataGridViewVuelos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewVuelos.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewVuelos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewVuelos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewVuelos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewVuelos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewVuelos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVuelos.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewVuelos.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewVuelos.EnableHeadersVisualStyles = false;
            this.dataGridViewVuelos.GridColor = System.Drawing.Color.White;
            this.dataGridViewVuelos.Location = new System.Drawing.Point(48, 167);
            this.dataGridViewVuelos.MultiSelect = false;
            this.dataGridViewVuelos.Name = "dataGridViewVuelos";
            this.dataGridViewVuelos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewVuelos.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewVuelos.RowHeadersWidth = 20;
            this.dataGridViewVuelos.Size = new System.Drawing.Size(860, 368);
            this.dataGridViewVuelos.TabIndex = 33;
            // 
            // button30
            // 
            this.button30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button30.Enabled = false;
            this.button30.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button30.FlatAppearance.BorderSize = 0;
            this.button30.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button30.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button30.Location = new System.Drawing.Point(372, 542);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(211, 64);
            this.button30.TabIndex = 27;
            this.button30.TabStop = false;
            this.button30.Text = "◉◉";
            this.button30.UseVisualStyleBackColor = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label29.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(322, 85);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(287, 39);
            this.label29.TabIndex = 11;
            this.label29.Text = "◉Mostrar Vuelos◉";
            // 
            // button37
            // 
            this.button37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button37.Enabled = false;
            this.button37.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button37.FlatAppearance.BorderSize = 0;
            this.button37.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button37.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button37.Location = new System.Drawing.Point(36, 43);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(886, 117);
            this.button37.TabIndex = 29;
            this.button37.TabStop = false;
            this.button37.UseVisualStyleBackColor = false;
            // 
            // mostrarGridViewVuelos
            // 
            this.mostrarGridViewVuelos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.mostrarGridViewVuelos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarGridViewVuelos.FlatAppearance.BorderSize = 0;
            this.mostrarGridViewVuelos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarGridViewVuelos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarGridViewVuelos.Location = new System.Drawing.Point(36, 542);
            this.mostrarGridViewVuelos.Name = "mostrarGridViewVuelos";
            this.mostrarGridViewVuelos.Size = new System.Drawing.Size(328, 64);
            this.mostrarGridViewVuelos.TabIndex = 14;
            this.mostrarGridViewVuelos.TabStop = false;
            this.mostrarGridViewVuelos.Text = "Mostrar";
            this.mostrarGridViewVuelos.UseVisualStyleBackColor = false;
            this.mostrarGridViewVuelos.Click += new System.EventHandler(this.mostrarGridViewVuelos_Click);
            // 
            // volverMenuMostrarVuelos
            // 
            this.volverMenuMostrarVuelos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.volverMenuMostrarVuelos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.volverMenuMostrarVuelos.FlatAppearance.BorderSize = 0;
            this.volverMenuMostrarVuelos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.volverMenuMostrarVuelos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volverMenuMostrarVuelos.Location = new System.Drawing.Point(594, 542);
            this.volverMenuMostrarVuelos.Name = "volverMenuMostrarVuelos";
            this.volverMenuMostrarVuelos.Size = new System.Drawing.Size(328, 64);
            this.volverMenuMostrarVuelos.TabIndex = 13;
            this.volverMenuMostrarVuelos.TabStop = false;
            this.volverMenuMostrarVuelos.Text = "Salir";
            this.volverMenuMostrarVuelos.UseVisualStyleBackColor = false;
            this.volverMenuMostrarVuelos.Click += new System.EventHandler(this.volverMenuMostrarVuelos_Click);
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button16.Enabled = false;
            this.button16.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button16.FlatAppearance.BorderSize = 5;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button16.Location = new System.Drawing.Point(36, 123);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(886, 483);
            this.button16.TabIndex = 34;
            this.button16.TabStop = false;
            this.button16.UseVisualStyleBackColor = false;
            // 
            // comprarBoletosPanel
            // 
            this.comprarBoletosPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.comprarBoletosPanel.Controls.Add(this.asientosTextBox);
            this.comprarBoletosPanel.Controls.Add(this.button18);
            this.comprarBoletosPanel.Controls.Add(this.label30);
            this.comprarBoletosPanel.Controls.Add(this.dateTimePickerBoleto);
            this.comprarBoletosPanel.Controls.Add(this.label32);
            this.comprarBoletosPanel.Controls.Add(this.clienteTextBox);
            this.comprarBoletosPanel.Controls.Add(this.label33);
            this.comprarBoletosPanel.Controls.Add(this.label34);
            this.comprarBoletosPanel.Controls.Add(this.vueloTextBox);
            this.comprarBoletosPanel.Controls.Add(this.label35);
            this.comprarBoletosPanel.Controls.Add(this.numeroBoletoTextBox);
            this.comprarBoletosPanel.Controls.Add(this.label36);
            this.comprarBoletosPanel.Controls.Add(this.button20);
            this.comprarBoletosPanel.Controls.Add(this.enviarRegistroBoletos);
            this.comprarBoletosPanel.Controls.Add(this.cancelarRegistroBoletos);
            this.comprarBoletosPanel.Controls.Add(this.button28);
            this.comprarBoletosPanel.Location = new System.Drawing.Point(0, 0);
            this.comprarBoletosPanel.Name = "comprarBoletosPanel";
            this.comprarBoletosPanel.Size = new System.Drawing.Size(961, 649);
            this.comprarBoletosPanel.TabIndex = 27;
            // 
            // asientosTextBox
            // 
            this.asientosTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.asientosTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asientosTextBox.Location = new System.Drawing.Point(487, 440);
            this.asientosTextBox.MaxLength = 4;
            this.asientosTextBox.Multiline = true;
            this.asientosTextBox.Name = "asientosTextBox";
            this.asientosTextBox.Size = new System.Drawing.Size(273, 31);
            this.asientosTextBox.TabIndex = 29;
            this.asientosTextBox.TabStop = false;
            this.asientosTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.asientosTextBox_KeyPress);
            // 
            // button18
            // 
            this.button18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button18.Enabled = false;
            this.button18.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button18.FlatAppearance.BorderSize = 0;
            this.button18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button18.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button18.Location = new System.Drawing.Point(372, 542);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(212, 64);
            this.button18.TabIndex = 27;
            this.button18.TabStop = false;
            this.button18.Text = "◉◉";
            this.button18.UseVisualStyleBackColor = false;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label30.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(310, 88);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(322, 39);
            this.label30.TabIndex = 12;
            this.label30.Text = "◉Comprar Boletos◉";
            // 
            // dateTimePickerBoleto
            // 
            this.dateTimePickerBoleto.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerBoleto.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerBoleto.Location = new System.Drawing.Point(486, 320);
            this.dateTimePickerBoleto.Name = "dateTimePickerBoleto";
            this.dateTimePickerBoleto.Size = new System.Drawing.Size(273, 35);
            this.dateTimePickerBoleto.TabIndex = 9;
            this.dateTimePickerBoleto.TabStop = false;
            this.dateTimePickerBoleto.Value = new System.DateTime(2023, 3, 16, 0, 0, 0, 0);
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label32.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(189, 443);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(273, 30);
            this.label32.TabIndex = 8;
            this.label32.Text = "◉ Cantidad de asientos";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clienteTextBox
            // 
            this.clienteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clienteTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clienteTextBox.Location = new System.Drawing.Point(487, 383);
            this.clienteTextBox.MaxLength = 20;
            this.clienteTextBox.Multiline = true;
            this.clienteTextBox.Name = "clienteTextBox";
            this.clienteTextBox.Size = new System.Drawing.Size(273, 31);
            this.clienteTextBox.TabIndex = 7;
            this.clienteTextBox.TabStop = false;
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label33.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(189, 383);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(273, 30);
            this.label33.TabIndex = 6;
            this.label33.Text = "◉ Identidicación del cliente";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label34.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(189, 324);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(273, 30);
            this.label34.TabIndex = 4;
            this.label34.Text = "◉ Fecha de la compra";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vueloTextBox
            // 
            this.vueloTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vueloTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vueloTextBox.Location = new System.Drawing.Point(487, 262);
            this.vueloTextBox.MaxLength = 20;
            this.vueloTextBox.Multiline = true;
            this.vueloTextBox.Name = "vueloTextBox";
            this.vueloTextBox.Size = new System.Drawing.Size(273, 31);
            this.vueloTextBox.TabIndex = 3;
            this.vueloTextBox.TabStop = false;
            this.vueloTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.vueloTextBox_KeyPress);
            // 
            // label35
            // 
            this.label35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label35.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(189, 262);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(273, 30);
            this.label35.TabIndex = 2;
            this.label35.Text = "◉ Número de vuelo";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numeroBoletoTextBox
            // 
            this.numeroBoletoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numeroBoletoTextBox.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeroBoletoTextBox.Location = new System.Drawing.Point(487, 202);
            this.numeroBoletoTextBox.MaxLength = 20;
            this.numeroBoletoTextBox.Multiline = true;
            this.numeroBoletoTextBox.Name = "numeroBoletoTextBox";
            this.numeroBoletoTextBox.Size = new System.Drawing.Size(273, 31);
            this.numeroBoletoTextBox.TabIndex = 1;
            this.numeroBoletoTextBox.TabStop = false;
            this.numeroBoletoTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numeroBoletoTextBox_KeyPress);
            // 
            // label36
            // 
            this.label36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label36.Font = new System.Drawing.Font("Bauhaus 93", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(189, 202);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(273, 31);
            this.label36.TabIndex = 0;
            this.label36.Text = "◉ Número del boleto";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button20
            // 
            this.button20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button20.Enabled = false;
            this.button20.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button20.FlatAppearance.BorderSize = 0;
            this.button20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button20.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button20.Location = new System.Drawing.Point(36, 43);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(886, 117);
            this.button20.TabIndex = 24;
            this.button20.TabStop = false;
            this.button20.UseVisualStyleBackColor = false;
            // 
            // enviarRegistroBoletos
            // 
            this.enviarRegistroBoletos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.enviarRegistroBoletos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enviarRegistroBoletos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.enviarRegistroBoletos.FlatAppearance.BorderSize = 0;
            this.enviarRegistroBoletos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.enviarRegistroBoletos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enviarRegistroBoletos.Location = new System.Drawing.Point(36, 542);
            this.enviarRegistroBoletos.Name = "enviarRegistroBoletos";
            this.enviarRegistroBoletos.Size = new System.Drawing.Size(331, 64);
            this.enviarRegistroBoletos.TabIndex = 13;
            this.enviarRegistroBoletos.TabStop = false;
            this.enviarRegistroBoletos.Text = "Aceptar";
            this.enviarRegistroBoletos.UseVisualStyleBackColor = false;
            this.enviarRegistroBoletos.Click += new System.EventHandler(this.enviarRegistroBoletos_Click);
            // 
            // cancelarRegistroBoletos
            // 
            this.cancelarRegistroBoletos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cancelarRegistroBoletos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelarRegistroBoletos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.cancelarRegistroBoletos.FlatAppearance.BorderSize = 0;
            this.cancelarRegistroBoletos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelarRegistroBoletos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelarRegistroBoletos.Location = new System.Drawing.Point(590, 542);
            this.cancelarRegistroBoletos.Name = "cancelarRegistroBoletos";
            this.cancelarRegistroBoletos.Size = new System.Drawing.Size(332, 64);
            this.cancelarRegistroBoletos.TabIndex = 14;
            this.cancelarRegistroBoletos.TabStop = false;
            this.cancelarRegistroBoletos.Text = "Cancelar";
            this.cancelarRegistroBoletos.UseVisualStyleBackColor = false;
            this.cancelarRegistroBoletos.Click += new System.EventHandler(this.cancelarRegistroBoletos_Click);
            // 
            // button28
            // 
            this.button28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button28.Enabled = false;
            this.button28.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button28.FlatAppearance.BorderSize = 5;
            this.button28.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button28.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button28.Location = new System.Drawing.Point(36, 83);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(886, 523);
            this.button28.TabIndex = 28;
            this.button28.TabStop = false;
            this.button28.UseVisualStyleBackColor = false;
            // 
            // mostrarBoletosPanel
            // 
            this.mostrarBoletosPanel.BackColor = System.Drawing.Color.SeaGreen;
            this.mostrarBoletosPanel.Controls.Add(this.dataGridViewBoletos);
            this.mostrarBoletosPanel.Controls.Add(this.button22);
            this.mostrarBoletosPanel.Controls.Add(this.label31);
            this.mostrarBoletosPanel.Controls.Add(this.button24);
            this.mostrarBoletosPanel.Controls.Add(this.mostrarGridViewBoletos);
            this.mostrarBoletosPanel.Controls.Add(this.volverMenuMostrarBoletos);
            this.mostrarBoletosPanel.Controls.Add(this.button35);
            this.mostrarBoletosPanel.Location = new System.Drawing.Point(0, 0);
            this.mostrarBoletosPanel.Name = "mostrarBoletosPanel";
            this.mostrarBoletosPanel.Size = new System.Drawing.Size(961, 649);
            this.mostrarBoletosPanel.TabIndex = 27;
            // 
            // dataGridViewBoletos
            // 
            this.dataGridViewBoletos.AllowUserToAddRows = false;
            this.dataGridViewBoletos.AllowUserToDeleteRows = false;
            this.dataGridViewBoletos.AllowUserToResizeColumns = false;
            this.dataGridViewBoletos.AllowUserToResizeRows = false;
            this.dataGridViewBoletos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewBoletos.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewBoletos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewBoletos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewBoletos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBoletos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewBoletos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBoletos.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBoletos.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewBoletos.EnableHeadersVisualStyles = false;
            this.dataGridViewBoletos.GridColor = System.Drawing.Color.White;
            this.dataGridViewBoletos.Location = new System.Drawing.Point(48, 167);
            this.dataGridViewBoletos.MultiSelect = false;
            this.dataGridViewBoletos.Name = "dataGridViewBoletos";
            this.dataGridViewBoletos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBoletos.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewBoletos.RowHeadersWidth = 20;
            this.dataGridViewBoletos.Size = new System.Drawing.Size(860, 368);
            this.dataGridViewBoletos.TabIndex = 32;
            // 
            // button22
            // 
            this.button22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button22.Enabled = false;
            this.button22.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button22.FlatAppearance.BorderSize = 0;
            this.button22.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button22.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button22.Location = new System.Drawing.Point(372, 542);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(211, 64);
            this.button22.TabIndex = 27;
            this.button22.TabStop = false;
            this.button22.Text = "◉◉";
            this.button22.UseVisualStyleBackColor = false;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label31.Font = new System.Drawing.Font("Bauhaus 93", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(311, 88);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(315, 39);
            this.label31.TabIndex = 11;
            this.label31.Text = "◉Mostrar Boletoss◉";
            // 
            // button24
            // 
            this.button24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button24.Enabled = false;
            this.button24.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button24.FlatAppearance.BorderSize = 0;
            this.button24.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button24.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button24.Location = new System.Drawing.Point(36, 43);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(886, 117);
            this.button24.TabIndex = 29;
            this.button24.TabStop = false;
            this.button24.UseVisualStyleBackColor = false;
            // 
            // mostrarGridViewBoletos
            // 
            this.mostrarGridViewBoletos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.mostrarGridViewBoletos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mostrarGridViewBoletos.FlatAppearance.BorderSize = 0;
            this.mostrarGridViewBoletos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mostrarGridViewBoletos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mostrarGridViewBoletos.Location = new System.Drawing.Point(36, 542);
            this.mostrarGridViewBoletos.Name = "mostrarGridViewBoletos";
            this.mostrarGridViewBoletos.Size = new System.Drawing.Size(328, 64);
            this.mostrarGridViewBoletos.TabIndex = 14;
            this.mostrarGridViewBoletos.TabStop = false;
            this.mostrarGridViewBoletos.Text = "Mostrar";
            this.mostrarGridViewBoletos.UseVisualStyleBackColor = false;
            this.mostrarGridViewBoletos.Click += new System.EventHandler(this.mostrarGridViewBoletos_Click);
            // 
            // volverMenuMostrarBoletos
            // 
            this.volverMenuMostrarBoletos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.volverMenuMostrarBoletos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.volverMenuMostrarBoletos.FlatAppearance.BorderSize = 0;
            this.volverMenuMostrarBoletos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.volverMenuMostrarBoletos.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volverMenuMostrarBoletos.Location = new System.Drawing.Point(594, 542);
            this.volverMenuMostrarBoletos.Name = "volverMenuMostrarBoletos";
            this.volverMenuMostrarBoletos.Size = new System.Drawing.Size(328, 64);
            this.volverMenuMostrarBoletos.TabIndex = 13;
            this.volverMenuMostrarBoletos.TabStop = false;
            this.volverMenuMostrarBoletos.Text = "Salir";
            this.volverMenuMostrarBoletos.UseVisualStyleBackColor = false;
            this.volverMenuMostrarBoletos.Click += new System.EventHandler(this.volverMenuMostrarBoletos_Click);
            // 
            // button35
            // 
            this.button35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button35.Enabled = false;
            this.button35.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button35.FlatAppearance.BorderSize = 5;
            this.button35.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button35.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button35.Location = new System.Drawing.Point(36, 123);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(886, 483);
            this.button35.TabIndex = 33;
            this.button35.TabStop = false;
            this.button35.UseVisualStyleBackColor = false;
            // 
            // Formulario
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(961, 649);
            this.Controls.Add(this.menuInicio);
            this.Controls.Add(this.registrarClientesPanel);
            this.Controls.Add(this.registrarAeropuertosPanel);
            this.Controls.Add(this.registrarAerolineasPanel);
            this.Controls.Add(this.registrarVuelosPanel);
            this.Controls.Add(this.comprarBoletosPanel);
            this.Controls.Add(this.mostrarClientesPanel);
            this.Controls.Add(this.mostrarAeropuertosPanel);
            this.Controls.Add(this.mostrarAerolineasPanel);
            this.Controls.Add(this.mostrarVuelosPanel);
            this.Controls.Add(this.mostrarBoletosPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Formulario";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.menuInicio.ResumeLayout(false);
            this.menuInicio.PerformLayout();
            this.registrarClientesPanel.ResumeLayout(false);
            this.registrarClientesPanel.PerformLayout();
            this.registrarAeropuertosPanel.ResumeLayout(false);
            this.registrarAeropuertosPanel.PerformLayout();
            this.registrarAerolineasPanel.ResumeLayout(false);
            this.registrarAerolineasPanel.PerformLayout();
            this.registrarVuelosPanel.ResumeLayout(false);
            this.registrarVuelosPanel.PerformLayout();
            this.mostrarClientesPanel.ResumeLayout(false);
            this.mostrarClientesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClientes)).EndInit();
            this.mostrarAeropuertosPanel.ResumeLayout(false);
            this.mostrarAeropuertosPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAeropuertos)).EndInit();
            this.mostrarAerolineasPanel.ResumeLayout(false);
            this.mostrarAerolineasPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAerolineas)).EndInit();
            this.mostrarVuelosPanel.ResumeLayout(false);
            this.mostrarVuelosPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVuelos)).EndInit();
            this.comprarBoletosPanel.ResumeLayout(false);
            this.comprarBoletosPanel.PerformLayout();
            this.mostrarBoletosPanel.ResumeLayout(false);
            this.mostrarBoletosPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBoletos)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

public class Cliente {
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string PrimerApellido { get; set; }
    public string SegundoApellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public char Genero { get; set; }

    public Cliente(string id, string nombre, string primerApellido, string segundoApellido, DateTime fechaNacimiento, char genero) {
        Id = id;
        Nombre = nombre;
        PrimerApellido = primerApellido;
        SegundoApellido = segundoApellido;
        FechaNacimiento = fechaNacimiento;
        Genero = genero;
    }
}

public class Aeropuerto { // Clase que contiene las propiedades de los Aeropuestos
    public string Codigo { get; set; }
    public string NombreAeropuerto { get; set; }
    public string Pais { get; set; }
    public string Ciudad { get; set; }
    public string Telefono { get; set; }
    public bool Estado { get; set; }

    // Metodo Aeropuerto que almacena todas las variables relacionadas a los Aeropuertos que luego se usara para crear el objeto
    public Aeropuerto(string codigo, string nombraeropuerto, string pais, string ciudad, string telefono, bool estado) {
        Codigo = codigo;
        NombreAeropuerto = nombraeropuerto;
        Pais = pais;
        Ciudad = ciudad;
        Telefono = telefono;
        Estado = estado;
    }
}

public class Aerolinea { // Clase que contiene las propiedades de los Aerolineas
    public int IdAerolinea { get; set; }
    public string NombreAerolinea { get; set; }
    public string TelefonoAerolinea { get; set; }
    public bool EstadoAerolinea { get; set; }

    // Metodo Aerolinea que almacena todas las variables relacionadas a los Aerolineas que luego se usara para crear el objeto
    public Aerolinea(int idaerolinea, string nombreaerolinea, string telefonoaerolinea, bool estadoaerolinea) {
        IdAerolinea = idaerolinea;
        NombreAerolinea = nombreaerolinea;
        TelefonoAerolinea = telefonoaerolinea;
        EstadoAerolinea = estadoaerolinea;
    }
}

public class Vuelo { // Clase que contiene las propiedades de los Vuelos
    public int NumeroVuelo { get; set; }
    public Aerolinea Aerolinea { get; set; }
    public Aeropuerto Origen { get; set; }
    public Aeropuerto Destino { get; set; }
    public string DuracionStr { get; set; }
    public int Capacidad { get; set; }

    // Metodo Vuelo que almacena todas las variables relacionadas a los Vuelos que luego se usara para crear el objeto
    public Vuelo(int numero, Aerolinea aerolinea, Aeropuerto origen, Aeropuerto destino, string duracionStr, int capacidad) {
        NumeroVuelo = numero;
        Aerolinea = aerolinea;
        Origen = origen;
        Destino = destino;
        DuracionStr = duracionStr;
        Capacidad = capacidad;
    }
}

public class Boleto { // Clase que contiene las propiedades de los Boletos
    public int NumeroBoleto { get; set; }
    public Vuelo Vuelo { get; set; }
    public Cliente Cliente { get; set; }
    public DateTime FechaCompra { get; set; }
    public int Asientos { get; set; }

    // Metodo Vuelo que almacena todas las variables relacionadas a los Vuelos que luego se usara para crear el objeto
    public Boleto(int numeroBoleto, Vuelo vuelo, Cliente cliente, DateTime fechaCompra, int asientos) {
        NumeroBoleto = numeroBoleto;
        Vuelo = vuelo;
        Cliente = cliente;
        FechaCompra = fechaCompra;
        Asientos = asientos;
    }
}