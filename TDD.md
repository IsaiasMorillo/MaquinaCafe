# Registro TDD

## Ciclo inicial

Se creó primero `MaquinaCafe.Tests/MaquinaTests.cs` con los escenarios de tamaños, azúcar, inventario y recogida. La primera ejecución de `dotnet test` no compiló porque el dominio todavía no existía:

```text
error CS0246: El nombre del tipo o del espacio de nombres 'TamanoVaso' no se encontró
error CS0103: El nombre 'TamanoVaso' no existe en el contexto actual
```

Después se implementó el dominio y la consola hasta que la suite quedó en verde.

## Corrección de la auditoría

Antes de tocar el código se agregaron tres pruebas:

- `No_permite_preparar_otro_cafe_sin_recoger_el_anterior`
- `Rechaza_inventario_inicial_negativo`
- `Termina_cuando_se_cierra_la_entrada`

La ejecución falló por las razones esperadas:

```text
No_permite_preparar_otro_cafe_sin_recoger_el_anterior [FAIL]
Assert.False() Failure
Expected: False
Actual:   True

Termina_cuando_se_cierra_la_entrada [FAIL]
La aplicación no terminó al cerrar la entrada.

Con error: 2, Superado: 11, Total: 13
```

La prueba de inventario negativo ya pasaba: es una prueba de regresión para una validación que existía pero no estaba protegida.

## GREEN

Se bloquearon las preparaciones mientras hay un vaso pendiente, se agregó `case null: return;` al menú y se mantuvo el inventario sin cambios ante el segundo intento. La prueba de proceso comprueba que la aplicación termina al cerrarse la entrada.

```text
Correctas! - Con error: 0, Superado: 12, Total: 12
```

## REFACTOR

- Las onzas y los nombres de los tamaños quedaron en `TamanoVasoInfo`.
- El menú y la selección usan el mismo arreglo obtenido del enum, sin duplicar 3/5/7 ni convertir la opción con un índice fijo.
- Se eliminó la prueba vacía `UnitTest1`.
- Se eliminó la dependencia `coverlet.collector`, que no se usaba.
- La salida usa UTF-8 para conservar las tildes al redirigirla.

Después del refactor, `dotnet test` volvió a pasar: 12/12.

## Comprobaciones manuales

- Preparar dos veces sin recoger: el segundo intento muestra "Recoja su vaso antes de preparar otro café." y no descuenta recursos.
- Entrada vacía: la aplicación termina en lugar de repetir el menú.
- Inventario por argumentos: `dotnet run --project MaquinaCafe -- 5 30 8` muestra 5 vasos, 30 oz y 8 cucharadas.
- Salida redirigida: los bytes de `Máquina` y `café` son UTF-8.
