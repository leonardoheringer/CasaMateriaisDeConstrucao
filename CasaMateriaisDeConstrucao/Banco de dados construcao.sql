-- Criação do banco de dados
CREATE DATABASE IF NOT EXISTS CasaMateriaisDeConstrucao;
USE CasaMateriaisDeConstrucao;

-- Tabela de Produtos
CREATE TABLE IF NOT EXISTS Produtos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Descricao TEXT,
    Preco DECIMAL(10, 2) NOT NULL,
    Estoque INT NOT NULL DEFAULT 0,
    Categoria VARCHAR(50),
    ImagemUrl VARCHAR(255)
);

-- Tabela de Clientes
CREATE TABLE IF NOT EXISTS Clientes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    CPF VARCHAR(14) UNIQUE,
    Telefone VARCHAR(15),
    Email VARCHAR(100),
    Endereco TEXT
);

-- Tabela de Vendas
CREATE TABLE IF NOT EXISTS Vendas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ClienteId INT NOT NULL,
    DataVenda DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Total DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);

-- Tabela de Itens de Venda
CREATE TABLE IF NOT EXISTS ItensVenda (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    VendaId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (VendaId) REFERENCES Vendas(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id)
);

-- Inserção de dados iniciais para teste

-- Produtos
INSERT INTO Produtos (Nome, Descricao, Preco, Estoque, Categoria) VALUES
('Cimento CP II 50kg', 'Cimento Portland Composto para uso geral', 32.90, 100, 'Cimento'),
('Tijolo Baiano', 'Tijolo cerâmico 6 furos 9x14x24cm', 1.20, 5000, 'Tijolos'),
('Areia Média', 'Areia média lavada, metro cúbico', 120.00, 30, 'Areia e Pedra'),
('Brita 1', 'Brita número 1, metro cúbico', 150.00, 25, 'Areia e Pedra'),
('Tinta Acrílica Branca 18L', 'Tinta acrílica para paredes internas e externas', 189.90, 20, 'Tintas'),
('Argamassa AC-III 20kg', 'Argamassa colante para assentamento de cerâmicas', 24.50, 80, 'Argamassas'),
('Cano PVC 100mm 3m', 'Cano para esgoto, diâmetro 100mm', 42.30, 60, 'Hidráulica'),
('Telha Cerâmica', 'Telha cerâmica tipo romana', 3.75, 2000, 'Telhas'),
('Porta de Madeira', 'Porta de madeira maciça 2,10x0,70m', 450.00, 15, 'Portas e Janelas'),
('Janela de Alumínio', 'Janela de alumínio branco 1,20x1,00m', 380.00, 12, 'Portas e Janelas');

-- Clientes
INSERT INTO Clientes (Nome, CPF, Telefone, Email, Endereco) VALUES
('João da Silva', '123.456.789-00', '(11) 9999-8888', 'joao@email.com', 'Rua das Flores, 123 - Centro - São Paulo/SP'),
('Maria Oliveira', '987.654.321-00', '(11) 7777-6666', 'maria@email.com', 'Av. Paulista, 1000 - Bela Vista - São Paulo/SP'),
('Carlos Souza', '456.789.123-00', '(11) 5555-4444', 'carlos@email.com', 'Rua dos Pinheiros, 45 - Pinheiros - São Paulo/SP'),
('Ana Santos', '321.654.987-00', '(11) 3333-2222', 'ana@email.com', 'Alameda Santos, 789 - Jardins - São Paulo/SP'),
('Pedro Costa', '789.123.456-00', '(11) 1111-0000', 'pedro@email.com', 'Rua Augusta, 500 - Consolação - São Paulo/SP');

-- Vendas e Itens de Venda (exemplos)
INSERT INTO Vendas (ClienteId, DataVenda, Total) VALUES
(1, '2023-05-10 09:30:00', 185.70),
(2, '2023-05-11 14:15:00', 450.00),
(3, '2023-05-12 10:45:00', 1200.00);

INSERT INTO ItensVenda (VendaId, ProdutoId, Quantidade, PrecoUnitario) VALUES
-- Venda 1
(1, 1, 2, 32.90), -- 2 sacos de cimento
(1, 2, 100, 1.20), -- 100 tijolos
-- Venda 2
(2, 9, 1, 450.00), -- 1 porta de madeira
-- Venda 3
(3, 3, 1, 120.00), -- 1 m³ de areia
(3, 4, 1, 150.00), -- 1 m³ de brita
(3, 5, 5, 189.90); -- 5 latas de tinta

-- Atualizando o total da venda 1 (para demonstrar cálculo)
UPDATE Vendas SET Total = (SELECT SUM(Quantidade * PrecoUnitario) FROM ItensVenda WHERE VendaId = 1) WHERE Id = 1;

select * from Clientes;