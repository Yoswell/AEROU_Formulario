using System.Windows.Forms;

namespace Tarea2_PrograAv {
    internal class Metodos {
        //Metodos que muestran los mesajes de acuerdo a las excepciones que ocurran, o si ha pasado algo
        public DialogResult Salir() {
            return MessageBox.Show("¿Está seguro que desea salir del sistema?", "Confirmación de salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public void RegistroAnticipado(string apartado) {
            MessageBox.Show("Para ingresar debe de haber al menos " + apartado, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void EspacioInsuficiente() {
            MessageBox.Show("Ya no hay espacio suficiente para el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void RegistroExitoso() {
            MessageBox.Show("El registro a sido guardado correctamente", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void LLenarEspacios() {
            MessageBox.Show("Por favor, llene todos los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void TextoInvalidoNombreAerolinea() {
            MessageBox.Show("La aerolinea ingresada no se encuentra registrada", "Aerolinea no registrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
         
        public void TextoInvalidoOrigen() {
            MessageBox.Show("Este aeropuerto de origen no se encuantra registrado", "Aeropuerto no registrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void TextoInvalidoDestino() {
            MessageBox.Show("Este aeropuerto de destino no se encuantra registrado", "Aeropuerto no registrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void TextoInvalidoVuelo() {
            MessageBox.Show("Este vuelo no se encuantra registrado", "Vuelo no registrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void TextoInvalidoCliente() {
            MessageBox.Show("Este cliente no se encuantra registrado", "Cliente no registrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void AeropuertosIguales() {
            MessageBox.Show("El aeropuerto de origen y el aeropuerto de destino no pueden ser los mismos", "Aeropuertos iguales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void BoletoYaRegistrado() {
            MessageBox.Show("Este numero de boleto ya a sido previamente registrado", "Boleto ya registrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void VueloYaRegistrado() {
            MessageBox.Show("Este numero de vuelo ya a sido previamente registrado", "Vuelo ya registrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void AerolineaYaRegistrada() {
            MessageBox.Show("Este ID de aerolinea ya a sido previamente registrado", "Aerolinea ya registrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void AeropuertoYaRegistrado() {
            MessageBox.Show("Este codigo de aeropuerto ya a sido previamente registrado", "Aeropuerto ya registrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ClienteYaRegistrado() {
            MessageBox.Show("Este ID de cliente ya a sido previamente registrado", "Cliente ya registrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void AsientosMenor_1() {
            MessageBox.Show("El numero de asientos tiene que ser mayor a (1) y menor a la cantidad especificada en el registro de vuelos", "Cantidad no valida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void AerolineaYAeropuertosNoRegistrados() {
            MessageBox.Show("Para acceder a esta opcion primero debe de registrar una aerolinea y dos aeropuertos, el de partida y el de destino", "Aerolinea y Aeropuertos no registrados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ClienteYVueloNoRegistrados() {
            MessageBox.Show("Para acceder a esta opcion primero debe de registrar al menos un cliente y un vuelo en los apartados correspondientes", "Cliente y Vuelo no registrados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //Metodos que ocultan los paneles y los muestran en pantalla, estos metodos resiven paneles por parmetro
        public void PanelesRegistro(Panel a, Panel b, Panel c, Panel d, Panel e) {
            a.Visible = false;
            b.Visible = false;
            c.Visible = false;
            d.Visible = false;
            e.Visible = false;
        }

        public void PanelesMostrar(Panel a, Panel b, Panel c, Panel d, Panel e) {
            a.Visible = false;
            b.Visible = false;
            c.Visible = false;
            d.Visible = false;
            e.Visible = false;
        }

        public void PanelCancelar(Panel a, Panel b) {
            a.Visible = true;
            b.Visible = false;
        }

        public void PaneEnviar(Panel a, Panel b) {
            a.Visible = true;
            b.Visible = false;
        }

        //Metodos que comprueban si un textbox esta vacio o a este solo se le ingreso una cadena vacia
        public bool EspaciosClientes(TextBox a, TextBox b, TextBox c, TextBox d) {
            if (string.IsNullOrWhiteSpace(a.Text) ||
                string.IsNullOrWhiteSpace(b.Text) ||
                string.IsNullOrWhiteSpace(c.Text) ||
                string.IsNullOrWhiteSpace(d.Text)) {
                return true;
            }
            return false;
        }

        public bool EspaciosAeropuertos(TextBox a, TextBox b, TextBox c, TextBox d, TextBox e) {
            if (string.IsNullOrWhiteSpace(a.Text) ||
                string.IsNullOrWhiteSpace(b.Text) ||
                string.IsNullOrWhiteSpace(c.Text) ||
                string.IsNullOrWhiteSpace(d.Text) ||
                string.IsNullOrWhiteSpace(e.Text)) {
                return true;
            }
            return false;
        }

        public bool EspaciosAerolineas(TextBox a, TextBox b, TextBox c) {
            if (string.IsNullOrWhiteSpace(a.Text) ||
                string.IsNullOrWhiteSpace(b.Text) ||
                string.IsNullOrWhiteSpace(c.Text)) {
                return true;
            }
            return false;
        }

        public bool EspaciosVuelos(TextBox a, TextBox b, TextBox c, TextBox d, TextBox e) {
            if (string.IsNullOrWhiteSpace(a.Text) ||
                string.IsNullOrWhiteSpace(b.Text) ||
                string.IsNullOrWhiteSpace(c.Text) ||
                string.IsNullOrWhiteSpace(d.Text) ||
                string.IsNullOrWhiteSpace(e.Text)) {
                return true;
            }
            return false;
        }

        public bool EspaciosBoletos(TextBox a, TextBox b, TextBox c, TextBox d) {
            if (string.IsNullOrWhiteSpace(a.Text) ||
                string.IsNullOrWhiteSpace(b.Text) ||
                string.IsNullOrWhiteSpace(c.Text) ||
                string.IsNullOrWhiteSpace(d.Text)) {
                return true;
            }
            return false;
        }

        //Metodo para vaciar los textbox despues de que se ha hecho un registro exitoso o al cancelar el registro
        public void VaciarTexto(TextBox a, TextBox b, TextBox c, TextBox d, TextBox e, TextBox f, TextBox g, TextBox h,
            TextBox i, TextBox j, TextBox k, TextBox l, TextBox m, TextBox n, TextBox o, TextBox p, TextBox q, TextBox r,
            TextBox s, TextBox t, TextBox u) {
            a.Text = null;
            b.Text = null;
            c.Text = null;
            d.Text = null;

            e.Text = null;
            f.Text = null;
            g.Text = null;
            h.Text = null;
            i.Text = null;

            s.Text = null;
            t.Text = null;
            u.Text = null;

            j.Text = null;
            k.Text = null;
            l.Text = null;
            m.Text = null;
            n.Text = null;

            o.Text = null;
            p.Text = null;
            q.Text = null;
            r.Text = null;
        }
    }
}
