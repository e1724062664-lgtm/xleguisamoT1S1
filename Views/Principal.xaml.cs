namespace xleguisamoT1S1.Views;

public partial class Principal : ContentPage
{
	public Principal()
	{
		InitializeComponent();
	}

    private async void OnCalcularClicked(object sender, EventArgs e)
    {
        // Obtener los valores ingresados (quitando espacios en blanco al inicio y final)
        string nombre = txtNombre.Text?.Trim() ?? string.Empty;
        string apellido = txtApellido.Text?.Trim() ?? string.Empty;
        string edadTexto = txtEdad.Text?.Trim() ?? string.Empty;
        string salarioTexto = txtSalario.Text?.Trim() ?? string.Empty;

        // Validar que Nombre, Apellido y Edad no estén vacíos
        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(apellido) ||
            string.IsNullOrWhiteSpace(edadTexto))
        {
            await DisplayAlertAsync("Campos Incompletos", "Por favor, llene todos los campos (Nombre, Apellido y Edad) para continuar.", "OK");
            return;
        }

        if (!int.TryParse(edadTexto, out int edad) || edad < 15)
        {
            await DisplayAlertAsync("Edad Inválida", "Por favor, ingrese una edad válida (número entero igual o mayor a 15).", "OK");
            return;
        }

        // Validar que el salario sea un número válido y mayor a cero
        if (double.TryParse(salarioTexto, out double salario) && salario > 0)
        {
            // Calcular el aporte al IESS (9.45%)
            double aporteIESS = salario * 0.0945;

            // Mensaje solicitado con saltos de línea (\n)
            string mensajeAlerta = $"Bienvenido {nombre} {apellido}\n" +
                                   $"Tienes {edad} años\n" +
                                   $"Tu aporte mensual es {aporteIESS:F2}";

            // Mostrar alerta en pantalla
            await DisplayAlertAsync("Resultado IESS", mensajeAlerta, "Aceptar");

            // --- Guardar en el disco C: ---
            try
            {
                string rutaDirectorio = @"C:\ArchivosIESS";

                if (!Directory.Exists(rutaDirectorio))
                {
                    Directory.CreateDirectory(rutaDirectorio);
                }

                string rutaArchivo = Path.Combine(rutaDirectorio, "registro_aportes_iess.txt");

                string contenidoRegistro = $"Fecha: {DateTime.Now}\n" +
                                           $"Afiliado: {nombre} {apellido}\n" +
                                           $"Edad: {edad}\n" +
                                           $"Salario: {salario:F2}\n" +
                                           $"Aporte IESS (9.45%): {aporteIESS:F2}\n" +
                                           "----------------------------------------\n";

                File.AppendAllText(rutaArchivo, contenidoRegistro);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"No se pudo guardar en el disco C: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlertAsync("Salario Inválido", "Por favor, ingrese un monto de salario válido mayor a cero.", "OK");
        }
    }
}