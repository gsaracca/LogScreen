using System.Collections.Generic;
using System.Linq;
using _.Models;
using MudBlazor;

namespace _.Services;

/// <summary>
/// Provee el árbol de operaciones disponibles.
/// En el futuro este servicio cargará los nodos desde DLLs externas.
/// </summary>
public class OperationTreeService
{
    private readonly List<OperationNode> _tree;

    public OperationTreeService()
    {
        _tree = BuildDefaultTree();
    }

    public IReadOnlyList<OperationNode> GetTree() => _tree;

    public OperationNode? FindById(string id)
    {
        foreach (var module in _tree)
        {
            if (module.Id == id) return module;
            var found = module.Children.FirstOrDefault(c => c.Id == id);
            if (found is not null) return found;
        }
        return null;
    }

    // Árbol genérico de ejemplo — se reemplazará al cargar DLLs
    private static List<OperationNode> BuildDefaultTree() =>
    [
        new OperationNode
        {
            Id = "modulo-maestros",
            Name = "Maestros",
            Icon = Icons.Material.Filled.TableChart,
            Children =
            [
                new() { Id = "op-clientes",    Name = "Clientes",    Icon = Icons.Material.Filled.People },
                new() { Id = "op-proveedores", Name = "Proveedores", Icon = Icons.Material.Filled.LocalShipping },
                new() { Id = "op-productos",   Name = "Productos",   Icon = Icons.Material.Filled.Inventory },
            ]
        },
        new OperationNode
        {
            Id = "modulo-operaciones",
            Name = "Operaciones",
            Icon = Icons.Material.Filled.SwapHoriz,
            Children =
            [
                new() { Id = "op-pedidos",   Name = "Pedidos",   Icon = Icons.Material.Filled.ShoppingCart },
                new() { Id = "op-facturas",  Name = "Facturas",  Icon = Icons.Material.Filled.Receipt },
                new() { Id = "op-pagos",     Name = "Pagos",     Icon = Icons.Material.Filled.Payments },
            ]
        },
        new OperationNode
        {
            Id = "modulo-reportes",
            Name = "Reportes",
            Icon = Icons.Material.Filled.BarChart,
            Children =
            [
                new() { Id = "op-ventas",    Name = "Ventas",    Icon = Icons.Material.Filled.TrendingUp },
                new() { Id = "op-stock",     Name = "Stock",     Icon = Icons.Material.Filled.Warehouse },
                new() { Id = "op-auditoria", Name = "Auditoría", Icon = Icons.Material.Filled.ManageSearch },
            ]
        },
        new OperationNode
        {
            Id = "modulo-sistema",
            Name = "Sistema",
            Icon = Icons.Material.Filled.Settings,
            Children =
            [
                new() { Id = "op-usuarios",  Name = "Usuarios",     Icon = Icons.Material.Filled.ManageAccounts },
                new() { Id = "op-permisos",  Name = "Permisos",     Icon = Icons.Material.Filled.Lock },
                new() { Id = "op-config",    Name = "Configuración",Icon = Icons.Material.Filled.Tune },
            ]
        },
    ];
}
