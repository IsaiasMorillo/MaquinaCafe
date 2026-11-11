# Máquina dispensadora de café

Aplicación de consola en C# para preparar café con inventario de vasos, café y azúcar, basada en los requisitos de `TDD Maquina Cafe.pdf`.

## Tecnologías

- .NET 10
- xUnit
- Sin persistencia ni dependencias de producción externas

## Estructura

- `MaquinaCafe/Program.cs`: menú e interacción en español.
- `MaquinaCafe/Models.cs`: tamaños, onzas, resultado y lógica de inventario/preparación.
- `MaquinaCafe.Tests/MaquinaTests.cs`: 12 pruebas automatizadas.
- `TDD.md`: registro breve de los ciclos RED/GREEN iniciales y de la corrección.

## Funcionamiento

El vaso pequeño consume 3 oz, el mediano 5 oz y el grande 7 oz. El usuario elige además cucharadas de azúcar (se permite cero). La máquina comprueba vasos, café y azúcar antes de descontar cualquiera; tras una preparación exitosa queda un vaso pendiente para recoger y no se permite preparar otro hasta recogerlo.

El PDF no define el inventario inicial, así que se usan valores demostrativos configurables al iniciar: 10 vasos, 100 oz de café y 20 cucharadas de azúcar. Se pueden sustituir pasando tres enteros no negativos en ese orden, por ejemplo `dotnet run --project MaquinaCafe -- 5 30 8`. No se impone una capacidad máxima ni un límite de azúcar distinto del inventario.

## TDD

Se escribieron primero pruebas para tamaños, consumo íntegro, falta de recursos, azúcar cero/negativa y recogida. Se ejecutaron inicialmente y fallaron al no existir el dominio (`TamanoVaso`/servicio); luego se implementó lo imprescindible hasta que pasaron. La corrección posterior se hizo igual: primero se agregaron pruebas que fallaban y después el código. Los ciclos y sus resultados están en `TDD.md`; la suite actual tiene 12 pruebas reales.

## Ejecutar

Desde esta carpeta:

```sh
dotnet run --project MaquinaCafe
dotnet test
dotnet build
```

El menú ofrece preparar café, consultar inventario, recoger un vaso y salir.
