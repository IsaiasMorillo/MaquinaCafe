using System.Diagnostics;
using MaquinaCafe;

namespace MaquinaCafe.Tests;

public class MaquinaTests
{
    [Theory]
    [InlineData(TamanoVaso.Pequeno, 3)]
    [InlineData(TamanoVaso.Mediano, 5)]
    [InlineData(TamanoVaso.Grande, 7)]
    public void Preparar_descuenta_todos_los_recursos(TamanoVaso tamano, int cafe)
    {
        var maquina = new ServicioMaquina(2, 20, 10);

        var resultado = maquina.Preparar(tamano, 2);

        Assert.True(resultado.Exito);
        Assert.Equal(1, maquina.VasosDisponibles);
        Assert.Equal(20 - cafe, maquina.CafeDisponible);
        Assert.Equal(8, maquina.AzucarDisponible);
    }

    [Fact]
    public void Fallo_no_descuenta_parcialmente_y_muestra_motivo()
    {
        var maquina = new ServicioMaquina(1, 3, 1);

        var resultado = maquina.Preparar(TamanoVaso.Mediano, 2);

        Assert.False(resultado.Exito);
        Assert.Contains("café", resultado.Mensaje, StringComparison.OrdinalIgnoreCase);
        Assert.Equal((1, 3, 1), (maquina.VasosDisponibles, maquina.CafeDisponible, maquina.AzucarDisponible));
    }

    [Fact]
    public void Permite_cero_azucar_y_rechaza_azucar_negativa()
    {
        var maquina = new ServicioMaquina(1, 3, 0);
        Assert.True(maquina.Preparar(TamanoVaso.Pequeno, 0).Exito);
        Assert.Throws<ArgumentOutOfRangeException>(() => maquina.Preparar(TamanoVaso.Pequeno, -1));
    }

    [Theory]
    [InlineData(0, 10, 10, "vasos")]
    [InlineData(1, 2, 10, "café")]
    [InlineData(1, 10, 1, "azúcar")]
    public void Informa_recurso_insuficiente_sin_cambios(int vasos, int cafe, int azucar, string esperado)
    {
        var maquina = new ServicioMaquina(vasos, cafe, azucar);
        var antes = (maquina.VasosDisponibles, maquina.CafeDisponible, maquina.AzucarDisponible);

        var resultado = maquina.Preparar(TamanoVaso.Pequeno, 2);

        Assert.False(resultado.Exito);
        Assert.Contains(esperado, resultado.Mensaje, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(antes, (maquina.VasosDisponibles, maquina.CafeDisponible, maquina.AzucarDisponible));
    }

    [Fact]
    public void Recoger_solo_es_posible_tras_preparar()
    {
        var maquina = new ServicioMaquina(1, 3, 0);
        Assert.False(maquina.RecogerVaso());
        Assert.True(maquina.Preparar(TamanoVaso.Pequeno, 0).Exito);
        Assert.True(maquina.RecogerVaso());
        Assert.False(maquina.RecogerVaso());
    }

    [Fact]
    public void No_permite_preparar_otro_cafe_sin_recoger_el_anterior()
    {
        var maquina = new ServicioMaquina(2, 6, 0);

        Assert.True(maquina.Preparar(TamanoVaso.Pequeno, 0).Exito);
        var segundo = maquina.Preparar(TamanoVaso.Pequeno, 0);

        Assert.False(segundo.Exito);
        Assert.Contains("Recoja", segundo.Mensaje, StringComparison.OrdinalIgnoreCase);
        Assert.Equal((1, 3, 0), (maquina.VasosDisponibles, maquina.CafeDisponible, maquina.AzucarDisponible));
        Assert.True(maquina.RecogerVaso());
        Assert.True(maquina.Preparar(TamanoVaso.Pequeno, 0).Exito);
    }

    [Fact]
    public void Rechaza_inventario_inicial_negativo()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ServicioMaquina(-1, 10, 10));
    }

    [Fact]
    public void Termina_cuando_se_cierra_la_entrada()
    {
        var inicio = new ProcessStartInfo("dotnet")
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        inicio.ArgumentList.Add(typeof(ServicioMaquina).Assembly.Location);

        using var proceso = Process.Start(inicio)!;
        proceso.StandardInput.Close();

        var termino = proceso.WaitForExit(5000);
        if (!termino && !proceso.HasExited) proceso.Kill(entireProcessTree: true);

        Assert.True(termino, "La aplicación no terminó al cerrar la entrada.");
    }
}
