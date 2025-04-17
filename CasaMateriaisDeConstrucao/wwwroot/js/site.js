document.addEventListener('DOMContentLoaded', function() {
    // Inicialização correta das tabs
    const tabLinks = document.querySelectorAll('[data-bs-toggle="tab"]');
    tabLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const tabId = this.getAttribute('data-bs-target');
            const tabPane = document.querySelector(tabId);
            
            // Remove a classe 'active' de todas as tabs e panes
            document.querySelectorAll('.nav-link').forEach(el => el.classList.remove('active'));
            document.querySelectorAll('.tab-pane').forEach(el => el.classList.remove('show', 'active'));
            
            // Adiciona a classe 'active' à tab e pane clicada
            this.classList.add('active');
            tabPane.classList.add('show', 'active');
            
            // Carrega os dados da tab quando clicada
            if (tabId === '#produtos') {
                carregarProdutos();
            } else if (tabId === '#clientes') {
                carregarClientes();
            } else if (tabId === '#vendas') {
                carregarVendas();
            }
        });
    });

    // Carrega os dados iniciais
    carregarProdutos();
    
    // Configura os eventos
    configurarEventos();
});

function configurarEventos() {
    // Produtos
    document.getElementById('btnSalvarProduto')?.addEventListener('click', salvarProduto);
    
    // Clientes
    document.getElementById('btnSalvarCliente')?.addEventListener('click', salvarCliente);
    
    // Vendas
    document.getElementById('btnAdicionarItem')?.addEventListener('click', adicionarItemVenda);
    document.getElementById('btnFinalizarVenda')?.addEventListener('click', finalizarVenda);
    document.getElementById('itemProdutoId')?.addEventListener('change', atualizarPrecoProduto);
}

// Funções para Produtos
async function carregarProdutos() {
    try {
        const response = await fetch('/api/produtos');
        if (!response.ok) throw new Error('Erro ao carregar produtos');
        const produtos = await response.json();
        
        const tabela = document.querySelector('#tabelaProdutos tbody');
        tabela.innerHTML = '';
        
        produtos.forEach(produto => {
            const row = tabela.insertRow();
            row.innerHTML = `
                <td>${produto.id}</td>
                <td>${produto.nome}</td>
                <td>${produto.descricao || ''}</td>
                <td>R$ ${produto.preco.toFixed(2)}</td>
                <td>${produto.estoque}</td>
                <td>${produto.categoria || ''}</td>
                <td class="table-actions">
                    <button class="btn btn-sm btn-primary" onclick="editarProduto(${produto.id})">Editar</button>
                    <button class="btn btn-sm btn-danger" onclick="excluirProduto(${produto.id})">Excluir</button>
                </td>
            `;
        });
    } catch (error) {
        console.error('Erro ao carregar produtos:', error);
        alert('Erro ao carregar produtos. Verifique o console para mais detalhes.');
    }
}

async function salvarProduto() {
    try {
        const produto = {
            id: document.getElementById('produtoId').value || 0,
            nome: document.getElementById('produtoNome').value,
            descricao: document.getElementById('produtoDescricao').value,
            preco: parseFloat(document.getElementById('produtoPreco').value),
            estoque: parseInt(document.getElementById('produtoEstoque').value),
            categoria: document.getElementById('produtoCategoria').value,
            imagemUrl: ''
        };

        if (!produto.nome || isNaN(produto.preco)) {
            throw new Error('Preencha os campos obrigatórios (Nome e Preço)');
        }

        const url = produto.id ? `/api/produtos/${produto.id}` : '/api/produtos';
        const method = produto.id ? 'PUT' : 'POST';
        
        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(produto)
        });
        
        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Erro ao salvar produto');
        }

        const modal = bootstrap.Modal.getInstance(document.getElementById('modalProduto'));
        modal.hide();
        await carregarProdutos();
        document.getElementById('formProduto').reset();
        alert('Produto salvo com sucesso!');
    } catch (error) {
        console.error('Erro ao salvar produto:', error);
        alert(error.message);
    }
}

async function editarProduto(id) {
    try {
        const response = await fetch(`/api/produtos/${id}`);
        if (!response.ok) throw new Error('Produto não encontrado');
        
        const produto = await response.json();
        
        document.getElementById('produtoId').value = produto.id;
        document.getElementById('produtoNome').value = produto.nome;
        document.getElementById('produtoDescricao').value = produto.descricao || '';
        document.getElementById('produtoPreco').value = produto.preco;
        document.getElementById('produtoEstoque').value = produto.estoque;
        document.getElementById('produtoCategoria').value = produto.categoria || '';
        
        const modal = new bootstrap.Modal(document.getElementById('modalProduto'));
        modal.show();
    } catch (error) {
        console.error('Erro ao editar produto:', error);
        alert('Erro ao carregar dados do produto');
    }
}

