using DataBaseFirst.Models;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestCategoriaService
{
    private Mock<ICategoriaRepository> _mockRepository;
    private CategoriaService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ICategoriaRepository>();
        //_service = new CategoriaService(_mockRepository.Object);
    }

    // Test 1: Registrar categor�a con c�digo repetido
    [TestMethod]
    public async Task RegistrarCategoria_DeberiaFallar_SiCodigoYaExiste()
    {
        var categoria = new Categorium { Codigo = "CAT01", Nombre_Categoria = "Bebidas", Estado = true };

        _mockRepository.Setup(r => r.ListarCategoriasAsync())
                       .ReturnsAsync(new List<Categorium>
                       {
                               new Categorium { Id_Categoria = 1, Codigo = "CAT01", Nombre_Categoria = "Snacks", Estado = true }
                       });

        var resultado = await _service.RegistrarCategoriaAsync(categoria);

        Assert.IsFalse(resultado.IsSuccess);
        Assert.AreEqual("El c�digo ya existe", resultado.Message);
    }

    // Test 2: Registrar categor�a con nombre repetido
    [TestMethod]
    public async Task RegistrarCategoria_DeberiaFallar_SiNombreYaExiste()
    {
        var categoria = new Categorium { Codigo = "CAT02", Nombre_Categoria = "Snacks", Estado = true };

        _mockRepository.Setup(r => r.ListarCategoriasAsync())
                       .ReturnsAsync(new List<Categorium>
                       {
                               new Categorium { Id_Categoria = 1, Codigo = "CAT01", Nombre_Categoria = "Snacks", Estado = true }
                       });

        var resultado = await _service.RegistrarCategoriaAsync(categoria);

        Assert.IsFalse(resultado.IsSuccess);
        Assert.AreEqual("El nombre ya existe", resultado.Message);
    }

    // Test 3: Registrar categor�a v�lida
    [TestMethod]
    public async Task RegistrarCategoria_DeberiaRegistrar_SiDatosValidos()
    {
        var categoria = new Categorium { Codigo = "CAT02", Nombre_Categoria = "L�cteos", Estado = true };

        _mockRepository.Setup(r => r.ListarCategoriasAsync()).ReturnsAsync(new List<Categorium>());
        _mockRepository.Setup(r => r.RegistrarCategoriaAsync(It.IsAny<Categorium>())).ReturnsAsync(1);

        var resultado = await _service.RegistrarCategoriaAsync(categoria);

        Assert.IsTrue(resultado.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, resultado.Message);
    }

    // Test 4: Obtener categor�a inexistente
    [TestMethod]
    public async Task ObtenerCategoria_DeberiaFallar_SiNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerCategoriaAsync(99)).ReturnsAsync((Categorium)null);

        var resultado = await _service.ObtenerCategoriaAsync(99);

        Assert.IsFalse(resultado.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, resultado.Message);
    }

    // Test 5: Obtener categor�a existente
    [TestMethod]
    public async Task ObtenerCategoria_DeberiaDevolver_SiExiste()
    {
        var categoria = new Categorium { Id_Categoria = 1, Codigo = "CAT01", Nombre_Categoria = "Snacks", Estado = true };

        _mockRepository.Setup(r => r.ObtenerCategoriaAsync(1)).ReturnsAsync(categoria);

        var resultado = await _service.ObtenerCategoriaAsync(1);

        Assert.IsTrue(resultado.IsSuccess);
        Assert.AreEqual(categoria, resultado.Data);
    }

    // Test 6: Editar categor�a inexistente
    [TestMethod]
    public async Task EditarCategoria_DeberiaFallar_SiNoExiste()
    {
        var categoria = new Categorium { Id_Categoria = 99, Nombre_Categoria = "Snacks" };

        _mockRepository.Setup(r => r.ObtenerCategoriaAsync(99)).ReturnsAsync((Categorium)null);

        var resultado = await _service.EditarCategoriaAsync(categoria);

        Assert.IsFalse(resultado.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, resultado.Message);
    }

    // Test 7: Editar categor�a v�lida
    [TestMethod]
    public async Task EditarCategoria_DeberiaEditar_SiExiste()
    {
        var categoria = new Categorium { Id_Categoria = 1, Nombre_Categoria = "Bebidas", Estado = true };

        _mockRepository.Setup(r => r.ObtenerCategoriaAsync(1))
                       .ReturnsAsync(new Categorium { Id_Categoria = 1, Nombre_Categoria = "Viejo", Estado = true });

        _mockRepository.Setup(r => r.ListarCategoriasAsync()).ReturnsAsync(new List<Categorium>());
        _mockRepository.Setup(r => r.EditarCategoriaAsync(It.IsAny<Categorium>())).ReturnsAsync(1);

        var resultado = await _service.EditarCategoriaAsync(categoria);

        Assert.IsTrue(resultado.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, resultado.Message);
    }

    // Test 8: Eliminar categor�a inexistente
    [TestMethod]
    public async Task EliminarCategoria_DeberiaFallar_SiNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerCategoriaAsync(99)).ReturnsAsync((Categorium)null);

        var resultado = await _service.EliminarCategoriaAsync(99);

        Assert.IsFalse(resultado.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, resultado.Message);
    }

    //  Test 9: Eliminar categor�a v�lida
    [TestMethod]
    public async Task EliminarCategoria_DeberiaEliminar_SiExiste()
    {
        _mockRepository.Setup(r => r.ObtenerCategoriaAsync(1))
                       .ReturnsAsync(new Categorium { Id_Categoria = 1, Nombre_Categoria = "Snacks" });

        _mockRepository.Setup(r => r.EliminarCategoriaAsync(1)).ReturnsAsync(1);

        var resultado = await _service.EliminarCategoriaAsync(1);

        Assert.IsTrue(resultado.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, resultado.Message);
    }
}
