namespace MaquinaCafe;

internal static class Program
{
    private static readonly TamanoVaso[] Tamanos = Enum.GetValues<TamanoVaso>();

    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        int[] inventario = { 10, 100, 20 };
        if (args.Length != 0)
        {
            if (args.Length != 3 || !args.Select(x => int.TryParse(x, out _)).All(x => x))
            { Console.WriteLine("Indique cero o tres enteros: vasos, café (oz) y azúcar."); return; }
            inventario = args.Select(int.Parse).ToArray();
        }

        ServicioMaquina maquina;
        try { maquina = new(inventario[0], inventario[1], inventario[2]); }
        catch (ArgumentOutOfRangeException) { Console.WriteLine("El inventario no puede ser negativo."); return; }

        while (true)
        {
            Console.WriteLine("\nMáquina de café\n1. Preparar café\n2. Consultar inventario\n3. Recoger vaso\n4. Salir");
            switch (Console.ReadLine())
            {
                case "1": Preparar(maquina); break;
                case "2": Console.WriteLine($"Vasos: {maquina.VasosDisponibles} | Café: {maquina.CafeDisponible} oz | Azúcar: {maquina.AzucarDisponible} cucharadas"); break;
                case "3": Console.WriteLine(maquina.RecogerVaso() ? "Vaso recogido." : "No hay un vaso pendiente de recoger."); break;
                case "4": return;
                case null: return;
                default: Console.WriteLine("Opción inválida."); break;
            }
        }
    }

    private static void Preparar(ServicioMaquina maquina)
    {
        Console.WriteLine("Tamaño: " + string.Join("  ", Tamanos.Select((tamano, i) => $"{i + 1}. {tamano.Nombre()} ({tamano.Onzas()} oz)")));
        if (!int.TryParse(Console.ReadLine(), out int opcion) || opcion < 1 || opcion > Tamanos.Length)
        { Console.WriteLine("Seleccione un tamaño válido."); return; }
        Console.Write("Cucharadas de azúcar: ");
        if (!int.TryParse(Console.ReadLine(), out int azucar) || azucar < 0)
        { Console.WriteLine("La cantidad de azúcar debe ser un número no negativo."); return; }
        var resultado = maquina.Preparar(Tamanos[opcion - 1], azucar);
        Console.WriteLine(resultado.Mensaje);
    }
}
