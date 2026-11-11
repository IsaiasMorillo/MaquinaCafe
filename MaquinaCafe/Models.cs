namespace MaquinaCafe;

public enum TamanoVaso
{
    Pequeno,
    Mediano,
    Grande
}

public static class TamanoVasoInfo
{
    public static int Onzas(this TamanoVaso tamano) => tamano switch
    {
        TamanoVaso.Pequeno => 3,
        TamanoVaso.Mediano => 5,
        TamanoVaso.Grande => 7,
        _ => throw new ArgumentOutOfRangeException(nameof(tamano))
    };

    public static string Nombre(this TamanoVaso tamano) => tamano switch
    {
        TamanoVaso.Pequeno => "Pequeño",
        TamanoVaso.Mediano => "Mediano",
        TamanoVaso.Grande => "Grande",
        _ => throw new ArgumentOutOfRangeException(nameof(tamano))
    };
}

public sealed record ResultadoPreparacion(bool Exito, string Mensaje);

public sealed class ServicioMaquina(int vasos, int cafe, int azucar)
{
    public int VasosDisponibles { get; private set; } = Validar(vasos, nameof(vasos));
    public int CafeDisponible { get; private set; } = Validar(cafe, nameof(cafe));
    public int AzucarDisponible { get; private set; } = Validar(azucar, nameof(azucar));
    private bool vasoPendiente;

    public ResultadoPreparacion Preparar(TamanoVaso tamano, int cucharadas)
    {
        if (cucharadas < 0) throw new ArgumentOutOfRangeException(nameof(cucharadas));
        int cafeNecesario = tamano.Onzas();
        if (vasoPendiente) return new(false, "Recoja su vaso antes de preparar otro café.");

        if (VasosDisponibles < 1) return new(false, "No hay vasos disponibles.");
        if (CafeDisponible < cafeNecesario) return new(false, "No hay suficiente café.");
        if (AzucarDisponible < cucharadas) return new(false, "No hay suficiente azúcar.");

        VasosDisponibles--;
        CafeDisponible -= cafeNecesario;
        AzucarDisponible -= cucharadas;
        vasoPendiente = true;
        return new(true, "Café preparado. Puede recoger su vaso.");
    }

    public bool RecogerVaso()
    {
        if (!vasoPendiente) return false;
        vasoPendiente = false;
        return true;
    }

    private static int Validar(int valor, string nombre) => valor < 0
        ? throw new ArgumentOutOfRangeException(nombre)
        : valor;
}