async function excluirProduto(id) {
    if (!confirm('Tem certeza que deseja excluir este produto?')) return;
    
    try {
        const response = await fetch(`/api/produtos/${id}`, {
            method: 'DELETE'
        });
        
        if (!response.ok) throw new Error('Erro ao excluir produto');
        
        await carregarProdutos();
        alert('Produto excluído com sucesso!');
    } catch (error) {
        console.error('Erro ao excluir produto:', error);
        alert('Erro ao excluir produto');
    }
}

// Funções para Clientes
async function carregarClientes() {
    try {
        const response = await fetch('/api/clientes');
        if (!response.ok) throw new Error('Erro ao carregar clientes');
        
        const clientes = await response.json();
        const tabela = document.querySelector('#tabelaClientes tbody');
        tabela.innerHTML = '';
        
        clientes.forEach(cliente => {
            const row = tabela.insertRow();
            row.innerHTML = `
                <td>${cliente.id}</td>
                <td>${cliente.nome}</td>
                <td>${cliente.cpf || ''}</td>
                <td>${cliente.telefone || ''}</td>
                <td>${cliente.email || ''}</td>
                <td class="table-actions">
                    <button class="btn btn-sm btn-primary" onclick="editarCliente(${cliente.id})">Editar</button>
                    <button class="btn btn-sm btn-danger" onclick="excluirCliente(${cliente.id})">Excluir</button>
                </td>
            `;
        });
    } catch (error) {
        console.error('Erro ao carregar clientes:', error);
        alert('Erro ao carregar clientes');
    }
}

async function salvarCliente() {
    try {
        const cliente = {
            id: document.getElementById('clienteId').value || 0,
            nome: document.getElementById('clienteNome').value,
            cpf: document.getElementById('clienteCpf').value,
            telefone: document.getElementById('clienteTelefone').value,
            email: document.getElementById('clienteEmail').value,
            endereco: document.getElementById('clienteEndereco').value
        };

        if (!cliente.nome) {
            throw new Error('Nome é obrigatório');
        }

        const url = cliente.id ? `/api/clientes/${cliente.id}` : '/api/clientes';
        const method = cliente.id ? 'PUT' : 'POST';
        
        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(cliente)
        });
        
        if (!response.ok) throw new Error('Erro ao salvar cliente');

        const modal = bootstrap.Modal.getInstance(document.getElementById('modalCliente'));
        modal.hide();
        await carregarClientes();
        document.getElementById('formCliente').reset();
        alert('Cliente salvo com sucesso!');
    } catch (error) {
        console.error('Erro ao salvar cliente:', error);
        alert(error.message || 'Erro ao salvar cliente');
    }
}

// Funções para Vendas
async function carregarVendas() {
    try {
        const response = await fetch('/api/vendas');
        if (!response.ok) throw new Error('Erro ao carregar vendas');
        
        const vendas = await response.json();
        const tabela = document.querySelector('#tabelaVendas tbody');
        tabela.innerHTML = '';
        
        vendas.forEach(venda => {
            const row = tabela.insertRow();
            row.innerHTML = `
                <td>${venda.id}</td>
                <td>${venda.cliente?.nome || 'Cliente não encontrado'}</td>
                <td>${new Date(venda.dataVenda).toLocaleDateString()}</td>
                <td>R$ ${venda.total.toFixed(2)}</td>
                <td class="table-actions">
                    <button class="btn btn-sm btn-info" onclick="detalhesVenda(${venda.id})">Detalhes</button>
                </td>
            `;
        });
    } catch (error) {
        console.error('Erro ao carregar vendas:', error);
        alert('Erro ao carregar vendas');
    }
}

// [Adicione aqui as demais funções para vendas...]

// Expor funções globais para os eventos HTML
window.editarProduto = editarProduto;
window.excluirProduto = excluirProduto;
window.editarCliente = editarCliente;
window.excluirCliente = excluirCliente;
window.detalhesVenda = detalhesVenda;
window.removerItemVenda = removerItemVenda;
window.abrirModalVenda = abrirModalVenda;