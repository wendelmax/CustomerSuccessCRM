using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Repositories;

namespace CustomerSuccessCRM.Lib.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Cliente>> ListarTodosAsync()
        {
            return await _repository.BuscarTodosAsync();
        }

        public async Task<Cliente> BuscarPorIdAsync(int id)
        {
            return await _repository.BuscarPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.Nome))
                throw new ArgumentException("Nome do cliente é obrigatório");

            if (string.IsNullOrEmpty(cliente.Email))
                throw new ArgumentException("Email do cliente é obrigatório");

            cliente.DataCadastro = DateTime.Now;
            cliente.Ativo = true;
            cliente.Status = StatusCliente.Prospecto;

            return await _repository.AdicionarAsync(cliente);
        }

        public async Task<bool> AtualizarAsync(Cliente cliente)
        {
            var clienteExistente = await _repository.BuscarPorIdAsync(cliente.Id);
            if (clienteExistente == null)
                throw new KeyNotFoundException($"Cliente com ID {cliente.Id} não encontrado");

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Telefone = cliente.Telefone;
            clienteExistente.Empresa = cliente.Empresa;
            clienteExistente.VendedorId = cliente.VendedorId;
            clienteExistente.Status = cliente.Status;
            clienteExistente.DataAtualizacao = DateTime.Now;

            return await _repository.AtualizarAsync(clienteExistente);
        }

        public async Task<bool> DeletarAsync(int id)
        {
            return await _repository.DeletarAsync(id);
        }

        public async Task<bool> AtivarClienteAsync(int id)
        {
            var cliente = await _repository.BuscarPorIdAsync(id);
            if (cliente == null)
                return false;

            cliente.Ativo = true;
            cliente.Status = StatusCliente.Ativo;
            cliente.DataAtualizacao = DateTime.Now;

            return await _repository.AtualizarAsync(cliente);
        }

        public async Task<bool> DesativarClienteAsync(int id)
        {
            var cliente = await _repository.BuscarPorIdAsync(id);
            if (cliente == null)
                return false;

            cliente.Ativo = false;
            cliente.Status = StatusCliente.Inativo;
            cliente.DataAtualizacao = DateTime.Now;

            return await _repository.AtualizarAsync(cliente);
        }

        public async Task<List<Cliente>> BuscarPorStatusAsync(StatusCliente status)
        {
            return await _repository.BuscarPorStatusAsync(status);
        }

        public async Task<List<Cliente>> BuscarPorVendedorAsync(string vendedorId)
        {
            return await _repository.BuscarPorVendedorAsync(vendedorId);
        }

        public async Task<List<Cliente>> ListarInativosAsync()
        {
            return await _repository.BuscarInativos();
        }

        public async Task<int> ContarClientesAtivosAsync()
        {
            return await _repository.ContarClientesAtivosAsync();
        }
    }
} 